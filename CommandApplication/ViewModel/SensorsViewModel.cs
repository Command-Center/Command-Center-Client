using CommandApplication.Model;
using LiveCharts;
using LiveCharts.Geared;
//using LiveCharts.Geared;
using LiveCharts.Wpf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CommandApplication.ViewModel
{
    class SensorsViewModel : INotifyPropertyChanged
    {
        
        private string title;

        public SeriesCollection SeriesCollection { get; set; }

        static LineSeries line_temp;
        static LineSeries line_pressure;
        static LineSeries line_humidity;

        static LineSeries lineX;
        static LineSeries lineY;
        static LineSeries lineZ;

        static LineSeries lineRoll;
        static LineSeries linePitch;
        static LineSeries lineYaw;
        private SensorWindow sensorWindow;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public SensorsViewModel(SensorWindow sensorWindow)
        {
            
            this.sensorWindow = sensorWindow;
            
            lineX = new GLineSeries
            {
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };

            lineY = new GLineSeries
            {
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };

            lineZ = new GLineSeries
            {
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };

            lineRoll = new GLineSeries
            {
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };

            linePitch = new GLineSeries
            {
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };

            lineYaw = new GLineSeries
            {
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };

            Graph graphXAcc = new Graph(this, sensorWindow, Topic.XAccTopic, lineX);
            Graph graphYAcc = new Graph(this, sensorWindow, Topic.YAccTopic, lineY);
            Graph graphZAcc = new Graph(this, sensorWindow, Topic.ZAccTopic, lineZ);

            Graph graphRoll = new Graph(this, sensorWindow, Topic.RollTopic, lineRoll);
            Graph graphPitch = new Graph(this, sensorWindow, Topic.PitchTopic, linePitch);
            Graph graphYaw = new Graph(this, sensorWindow, Topic.YawTopic, lineYaw);

            SideBarInfo sideBarInfo = new SideBarInfo(this, sensorWindow);

            subscribeAllAndMakeVisible();
        }

        private void subscribeAllAndMakeVisible()
        {
            //var list = Topic.AllTopics.ToArray();
            //Mqtt.Subscribe(list);

            sensorWindow.accXChart.Visibility = Visibility.Visible;
            sensorWindow.accYChart.Visibility = Visibility.Visible;
            sensorWindow.accZChart.Visibility = Visibility.Visible;

            sensorWindow.rollChart.Visibility = Visibility.Visible;
            sensorWindow.pitchChart.Visibility = Visibility.Visible;
            sensorWindow.yawChart.Visibility = Visibility.Visible;

            sensorWindow.accXTitle.Visibility = Visibility.Visible;
            sensorWindow.accYTitle.Visibility = Visibility.Visible;
            sensorWindow.accZTitle.Visibility = Visibility.Visible;
        }
        //Trenger ikke denne
        public string Title
        {
            get { return title; }
            set
            {
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
