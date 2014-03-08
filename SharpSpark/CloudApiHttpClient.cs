using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Maybe5.SharpSpark
{
    class CloudApiHttpClient
    {
        const int TIMEOUT_SECONDS = 10;
        public string AccessToken { get; private set; }
        public string DeviceId { get; private set; }

        public CloudApiHttpClient(string accessToken, string deviceId)
        {
            AccessToken = accessToken;
            DeviceId = deviceId;
        }

        public string GetRawResultForGetDevices()
        {
            using (var client = new HttpClient() { Timeout = new TimeSpan(0, 0, TIMEOUT_SECONDS) })
            {
                var result = client.GetAsync(String.Format("https://api.spark.io/v1/devices?access_token={0}", AccessToken)).Result;
                result.EnsureSuccessStatusCode();
                return result.Content.ReadAsStringAsync().Result;
            }
        }

        public HttpResponseMessage GetRawResultForGet(string variableName)
        {
            using (var client = new HttpClient() { Timeout = new TimeSpan(0, 0, TIMEOUT_SECONDS) })
            {
                return client.GetAsync(String.Format("https://api.spark.io/v1/devices/{0}/{1}?access_token={2}", DeviceId, variableName, AccessToken)).Result;
                
            }
        }


        public HttpResponseMessage GetRawResultForPost(string functionKey, string[] args)
        {
            using (var client = new HttpClient() { Timeout = new TimeSpan(0, 0, TIMEOUT_SECONDS) })
            {
                var content = new FormUrlEncodedContent(new[] 
                {
                    new KeyValuePair<string, string>("params", String.Join(",",args))
                });
                return client.PostAsync(String.Format("https://api.spark.io/v1/devices/{0}/{1}?access_token={2}", DeviceId, functionKey, AccessToken), content).Result;
                
            }
        }
    }
}
