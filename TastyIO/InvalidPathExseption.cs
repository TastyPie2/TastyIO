using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TastyIO
{
    public class InvalidPathExseption : Exception
    {

        public InvalidPathExseption(string path) : base($"Invalid path: {path}")
        {

        }
    }
}
