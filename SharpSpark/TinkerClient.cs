using System;

namespace Maybe5.SharpSpark
{
    
    public class TinkerClient : SparkClient
    {
        const int DIGITAL_HIGH_RETURN_VALUE = 1;

        public enum DigitalPins { D0, D1, D2, D3, D4, D5, D6, D7 };
        public enum AnalogPins { A0, A1, A2, A3, A4, A5, A6, A7 };
        public enum DigitalValue { Low, High};

        public TinkerClient(string accessToken, string deviceId) : base(accessToken, deviceId) { }

        public DigitalValue DigitalRead(DigitalPins pin)
        {
            
            var returnValue = ExecuteFunctionReturnValue("digitalread", pin.ToString());

            if (returnValue == SparkClient.ERROR_RETURN_VALUE)
            {
                throw new Exception("Failed to read value.");
            }
            return returnValue == DIGITAL_HIGH_RETURN_VALUE ? DigitalValue.High : DigitalValue.Low;
        }

        public int DigitalWrite(DigitalPins pin, DigitalValue pinValue)
        {
            var returnValue = ExecuteFunctionReturnValue("digitalwrite", pin.ToString(), pinValue.ToString().ToUpper());
            return returnValue;
        }

        public int AnalogRead(AnalogPins pin)
        {
            return ExecuteFunctionReturnValue("analogread", pin.ToString());
        }

        public int AnalogWrite(AnalogPins pin, int pinValue)
        {
            return ExecuteFunctionReturnValue("analogwrite", pin.ToString(), pinValue.ToString());
        }
    }
}
