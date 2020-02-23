using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Messages
{
    internal struct OrientationMessage
    {
        public DateTime timestamp { get; set; }
        public double pitch { get; set; }

        public double yaw { get; set; }

        public double roll { get; set; }
    }
}
