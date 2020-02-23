using System;
using System.Collections.Concurrent;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace CommandApplication
{
    public class Mqtt
    {
        private const string brokerAddress = "192.168.0.15";
        //string[] subscriberList = new string[] { "#" };
        static ConcurrentQueue<string[]> incomingMessageQueue = new ConcurrentQueue<string[]>();
        private static MqttClient mqttClient;

        public Mqtt()
        {
            try
            {
                mqttClient = new MqttClient(brokerAddress);
                string clientId = Guid.NewGuid().ToString();
                
                mqttClient.Connect(clientId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Cant connect to server");
                System.Diagnostics.Trace.WriteLine(ex.Message);

            }

            mqttClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
            
        }

        internal bool IsConnected()
        {
            return mqttClient.IsConnected;
        }

        public void Publish(string topic, string message)
        {
            mqttClient.Publish(topic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            
        }

        public static void Subscribe(string[] topics)
        {
            mqttClient.Subscribe(topics, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
            System.Diagnostics.Trace.WriteLine("Subscribed");
            System.Diagnostics.Trace.WriteLine(mqttClient.IsConnected);
        }
        public static void Unsubscribe(string[] topic)
        {
            mqttClient.Unsubscribe(topic);
            System.Diagnostics.Trace.WriteLine("Unsubscribed");
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
            //System.Diagnostics.Trace.WriteLine("Mottatt: " + message);
        }

    }
}
