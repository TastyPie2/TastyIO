using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TastyIO
{
    public static class IOUtility
    {

        #region Get
        public static bool TryGet<T>(Func<T> func, out T result, out Exception exception)
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

        public static bool TryGetDirectories(string dir, out string[] dirs)
        {
            bool result = TryGet(() => Directory.GetDirectories(dir), out dirs, out var ex);
            if(!result)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                IOLoger.LogWarningAsync(ex);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            }

            return result;
        }

        public static bool TryGetFiles(string dir, out string[] dirs)
        {
            bool result = TryGet(() => Directory.GetFiles(dir), out dirs, out var ex);
            if (!result)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                IOLoger.LogWarningAsync(ex);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            }

            return result;
        }

        public static bool TryOpenRead(string path, out StreamReader reader)
        {
            bool result = TryGet(() => new StreamReader(path), out reader, out var ex);
            if (!result)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                IOLoger.LogWarningAsync(ex);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            }

            return result;
        }

        public static bool TryOpenWrite(string path, out StreamWriter writer)
        {
            bool result = TryGet(() => new StreamWriter(path), out writer, out var ex);
            if (!result)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                IOLoger.LogWarningAsync(ex);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            }

            return result;
        }
        #endregion

        #region Try
        public static bool Try(Action func, out Exception exception)
        {
            try
            {
                func.Invoke();
                exception = null;
                return true;
            }
            catch (Exception ex)
            {
                exception = ex;
                return false;
            }
        }
        #endregion
    }
}
