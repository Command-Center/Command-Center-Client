using CommandApplication.Messages;
using CommandApplication.ViewModel;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace CommandApplication.Model
{
    class Graph
    {
        ConcurrentQueue<string[]> incomingQueue;

        SingleGraphWindow singleGraphWindow;

        public SeriesCollection SeriesCollection { get; set; }

        LineSeries LineSeries;
        private int keepRecords = 50;
        private bool running = true;
        private string[] topic;
        private string identifier;
        private string identifier_top;
        private SingleGraphViewModel sgvm;
        private SensorsViewModel svm;
        private SensorWindow sensorWindow;
        private Window windowToPlotOn;

        public Graph(SingleGraphViewModel sgvm, SingleGraphWindow sgw, string identifier, LineSeries lineSeries)
        {
            //Get queue based on identifier
            incomingQueue = Mqtt.GetIncomingQueue();
            this.singleGraphWindow = sgw;
            this.windowToPlotOn = sgw;
            this.sgvm = sgvm;
            LineSeries = lineSeries;
            this.identifier = identifier;
            
            if(identifier == "xacceleration" || identifier == "yacceleration" || identifier == "zacceleration")
            {
                identifier_top = "acceleration";
                topic = new string[] { identifier_top };
            }
            else if (identifier == "pitch" || identifier == "yaw" || identifier == "roll")
            {
                identifier_top = "orientation";
                topic = new string[] { identifier_top };
            }

            singleGraphWindow.chart.Series.Add(LineSeries);
            
            sgvm.Title = SetTitle(identifier);

            Mqtt.Subscribe(topic);

            switch (identifier)
            {
                case Topic.XAccTopic:
                    LineSeries.Title = "AccX";
                    break;
                case Topic.YAccTopic:
                    LineSeries.Title = "AccY";
                    break;
                case Topic.ZAccTopic:
                    LineSeries.Title = "AccZ";
                    break;
                case Topic.RollTopic:
                    LineSeries.Title = "Roll";
                    break;
                case Topic.PitchTopic:
                    LineSeries.Title = "Pitch";
                    break;
                case Topic.YawTopic:
                    LineSeries.Title = "Yaw";
                    break;
                default:
                    break;
            }

            Run();
        }

        //Constructor for multiple sensor window
        public Graph(SensorsViewModel svm, SensorWindow sw, string identifier, LineSeries lineSeries)
        {
            //Get queue based on identifier
            incomingQueue = Mqtt.GetIncomingQueue();
            this.sensorWindow = sw;
            this.windowToPlotOn = sw;
            this.svm = svm;
            LineSeries = lineSeries;
            this.identifier = identifier;

            if (identifier == "xacceleration" || identifier == "yacceleration" || identifier == "zacceleration")
            {
                identifier_top = "acceleration";
                topic = new string[] { identifier_top };
            }
            else if (identifier == "pitch" || identifier == "yaw" || identifier == "roll")
            {
                identifier_top = "orientation";
                topic = new string[] { identifier_top };
            }

            svm.Title = SetTitle(identifier);
            Mqtt.Subscribe(topic);

            switch (identifier)
            {
                case Topic.XAccTopic:
                    sensorWindow.accXChart.Series.Add(LineSeries);
                    LineSeries.Title = "AccX";
                    break;
                case Topic.YAccTopic:
                    sensorWindow.accYChart.Series.Add(LineSeries);
                    LineSeries.Title = "AccY";
                    break;
                case Topic.ZAccTopic:
                    sensorWindow.accZChart.Series.Add(LineSeries);
                    LineSeries.Title = "AccZ";
                    break;
                case Topic.RollTopic:
                    sensorWindow.rollChart.Series.Add(LineSeries);
                    LineSeries.Title = "Roll";
                    break;
                case Topic.PitchTopic:
                    sensorWindow.pitchChart.Series.Add(LineSeries);
                    LineSeries.Title = "Pitch";
                    break;
                case Topic.YawTopic:
                    sensorWindow.yawChart.Series.Add(LineSeries);
                    LineSeries.Title = "Yaw";
                    break;
                default:
                    break;
            }
            
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

                        if(topic == identifier_top)
                        {
                            AccelerationMessage acc;
                            OrientationMessage orient;

                            switch (identifier)
                            {
                                case Topic.XAccTopic:
                                    acc = JsonConvert.DeserializeObject<AccelerationMessage>(jsonMessage);
                                    PlottingMethod(acc.x);
                                    break;
                                case Topic.YAccTopic:
                                    acc = JsonConvert.DeserializeObject<AccelerationMessage>(jsonMessage);
                                    PlottingMethod(acc.y);
                                    break;
                                case Topic.ZAccTopic:
                                    acc = JsonConvert.DeserializeObject<AccelerationMessage>(jsonMessage);
                                    PlottingMethod(acc.z);
                                    break;
                                case Topic.PitchTopic:
                                    orient = JsonConvert.DeserializeObject<OrientationMessage>(jsonMessage);
                                    PlottingMethod(orient.pitch);
                                    break;
                                case Topic.RollTopic:
                                    orient = JsonConvert.DeserializeObject<OrientationMessage>(jsonMessage);
                                    PlottingMethod(orient.roll);
                                    break;
                                case Topic.YawTopic:
                                    orient = JsonConvert.DeserializeObject<OrientationMessage>(jsonMessage);
                                    PlottingMethod(orient.yaw);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    Thread.Sleep(100);
                }
            }).Start();
        }

        private void PlottingMethod(double value)
        {
            windowToPlotOn.Dispatcher.Invoke(new Action(() => {
                if (LineSeries.Values.Count < keepRecords)
                {
                    LineSeries.Values.Add(value);
                }
                if (LineSeries.Values.Count > keepRecords - 1)
                {
                    var firstValue = LineSeries.Values[0];
                    LineSeries.Values.Remove(firstValue);
                }
            }));
            
        }

        private string SetTitle(string identifier)
        {
            string title;
            switch (identifier)
            {
                case Topic.XAccTopic:
                    title = "Acceleration X";
                    break;
                case Topic.YAccTopic:
                    title = "Acceleration Y";
                    break;
                case Topic.ZAccTopic:
                    title = "Acceleration Z";
                    break;
                case Topic.PitchTopic:
                    title = "Pitch";
                    break;
                case Topic.RollTopic:
                    title = "Roll";
                    break;
                case Topic.YawTopic:
                    title = "Yaw";
                    break;
                default:
                    title = "";
                    break;
            }
            return title;
        }
    }
}
