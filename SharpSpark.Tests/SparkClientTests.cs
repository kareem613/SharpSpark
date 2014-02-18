using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maybe5.SharpSpark;
using System.Configuration;
using System.Linq;

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
        public void GivenInvalidFunctionExpectErrorFunctionNotFound()
        {
            SparkFunctionResult result = client.ExecuteFunction("thisdoesnotexist");

            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("Function not found", result.ErrorResult.Error);
        }

        [TestMethod]
        public void GivenInvalidAccessTokenExpectErrorInvalidGrant()
        {
            var badClient = new SparkClient("badtoken", "");
            SparkFunctionResult result = badClient.ExecuteFunction("doesn't matter");

            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("invalid_grant", result.ErrorResult.Error);
        }

        [TestMethod]
        public void GivenInvalidDeviceIdExpectErrorPermissionDenied()
        {
            var badClient = new SparkClient(client.AccessToken, "baseid");
            SparkFunctionResult result = badClient.ExecuteFunction("doesn't matter");

            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("Permission Denied", result.ErrorResult.Error);
        }

        [Ignore]//This test only passes if the device is offline
        [TestMethod]
        public void GivenOfflineDeviceFunctionExpectErrorTimedOut()
        {
            SparkFunctionResult result = client.ExecuteFunction("digitalRead");

            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("Timed out.", result.ErrorResult.Error);
        }
        
    }
}
