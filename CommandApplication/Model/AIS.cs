using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommandApplication.Model
{
    class AIS
    {
        public PositionReport PositionReport { get; set; }
        public ShipAndVoyageData ShipAndVoyageData { get; set; }
        public string res { get; set; }
        public AIS(string rawAIS)
        {
            if(rawAIS.Substring(0, rawAIS.IndexOf("{")).Equals("ShipAndVoyageData"))
            {
                int from = rawAIS.IndexOf("ShipAndVoyageData{") + "ShipAndVoyageData{".Length;
                int to = rawAIS.IndexOf("} AISMessage");
                res = rawAIS.Substring(from, to-from);
                var ShipAndVoyageArray = res.Split(',');
                ShipAndVoyageData = new ShipAndVoyageData(ShipAndVoyageArray);
            }
            else //Må legge inn flere cases.
            {
                int from = rawAIS.IndexOf("PositionReport{") + "PositionReport{".Length;
                int to = rawAIS.IndexOf("} AISMessage");
                res = rawAIS.Substring(from, to - from);
            }
            


            


        }
        
    }
}
