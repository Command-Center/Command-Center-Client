using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Model
{
    class GPS
    {
        public double longitude { get; set; }
        public double latitude { get; set; }
        public double hspeed { get; set; }
        public int mode { get; set; }
        public double alt { get; set; }
        public double climb { get; set; }
    }
}
