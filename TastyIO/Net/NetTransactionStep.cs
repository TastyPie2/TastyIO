using System;
using System.Collections.Generic;
using System.Text;

namespace TastyIO.Net
{
    public class NetTransactionStep
    {
        public enum StepType
        {
            End,
            Read,
            Write,
        }

        public string Message { get; set; }

        public StepType Step { get; set; }

    }
}
