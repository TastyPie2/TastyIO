using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using TastyIO;
using TestUnit.Utils;

namespace TestUnit
{
    [TestClass, TestCategory("TastyDir")]
    public class TastyDirTest
    {
        [TestMethod]
        public void VerifyPath()
        {
            //Valid path
            if(!TastyDir.VerifyPath("C:\\"))
            {
                Assert.Fail();
            }

            //Invalid path
            if(TastyDir.VerifyPath("C\\"))
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void GetDirecoriesRecursive()
        {
            TastyDir.GetDirecoriesRecursive(AppDomain.CurrentDomain.BaseDirectory);
        }

        [TestMethod]
        public void Copy()
        {
            string original = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(original);

            //GenerateJunk
            Random random = new Random();
            Junkinator.GenerateJunkFiles(original, random.Next(1, 100), random.Next(1, 10000));

            string copy = TastyDir.CopyDir(original, AppDomain.CurrentDomain.BaseDirectory, true);

            TastyDir.DeleteDir(original);
            TastyDir.DeleteDir(copy);
        }

        [TestMethod]
        public void Move()
        {
            string original = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(original);

            //GenerateJunk
            Random random = new Random();
            Junkinator.GenerateJunkFiles(original, random.Next(1, 100), random.Next(1, 10000));

            original = TastyDir.MoveDir(original, AppDomain.CurrentDomain.BaseDirectory, true);

            TastyDir.DeleteDir(original);
        }

        [TestMethod]
        public void Delete()
        {
            Guid guid = Guid.NewGuid();
            string appTempFolder = Path.Combine(Path.GetTempPath(), guid.ToString());
            string tempFile = Path.Combine(appTempFolder, $"{Guid.NewGuid()}.tmp");
            
            Directory.CreateDirectory(appTempFolder);
            using FileStream stream = File.OpenWrite(tempFile);

            stream.Write(Encoding.UTF8.GetBytes(
                "Hello world!\n" +
                "This is a temp file."));

            stream.Flush();
            stream.Close();
            stream.Dispose();

            TastyDir.DeleteDir(appTempFolder);

            if(File.Exists(tempFile))
            {
                Assert.Fail("File not deleted");
            }
            if(Directory.Exists(appTempFolder))
            {
                Assert.Fail("Folder not deleted");
            }
        }
    }
}