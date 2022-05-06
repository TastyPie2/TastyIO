using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TastyIO
{
    public class TextFilePointer : JsonFileDataPointer<string>
    {
        public TextFilePointer(string filePath) : base(filePath)
        {
        }

        public TextFilePointer(string dir, string fileName) : base(dir, fileName)
        {
        }

        public override string Get()
        {
            IOUtility.TryGet<string>(() => 
            {
                using (var reader = new StreamReader(Filepath))
                {
                    return reader.ReadToEnd();
                }
            }, out var result, out var ex);

            if (ex != null)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                IOLoger.LogWarningAsync(ex);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            }
            return result;
        }

        public override void Set(string value)
        {
            IOUtility.Try(() =>
            {
                using (var fs = File.CreateText(Filepath))
                {
                    fs.WriteLine(value);
                }
            }, out var ex);

            if (ex != null)
            {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
                IOLoger.LogWarningAsync(ex);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed. Consider applying the 'await' operator to the result of the call.
            }
        }
    }
}
