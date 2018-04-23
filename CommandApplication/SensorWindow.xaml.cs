using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Net.WebSockets;
using LiveCharts;
using LiveCharts.Wpf;
using CommandApplication.Model;
using System.Globalization;
using Newtonsoft.Json;
using LiveCharts.Geared;

namespace CommandApplication
{
    /// <summary>
    /// Interaction logic for SensorWindow.xaml
    /// </summary>
    public partial class SensorWindow : Window
    {
        
        //private const string UrlBase = "ws://129.242.174.142:8080/";
        private const string UrlBase = "ws://" + Constants.ServerAddressDemo + ":8080/";
        private readonly ClientWebSocket socket_temp;
        private readonly ClientWebSocket socket_pressure;
        private readonly ClientWebSocket socket_humidity;
        private readonly ClientWebSocket socket_orientation;
        private readonly ClientWebSocket socket_acceleration;
        private readonly ClientWebSocket socket_gps;
        private readonly ClientWebSocket socket_ir1;
        private readonly ClientWebSocket socket_ir2;

        private const string Start = "START";
        private const string Stop = "STOP";
        private const string Temp = "temp";
        private const string Pressure = "pressure";
        private const string Humidity = "humidity";
        private const string Orientation = "orientation";
        private const string Acceleration = "acceleration";
        private const string Gps = "gps";
        private const string IR1 = "irtemp1";
        private const string IR2 = "irtemp2";

        private int RollForCalibrating = 0;
        private int PitchForCalibrating = 0;
        private int YawForCalibrating = 0;

        private static bool connected = false;

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

        public SensorWindow()
        {
            InitializeComponent();
            socket_temp = new ClientWebSocket();
            socket_pressure = new ClientWebSocket();
            socket_humidity = new ClientWebSocket();
            socket_acceleration = new ClientWebSocket();
            socket_orientation = new ClientWebSocket();
            socket_gps = new ClientWebSocket();
            socket_ir1 = new ClientWebSocket();
            socket_ir2 = new ClientWebSocket();

            string curDir = System.IO.Directory.GetCurrentDirectory();
            serverStatus.Visibility = Visibility.Hidden;

            

            line_temp = new GLineSeries
            {
                Title = "Temperature",
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
                
            };
            line_pressure = new GLineSeries
            {
                Title = "Pressure",
                Values = new GearedValues<double> {  }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };
            line_humidity = new GLineSeries
            {
                Title = "Humidity",
                Values = new GearedValues<double> {  }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };

            lineX = new GLineSeries
            {
                Title = "AccX",
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };
            lineY = new GLineSeries
            {
                Title = "AccY",
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };
            lineZ = new GLineSeries
            {
                Title = "AccZ",
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };

            line_roll = new GLineSeries
            {
                Title = "Roll",
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };
            line_pitch = new GLineSeries
            {
                Title = "Pitch",
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
            };
            line_yaw = new GLineSeries
            {
                Title = "Yaw",
                Values = new GearedValues<double> { }.WithQuality(Quality.Medium),
                PointGeometry = null,
                Fill = Brushes.Transparent
                
        };


            //tempChart.Series.Add(line_temp);
            
            accXChart.Series.Add(lineX);
            accYChart.Series.Add(lineY);
            accZChart.Series.Add(lineZ);

            rollChart.Series.Add(line_roll);
            pitchChart.Series.Add(line_pitch);
            yawChart.Series.Add(line_yaw);

            //BrowserMT.Address = new Uri(String.Format("file:///{0}/Views/marinetrafficmap.html", curDir)).ToString();

            StartReceiveFromServer(this, socket_pressure, Pressure);
            StartReceiveFromServer(this, socket_temp, Temp);
            StartReceiveFromServer(this, socket_humidity, Humidity);

            StartReceiveFromServer(this, socket_acceleration, Acceleration);

            StartReceiveFromServer(this, socket_orientation, Orientation);

            StartReceiveFromServer(this, socket_gps, Gps);

            StartReceiveFromServer(this, socket_ir1, IR1);
            StartReceiveFromServer(this, socket_ir2, IR2);


            accXChart.Visibility = Visibility.Collapsed;
            accYChart.Visibility = Visibility.Collapsed;
            accZChart.Visibility = Visibility.Collapsed;
            
            rollChart.Visibility = Visibility.Visible;
            pitchChart.Visibility = Visibility.Visible;
            yawChart.Visibility = Visibility.Visible;

            accXTitle.Visibility = Visibility.Collapsed;
            accYTitle.Visibility = Visibility.Collapsed;
            accZTitle.Visibility = Visibility.Collapsed;

        }

