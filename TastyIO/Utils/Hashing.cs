using System.Security.Cryptography;
using System.Text;

namespace TastyIO.Utils
{
    /// <summary>
    /// Contains methods to simplify hashing
    /// </summary>
    public static class Hashing
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="algorithmName"></param>
        /// <returns></returns>
        public static string HashString(string data, HashAlgorithmName algorithmName)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            return Encoding.UTF8.GetString(HashByte(byteData, algorithmName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="algorithmName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static byte[] HashByte(byte[] data, HashAlgorithmName algorithmName)
        {
            byte[] hash;

            switch (algorithmName.Name)
            {
                case "MD5":
                    MD5 md5 = MD5.Create();
                    hash = md5.ComputeHash(data);
                    md5.Dispose();
                    break;

                case "SHA1":
                    SHA1 sha1 = SHA1.Create();
                    hash = sha1.ComputeHash(data);
                    sha1.Dispose();
                    break;

                case "SHA256":
                    SHA256 sha256 = SHA256.Create();
                    hash = sha256.ComputeHash(data);
                    sha256.Dispose();
                    break;

                case "SHA384":
                    SHA384 sha384 = SHA384.Create();
                    hash = sha384.ComputeHash(data);
                    sha384.Dispose();
                    break;

                case "SHA512":
                    SHA512 sha512 = SHA512.Create();
                    hash = sha512.ComputeHash(data);
                    sha512.Dispose();
                    break;

                default:
                    throw new ArgumentException("Unkown algorithm");
            }

            return hash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="algorithmName"></param>
        /// <returns></returns>
        public static string HashFile(string filePath, HashAlgorithmName algorithmName)
        {
            using StreamReader reader = new StreamReader(filePath);
            return HashStream(reader.BaseStream, algorithmName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="algorithmName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string HashStream(Stream stream, HashAlgorithmName algorithmName)
        {
            byte[] hash;

            switch (algorithmName.Name)
            {
                case "MD5":
                    MD5 md5 = MD5.Create();
                    hash = md5.ComputeHash(stream);
                    md5.Dispose();
                    break;

                case "SHA1":
                    SHA1 sha1 = SHA1.Create();
                    hash = sha1.ComputeHash(stream);
                    sha1.Dispose();
                    break;

                case "SHA256":
                    SHA256 sha256 = SHA256.Create();
                    hash = sha256.ComputeHash(stream);
                    sha256.Dispose();
                    break;

                case "SHA384":
                    SHA384 sha384 = SHA384.Create();
                    hash = sha384.ComputeHash(stream);
                    sha384.Dispose();
                    break;

                case "SHA512":
                    SHA512 sha512 = SHA512.Create();
                    hash = sha512.ComputeHash(stream);
                    sha512.Dispose();
                    break;

                default:
                    throw new ArgumentException("Unkown algorithm");
            }

            return Encoding.UTF8.GetString(hash);
        }
    }
}
