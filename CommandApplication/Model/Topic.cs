using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Model
{
    class Topic
    {
        public const string XAccTopic = "xacceleration";
        public const string YAccTopic = "yacceleration";
        public const string ZAccTopic = "zacceleration";
        public const string AccelerationTopic = "acceleration";

        public const string PitchTopic = "pitch";
        public const string RollTopic = "roll";
        public const string YawTopic = "yaw";
        public const string OrientationTopic = "orientation";

        public const string GpsTopic = "gps";

        public const string Ir1Topic = "ir1";

        public const string Ir2Topic = "ir2";

        public const string TemperatureTopic = "temperature";
        public const string HumidityTopic = "humidity";
        public const string PressureTopic = "pressure";

        public static List<string> AllTopics = new List<string>()
        {
            XAccTopic,
            YAccTopic,
            ZAccTopic,
            PitchTopic,
            RollTopic,
            YawTopic,
            GpsTopic,
            Ir1Topic,
            Ir2Topic,
            TemperatureTopic,
            HumidityTopic,
            PressureTopic,
            AccelerationTopic,
            OrientationTopic
        };
    }
}
