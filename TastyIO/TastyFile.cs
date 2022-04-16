using System.Security.Cryptography;
using Newtonsoft.Json;
using TastyIO.Utils;

namespace TastyIO
{
    /// <summary>
    /// 
    /// </summary>
    public static class TastyFile
    {
        #region Variables
        static List<string> tempFiles = new List<string>();
        #endregion

        #region Methods
        #region Temp
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// /// <exception cref="InvalidPathExseption"></exception>
        public static string CreateTempFile()
        {
            string dir = TastyDir.CreateTempDir();
            string file = CreateFile(dir, Guid.NewGuid().ToString(), "tmp");

            return file;
        }

        public static void ClearTempFiles()
        {
            foreach(string file in tempFiles)
            {
                DeleteFile(file);
                tempFiles.Remove(file);
            }
        }

        #endregion

        #region CreationAndDeleteion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="filename"></param>
        /// <param name="fileExtention"></param>
        /// <returns></returns>
        /// <exception cref="InvalidPathExseption"></exception>
        public static string CreateFile(string dir, string filename, string fileExtention)
        {
            if(!fileExtention.StartsWith("."))
            {
                fileExtention += ".";
            }

            string filePath = Path.Combine(dir, filename + fileExtention);
            ThrowIfInvalidPath(filePath, false);

            if (VerifyPath(filePath))
            {
                using FileStream fs = File.Create(filePath);
                fs.Close();
                fs.Dispose();
                
                return filePath;
            }
            else
            {
                throw new InvalidPathExseption(filePath);
            }
        }

        public static void DeleteFile(string filePath)
        {
            ThrowIfInvalidPath(filePath, true);
            File.Delete(filePath);
        }

        public static void DeleteFile(string dir, string filename)
        {
            string filePath = Path.Combine(dir, filename);
            ThrowIfInvalidPath(filePath, true);

            File.Delete(filePath);
        }

        #endregion

        #region ReadAndWrite

        public static StreamReader OpenRead(string filePath)
        {
            ThrowIfInvalidPath(filePath, true);
            return new StreamReader(filePath);
        }

        public static StreamReader OpenRead(string dir, string filename)
        {
            string filePath = Path.Combine(dir, filename);

            ThrowIfInvalidPath(filePath, true);
            return new StreamReader(filePath);
        }

        public static StreamWriter OpenWrite(string filePath)
        {
            ThrowIfInvalidPath(filePath, false);
            return new StreamWriter(filePath);
        }

        public static StreamWriter OpenWrite(string dir, string filename)
        {
            string filePath = Path.Combine(dir, filename);

            ThrowIfInvalidPath(filePath, false);
            return new StreamWriter(filePath);
        }

        public static FileStream OpenReadWrite(string filePath)
        {
            ThrowIfInvalidPath(filePath, false);

            int bufferSize = 4096;
            
            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, bufferSize);        
            return fileStream;
        }

        public static Stream OpenReadWrite(string dir, string filename)
        {
            string filePath = Path.Combine(dir, filename);
            ThrowIfInvalidPath(filePath, false);

            int bufferSize = 4096;

            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite, bufferSize);
            return fileStream;
        }

        #region Json

        public static void SerializeObjectJson<T>(string filePath, T data)
        {
            ThrowIfInvalidPath(filePath, false);

            string json = JsonConvert.SerializeObject(data);

            using StreamWriter writer = OpenWrite(filePath);
            writer.Write(json);
        }

        public static T? DeserializeObjectJson<T>(string filePath)
        {
            ThrowIfInvalidPath(filePath, true);

            string fileContent = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(fileContent);
        }

        #endregion

        #endregion

        #region Comparason
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

        #endregion

        #region Validation
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool VerifyPath(string path)
        {
            return TastyDir.VerifyPath(path);
        }

        public static void ThrowIfInvalidPath(string path, bool reqireExistance)
        {
            if(!VerifyPath(path))
            {
                throw new InvalidPathExseption(path);
            }
            if(reqireExistance && File.Exists(path))
            {
                throw new IOException($"File dose not exist: {path}");
            }

        }
        #endregion

        #region FileSearch

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
            List<string> result = GetFilesRecursive(dir).ToList();

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
                result.AddRange(GetFilesRecursive(dir).ToList());
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
        #endregion
        #endregion
    }
}
