using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOBenchmarks
{
    internal static class ResultsFormatter
    {
        public static string FormattDirectoryResults(long dirCount, int totalSecs)
        {
            var condensedNumStr = Utils.CondenceNumber(dirCount);
            return String.Format("Found: {0} directories in {1}s", condensedNumStr, totalSecs);
        }
    }
}
