using System;
using Microsoft.Extensions.Options;

namespace Swift
{
    public interface IJourneyCalculator 
    {
        double CalculateTimeFromDepot(double lat, double lng);
        double CalculateTimeBetweenPoints(double startLat, double startLng, double endLat, double endLong);
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

        public double CalculateTimeFromDepot(double lat, double lng)
        {
            return CalculateTimeBetweenPoints(lat, lng, _depotLat, _depotLong);
        }
        public double CalculateTimeBetweenPoints(double startLat, double startLng, double endLat, double endLong)
        {
            var pi = Math.PI;
            var baseRad = pi * startLat / 180;
            var targetRad = pi * _depotLat/ 180;
            var theta = startLng - _depotLong;
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