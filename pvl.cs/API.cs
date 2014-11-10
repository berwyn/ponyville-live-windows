using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Newtonsoft.Json;
using PortableRest;
using System.Collections.Generic;

namespace PVL
{
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

        #region >> /station

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

        #endregion

        #region >> /nowplaying

        public async Task<Dictionary<string, NowPlayingInfo>> NowPlayingFetchTask()
        {
            var request = new RestRequest("/nowplaying");

            var response = await SendAsync<DictionaryResponse<NowPlayingInfo>>(request);
            return response.Content.Result;
        }

        #endregion
    }
}