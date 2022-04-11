using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TastyIO;
using TestUnit.Utils;

namespace TestUnit
{
    [TestClass, TestCategory("TastyFile")]
    public class TastyFileTest
    {
        [TestMethod]
        public void HashFile()
        {
            string referenceHash;
            string resultHash;

            //Temp data
            string tempFile = Path.GetTempFileName();
            File.WriteAllText(tempFile, "Hello World!");
            byte[] data = Encoding.UTF8.GetBytes(File.ReadAllText(tempFile));

            //Manual method
            using MD5 md = MD5.Create();
            referenceHash = Encoding.UTF8.GetString(md.ComputeHash(data));

            //Semi manual
            resultHash = TastyFile.HashFile(tempFile, HashAlgorithmName.MD5);

            //Cleanup
            File.Delete(tempFile);

            //Compare
            if (referenceHash != resultHash)
                Assert.Fail("Hashes does not match.");
        }

        [TestMethod]
        public void CompareFiles()
        {
            string tempData = "Hello World!";
            //Create tmp files
            string temp1 = Path.GetTempFileName();
            string temp2 = Path.GetTempFileName();

            //Write data
            File.WriteAllText(temp1, tempData);
            File.WriteAllText(temp2, tempData);

            //Compare
            if (!TastyFile.CompareFiles(temp1, temp2))
            {
                Assert.Fail();
            }

            //Cleanup
            File.Delete(temp1);
            File.Delete(temp2);
        }

        [TestMethod]
        public void VerifyPath()
        {
            //Valid
            if (!TastyFile.VerifyPath("C:\\exsample.tmp"))
            {
                Assert.Fail();
            }

            //Invalid
            if (TastyFile.VerifyPath("C:\\exsample."))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetFilesRecursive()
        {
            string tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            int fileCount = 25;
            int fileLenght = 500;

            Directory.CreateDirectory(tempDir);
            Console.WriteLine(tempDir);

            //Make files
            string[] referenceFiles = Junkinator.GenerateJunkFiles(tempDir, fileCount, fileLenght);

            //GetFiles
            string[] results = TastyFile.GetFilesRecursive(tempDir);

            //Verify Count
            if (results.Length != referenceFiles.Length)
            {
                Assert.Fail();
            }

            //Verify Paths
            foreach (string file in referenceFiles)
            {
                if (!File.Exists(file))
                {
                    Assert.Fail();
                }
            }

            //Cleanup
            TastyDir.DeleteDir(tempDir);
        }
    }
}
