using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TastyIO
{
    public class DirectoryUtils
    {
        #region Convenience

        public bool TryGetDirectories(string dir, out string[] dirs)
        {
            var notNull = IOUtility.TryGetDirectories(dir, out dirs);

            if (!notNull)
            {
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
            try
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
            catch (Exception ex)
            {
                IOLoger.LogErrorAsnyc(ex);
                throw;
            }
        }

        public List<string> GetDirectoriesRecursiveParallel(string dir)
        {
            try
            {
                if (dir == null)
                    throw new ArgumentNullException(string.Format("{0} cannot be null.", nameof(dir)));

                var results = new List<string>();
                var currentDirs = new List<string> { dir };

            loopStart:
                var nextDirs = new List<string>();
                Parallel.ForEach(currentDirs, (currentDir) =>
                {
                    if (TryGetDirectories(currentDir, out var dirs))
                    {
                        lock (results)
                        {
                            lock (nextDirs)
                            {
                                results.AddRange(dirs);
                                nextDirs.AddRange(dirs);
                            }
                        }
                    }
                });

                if (nextDirs.Count > 0)
                {
                    currentDirs = nextDirs;
                    goto loopStart;
                }

                return results;

            }
            catch (Exception ex)
            {
                IOLoger.LogErrorAsnyc(ex);
                throw;
            }
        }

        #endregion

        #region DirectoryManagement

        public static void CreateDirectory(string dir)
        {

        }

        public static async Task CreateDirectoryAsync(string dir)
        {

        }

        public static bool TryCreateDirectory(string dir)
        {

        }

        public static async Task<bool> TryCreateDirectoryAsync(string dir)
        {

        }


        #endregion
    }
}
