using Maybe5.SharpSpark;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSpark.Exceptions
{
    public class SparkException : Exception
    {
        public SparkException(string message) : base(message) { }
        public SparkException(string message, Exception innerException) : base(message, innerException) { }

        public SparkException(string message, SparkError error)
            : base(message)
        {
            this.SparkError = error;
        }

        public SparkError SparkError { get; set; }
    }
}
