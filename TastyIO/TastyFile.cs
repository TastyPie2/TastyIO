using System.Security.Cryptography;
using System.Text;

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
        /// <param name="path"></param>
        /// <param name="algorithmName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string Hash(string path, HashAlgorithmName algorithmName)
        {
            using Stream stream = File.OpenText(path).BaseStream;
            byte[] hash;
            string result;

            switch (algorithmName.Name)
            {
                case "MD5":
                    MD5 md5 = MD5.Create();
                    hash = md5.ComputeHash(stream);
                    result = Encoding.UTF8.GetString(hash);
                    md5.Dispose();
                    break;

                case "SHA1":
                    SHA1 sha1 = SHA1.Create();
                    hash = sha1.ComputeHash(stream);
                    result = Encoding.UTF8.GetString(hash);
                    sha1.Dispose();
                    break;

                case "SHA256":
                    SHA256 sha256 = SHA256.Create();
                    hash = sha256.ComputeHash(stream);
                    result = Encoding.UTF8.GetString(hash);
                    sha256.Dispose();
                    break;

                case "SHA384":
                    SHA384 sha384 = SHA384.Create();
                    hash = sha384.ComputeHash(stream);
                    result = Encoding.UTF8.GetString(hash);
                    sha384.Dispose();
                    break;

                case "SHA512":
                    SHA512 sha512 = SHA512.Create();
                    hash = sha512.ComputeHash(stream);
                    result = Encoding.UTF8.GetString(hash);
                    sha512.Dispose();
                    break;

                default:
                    throw new ArgumentException("Unkown algorithm");
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file1"></param>
        /// <param name="file2"></param>
        /// <returns></returns>
        public static bool Compare(string file1, string file2)
        {
            string hash1 = Hash(file1, HashAlgorithmName.MD5);
            string hash2 = Hash(file2, HashAlgorithmName.MD5);

            return hash1 == hash2;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Verify(string path)
        {
            return TastyDir.Verify(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string[] GetFilesRecursive(string dir)
        {
            List<string> result = new();

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
