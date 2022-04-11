using System.Text.RegularExpressions;
using TastyIO.OS;

namespace TastyIO
{
    /// <summary>
    /// 
    /// </summary>
    static public class TastyDir
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static bool VerifyPath(string dir)
        {
            string root = dir.Split("\\").First() + "\\";

            //Win32
            switch (Environment.OSVersion.Platform)
            {

                case PlatformID.Win32NT:
                    Regex rootVerification = new(@"[A-Z]:\\", RegexOptions.IgnoreCase);
                    string unRooted = Path.GetRelativePath(root, dir);

                    if (dir.EndsWith(' ') || dir.EndsWith('.'))
                        return false;

                    if (dir.Length >= Win.maxPathLenght)
                        return false;

                    if (!rootVerification.IsMatch(root))
                    {
                        return false;
                    }

                    foreach (char character in unRooted.ToUpper().ToCharArray())
                    {
                        if (Win.forbiddenChars.Contains(character))
                            return false;
                    }

                    foreach (string part in dir.Split("\\"))
                    {
                        if (Win.reservedNames.Contains(part.ToUpper()))
                            return false;
                    }
                    break;

                case PlatformID.Unix:
                    //Linux and macos
                    if (dir.Length >= Unix.maxPathLenght)
                    {
                        return false;
                    }
                    if (dir.Split("\\").Last().Length >= Unix.maxFilenameLenght)
                    {
                        return false;
                    }
                    break;


                case PlatformID.Other:
                    //Unkown
                    throw new PlatformNotSupportedException($"{Environment.OSVersion} is not supported.");
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string[] GetDirecoriesRecursive(string dir)
        {
            return GetDirecories(dir);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirs"></param>
        /// <returns></returns>
        public static string[] GetDirecoriesRecursive(params string[] dirs)
        {
            List<string> result = new();
            foreach(string dir in dirs)
            {
                result.AddRange(GetDirecories(dir));
            }

            return result.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="destination"></param>
        /// <param name="overide"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string CopyDir(string target, string destination, bool overide)
        {
            string subDest = Path.Combine(destination, target.Split("\\").Last());

            foreach (string file in TastyFile.GetFilesRecursive(target))
            {
                FileInfo fileInfo = new(Path.Combine(subDest, Path.GetRelativePath(target, file)));
                Directory.CreateDirectory(fileInfo.Directory?.Name ?? throw new Exception($"file: {file} is not rooted."));
                File.Copy(file, fileInfo.FullName, overide);
            }

            return subDest;
        }

        /// <summary>
        /// Moves the dir and all of its contents
        /// </summary>
        public static string MoveDir(string target, string destination, bool overide)
        {
            string subDest = Path.Combine(destination, target.Split("\\").Last());

            foreach (string file in TastyFile.GetFilesRecursive(target))
            {
                FileInfo fileInfo = new(Path.Combine(subDest, Path.GetRelativePath(target, file)));
                Directory.CreateDirectory(fileInfo.Directory?.Name ?? throw new Exception($"file: {file} is not rooted."));
                File.Move(file, fileInfo.FullName, overide);
            }

            return subDest;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public static void DeleteDir(string target)
        {
            foreach (string file in TastyFile.GetFilesRecursive(target))
            {
                File.Delete(file);
            }
            Directory.Delete(target, true);
        }

        private static string[] GetDirecories(string parrentDir)
        {
            List<string> result = new();
            try
            {
                string[] dirs = Directory.GetDirectories(parrentDir);
                foreach (string dir in dirs)
                {
                    result.AddRange(GetDirecories(dir));
                }
            }
            catch
            {

            }
            return result.ToArray();
        }

    }
}