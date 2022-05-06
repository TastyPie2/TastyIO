using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TastyIO
{
    public interface IFilePointer<T>
    {
        string Filepath { get; }
        string Filename { get; }
        FileInfo Info { get; }
        Type DataType { get; }

        T Get();

        void Set(T value);

        void Rename(string name);

        void Move(string destinationDir);

        void Create();

        void Delete();
    }
}
