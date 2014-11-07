using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Newtonsoft.Json;
using PortableRest;

namespace PVL
{
    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

    public class ArrayResponse<T>
    {
        public string Status { get; set; }
        public T[] Result { get; set; }
    }

    public class Station
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("genre")]
        public string Genre { get; set; }
        [JsonProperty("image_url")]
        public string ImageURL { get; set; }
        [JsonProperty("stream_url")]
        public string StreamURL { get; set; }
    }

    public class API : RestClient
    {
        private static readonly string API_BASE = "http://ponyvillelive.com/api";

        private static API _instance = new API();

        public static API Instance
        {
            get { return _instance; }
        }

        public API()
        {
            BaseUrl = API_BASE;
            UserAgent = "pvl.cs/0.1.0";
        }

        public async Task<Station[]> StationFetchTask()
        {
            var request = new RestRequest("/station/list");
            var response = await SendAsync<ArrayResponse<Station>>(request);
            return response.Content.Result;
        }

        public async Task<Station[]> StationFetchTask(string category)
        {
            var request = new RestRequest("/station/list/category/{category}");
            request.AddUrlSegment("category", category);
            
            var response = await SendAsync<ArrayResponse<Station>>(request);
            return response.Content.Result;
        }
    }
}