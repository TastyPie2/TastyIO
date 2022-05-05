using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TastyIO
{
    public class FileUtils
    {
        #region Convenience
        public bool TryGetFiles(string dir, out string[] files)
        {
            try
            {
                files = Directory.GetFiles(dir);
                return true;
            }
            catch (Exception ex)
            {
                IOLoger.LogWarningAsync(ex);
                files = null;
                return false;
            }
        }

        public bool TryGetDirectories(string dir, out string[] dirs)
        {
            try
            {
                dirs = Directory.GetDirectories(dir);
                return true;
            }
            catch (Exception ex)
            {
                IOLoger.LogWarningAsync(ex);
                dirs = null;
                return false;
            }
        }

        /// <summary>
        /// Including the leading dot.
        /// </summary>
        /// <returns></returns>
        public bool TryGetFileExtention(string filePath, out string fileExtention)
        {
            try
            {
                fileExtention = filePath.Split('.').LastOrDefault() ?? throw new Exception(string.Format("No file extention found on file {0}", filePath));
                return true;
            }
            catch (Exception ex)
            {
                IOLoger.LogWarningAsync(ex);
                fileExtention = null;
                return false;
            }
        }

        /// <summary>
        /// Returns filename without extention.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool TryGetFileName(string filePath, out string filename)
        { 
            try
            {
                filename = filePath.Split('\\').LastOrDefault() ?? throw new Exception(string.Format("{0} is directory and has not filename.", nameof(filePath)));
                return true;
            }
            catch (Exception ex)
            {
                IOLoger.LogWarningAsync(ex);
                filename = null;
                return false;
            }
        }
        #endregion

        #region RecursiveSearch
        //Not very DRY -_-
        public IEnumerable<string> EnumerateFilesRecursive(string dir)
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
                }
                if (TryGetFiles(currentDir, out var files))
                {
                    foreach (var file in files)
                        yield return file;
                }
            }

            currentDirs = nextDirs;
            if (nextDirs.Count > 0)
                goto loopStart;
        }

        //Not very DRY -_-
        public List<string> GetFilesRecursive(string dir)
        {
            if (dir == null)
                throw new ArgumentNullException(string.Format("{0} cannot be null.", nameof(dir)));

            var files = new List<string>();
            var currentDirs = new List<string>() { dir };

        loopStart:
            var nextDirs = new List<string>();
            foreach (var currentDir in currentDirs)
            {
                if(TryGetDirectories(currentDir, out var dirs))
                    nextDirs.AddRange(dirs);

                if(TryGetFiles(currentDir, out var fs))
                    files.AddRange(fs);
            }

            if (nextDirs.Count > 0)
            {
                currentDirs = nextDirs;
                goto loopStart;
            }

            return files;
        }

        //Not very DRY -_-
        public List<string> GetFilesRecursiveParallel(string dir)
        {
            try
            {
                if (dir == null)
                    throw new ArgumentNullException(string.Format("{0} cannot be null.", nameof(dir)));

                var files = new List<string>();
                var currentDirs = new List<string>() { dir };

            loopStart:
                var nextDirs = new List<string>();
                //Gota use them treads
                Parallel.ForEach(currentDirs, currentDir =>
                {
                    if (TryGetDirectories(currentDir, out var dirs))
                    {
                        nextDirs.AddRange(dirs);
                    }

                    if (TryGetFiles(currentDir, out var fs))
                    {
                        files.AddRange(fs);
                    }
                });

                if (nextDirs.Count > 0)
                {
                    currentDirs = nextDirs;
                    goto loopStart;
                }

                return files;
            }
            catch (Exception ex)
            {
                IOLoger.LogErrorAsnyc(ex);
                throw;
            }
        }
        #endregion

        #region Filter
        /// <summary>
        /// Extention including the leading dot.
        /// Multithreaded.
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public ICollection<string> FilerByExtention(ICollection<string> collection, string extention, bool ignoreCaseing = false)
        {
            if (ignoreCaseing)
                extention = extention.ToLower();

            var enumerator = collection.GetEnumerator();
            var result = (ICollection<string>)Activator.CreateInstance(collection.GetType());

            Parallel.For(0, collection.Count, i => 
            {
                if(enumerator.MoveNext())
                {
                    if(TryGetFileExtention(enumerator.Current, out var ext))
                    {

                        if (ignoreCaseing && ext.ToLower() == extention)
                            result.Add(enumerator.Current);
                        else if (ext == extention)
                            result.Add(enumerator.Current);
                    }
                }
            });

            return result;
        }

        public ICollection<string> FilterByFileName(ICollection<string> collection, string filename, bool ignoreCase = false)
        {
            if(ignoreCase)
                filename = filename.ToLower();

            var enu = collection.GetEnumerator();
            var result = (ICollection<string>)Activator.CreateInstance(collection.GetType());

            Parallel.For(0, collection.Count, i =>
            {
                if (enu.MoveNext())
                {
                    if(TryGetFileName(enu.Current, out var name))
                    {
                        if (ignoreCase && name.ToLower() == filename)
                            result.Add(enu.Current);
                        else if (filename == name)
                            result.Add(enu.Current);
                    }
                }
            });

            return result;
        }

        #endregion
    }
}