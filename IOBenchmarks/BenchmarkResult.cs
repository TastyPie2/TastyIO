namespace IOBenchmarks.Benchmarks
{
    public class BenchmarkResult
    {
        public bool IsSuccess;

        public List<Exception> Warnings { get; set; }

        public List<Exception> Errors { get; set; }

        public string Message { get; set; }

        public void AddWarning(Exception ex)
        {
            Warnings.Add(ex);
        }

        public void AddError(Exception ex)
        {
            Errors.Add(ex);
        }

        public void SetMessage(string v)
        {
            Message = v;
        }

        public BenchmarkResult()
        {
            Warnings = new List<Exception>();
            Errors = new List<Exception>();
            Message = string.Empty;
        }

        public bool Validate(out Exception? reason, out Type? exceptionType)
        {
            if (Message == null)
            {
                reason = new NullReferenceException($"{nameof(Message)} cannot be null.");
                exceptionType = typeof(NullReferenceException);
                return false;
            }
            if (Warnings == null)
            {
                reason = new NullReferenceException($"{nameof(Warnings)} cannot be null.");
                exceptionType = typeof(NullReferenceException);
                return false;
            }
            if (Errors == null)
            {
                reason = new NullReferenceException($"{nameof(Errors)} cannot be null.");
                exceptionType = typeof(NullReferenceException);
                return false;
            }

            reason = null;
            exceptionType = null;
            return true;
        }
    }
}