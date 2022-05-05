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
                LogErrorAsnyc(new Exception("Unable to create log file."));
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
                LogErrorAsnyc(new Exception("Unable to create log file."));
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

        private static async Task LogExceptionAsync(Exception ex, LogType logType)
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

        private static async Task LogMessage(string msg, LogType logType)
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

        public static async Task<bool> LogWarningAsync(Exception ex)
        {
            LogExceptionAsync(ex, LogType.Warning);

            if (OnWarning == null)
                return false;

            var msg = ex.Message;
            OnWarning.Invoke(DateTime.Now, ex, msg);
            return true;
        }

        public static async Task<bool> LogErrorAsnyc(Exception ex)
        {
            LogExceptionAsync(ex, LogType.Error);

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