using CommandApplication.Messages;
using CommandApplication.Model;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using Newtonsoft.Json;
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

        SingleGraph singleGraph;

        private string title;
        private string identifier;

        public SeriesCollection SeriesCollection { get; set; }

        LineSeries lineSeries;
        private int keepRecords = 50;
        private bool running = true;
        private string[] topic;
        public event PropertyChangedEventHandler PropertyChanged;

        public SingleGraphViewModel(SingleGraph sg, string identifier)
        {
            lineSeries = new GLineSeries
            {
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };

            Graph graph = new Graph(this, sg, identifier, lineSeries);
            this.identifier = identifier;
            this.singleGraph = sg;
        }
        
        internal void Unsubscribe()
        {
            Mqtt.Unsubscribe(new string[] { identifier });
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