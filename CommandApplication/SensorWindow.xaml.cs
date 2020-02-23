using System;
using System.Windows;
using CommandApplication.Model;
using CommandApplication.ViewModel;

namespace CommandApplication
{
    /// <summary>
    /// Interaction logic for SensorWindow.xaml
    /// </summary>
    public partial class SensorWindow : Window
    {
        SensorsViewModel sensorsViewModel;

        private int RollForCalibrating = 0;
        private int PitchForCalibrating = 0;
        private int YawForCalibrating = 0;

        private static bool connected = false;

        public SensorWindow()
        {
            InitializeComponent();

            DataContext = sensorsViewModel = new SensorsViewModel(this);
        }

        private void Button_Disconnect(object sender, RoutedEventArgs e)
        {
            
        }
        //Keep sensor still while calibrating for zero values.
        private void Button_Calibrate(object sender, RoutedEventArgs e)
        {
            CalibrateOrientations();
        }
        private void CalibrateOrientations()
        {
            if (connected)
            {
                //Leser siste verdi fra label i SensorWindow.xaml
                //Midlertidig. Snitt over tid?
                //OBS; Not Thread-safe
                var roll_temp = Convert.ToInt32(rollLabel.Content);
                var yaw_temp = Convert.ToInt32(yawLabel.Content);
                var pitch_temp = Convert.ToInt32(pitchLabel.Content);
                RollForCalibrating = roll_temp;
                PitchForCalibrating = pitch_temp;
                YawForCalibrating = yaw_temp;


                //yawCalLabel.Content = yaw_temp;
                //rollCalLabel.Content = roll_temp;
                //pitchCalLabel.Content = pitch_temp;
            }
        }
        private void Button_Check_Accx(object sender, RoutedEventArgs e)
        {
            if (accxCheck.IsChecked)
            {
                accXChart.Visibility = Visibility.Visible;
                accXTitle.Visibility = Visibility.Visible;
            }
            else
            {
                accXChart.Visibility = Visibility.Collapsed;
                accXTitle.Visibility = Visibility.Collapsed;
            }
        }
        private void Button_Check_Accy(object sender, RoutedEventArgs e)
        {
            if (accyCheck.IsChecked)
            {
                accYChart.Visibility = Visibility.Visible;
                accYTitle.Visibility = Visibility.Visible;
            }
            else
            {
                accYChart.Visibility = Visibility.Collapsed;
                accYTitle.Visibility = Visibility.Collapsed;
            }
        }
        private void Button_Check_Accz(object sender, RoutedEventArgs e)
        {
            if (acczCheck.IsChecked)
            {
                accZChart.Visibility = Visibility.Visible;
                accZTitle.Visibility = Visibility.Visible;
            }
            else
            {
                accZChart.Visibility = Visibility.Collapsed;
                accZTitle.Visibility = Visibility.Collapsed;
            }
        }
        private void Button_Check_Yaw(object sender, RoutedEventArgs e)
        {
            if (yawCheck.IsChecked)
            {
                yawChart.Visibility = Visibility.Visible;
            }
            else
            {
                yawChart.Visibility = Visibility.Collapsed;
            }
        }
        private void Button_Check_Pitch(object sender, RoutedEventArgs e)
        {
            if (pitchCheck.IsChecked)
            {
                pitchChart.Visibility = Visibility.Visible;
            }
            else
            {
                pitchChart.Visibility = Visibility.Collapsed;
            }
        }
        private void Button_Check_Roll(object sender, RoutedEventArgs e)
        {
            if (rollCheck.IsChecked)
            {
                rollChart.Visibility = Visibility.Visible;
            }
            else
            {
                rollChart.Visibility = Visibility.Collapsed;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Mqtt.Unsubscribe(Topic.AllTopics.ToArray());
        }
    }
}
