using Maybe5.SharpSpark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSpark.Exceptions
{
    public class SparkDeviceException : SparkException
    {
        public SparkDeviceException(string message) : base(message) { }
        public SparkDeviceException(string message, SparkVariableResult result) : base(message) {
            this.VariableResult = result;
        }

        public SparkDeviceException(string message, SparkError error)
            : base(message, error)
        {
         
        }
        public SparkDeviceException(string message, Exception innerException) : base(message, innerException) { }

        public SparkVariableResult VariableResult { get; set; }

    }
}
