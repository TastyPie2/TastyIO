using System;
using System.Collections.Generic;
using System.Text;

namespace TastyIO
{
    public class DirectoryUtils
    {
        #region Events

        public event WarningDelegate OnWarning;
        public event ErrorDelegate OnError;

        #endregion

        #region Convenience

        private bool SendWarning(Exception ex)
        {
            if (OnWarning == null)
                return false;

            var msg = ex.Message;
            OnWarning.Invoke(DateTime.Now, ex, msg);
            return true;
        }

        private bool SendError(Exception ex)
        {
            if (OnError == null)
                return false;

            var msg = ex.Message;
            OnError.Invoke(DateTime.Now, ex, msg);
            return true;
        }

        public bool TryGetDirectories(string dir, out string[] dirs)
        {
            var notNull = IOUtility.TryGetDirectories(dir, out dirs, out var ex);

            if (!notNull)
            {
                SendWarning(ex);
                return false;
            }

            return true;
        }
        #endregion

        #region RecursiveSearch

        public IEnumerable<string> EnumerateDirectoriesRecursive(string dir)
        {
            if (dir == null)
                throw new ArgumentNullException(string.Format("{0} cannot be null.", nameof(dir)));

            //Optimize for memory
            List<string> currentDirs = new List<string>() { dir };
        loopStart:

            List<string> nextDirs = new List<string>();
            foreach (var currentDir in currentDirs)
            {
                if (TryGetDirectories(currentDir, out var dirs))
                {
                    nextDirs.AddRange(dirs);

                    foreach(var d in dirs)
                        yield return d;
                }
            }

            currentDirs = nextDirs;
            if (nextDirs.Count > 0)
                goto loopStart;
        }

        public List<string> GetDirectoriesRecursive(string dir)
        {
            if (dir == null)
                throw new ArgumentNullException(string.Format("{0} cannot be null.", nameof(dir)));

            var results = new List<string>();
            var currentDirs = new List<string> { dir };

            loopStart:
            var nextDirs = new List<string>();
            foreach(var currentDir in currentDirs)
            {
                if(TryGetDirectories(currentDir, out var dirs))
                {
                    results.AddRange(dirs);
                    nextDirs.AddRange(dirs);
                }
            }

            if(nextDirs.Count > 0)
            {
                currentDirs = nextDirs;
                goto loopStart;
            }

            return results;
        }

        public List<string> GetDirectoriesRecursiveParallel(string dir)
        {
            try
            {
                if (dir == null)
                throw new ArgumentNullException(string.Format("{0} cannot be null.", nameof(dir)));

                var results = new List<string>();
                var currentDirs = new List<string>() { dir };

                loopStart:
            }
            catch (Exception ex)
            {
                SendError(ex);
                throw;
            }
        }
        #endregion
    }
}
