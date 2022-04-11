using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace TestUnit.Utils
{
    internal static class Junkinator
    {
        public static string GenerateStringJunk(int lenght)
        {
            byte[] buffer = new byte[lenght];

            Random rand = new Random();
            rand.NextBytes(buffer);

            return Encoding.UTF8.GetString(buffer);
        }

        public static byte[] GenerateByteJunk(int lenght)
        {
            byte[] buffer = new byte[lenght];

            Random rand = new Random();
            rand.NextBytes(buffer);

            return buffer;
        }


        public static void GenerateJunkFile(string path, int lenght)
        {
            File.WriteAllBytes(path, GenerateByteJunk(lenght));
        }

        public static string[] GenerateJunkFiles(string dir, int fileCount, int fileLenght)
        {
            string[] files = new string[fileCount];
            for(int i = 0; i < files.Length; i++)
            {
                files[i] = Path.Combine(dir, Guid.NewGuid().ToString() + ".jnk");
                GenerateJunkFile(files[i], fileLenght);
            }

            return files;
        }

    }
}
