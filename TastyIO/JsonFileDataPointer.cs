using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace TastyIO
{
    public class JsonFileDataPointer<T> : IFilePointer<T>
    {
        public string Lable { get; set; }

        public string Filepath { get; protected set; }
        public string Filename { get; protected set; }
        public Type DataType { get; protected set; }
        public virtual FileInfo Info { get; protected set; }


        public JsonFileDataPointer(string filePath)
        {
            Info = new FileInfo(filePath);
            Filename = Info.Name;
            Filepath = filePath;
        }

        public JsonFileDataPointer(string dir, string fileName)
        {
            var filePath = Path.Combine(dir, fileName);

            Info = new FileInfo(filePath);
            Filename = Info.Name;
            Filepath = filePath;
        }

        public virtual T Get()
        {
            IOUtility.TryGet<T>(() =>
            {
                string json;
                using (StreamReader reader = new StreamReader(Filepath))
                {
                    json = reader.ReadToEnd();
                    reader.Close();
                }

                return JsonConvert.DeserializeObject<T>(json);
            }, out T result, out var ex);

            if (ex != null)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                IOLoger.LogWarningAsync(ex);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            }
            return result;
        }

        public virtual void Set(T value)
        {
            IOUtility.Try(() =>
            {
                var json = JsonConvert.SerializeObject(value, Formatting.Indented);
                using (var fs = File.CreateText(Filepath))
                {
                    fs.WriteLine(json);
                    fs.Close();
                }
            }, out var ex);

            if (ex != null)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                IOLoger.LogWarningAsync(ex);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            }
        }

        public virtual void Rename(string name)
        {
            Filepath = FileUtils.Rename(Filepath, name);
            Filename = name;
        }

        public virtual void Move(string destinationDir)
        { 
            FileUtils.Move(Filepath, destinationDir);
            Filepath = destinationDir;
        }

        public virtual void Delete()
        {
            FileUtils.DeleteFile(Filepath);
        }

        public virtual void Create()
        {
            FileUtils.CreateFile(Filepath, JsonConvert.SerializeObject(default(T)));
        }

        
    }
}