using System.Diagnostics;
using System.Reflection;

namespace IOBenchmarks.Benchmarks
{
    [Benchmark]
    public class RecursiveDirectoriesBenchmark : IBenchmark
    {
        BenchmarkResult results;

        public RecursiveDirectoriesBenchmark()
        {

        }

        public BenchmarkResult Benchmark()
        {

            Console.ForegroundColor = Program.FOREGROND_COLOR;
            Console.BackgroundColor = Program.BACKGROUND_COLOR;

            results = new();

            DirectoryUtils directoryUtils = new DirectoryUtils();

            try
            {
                directoryUtils.OnError += DirectoryUtils_OnError;
                directoryUtils.OnWarning += DirectoryUtils_OnWarning;

                Stopwatch sw = Stopwatch.StartNew();

                //NOT LONG ENOUGH
                var dirs = directoryUtils.GetDirectoriesRecursiveParallel("C:\\");

                sw.Stop();
                results.SetMessage(ResultsFormatter(dirs.Count, (int)sw.Elapsed.TotalSeconds));

                if (results.Validate(out var exception, out _))
                {
                    return results;
                }
                else
                {
                    if (exception != null)
                        throw exception;
                    else
                        throw new Exception("Validation failed and exception null?????????????????\nThis should NEVER happen.");
                }
            }
            catch (Exception ex)
            {
                DirectoryUtils_OnError(DateTime.Now, ex, ex.Message);
                throw;
            }
            finally
            {
                directoryUtils.OnError -= DirectoryUtils_OnError;
                directoryUtils.OnWarning -= DirectoryUtils_OnWarning;
            }
        }

        private static string ResultsFormatter(long dirCount, int totalSecs)
        {
            var condensedNumStr = NumberCondenser(dirCount);
            return String.Format("Searched: {0} directories in {1}s", condensedNumStr, totalSecs);
        }

        private static string NumberCondenser(long num)
        {
            const long thousen = 0x3E8;
            const long million = 0xF4240;
            const long billion = 0x3B9ACA00;

            var result = "";

            //Ifchain
            if (num / thousen < 1)
            {
                result = String.Format("{0}", num);
            }
            else if (num / thousen <= thousen * thousen)
            {
                result = String.Format("{0}K", num / thousen);
            }
            else if (num / million <= million * thousen)
            {
                result = String.Format("{0}M", num / million);
            }
            else
            {
                result = String.Format("{0}G", num / billion);
            }

            return result;
        }

        private void DirectoryUtils_OnWarning(DateTime localTime, Exception ex, string message)
        {
#if DEBUG
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("{0} {1} {2}", localTime, ex.GetType(), message);
            Console.ForegroundColor = Program.BACKGROUND_COLOR;
#endif
            results.AddWarning(ex);
        }

        private void DirectoryUtils_OnError(DateTime localTime, Exception ex, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0} {1} {2}", localTime, ex.GetType(), message);
            Console.ForegroundColor = Program.FOREGROND_COLOR;

            results.AddError(ex);
        }
    }
}
