using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TastyIO
{
    public static class IOLoger
    {
        public delegate void WarningDelegate(DateTime localTime, Exception ex, string message);

        public delegate void ErrorDelegate(DateTime localTime, Exception ex, string message);

        public static event WarningDelegate OnWarning;

        public static event ErrorDelegate OnError;

        public static Stream OutputStream;

        #region Loging

        public static void CreateOutputFile()
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, "IOLog.txt");
            IOUtility.TryOpenWrite(filePath, out var writer);

            if (writer == null)
                LogErrorAsnyc(new Exception("Unable to create log file.")).GetAwaiter();
            else
                OutputStream = writer.BaseStream;
        }

        public static void SetOutputStream(Stream stream)
        {
            lock (OutputStream)
            {
                OutputStream.Close();
                OutputStream.Dispose();
                OutputStream = stream;
            }
        }

        public static void SetOutputFile(string path)
        {
            IOUtility.TryOpenWrite(path, out var writer);

            if (writer == null)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                LogErrorAsnyc(new Exception("Unable to create log file."));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            else
                OutputStream = writer.BaseStream;
        }

        public static void SetOutputFile(string dir, string filename)
        {
            SetOutputFile(Path.Combine(dir, filename));
        }

        #region LogType

        internal enum LogType
        {
            Information,
            Debug,
            Warning,
            Error,
        }

        private static string LogtypeToString(LogType logType)
        {
            switch (logType)
            {
                case LogType.Information:
                    return "[Information]";

                case LogType.Debug:
                    return "[Debug]";

                case LogType.Warning:
                    return "[Warn]";

                case LogType.Error:
                    return "[Error]";

                default:
                    return "[Unknown]";
            }
        }

        #endregion LogType

        #region Formatting

        private static string FormatException(Exception ex, LogType logType)
        {
            string prefix = LogtypeToString(logType);
            return String.Format("{0} {1} {2} {3}\n", DateTime.Now, prefix, ex.GetType().Name, ex.Message, ex);
        }

        private static string FormatMessage(string msg, LogType logType)
        {
            string prefix = LogtypeToString(logType);
            return String.Format("{0} {1} {2}\n", DateTime.Now, prefix, msg);
        }

        #endregion Formatting

        #region PrivateLoging

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        private static async Task LogExceptionAsync(Exception ex, LogType logType)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (OutputStream != null)
            {
                lock (OutputStream)
                {
                    byte[] streamMsg = Encoding.UTF8.GetBytes(FormatException(ex, logType));
                    OutputStream.WriteAsync(streamMsg, 0, streamMsg.Length).GetAwaiter();
                }
            }
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        private static async Task LogMessage(string msg, LogType logType)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (OutputStream != null)
            {
                lock (OutputStream)
                {
                    byte[] streamMsg = Encoding.UTF8.GetBytes(FormatMessage(msg, logType));
                    OutputStream.WriteAsync(streamMsg, 0, streamMsg.Length).GetAwaiter();
                }
            }
        }

        #endregion PrivateLoging

        #region PublicLoging

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public static async Task<bool> LogWarningAsync(Exception ex)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            LogExceptionAsync(ex, LogType.Warning);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

            if (OnWarning == null)
                return false;

            var msg = ex.Message;
            OnWarning.Invoke(DateTime.Now, ex, msg);
            return true;
        }

#pragma warning disable CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        public static async Task<bool> LogErrorAsnyc(Exception ex)
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            LogExceptionAsync(ex, LogType.Error);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.

            if (OnError == null)
                return false;

            var msg = ex.Message;
            OnError.Invoke(DateTime.Now, ex, msg);
            return true;
        }

        public static async Task LogInformation(string message)
        {
            await LogMessage(message, LogType.Information);
        }

        public static async Task LogDebug(string message)
        {
            await LogMessage(message, LogType.Debug);
        }

        #endregion PublicLoging

        #endregion Loging
    }
}