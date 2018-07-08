using CommandApplication.Messages;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace CommandApplication
{
    internal class SingleGraphViewModel : INotifyPropertyChanged
    {
        ConcurrentQueue<string[]> incomingQueue;
        
        SingleGraph2 singleGraph;

        private string title = "TEST";
        private string identifier;

        public SeriesCollection SeriesCollection { get; set; }

        static LineSeries lineSeries;
        private int keepRecords = 50;
        private bool running = true;
        private string[] topic;
        public event PropertyChangedEventHandler PropertyChanged;

        public SingleGraphViewModel(SingleGraph2 sg, string identifier)
        {
            //Get queue based on identifier
            incomingQueue = Mqtt.GetIncomingQueue();
            

            this.identifier = identifier;
            this.identifier = "xacc";
            this.singleGraph = sg;

            lineSeries = new GLineSeries
            {
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };
            singleGraph.chart.Series.Add(lineSeries);

            switch (identifier)
            {
                case "xacc":
                    topic = new string[] { "accx" };
                    Mqtt.Subscribe(topic);
                    lineSeries.Title = "AccX";
                    Title = setTitle(identifier);
                    break;
                default:
                    break;
            }


            //run();
        }

        internal void Unsubscribe()
        {
            Mqtt.Unsubscribe(topic);
        }

        private void run()
        {
            new Thread(() => {
                while (running)
                {
                    string[] message;
                    incomingQueue.TryDequeue(out message);
                    //Convert message to object.
                    object res = ConvertToObject.convertToObject(message[0], message[1]);
                    Xacc resObject = (Xacc)res;

                    //Plot the shit.
                    if (lineSeries.Values.Count < keepRecords)
                    {
                        lineSeries.Values.Add(resObject.XAcceleration);
                    }
                    if (lineSeries.Values.Count > keepRecords - 1)
                    {
                        var firstValue = lineSeries.Values[0];
                        lineSeries.Values.Remove(firstValue);
                    }
                }
            }).Start();
            
        }
        private string setTitle(string identifier)
        {
            string title;
            switch (identifier)
            {
                case "xacc":
                    title = "Acceleration X";
                    break;
                default:
                    title = "TEST2";
                    break;
            }
            return title;
        }
        public string Title
        {
            get { return title; }
            set {
                title = value;
                NotifyPropertyChanged();
            }
            
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}