
namespace Maybe5.SharpSpark
{
    public class SparkResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Last_app { get; set; }
        public bool Connected { get; set; }
        public string Return_value { get; set; }
        public SparkErrorResult ErrorResult { get; set; }
        public bool HasErrors
        {
            get
            {
                return ErrorResult != null;
            }
        }

        
    }
}
