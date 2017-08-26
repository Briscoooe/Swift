using System.Collections.Generic;

namespace Swift 
{
    public class DeliveryService : IDeliveryService 
    {
        private readonly IRequestSender _requestSender;
        private readonly List<long> _droneIds;
        private readonly List<long> _packageIds;

        public DeliveryService(IRequestSender requestSender)
        {
            _droneIds = new List<long> { 1593, 1251 };
            _packageIds = new List<long> { 1029438, 1029439};
            _requestSender = requestSender;
        }

        public void Start()
        {
            var drones = _requestSender.GetObjects<Drone>("drones").Result;
            var packages = _requestSender.GetObjects<Package>("packages").Result;
        }

        public void Stop()
        {
            
        }
    }
}