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
        static ConcurrentQueue<string[]> incomingMessageQueue = new ConcurrentQueue<string[]>();

        internal bool IsConnected()
        {
            return mqttClient.IsConnected;
        }

        private static MqttClient mqttClient;

        public Mqtt()
        {
            mqttClient = new MqttClient(brokerAddress);
            mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            string clientId = Guid.NewGuid().ToString();
            Subscribe(subscriberList);
            Publish("testtopic","message");
        }
        
        public void Publish(string topic, string message)
        {
            mqttClient.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
        }
        public static void Subscribe(string[] topics)
        {
            mqttClient.Subscribe(topics, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

        }
        public static void Unsubscribe(string[] topic)
        {
            mqttClient.Unsubscribe(topic);
        }
        public static ConcurrentQueue<string[]> GetIncomingQueue()
        {
            return incomingMessageQueue;
        }


        private void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // If none wants the message: throw away.
            // Else: enqueue.
            var message = new string[] { e.Topic, Encoding.UTF8.GetString(e.Message) };
            //Handle message
            incomingMessageQueue.Enqueue(message);
        }

    }
}
