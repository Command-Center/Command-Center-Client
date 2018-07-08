using System;
using Newtonsoft.Json;
using CommandApplication.Messages;
using CommandApplication.Model;

namespace CommandApplication
{
    internal class ConvertToObject
    {
        internal static object convertToObject(string topic, string message)
        {
            switch (topic)
            {
                case Topic.XAccTopic:
                    Xacc xacc = JsonConvert.DeserializeObject<Xacc>(message);
                    return xacc;
                default:
                    return null;
            }
        }
    }
}