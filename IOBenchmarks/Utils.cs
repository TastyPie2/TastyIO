using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOBenchmarks
{
    internal sealed class Utils
    {
        public static string CondenceNumber(long num)
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

        public static void IOLoger_OnError(DateTime localTime, Exception ex, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[ERROR] {0} {1} {2}", localTime, message, ex.Message);
            Console.ForegroundColor = Program.FOREGROND_COLOR;
        }

        public static void IOLoger_OnWarning(DateTime localTime, Exception ex, string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[WARN] {0} {1} {2}", localTime, message, ex.Message);
            Console.ForegroundColor = Program.FOREGROND_COLOR;
        }
    }
}
