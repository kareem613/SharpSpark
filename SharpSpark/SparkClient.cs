using Newtonsoft.Json;
using System;

namespace Maybe5.SharpSpark
{
    public class SparkClient
    {
        public const int ERROR_RETURN_VALUE = 1;
        
        public string AccessToken
        {
            get
            {
                return CloudApiClient.AccessToken;
            }
        }

        public string DeviceId
        {
            get
            {
                return CloudApiClient.DeviceId;
            }
        }
        
        private CloudApiHttpClient CloudApiClient { get; set; }

        public SparkClient(string accessToken, string deviceId)
        {
            CloudApiClient = new CloudApiHttpClient(accessToken, deviceId);
        }

        public SparkVariableResult GetVariable(string variableName)
        {
            return CloudGet(variableName);
        }

        public SparkFunctionResult ExecuteFunction(string functionKey, params string[] args)
        {
            return CloudPost(functionKey, args);
        }

        public T GetVariableReturnValue<T>(string variableName)
        {
            var result = GetVariable(variableName);
            return (T)Convert.ChangeType(result.Result.ToString(),typeof(T));
        }

        public int ExecuteFunctionReturnValue(string functionKey, params string[] args)
        {
            var result = ExecuteFunction(functionKey, args);
            return int.Parse(result.Return_value.ToString());
        }

        private SparkVariableResult CloudGet(string variableName)
        {
            var json = CloudApiClient.GetRawResultForGet(variableName);
            return JsonConvert.DeserializeObject<SparkVariableResult>(json);
        }

        private SparkFunctionResult CloudPost(string functionKey, string[] args)
        {
            var response = CloudApiClient.GetRawResultForPost(functionKey, args);
            var rawContent = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode || rawContent.StartsWith("{\n  \"error\":"))
            {
                return new SparkFunctionResult() { ErrorResult = JsonConvert.DeserializeObject<SparkError>(rawContent) };
            }
            return JsonConvert.DeserializeObject<SparkFunctionResult>(rawContent);
        }

        public SparkDevice GetDevice()
        {
            var json = CloudApiClient.GetRawResultForGet(String.Empty);
            return JsonConvert.DeserializeObject<SparkDevice>(json);
        }


       
    }
}
