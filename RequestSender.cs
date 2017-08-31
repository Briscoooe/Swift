using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Swift
{
    public class RequestSender : IRequestSender
    {
        private readonly HttpClient _client;
        private readonly string _apiUrl;
        private DataContractJsonSerializer _serializer;

        public RequestSender(IOptions<AppSettings> config) 
        {
            _client = new HttpClient();
            _apiUrl = config.Value.ApiUrl;
        }

        public async Task<List<T>> GetObjects<T>(string endpoint)
        {
            _serializer = new DataContractJsonSerializer(typeof(List<T>));
            
            var json = _client.GetStreamAsync(string.Format(_apiUrl, endpoint));
            var objects = _serializer.ReadObject(await json) as List<T>;
            return objects;
        }
    }
}
