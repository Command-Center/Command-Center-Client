using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Messages
{
    internal struct AccelerationMessage
    {
        public DateTime Datetime { get; set; }
        public double z { get; set; }

        public double y { get; set; }

        public double x { get; set; }
    }
}
