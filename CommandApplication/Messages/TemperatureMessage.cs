using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Messages
{
    class TemperatureMessage
    {
        public DateTime timestamp { get; set; }

        public double value { get; set; }
    }
}
