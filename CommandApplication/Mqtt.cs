using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace CommandApplication
{
    public class Mqtt
    {
        private string brokerAddress = "127.0.0.1";
        string[] subscriberList = new string[] { "#" };
        ConcurrentQueue<string> incomingMessageQueue = new ConcurrentQueue<string>();

        internal bool IsConnected()
        {
            return mqttClient.IsConnected;
        }

        private MqttClient mqttClient;

        public Mqtt()
        {
            mqttClient = new MqttClient(brokerAddress);
            mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            mqttClient.Subscribe(subscriberList, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE});
            Publish("testtopic","message");
        }
        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            //Handle message
            incomingMessageQueue.Enqueue(Encoding.UTF8.GetString(e.Message));
        }
        public void Publish(string topic, string message)
        {
            mqttClient.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public ConcurrentQueue<string> GetIncomingQueue()
        {
            return incomingMessageQueue;
        }
        
    }
}
