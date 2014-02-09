using System.Collections.Generic;

namespace Maybe5.SharpSpark
{
    public class SparkDevice
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string,string> Variables { get; set; }
        public List<string> Functions { get; set; }
    }
}
