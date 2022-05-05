using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOBenchmarks.Benchmarks;

namespace IOBenchmarks.Modules
{
    [Benchmark]
    internal class RecursiveDirectoriesBenchmark : IBenchmark
    {
        public RecursiveDirectoriesBenchmark()
        {

        }

        public BenchmarkResult Benchmark()
        {
            IOLoger.OnWarning += Utils.IOLoger_OnWarning;
            IOLoger.OnError += Utils.IOLoger_OnError;

            var results = new BenchmarkResult();
            results.Name = "RecursiveDirectories";
            results.IsSuccess = true;
            try
            {
                var sw = Stopwatch.StartNew();


                var dirs = DirectoryUtils.GetDirectoriesRecursive("C:\\");

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
