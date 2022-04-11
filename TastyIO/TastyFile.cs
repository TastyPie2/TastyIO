using System.Security.Cryptography;
using System.Text;
using TastyIO.Utils;

namespace TastyIO
{
    /// <summary>
    /// 
    /// </summary>
    public static class TastyFile
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="algorithmName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string HashFile(string filePath, HashAlgorithmName algorithmName)
        {
            //Abstracted to different class
            string result = Hashing.HashFile(filePath, algorithmName);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        public static bool CompareFiles(string file1, string file2)
        {
            string hash1 = HashFile(file1, HashAlgorithmName.MD5);
            string hash2 = HashFile(file2, HashAlgorithmName.MD5);

            return hash1 == hash2;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool VerifyPath(string path)
        {
            return TastyDir.VerifyPath(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string[] GetFilesRecursive(string dir)
        {
            List<string> result = new();

            result.AddRange(Directory.GetFiles(dir));

            foreach (string di in TastyDir.GetDirecoriesRecursive(dir))
            {
                result.AddRange(Directory.GetFiles(di));
            }

            return result.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extentionFilter"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string[] GetFilesRecursive(string[] extentionFilter, string dir)
        {
            List<string> result = new();

            foreach (string di in TastyDir.GetDirecoriesRecursive(dir))
            {
                result.AddRange(Directory.GetFiles(di));
            }

            result = result.Where(s => extentionFilter.Contains(new FileInfo(s).Extension)).ToList();
            return result.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirs"></param>
        /// <returns></returns>
        public static string[] GetFilesRecursive(params string[] dirs)
        {
            List<string> result = new();

            foreach (string dir in dirs)
            {
                foreach (string di in TastyDir.GetDirecoriesRecursive(dir))
                {
                    result.AddRange(Directory.GetFiles(di));
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="extentionFilter"></param>
        /// <param name="dirs"></param>
        /// <returns></returns>
        public static string[] GetFilesRecursive(string[] extentionFilter, params string[] dirs)
        {
            return GetFilesRecursive(dirs).Where(s => extentionFilter.Contains(new FileInfo(s).Extension)).ToArray();
        }
    }
}
