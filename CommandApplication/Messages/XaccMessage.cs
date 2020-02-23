using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Messages
{
    public struct XaccMessage
    {
        public DateTime Datetime { get; set; }
        public double XAcceleration { get; set; }
    }
}
