namespace IOBenchmarks.Benchmarks
{
    public class BenchmarkResult
    {
        public bool IsSuccess { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }


        public void SetMessage(string v)
        {
            Message = v;
        }

        public BenchmarkResult()
        {
            Message = string.Empty;
            Name = string.Empty;
        }

        public bool Validate(out Exception? reason, out Type? exceptionType)
        {
            if (Message == null)
            {
                reason = new NullReferenceException($"{nameof(Message)} cannot be null.");
                exceptionType = typeof(NullReferenceException);
                return false;
            }

            reason = null;
            exceptionType = null;
            return true;
        }
    }
}