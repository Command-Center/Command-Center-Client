using CommandApplication.Model;
using LiveCharts;
using LiveCharts.Geared;
using LiveCharts.Wpf;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CommandApplication.ViewModel
{
    class SensorsViewModel
    {
        SensorWindow sensorWindow;

        public SeriesCollection SeriesCollection { get; set; }

        static LineSeries line_temp;
        static LineSeries line_pressure;
        static LineSeries line_humidity;

        static LineSeries lineX;
        static LineSeries lineY;
        static LineSeries lineZ;

        static LineSeries line_roll;
        static LineSeries line_pitch;
        static LineSeries line_yaw;

        static LineSeries line_orientation;
        static LineSeries line_acceleration;

        ConcurrentQueue<string[]> incomingQueue;

        public SensorsViewModel(SensorWindow sensorWindow)
        {
            incomingQueue = Mqtt.GetIncomingQueue();
            

            line_temp = line_pressure = line_humidity = lineX = lineY
            = lineZ = line_roll = line_pitch = line_yaw =
            new GLineSeries
            {
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };

            sensorWindow.accXChart.Series.Add(lineX);
            sensorWindow.accYChart.Series.Add(lineY);
            sensorWindow.accZChart.Series.Add(lineZ);
            
            sensorWindow.rollChart.Series.Add(line_roll);
            sensorWindow.pitchChart.Series.Add(line_pitch);
            sensorWindow.yawChart.Series.Add(line_yaw);

            subscribeAllAndMakeVisible();

            


        }

        private void subscribeAllAndMakeVisible()
        {
            Mqtt.Subscribe(Topic.AllTopics.ToArray());

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
    }
}
