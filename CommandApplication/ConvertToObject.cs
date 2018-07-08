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
                    YaccMessage xacc = JsonConvert.DeserializeObject<YaccMessage>(message);
                    return xacc;
                default:
                    return null;
            }
        }
    }
}