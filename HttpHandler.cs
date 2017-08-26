using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;

namespace Swift
{
    public class HttpHandler : IHttpHandler
    {
        private readonly HttpClient _client;
        private readonly string _apiUrl;
        private DataContractJsonSerializer _serializer;

        public HttpHandler(HttpClient client, string apiUrl) 
        {
            _client = client;
            _apiUrl = apiUrl;
        }

        public async Task<List<T>> GetObjects<T>(string endpoint)
        {
            _serializer = new DataContractJsonSerializer(typeof(List<T>));
            
            var json = _client.GetStreamAsync(string.Format("{0}{1}", _apiUrl, endpoint));
            var objects = _serializer.ReadObject(await json) as List<T>;
            return objects;
        }
    }
}
