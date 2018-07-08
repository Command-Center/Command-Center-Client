using System;
using Newtonsoft.Json;
using CommandApplication.Messages;

namespace CommandApplication
{
    internal class ConvertToObject
    {
        internal static object convertToObject(string topic, string message)
        {
            switch (topic)
            {
                case "xacc":
                    Xacc xacc = JsonConvert.DeserializeObject<Xacc>(message);
                    return xacc;
                default:
                    return null;
            }
        }
    }
}