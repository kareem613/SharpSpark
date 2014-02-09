
namespace Maybe5.SharpSpark
{
    public class SparkFunctionResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Last_app { get; set; }
        public bool Connected { get; set; }
        public string Return_value { get; set; }
        public SparkError ErrorResult { get; set; }
        public bool HasErrors
        {
            get
            {
                return ErrorResult != null;
            }
        }

        
    }
}
