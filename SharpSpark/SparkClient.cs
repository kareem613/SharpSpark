using Newtonsoft.Json;
using SharpSpark.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Maybe5.SharpSpark
{
    public class SparkClient
    {
        public const int ERROR_RETURN_VALUE = 1;

        private string[] API_ERROR_MESSAGES = new string[] { "Permission Denied", "invalid_grant" };
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
            var result = CloudGet(variableName);
            if(result.HasError)
            {
                throw new SparkDeviceException(result.Error,result);
            }
            return result;
        }

        public SparkFunctionResult ExecuteFunction(string functionKey, params string[] args)
        {
            var result = CloudPost(functionKey, args);
            if (result.HasErrors)
            {
                if(API_ERROR_MESSAGES.Contains(result.ErrorResult.Error))
                    throw new SparkApiException(result.ErrorResult.Error, result.ErrorResult);

                throw new SparkDeviceException(result.ErrorResult.Error, result.ErrorResult);
            }
            return result;
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
            var response = CloudApiClient.GetRawResultForGet(variableName);
            var rawContent = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode || rawContent.StartsWith("{\n  \"error\":"))
            {
                return new SparkVariableResult() { ErrorResult = JsonConvert.DeserializeObject<SparkError>(rawContent) };
            }
            return JsonConvert.DeserializeObject<SparkVariableResult>(rawContent);
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
            var response = CloudApiClient.GetRawResultForGet(String.Empty);
            var rawContent = response.Content.ReadAsStringAsync().Result;
            if (!response.IsSuccessStatusCode || rawContent.StartsWith("{\n  \"error\":"))
            {
                var sparkError =  JsonConvert.DeserializeObject<SparkError>(rawContent);
                throw new SparkApiException(sparkError.Error);
            }
            return JsonConvert.DeserializeObject<SparkDevice>(rawContent);
        }

        public List<SparkDevice> GetAllDevices()
        {
            var json = CloudApiClient.GetRawResultForGetDevices();
            return JsonConvert.DeserializeObject<List<SparkDevice>>(json);
        }
    }
}