        private static async Task StartReceiveFromServer(SensorWindow window, ClientWebSocket socket, string measurement)
        {


            System.Diagnostics.Trace.WriteLine(measurement);
            //if(measurement == Orientation)
            //{
            //    var lineX = lineX;
            //    var lineY =
            //    var lineZ = 
            //} else if (measurement == Acceleration) {
            //    var lineRoll = 
            //}
            //else
            //{
            var line = GetLines(measurement);
            
            //}
            


            //byte[] recvBuf;
            bool receiving = false;
            int keepRecords = 150;
            bool firstRecord = true;
            
            Uri uri = new Uri(UrlBase + measurement);

            try
            {
                await socket.ConnectAsync(uri, System.Threading.CancellationToken.None);
                window.serverStatus.Visibility = Visibility.Visible;
                window.serverStatus.Foreground = new SolidColorBrush(Colors.Green);
                window.serverStatus.Content = "Connected to server";
                connected = true;
                receiving = true;
            } catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
                window.serverStatus.Content = e.Message;
                window.serverStatus.Visibility = Visibility.Visible;
                
            }
            
            //if(measurement == Acceleration || measurement == Orientation)
            //{
            //    recvBuf = new byte[32];
            //}
            //else
            //{
            //    recvBuf = new byte[32];
            //}
            
            //var recvSeg = new ArraySegment<byte>(recvBuf);

            var sendBuf = new byte[16];
            
            sendBuf = GetBytes(Start); //HACK
            
            var sendSeg = new ArraySegment<byte>(sendBuf);

            await socket.SendAsync(sendSeg, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);


            while (receiving)
            {
                string stringResult = "";
                var recvBuf = new byte[128]; //TODO: Should change based on response. GPS needs bigger than rest.
                var recvSeg = new ArraySegment<byte>(recvBuf);
                var result = await socket.ReceiveAsync(recvSeg, System.Threading.CancellationToken.None);
                var resultArray = recvSeg.Take(recvSeg.Count).ToArray();
                resultArray = RemoveTrailingZeros(resultArray);
                stringResult += Encoding.UTF8.GetString(resultArray);
                if (measurement == Orientation || measurement == Acceleration)
                {
                    //System.Diagnostics.Trace.WriteLine(stringResult);
                    var res = SplitXYZ(stringResult);
                    if(measurement == Orientation)
                    {
                        
                        

                        var pitch = Convert.ToDouble(res[0], CultureInfo.InvariantCulture);
                        var roll = Convert.ToDouble(res[1], CultureInfo.InvariantCulture);
                        var yaw = Convert.ToDouble(res[2], CultureInfo.InvariantCulture);

                        //System.Diagnostics.Trace.WriteLine("orient: " + pitch + " " + roll + " " + yaw);

                        //Account for calibration
                        pitch = pitch - window.PitchForCalibrating;
                        if(pitch > 180) { pitch = pitch - 360; }
                        roll = roll - window.RollForCalibrating;
                        if(roll > 180) { roll = roll - 360; }
                        yaw = yaw - window.YawForCalibrating;
                        if(yaw > 180) { yaw = yaw - 360; }

                        window.Dispatcher.Invoke(new Action(() => {
                            window.rollLabel.Content = roll;
                            window.pitchLabel.Content = pitch;
                            window.yawLabel.Content = yaw;
                        }));
                        


                        //Automatic calibration on startup
                        if(firstRecord)
                        {
                            window.CalibrateOrientations();

                            //Account for calibration
                            pitch = pitch - window.PitchForCalibrating;
                            if (pitch > 180) { pitch = pitch - 360; }
                            roll = roll - window.RollForCalibrating;
                            if (roll > 180) { roll = roll - 360; }
                            yaw = yaw - window.YawForCalibrating;
                            if (yaw > 180) { yaw = yaw - 360; }

                            window.rollLabel.Content = roll;
                            window.pitchLabel.Content = pitch;
                            window.yawLabel.Content = yaw;

                            firstRecord = false;
                        }

                        //line_yaw.Values.Remove(first_yaw);

                        if (line_roll.Values.Count < keepRecords)
                        {
                            try
                            {
                                window.Dispatcher.Invoke(new Action(() => {
                                    line_roll.Values.Add(roll);
                                    line_pitch.Values.Add(pitch);
                                    line_yaw.Values.Add(yaw);
                                }));
                                
                            }
                            catch (Exception e)
                            {
                                System.Diagnostics.Trace.WriteLine("Feilen: " + e);
                                System.Diagnostics.Trace.WriteLine("Tallet er: " + roll);
                                System.Diagnostics.Trace.WriteLine("Tallet er: " + pitch);
                                System.Diagnostics.Trace.WriteLine("Tallet er: " + yaw);
                            }
                        }
                        if (line_roll.Values.Count > keepRecords - 1)
                        {
                            var first_roll = line_roll.Values[0]; //TODO: Thread-safe?
                            var first_pitch = line_pitch.Values[0];
                            var first_yaw = line_yaw.Values[0];
                            //var first_yaw = line_yaw.Values.DefaultIfEmpty(0).FirstOrDefault();

                            window.Dispatcher.Invoke(new Action(() => {
                                line_roll.Values.Remove(first_roll);
                                line_pitch.Values.Remove(first_pitch);
                                line_yaw.Values.Remove(first_yaw);
                            }));
                            
                        }
                        
                        
                    }
                    else //Acceleration
                    {
                        var x = res[0];
                        var y = res[1];
                        var z = res[2];

                        window.Dispatcher.Invoke(new Action(() => {
                            window.accxLabel.Content = x;
                            window.accyLabel.Content = y;
                            window.acczLabel.Content = z;
                        }));

                        

                        if (lineX.Values.Count < keepRecords)
                        {
                            window.Dispatcher.Invoke(new Action(() => {
                                lineX.Values.Add(Convert.ToDouble(x, CultureInfo.InvariantCulture));
                                lineY.Values.Add(Convert.ToDouble(y, CultureInfo.InvariantCulture));
                                lineZ.Values.Add(Convert.ToDouble(z, CultureInfo.InvariantCulture));
                            }));
                            
                        }
                        if(lineX.Values.Count > keepRecords - 1)
                        {
                            var first_X = lineX.Values[0]; //TODO: Thread-safe?
                            var first_Y = lineY.Values[0];
                            var first_Z = lineZ.Values[0];

                            
                            window.Dispatcher.Invoke(new Action(() => {
                                lineX.Values.Remove(first_X);
                                lineY.Values.Remove(first_Y);
                                lineZ.Values.Remove(first_Z);
                            }));

                        }
                        

                    }
                }
                else if(measurement == Gps)
                {
                    //JSON
                    System.Diagnostics.Trace.WriteLine(stringResult);
                    try
                    {
                        GPS GPS = JsonConvert.DeserializeObject<GPS>(stringResult);
                        window.latLabel.Content = GPS.latitude;
                        window.longLabel.Content = GPS.longitude;
                        window.speedLabel.Content = GPS.hspeed;
                        window.altLabel.Content = GPS.alt;
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex);
                    }
                    
                }
                
