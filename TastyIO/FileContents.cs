using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TastyIO
{
    internal class FileContents
    {
        public string FilePath { get; private set; }
        
        public string FileName { get; private set; }

        public string FileExtention { get; private set; }

        public FileInfo FileInfo { get; private set; }

        
    }
}
