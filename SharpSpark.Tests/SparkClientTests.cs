using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maybe5.SharpSpark;
using System.Configuration;
using System.Linq;
using SharpSpark.Exceptions;

namespace Maybe5.SharpSpark.Tests
{
    [TestClass]
    public class SparkClientTests
    {
        SparkClient client;

        [TestInitialize]
        public void Setup()
        {
            var accessToken = ConfigurationManager.AppSettings["accessToken"];
            var deviceId = ConfigurationManager.AppSettings["deviceId"];
            client = new SparkClient(accessToken, deviceId);
        }

        [TestMethod]
        public void GivenTestFirmwareClientInfoExpectFunctions()
        {
            SparkDevice device = client.GetDevice();

            Assert.AreEqual("returnOne", device.Functions.Single(), "Test function was not found. Ensure TestFirmware.cpp is flashed to the device.");
        }

        [TestMethod]
        public void GivenTestFirmwareClientInfoExpectVariables()
        {
            SparkDevice device = client.GetDevice();

            var variable = device.Variables.Where(v=>v.Key == "var0").SingleOrDefault();
            Assert.IsNotNull(variable);
            Assert.AreEqual("int32", variable.Value);
        }

        [TestMethod]
        public void GivenClientInfoExpectDeviceInfo()
        {
            SparkDevice device = client.GetDevice();

            Assert.IsNotNull(device);
            Assert.IsNotNull(device.Name);
            Assert.AreEqual(client.DeviceId, device.Id);
        }

        [TestMethod]
        public void GivenClientWhenExecuteReturnOneExpect1()
        {
            SparkFunctionResult result = client.ExecuteFunction("returnOne");

            Assert.AreEqual("1", result.Return_value);
        }

        [TestMethod]
        public void GivenClientWhenGetVar0Expect0()
        {
            SparkVariableResult result = client.GetVariable("var0");

            Assert.AreEqual("0", result.Result);
        }

        [TestMethod]
        public void GivenClientWhenGetVarFalseExpectFalse()
        {
            bool result = client.GetVariableReturnValue<bool>("varFalse");

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void GivenClientWhenGetVarStringExpectString()
        {
            string result = client.GetVariableReturnValue<string>("varA");

            Assert.AreEqual("A", result);
        }

        [TestMethod]
        [Ignore]//Waiting for double bug to get resolved. https://community.spark.io/t/strange-results-using-and-returning-doubles-solved/2836/8
        public void GivenClientWhenGetVarDoubleExpectDouble()
        {
            double result = client.GetVariableReturnValue<double>("var1dot1");

            Assert.AreEqual(1.1m, result);
        }

        [TestMethod]
        [ExpectedException(typeof(SparkDeviceException),"Variable not found")]
        public void GivenInvalidVariableExpectErrorVariableNotFound()
        {
            SparkVariableResult result = client.GetVariable("thisdoesnotexist");
        }

        [TestMethod]
        [ExpectedException(typeof(SparkDeviceException), "Function not found")]
        public void GivenInvalidFunctionExpectErrorFunctionNotFound()
        {
            SparkFunctionResult result = client.ExecuteFunction("thisdoesnotexist");
        }

        [TestMethod]
        [ExpectedException(typeof(SparkApiException), "invalid_grant")]
        public void GivenInvalidAccessTokenExpectErrorInvalidGrant()
        {
            var badClient = new SparkClient("badtoken", "");
            SparkFunctionResult result = badClient.ExecuteFunction("doesn't matter");
        }

        [TestMethod]
        [ExpectedException(typeof(SparkApiException), "Permission Denied")]
        public void GivenInvalidDeviceIdExpectErrorPermissionDenied()
        {
            var badClient = new SparkClient(client.AccessToken, "badid");
            SparkFunctionResult result = badClient.ExecuteFunction("doesn't matter");
        }

        [Ignore]//This test only passes if the device is offline
        [TestMethod]
        [ExpectedException(typeof(SparkDeviceException), "Timed out.")]
        public void GivenOfflineDeviceFunctionExpectErrorTimedOut()
        {
            SparkFunctionResult result = client.ExecuteFunction("returnOne");
        }

        [Ignore]//This test only passes if the device is offline
        [TestMethod]
        [ExpectedException(typeof(SparkDeviceException), "Timed out.")]
        public void GivenOfflineDeviceVariableExpectErrorTimedOut()
        {
            SparkVariableResult result = client.GetVariable("var0");
        }
        
    }
}
