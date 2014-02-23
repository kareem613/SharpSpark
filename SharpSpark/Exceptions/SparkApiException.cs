using Maybe5.SharpSpark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSpark.Exceptions
{
    public class SparkApiException: SparkException
    {
        public SparkApiException(string message) : base(message) { }

        public SparkApiException(string message, SparkError error)
            : base(message, error)
        {
        }
        public SparkApiException(string message, Exception innerException) : base(message, innerException) { }
    }
}
