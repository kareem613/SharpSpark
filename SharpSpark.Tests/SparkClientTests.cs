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
        const string TEST_FUNCTION = "returnOne";
        const string TEST_FUNCTION_VALUE = "1";
        const string TEST_VARIABLE = "var0";
        const string TEST_VARIABLE_VALUE = "0";

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

            Assert.AreEqual(TEST_FUNCTION, device.Functions.Single(), "Test function was not found. Ensure TestFirmware.cpp is flashed to the device.");
        }

        [TestMethod]
        public void GivenTestFirmwareClientInfoExpectVariables()
        {
            SparkDevice device = client.GetDevice();

            Assert.AreEqual(TEST_VARIABLE, device.Variables.Single().Key);
            Assert.AreEqual("int32", device.Variables.Single().Value);
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
            SparkFunctionResult result = client.ExecuteFunction(TEST_FUNCTION);

            Assert.AreEqual(TEST_FUNCTION_VALUE, result.Return_value);
        }

        [TestMethod]
        public void GivenClientWhenGetVar0Expect0()
        {
            SparkVariableResult result = client.GetVariable(TEST_VARIABLE);

            Assert.AreEqual(TEST_VARIABLE_VALUE, result.Result);
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