                else
                {
                    System.Diagnostics.Trace.WriteLine(measurement);
                    window.Dispatcher.Invoke(new Action(() => {
                        //line.Values.Add(Convert.ToDouble(stringResult, CultureInfo.InvariantCulture));
                        switch (measurement)
                        {
                            case Temp:
                                window.tempLabel.Content = stringResult;
                                break;
                            case Pressure:
                                window.presLabel.Content = stringResult;
                                break;
                            case Humidity:
                                window.humLabel.Content = stringResult;
                                break;
                            case IR1:
                                window.irLabel.Content = stringResult;
                                break;
                            case IR2:
                                window.ir2Label.Content = stringResult;
                                break;
                            default:
                                break;
                        }
                    }));
                    
                }
                


            }
            //System.Diagnostics.Trace.WriteLine(Convert.ToDouble(stringResult, CultureInfo.InvariantCulture));

        }

        private static LineSeries GetLines(string measurement)
        {
            switch(measurement)
            {
                case Temp:
                    return line_temp;
                case Pressure:
                    return line_pressure;
                case Humidity:
                    return line_humidity;
                case Orientation:
                    return null;
                case Acceleration:
                    return null;
                default:
                    return null;
            }
            
        }

        private static async Task DisconnectFromServer(SensorWindow window, ClientWebSocket socket, string measurement)
        {
            var sendBuf = new byte[16];
            sendBuf = GetBytes(Stop);
            var sendSeg = new ArraySegment<byte>(sendBuf);
            await socket.SendAsync(sendSeg, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
            connected = false; //Need to wait for await?

        }
        static byte[] GetBytes(string str)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(str);
            return bytes;
        }
        static string[] SplitXYZ(string resString)
        {
            var arr = resString.Split(null); //Whitespace split
            return arr;
        }

        private static byte[] RemoveTrailingZeros(byte[] input)
        {
            int res = 0;
            for(int i=input.Length - 1; i>=0 ; i--)
            {
                if(input[i] != 0)
                {
                    res = i;
                    break;
                }
            }
            var output = input.Take(res + 1).ToArray();
            return output;
        }


        private void Button_Disconnect(object sender, RoutedEventArgs e)
        {
            DisconnectFromServer(this, socket_temp, Temp);
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
    }
}
