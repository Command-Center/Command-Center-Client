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

        static LineSeries lineX;
        static LineSeries lineY;
        static LineSeries lineZ;

        static LineSeries line_roll;
        static LineSeries line_pitch;
        static LineSeries line_yaw;

        static LineSeries line_orientation;
        static LineSeries line_acceleration;
        private int keepRecords = 50;
        private bool running = true;
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

            //lineX = new GLineSeries
            //{
            //    Title = "AccX",
            //    Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
            //    PointGeometry = null,
            //    Fill = Brushes.Transparent
            //};
            //lineY = new GLineSeries
            //{
            //    Title = "AccY",
            //    Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
            //    PointGeometry = null,
            //    Fill = Brushes.Transparent
            //};
            //lineZ = new GLineSeries
            //{
            //    Title = "AccZ",
            //    Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
            //    PointGeometry = null,
            //    Fill = Brushes.Transparent
            //};

            //line_roll = new GLineSeries
            //{
            //    Title = "Roll",
            //    Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
            //    PointGeometry = null,
            //    Fill = Brushes.Transparent
            //};
            //line_pitch = new GLineSeries
            //{
            //    Title = "Pitch",
            //    Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
            //    PointGeometry = null,
            //    Fill = Brushes.Transparent
            //};
            //line_yaw = new GLineSeries
            //{
            //    Title = "Yaw",
            //    Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
            //    PointGeometry = null,
            //    Fill = Brushes.Transparent

            //};

            switch (identifier)
            {
                case "xacc":
                    lineSeries.Title = "AccX";
                    Title = setTitle(identifier);
                    break;
                default:
                    break;
            }


            //run();
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
                    if (lineX.Values.Count < keepRecords)
                    {
                        lineSeries.Values.Add(resObject.XAcceleration);
                    }
                    if (lineX.Values.Count > keepRecords - 1)
                    {
                        var firstValue = lineSeries.Values[0];
                        lineX.Values.Remove(firstValue);
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