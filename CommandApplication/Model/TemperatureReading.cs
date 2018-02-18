using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Model
{
    public class TemperatureReading
    {
        public int id { get; set; }
        public DateTime TimeStamp { get; set; }
        public double Temperature { get; set; }
    }
}
