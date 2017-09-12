namespace Swift
{
    public class App
    {
        private readonly IDeliveryService _deliveryService;

        public App(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        public void Run()
        {
            _deliveryService.Start();
        }

        public void Stop()
        {
            _deliveryService.Stop();
        }
    }
}