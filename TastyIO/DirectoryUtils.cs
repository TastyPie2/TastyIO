using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace TastyIO
{
    public static class DirectoryUtils
    {
        #region Convenience

        public static bool TryGetDirectories(string dir, out string[] dirs)
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

        public static IEnumerable<string> EnumerateDirectoriesRecursive(string dir)
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

        public static List<string> GetDirectoriesRecursive(string dir)
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

        public static List<string> GetDirectoriesRecursiveParallel(string dir)
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

        #region Creation
        public static void CreateDirectory(string dir)
        {
            try
            {
                Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                IOLoger.LogErrorAsnyc(ex);
                throw;
            }
        }

        public static void CreateDirectory(string dir, string directoryName)
        {
            CreateDirectory(Path.Combine(dir, directoryName));
        }

        public static async Task CreateDirectoryAsync(string dir)
        {
            try
            {
                Directory.CreateDirectory(dir);
            }
            catch (Exception ex)
            {
                IOLoger.LogErrorAsnyc(ex);
                throw;
            }
        }

        public static async Task CreateDirectoryAsync(string parentDir, string directoryName)
        {
            await CreateDirectoryAsync(Path.Combine(parentDir, directoryName));
        }

        public static bool TryCreateDirectory(string dir)
        {
            if(!IOUtility.Try(() => Directory.CreateDirectory(dir), out var ex))
            {
                IOLoger.LogWarningAsync(ex);
                return false;
            }
            return true;
        }

        public static bool TryCreateDirectory(string parentDir, string directoryName)
        {
            return TryCreateDirectory(Path.Combine(parentDir, directoryName));
        }

        public static async Task<bool> TryCreateDirectoryAsync(string dir)
        {
            return await Task.Run(() => TryCreateDirectory(dir));
        }

        public static async Task<bool> TryCreateDirectoryAsync(string parentDir, string directoryName)
        {
            return await TryCreateDirectoryAsync(Path.Combine(parentDir, directoryName));
        }

        public static void CreateDirectories(IEnumerable<string> dirs)
        {
            try
            {
                foreach(string dir in dirs)
                {
                    CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                IOLoger.LogErrorAsnyc(ex);
                throw;
            }
        }

        public static List<string> CreateDirectories(string parrentDirectory, IEnumerable<string> directoryNames)
        {
            List<string> dirs = new List<string>();
            foreach(var dir in directoryNames)
            {
                dirs.Add(Path.Combine(parrentDirectory, dir));
            }
            CreateDirectories(dirs);

            return dirs;
        }
        
        public static bool TryCreateDirectories(IEnumerable<string> dirs)
        {
            if(!IOUtility.Try(() => 
            { 
                foreach(var dir in dirs)
                {
                    Directory.CreateDirectory(dir);
                }
            }, out var ex))
            {
                IOLoger.LogWarningAsync(ex);
                return false;
            }

            return true;
        }

        public static async Task<bool> TryCreateDirectoriesAsync(IEnumerable<string> dirs)
        {
            return await Task.Run(() => TryCreateDirectories(dirs));
        }

        public static bool TryCreateDirectoriesParallel(IEnumerable<string> dirs)
        {
            if (!IOUtility.Try(() =>
            {
                Parallel.ForEach(dirs, dir =>
                {
                    Directory.CreateDirectory(dir);
                });
            }, out var ex))
            {
                IOLoger.LogWarningAsync(ex);
                return false;
            }

            return true;
        }
        #endregion
        #endregion

        #region Erasure
        public static void DeleteDirectory(string dir)
        {
            try
            {
                var dirs = GetDirectoriesRecursive(dir);
                var files = FileUtils.GetFilesRecursive(dir);

                foreach (var file in files)
                {
                    File.Delete(file);
                }
                foreach(var d in dirs)
                {
                    Directory.Delete(d);
                }
                
            }
            catch (Exception ex)
            {
                IOLoger.LogErrorAsnyc(ex);
                throw;
            }
        }

        public static void DeleteDirectory(string dir, string directoryName)
        {
            DeleteDirectory(Path.Combine(dir, directoryName));
        }

        public static async Task DeleteDirectoryAsync(string dir)
        {
            try
            {
                await Task.Run(() => DeleteDirectory(dir));
            }
            catch (Exception ex)
            {
                IOLoger.LogErrorAsnyc(ex);
                throw;
            }
        }

        public static async Task DeleteDirectoryAsync(string parentDir, string directoryName)
        {
            await DeleteDirectoryAsync(Path.Combine(parentDir, directoryName));
        }

        public static bool TryDeleteDirectory(string dir)
        {
            if (!IOUtility.Try(() => DeleteDirectory(dir), out var ex))
            {
                IOLoger.LogWarningAsync(ex);
                return false;
            }
            return true;
        }

        public static bool TryDeleteDirectory(string parentDir, string directoryName)
        {
            return TryDeleteDirectory(Path.Combine(parentDir, directoryName));
        }

        public static async Task<bool> TryDeleteDirectoryAsync(string dir)
        {
            return await Task.Run(() => TryDeleteDirectory(dir));
        }

        public static async Task<bool> TryDeleteDirectoryAsync(string parentDir, string directoryName)
        {
            return await TryDeleteDirectoryAsync(Path.Combine(parentDir, directoryName));
        }

        public static void DeleteDirectories(IEnumerable<string> dirs)
        {
            try
            {
                foreach (string dir in dirs)
                {
                    DeleteDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                IOLoger.LogErrorAsnyc(ex);
                throw;
            }
        }

        public static List<string> DeleteDirectories(string parrentDirectory, IEnumerable<string> directoryNames)
        {
            List<string> dirs = new List<string>();
            foreach (var dir in directoryNames)
            {
                dirs.Add(Path.Combine(parrentDirectory, dir));
            }
            DeleteDirectories(dirs);

            return dirs;
        }

        public static bool TryDeleteDirectories(IEnumerable<string> dirs)
        {
            if (!IOUtility.Try(() =>
            {
                foreach (var dir in dirs)
                {
                    DeleteDirectory(dir);
                }
            }, out var ex))
            {
                IOLoger.LogWarningAsync(ex);
                return false;
            }

            return true;
        }

        public static async Task<bool> TryDeleteDirectoriesAsync(IEnumerable<string> dirs)
        {
            return await Task.Run(() => TryDeleteDirectories(dirs));
        }

        public static bool TryDeleteDirectoriesParallel(IEnumerable<string> dirs)
        {
            if (!IOUtility.Try(() =>
            {
                Parallel.ForEach(dirs, dir =>
                {
                    DeleteDirectory(dir);
                });
            }, out var ex))
            {
                IOLoger.LogWarningAsync(ex);
                return false;
            }

            return true;
        }
        #endregion
    }
}
