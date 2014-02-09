using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maybe5.SharpSpark;
using System.Configuration;

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
        public void GivenClientInfoExpectDeviceInfo()
        {
            SparkDevice device = client.GetDevice();

            Assert.IsNotNull(device);
            Assert.IsNotNull(device.Name);
            Assert.AreEqual(client.DeviceId, device.Id);
        }

        [TestMethod]
        public void GivenInvalidFunctionExpectErrorFunctionNotFound()
        {
            SparkResult result = client.ExecuteFunction("thisdoesnotexist");

            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("Function not found", result.ErrorResult.Error);
        }

        [TestMethod]
        public void GivenInvalidAccessTokenExpectErrorInvalidGrant()
        {
            var badClient = new SparkClient("badtoken", "");
            SparkResult result = badClient.ExecuteFunction("thisdoesnotexist");

            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("invalid_grant", result.ErrorResult.Error);
        }

        [Ignore]//This test only passes if the device is offline
        [TestMethod]
        public void GivenOfflineDeviceFunctionExpectErrorTimedOut()
        {
            SparkResult result = client.ExecuteFunction("digitalRead");

            Assert.IsTrue(result.HasErrors);
            Assert.AreEqual("Timed out.", result.ErrorResult.Error);
        }
        
    }
}
