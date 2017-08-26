using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Swift 
{
    public class DeliveryService : IDeliveryService 
    {
        private readonly IRequestSender _requestSender;
        private readonly IJourneyCalculator _journeyCalculator;

        public DeliveryService(IRequestSender requestSender, IJourneyCalculator journeyCalculator)
        {
            _requestSender = requestSender;
            _journeyCalculator = journeyCalculator;
        }

        public void Start()
        {
            var packages = _requestSender.GetObjects<Package>("packages").Result;
            var drones = _requestSender.GetObjects<Drone>("drones").Result;            
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();

            var output = new Output();
            var assignments = new List<Assignment>();
            var unassigned = new List<long>();
            foreach(var package in packages)
            {
                var assignment = new Assignment();
                assignment.PackageId = package.Id;
                var depotToDestination = _journeyCalculator.CalculateTimeFromDepot( 
                    package.Destination.Latitude,
                    package.Destination.Longitude);
                
                var timeToDeadline = package.Deadline - now;
                double shortestDeliveryTime  = long.MaxValue;
                foreach(var drone in drones)
                {
                    double timeBeforeDeliveryStart = 0;
                    // If it is doing a delivery
                    if(drone.Package.Length > 0)
                    {
                        // Add the time taken to perform the delivery
                        timeBeforeDeliveryStart += _journeyCalculator.CalculateTimeBetweenPoints(
                            drone.Location.Latitude,
                            drone.Location.Longitude,
                            package.Destination.Latitude,
                            package.Destination.Longitude
                        );
                        // Add the time taken to return from the destination to the depot
                        timeBeforeDeliveryStart += _journeyCalculator.CalculateTimeFromDepot(
                            package.Destination.Latitude,
                            package.Destination.Longitude
                        );
                    }
                    else 
                    {
                        // Add the time taken to return from its current location to the depot
                        timeBeforeDeliveryStart += _journeyCalculator.CalculateTimeFromDepot(
                            drone.Location.Latitude,
                            drone.Location.Longitude
                        );
                    }
                    var totalTimeForJourney = timeBeforeDeliveryStart + depotToDestination;
                    // If the drone will make it on time AND it will take less time than the current shortest time
                    if(totalTimeForJourney < timeToDeadline && shortestDeliveryTime > totalTimeForJourney)
                    {
                        shortestDeliveryTime = totalTimeForJourney;
                        assignment.DroneId = drone.Id;
                    }
                }
                if(assignment.DroneId == 0)
                {
                    unassigned.Add(package.Id);
                }
                else 
                {
                    assignments.Add(assignment);
                }
            }
        }

        public void Stop()
        {
            
        }
    }
}