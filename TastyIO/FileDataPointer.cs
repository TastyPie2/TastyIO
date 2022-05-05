using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace TastyIO
{
    public class FileDataPointer<T>
    {
        public string Lable { get; set; }
        public string FilePath { get; private set; }
        public string FileName { get; private set; }
        
        public void Rename()
        {

        }

        public void Move()
        { 
        
        }

        public T GetData()
        {
            IOUtility.TryGet<T>(() => 
            { 
                string json;
                using (StreamReader reader = new StreamReader(FilePath))
                {
                    json = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<T>(json);
            }, out T result ,out var ex);

            if(ex != null)
            {
                IOLoger.LogWarningAsync(ex);
            }
            return result;
        }

        public void SetData(T value)
        {
            IOUtility.Try(() =>
            { 
                var json = JsonConvert.SerializeObject(value);
                using(StreamWriter writer = new StreamWriter(FilePath))
                {
                    writer.Write(json);
                    writer.Flush();
                }
            }, out var ex);

            if(ex != null)
            {
                IOLoger.LogWarningAsync(ex);
            }
        }
    }
}
