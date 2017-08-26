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
            var drones = _requestSender.GetObjects<Drone>("drones").Result;
            var packages = _requestSender.GetObjects<Package>("packages").Result;
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();

            foreach(var drone in drones)
            {
                foreach(var package in packages)
                {
                    if(drone.Package.Length == 0)
                    {
                        var time = _journeyCalculator.CalculateJourneyTime(
                            drone.Location.Latitude, 
                            drone.Location.Longitude,
                            package.Destination.Latitude,
                            package.Destination.Longitude);

                        System.Console.WriteLine("Estimate time for journey: {0} minutes", time / 60);
                        System.Console.WriteLine("Time until delivery due: {0} minutes\n", (package.Deadline - now) / 60);
                    }
                }
            }
        }

        public void Stop()
        {
            
        }
    }
}