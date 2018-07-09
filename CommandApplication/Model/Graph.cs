using CommandApplication.Messages;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using Newtonsoft.Json;
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

        static LineSeries lineSeries;
        private int keepRecords = 50;
        private bool running = true;
        private string[] topic;
        private SingleGraphViewModel sgvm;
        private SensorWindow sensorWindow;

        public Graph(SingleGraphViewModel sgvm, SingleGraph sg, string identifier, LineSeries lineSeries)
        {
            //Get queue based on identifier
            incomingQueue = Mqtt.GetIncomingQueue();
            this.singleGraph = sg;
            this.sgvm = sgvm;
            

            singleGraph.chart.Series.Add(lineSeries);

            switch (identifier)
            {
                case Topic.XAccTopic:
                    topic = new string[] { Topic.XAccTopic };
                    Mqtt.Subscribe(topic);
                    lineSeries.Title = "AccX";
                    sgvm.Title = setTitle(identifier);
                    break;
                default:
                    break;
            }
            run();
        }
        //Constructor for multiple sensor window
        public Graph(SingleGraphViewModel sgvm, SensorWindow sw, string identifier, LineSeries lineSeries)
        {
            //Get queue based on identifier
            incomingQueue = Mqtt.GetIncomingQueue();
            this.sensorWindow = sw;
            this.sgvm = sgvm;

            // chose based on identifier
            sensorWindow.accXChart.Series.Add(lineSeries);

            switch (identifier)
            {
                case Topic.XAccTopic:
                    topic = new string[] { Topic.XAccTopic };
                    Mqtt.Subscribe(topic);
                    lineSeries.Title = "AccX";
                    sgvm.Title = setTitle(identifier);
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

                        switch (topic)
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
                    Thread.Sleep(100);
                }
            }).Start();

        }
        private void plottingMethod(double value)
        {
            if (lineSeries.Values.Count < keepRecords)
            {
                lineSeries.Values.Add(value);
            }
            if (lineSeries.Values.Count > keepRecords - 1)
            {
                var firstValue = lineSeries.Values[0];
                lineSeries.Values.Remove(firstValue);
            }
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
