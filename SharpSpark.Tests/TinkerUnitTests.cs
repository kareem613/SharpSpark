using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Maybe5.SharpSpark;
using System.Configuration;


namespace Maybe5.SharpSpark.Tests
{
    [TestClass]
    public class TinkerUnitTests
    {
        TinkerClient client;

        [TestInitialize]
        public void Setup()
        {
            var accessToken = ConfigurationManager.AppSettings["accessToken"];
            var deviceId = ConfigurationManager.AppSettings["deviceId"];
            client = new TinkerClient(accessToken, deviceId);
        }

        [TestMethod]
        public void GivenTinkerAppFirmwareClientInfoExpectFunctions()
        {
            SparkDevice device = client.GetDevice();

            Assert.IsTrue(device.Functions.Contains("digitalread"), "digitalread function was not found. Ensure Tinker firmware is flashed to the device.");
            Assert.IsTrue(device.Functions.Contains("digitalwrite"), "digitalwrite function was not found. Ensure Tinker firmware is flashed to the device.");
            Assert.IsTrue(device.Functions.Contains("analogread"), "analogread function was not found. Ensure Tinker firmware is flashed to the device.");
            Assert.IsTrue(device.Functions.Contains("analogwrite"), "analogwrite function was not found. Ensure Tinker firmware is flashed to the device.");
        }

        [TestMethod]
        public void GivenLowPin7WhenDigitalReadExpectPinoutLow()
        {
            var pinValue = client.DigitalRead(TinkerClient.DigitalPins.D7);

            Assert.AreEqual(TinkerClient.DigitalValue.Low, pinValue);
        }

        [TestMethod]
        public void GivenLowPin7WhenDigitalWriteHighExpectValidReturnValue()
        {
            client.DigitalWrite(TinkerClient.DigitalPins.D7, TinkerClient.DigitalValue.High);
            
            var pinValue = client.DigitalRead(TinkerClient.DigitalPins.D7);

            Assert.AreNotEqual(TinkerClient.ERROR_RETURN_VALUE, pinValue);
        }

        [TestMethod]
        public void GivenLowA5WhenAnalogReadExpectValidReturnValue()
        {
            
            var pinValue = client.AnalogRead(TinkerClient.AnalogPins.A5);

            Assert.AreNotEqual(TinkerClient.ERROR_RETURN_VALUE, pinValue);
        }

        [TestMethod]
        public void GivenA5WhenAnalogWrite255ExpectValidReturnValue()
        {
            var pinValue = client.AnalogRead(TinkerClient.AnalogPins.A5);

            Assert.AreNotEqual(TinkerClient.ERROR_RETURN_VALUE, pinValue);
        }
    }
}
