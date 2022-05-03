using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TastyIO
{
    public delegate void WarningDelegate(DateTime localTime, Exception ex, string message);
    public delegate void ErrorDelegate(DateTime localTime, Exception ex, string message);

    internal static class IOUtility
    {
        public static bool TryGet<T>(out T result, out Exception exception, Func<T> func)
        {
            try
            {
                result = func.Invoke();
                exception = null;
                return true;
            }
            catch (Exception ex)
            {
                result = default(T);
                exception = ex;
                return false;
            }
        }

        public static bool TryGetDirectories(string dir, out string[] dirs, out Exception ex)
        {
            try
            {
                ex = null;
                dirs = Directory.GetDirectories(dir);
                return true;
            }
            catch (Exception e)
            {
                dirs = null;
                ex = e;
                return false;
            }
        }
    }
}
