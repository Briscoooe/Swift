using System;
using Microsoft.Extensions.Options;

namespace Swift
{
    public interface IJourneyCalculator 
    {
        double CalculateJourneyTime(double droneLat, double droneLong, double packageLat, double packageLong);
    }
    public class JourneyCalculator : IJourneyCalculator
    {
        private readonly double _depotLat;
        private readonly double _depotLong;
        private const int _droneSpeed = 50;
        
        public JourneyCalculator(IOptions<AppSettings> config)
        {
            _depotLat = config.Value.DepotLocation.Latitude;
            _depotLong = config.Value.DepotLocation.Longitude;
        }

        public double CalculateJourneyTime(double droneLat, double droneLong, double packageLat, double packageLong)
        {
            var timeToDepot = CalculateTimeBetweenPoints(droneLat, droneLong);
            var timeToDestination = CalculateTimeBetweenPoints(packageLat, packageLong);

            return timeToDepot + timeToDestination;
        }

        private double CalculateTimeBetweenPoints(double lat1, double long1)
        {
            var pi = Math.PI;
            var baseRad = pi * lat1 / 180;
            var targetRad = pi * _depotLat/ 180;
            var theta = long1 - _depotLong;
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