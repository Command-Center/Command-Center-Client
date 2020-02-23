using CommandApplication.Messages;
using CommandApplication.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace CommandApplication.ViewModel
{
    internal class SideBarInfo
    {
        private SensorsViewModel sensorsViewModel;
        private SensorWindow sensorWindow;
        private ConcurrentQueue<string[]> incomingQueue;
        private bool running = true;

        public SideBarInfo(SensorsViewModel sensorsViewModel, SensorWindow sensorWindow)
        {
            this.sensorsViewModel = sensorsViewModel;
            this.sensorWindow = sensorWindow;

            this.incomingQueue = Mqtt.GetIncomingQueue();
            Mqtt.Subscribe(new string[] { "temperature" });

            Run();
        }

        private void Run()
        {
            new Thread(() => {
                while (running)
                {
                    string[] message;
                    incomingQueue.TryDequeue(out message);
                    if (message != null)
                    {
                        var topic = message[0];
                        var jsonMessage = message[1];

                        switch (topic)
                        {
                            case Topic.OrientationTopic:
                                var orient = JsonConvert.DeserializeObject<OrientationMessage>(jsonMessage);
                                sensorWindow.Dispatcher.Invoke(new Action(() =>
                                {
                                    sensorWindow.rollLabel.Content = orient.roll;
                                    sensorWindow.yawLabel.Content = orient.yaw;
                                    sensorWindow.pitchLabel.Content = orient.pitch;
                                }));
                                break;
                            case Topic.AccelerationTopic:
                                var acc = JsonConvert.DeserializeObject<AccelerationMessage>(jsonMessage);
                                sensorWindow.Dispatcher.Invoke(new Action(() =>
                                {
                                    sensorWindow.accxLabel.Content = acc.x;
                                    sensorWindow.accyLabel.Content = acc.y;
                                    sensorWindow.acczLabel.Content = acc.z;
                                }));
                                break;
                            case Topic.TemperatureTopic:
                                var temp = JsonConvert.DeserializeObject<TemperatureMessage>(jsonMessage);
                                sensorWindow.Dispatcher.Invoke(new Action(() =>
                                {
                                sensorWindow.tempLabel.Content = temp.value;
                                }));
                                break;
                            case Topic.PressureTopic:
                                var pressure = JsonConvert.DeserializeObject<PressureMessage>(jsonMessage);
                                sensorWindow.Dispatcher.Invoke(new Action(() =>
                                {
                                    sensorWindow.presLabel.Content = pressure.value;
                                }));
                                break;
                            default:
                                break;
                        }
                    }
                    Thread.Sleep(100);
                }
            }).Start();
        }
    }
}