using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maybe5.SharpSpark
{
    public class SparkVariableResult
    {
        public string Cmd { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
        public string Error { get; set; }
        public CoreInfo CoreInfo { get; set; }

        public bool HasError
        {
            get
            {
                return Error != null || ErrorResult != null;
            }
        }

        public SparkError ErrorResult { get; set; }
    }
}
