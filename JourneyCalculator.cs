using System;
using Microsoft.Extensions.Options;

namespace Swift
{
    public interface IJourneyCalculator 
    {
        double CalculateTimeFromDepot(Coordinate position);
        double CalculateTimeToCompleteDelivery(Coordinate position1, Coordinate position2);
    }
    public class JourneyCalculator : IJourneyCalculator
    {
        private readonly Coordinate _depotLocation;
        private const int _droneSpeed = 50;
        
        public JourneyCalculator(IOptions<AppSettings> config)
        {
            _depotLocation = config.Value.DepotLocation;
        }

        public double CalculateTimeFromDepot(Coordinate position)
        {
            return CalculateTimeBetweenPoints(position, _depotLocation);
        }

        public double CalculateTimeToCompleteDelivery(Coordinate position1, Coordinate position2)
        {
            var timeToDestination = CalculateTimeBetweenPoints( position1, position2);
            var timeToDepot = CalculateTimeFromDepot(position2);

            return timeToDestination + timeToDepot;
        }
        
        private double CalculateTimeBetweenPoints(Coordinate position1, Coordinate position2)
        {
            var pi = Math.PI;
            var baseRad = pi * position1.Latitude / 180;
            var targetRad = pi * position2.Latitude / 180;
            var theta = position1.Longitude - position2.Longitude;
            var thetaRad = pi * theta / 180;

            double dist =
                Math.Sin(baseRad) * Math.Sin(targetRad) + Math.Cos(baseRad) *
                Math.Cos(targetRad) * Math.Cos(thetaRad);
            dist = Math.Acos(dist);

            dist = dist * 180 / pi;
            dist = dist * 60 * 1.1515;

            dist *= 1.60934;
            return (dist / _droneSpeed) * 60 * 60;
        }
    }
}