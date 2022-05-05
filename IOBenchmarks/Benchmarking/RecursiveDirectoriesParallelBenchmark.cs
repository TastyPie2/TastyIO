using System.Diagnostics;
using System.Reflection;

namespace IOBenchmarks.Benchmarks
{
    [Benchmark]
    internal class RecursiveDirectoriesParallelBenchmark : IBenchmark
    {
        public RecursiveDirectoriesParallelBenchmark()
        {

        }

        public BenchmarkResult Benchmark()
        {
            IOLoger.OnWarning += Utils.IOLoger_OnWarning;
            IOLoger.OnError += Utils.IOLoger_OnError;

            var results = new BenchmarkResult();
            results.Name = "RecursiveDirectoriesParallel";
            results.IsSuccess = true;
            try
            {
                var directoryUtils = new DirectoryUtils();
                var sw = Stopwatch.StartNew();


                var dirs = directoryUtils.GetDirectoriesRecursiveParallel("C:\\");

                sw.Stop();
                results.SetMessage(ResultsFormatter.FormattDirectoryResults(dirs.Count, (int)sw.Elapsed.TotalSeconds));

                return results;
            }
            catch (Exception ex)
            {
                results.IsSuccess = false;
                IOLoger.LogErrorAsnyc(ex);
                throw;
            }
            finally
            {
                IOLoger.OnWarning -= Utils.IOLoger_OnWarning;
                IOLoger.OnError -= Utils.IOLoger_OnError;
            }
        }
    }
}
