using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Swift
{
    class Program
    {
        static void Main(string[] args)
        {
            // if(args.Length == 0)
            // {
            //     throw new ArgumentException("Not enough arguments");
            // }
            // else if(args.Length > 1) 
            // {
            //     throw new ArgumentException("Too many arguments");
            // }

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<App>().Run();
            
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false)
                .Build();
            serviceCollection.AddOptions();
            serviceCollection.Configure<AppSettings>(configuration.GetSection("configuration"));

            serviceCollection.AddTransient<IJourneyCalculator, JourneyCalculator>();
            serviceCollection.AddTransient<IRequestSender, RequestSender>();
            serviceCollection.AddTransient<IDeliveryService, DeliveryService>();
            serviceCollection.AddTransient<App>();
        }
    }
}
