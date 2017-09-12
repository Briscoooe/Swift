using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            AssignDrones();
        }

        public void Stop()
        {
            
        }

        private void AssignDrones()
        {
            // Retrieve the packages, sorted by which must leave soonest
            var packages = _requestSender.GetObjects<Package>("packages").Result
                .OrderBy(p => p.Deadline - _journeyCalculator.CalculateTimeFromDepot(p.Destination));

            // Retrieve the drones, sorting by the shortest trip back to the depot (including their current delivery)
            var drones = _requestSender.GetObjects<Drone>("drones").Result
                .OrderBy(d => _journeyCalculator.CalculateTimeToCompleteDelivery(
                    d.Location, d.Package.Length > 0 ? d.Package[0].Destination : d.Location
                ));

            var assignments = new List<Assignment>();
            var unassigned = new List<long>();
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();                                        
            foreach(var package in packages)
            {
                var depotToPackageTime = _journeyCalculator.CalculateTimeFromDepot(package.Destination);
                // Iterate over drones that have not already been assigned
                foreach(var drone in drones.Where(d => !assignments.Any(a => a.DroneId == d.Id)))
                {
                    var droneToDepotTime = GetDroneToDepotTime(drone);
                    var totalTime = droneToDepotTime + depotToPackageTime;
                    // As the drones are sorted by the quickest return journey to the depot
                    // If the first one in the sorted list cannot make the delivery on time, none of them can
                    // Thus its added to the "unassigned" list
                    if(package.Deadline - totalTime > now)
                    {   
                        unassigned.Add(package.Id);
                    }
                    // Otherwise, the assignment is created 
                    else
                    {
                        assignments.Add(new Assignment()
                        {
                            PackageId = package.Id,
                            DroneId = drone.Id
                        });
                    }
                    break;
                }
            }
            PrintOutput(assignments, unassigned);
        }

        private double GetDroneToDepotTime(Drone drone)
        {
            double droneToDepotTime = 0;
            if(drone.Package.Length == 0)
            {
                droneToDepotTime += _journeyCalculator.CalculateTimeFromDepot(drone.Location);
            }
            else
            {
                droneToDepotTime += _journeyCalculator.CalculateTimeToCompleteDelivery(
                    drone.Location, drone.Package[0].Destination);
            }
            return droneToDepotTime;
        }

        private void PrintOutput(List<Assignment> assignments, List<long> unassignedPackagesIds)
        {
            System.Console.WriteLine("Assignments:");
            foreach(var assignment in assignments)
            {
                System.Console.WriteLine("[droneId: {0}, packageId: {1}]", assignment.DroneId, assignment.PackageId);
            }
            System.Console.WriteLine("unassigned:");            
            foreach(var id in unassignedPackagesIds)
            {
                System.Console.Write(id);
            }
        }
    }
}