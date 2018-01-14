using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Model
{
    class ShipAndVoyageData
    {
        public string MessageType { get; set; }
        public int IMO { get; set; }
        public string CallSign { get; set; }
        public string ShipName { get; set; }
        public string ShipType { get; set; }
        public double ToBow { get; set; }
        public double ToStern { get; set; }
        public double ToStarboard { get; set; }
        public double ToPort { get; set; }
        public string PositionFixingDevice { get; set; }
        public string ETA { get; set; }
        public double Draught { get; set; }
        public string Destination { get; set; }
        public string DataTerminalReady { get; set; }
        public ShipAndVoyageData(string[] inpArray)
        {
            MessageType = extractString(inpArray[0]);
            
            CallSign = extractString(inpArray[2]);
            ShipName = extractString(inpArray[3]);
            ShipType = extractString(inpArray[4]);
            IMO = extractInt(inpArray[1]);
            ToBow = extractDouble(inpArray[5]);
            ToStern = extractDouble(inpArray[6]);
            ToStarboard = extractDouble(inpArray[7]);
            ToPort = extractDouble(inpArray[8]);
            PositionFixingDevice = extractString(inpArray[9]);
            ETA = extractString(inpArray[10]);
            Draught = extractDouble(inpArray[11]);
            Destination = extractString(inpArray[12]);
            DataTerminalReady = extractString(inpArray[13]);
        }
        private string extractString(string s)
        {
            if (s.Contains("'"))
            {
                int from = s.IndexOf("='") + "='".Length;
                int to = s.Length - 1;
                s = s.Substring(from, to - from);
            }
            else
            {
                int from = s.IndexOf("=") + 1;
                int to = s.Length;
                s = s.Substring(from, to - from);
            }
            return s;
        }
        private int extractInt(string s)
        {
            if(!s.Contains("IMO"))
            {
                int from = s.IndexOf("=") + 1;
                int to = s.Length;
                s = s.Substring(from, to - from);
            }
            else
            {
                int from = s.IndexOf("[imo=") + "[imo=".Length;
                int to = s.Length - 1;
                s = s.Substring(from, to - from);
            }
            return int.Parse(s);
        }
        private double extractDouble(string s)
        {
            int from = s.IndexOf("=") + 1;
            int to = s.Length;
            s = s.Substring(from, to - from);

            return Convert.ToDouble(s, CultureInfo.InvariantCulture);
        }
    }
}
