using CommandApplication.Messages;
using CommandApplication.ViewModel;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Media;

namespace CommandApplication.Model
{
    class Graph
    {
        ConcurrentQueue<string[]> incomingQueue;

        SingleGraph singleGraph;

        public SeriesCollection SeriesCollection { get; set; }

        LineSeries LineSeries;
        private int keepRecords = 50;
        private bool running = true;
        private string[] topic;
        private string identifier;
        private SingleGraphViewModel sgvm;
        private SensorsViewModel svm;
        private SensorWindow sensorWindow;

        public Graph(SingleGraphViewModel sgvm, SingleGraph sg, string identifier, LineSeries lineSeries)
        {
            //Get queue based on identifier
            incomingQueue = Mqtt.GetIncomingQueue();
            this.singleGraph = sg;
            this.sgvm = sgvm;
            LineSeries = lineSeries;
            this.identifier = identifier;
            

            singleGraph.chart.Series.Add(LineSeries);
            topic = new string[] { identifier };
            sgvm.Title = setTitle(identifier);

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
            run();
        }
        //Constructor for multiple sensor window
        public Graph(SensorsViewModel svm, SensorWindow sw, string identifier, LineSeries lineSeries)
        {
            //Get queue based on identifier
            incomingQueue = Mqtt.GetIncomingQueue();
            this.sensorWindow = sw;
            this.svm = svm;
            LineSeries = lineSeries;
            this.identifier = identifier;

            topic = new string[] { identifier };
            svm.Title = setTitle(identifier);
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
            
            run();
        }
        private void run()
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

                        
                        if(topic == identifier)
                        {
                            switch (identifier)
                            {
                                case Topic.XAccTopic:
                                    XaccMessage xacc = JsonConvert.DeserializeObject<XaccMessage>(jsonMessage);
                                    plottingMethod(xacc.XAcceleration);
                                    break;
                                case Topic.YAccTopic:
                                    YaccMessage yacc = JsonConvert.DeserializeObject<YaccMessage>(jsonMessage);
                                    plottingMethod(yacc.YAcceleration);
                                    break;
                                case Topic.ZAccTopic:
                                    ZaccMessage zacc = JsonConvert.DeserializeObject<ZaccMessage>(jsonMessage);
                                    plottingMethod(zacc.ZAcceleration);
                                    break;
                                case Topic.PitchTopic:
                                    PitchMessage pitch = JsonConvert.DeserializeObject<PitchMessage>(jsonMessage);
                                    plottingMethod(pitch.Pitch);
                                    break;
                                case Topic.RollTopic:
                                    RollMessage roll = JsonConvert.DeserializeObject<RollMessage>(jsonMessage);
                                    plottingMethod(roll.Roll);
                                    break;
                                case Topic.YawTopic:
                                    YawMessage yaw = JsonConvert.DeserializeObject<YawMessage>(jsonMessage);
                                    plottingMethod(yaw.Yaw);
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
        private void plottingMethod(double value)
        {
            sensorWindow.Dispatcher.Invoke(new Action(() => {
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
        private string setTitle(string identifier)
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
