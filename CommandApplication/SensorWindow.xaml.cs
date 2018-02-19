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

namespace CommandApplication
{
    /// <summary>
    /// Interaction logic for SensorWindow.xaml
    /// </summary>
    public partial class SensorWindow : Window
    {
        private const string Start = "START";
        private const string Stop = "STOP";
        private const string UrlBase = "ws://129.242.174.142:8080/";
        private readonly ClientWebSocket socket_temp;
        private readonly ClientWebSocket socket_pressure;
        private readonly ClientWebSocket socket_humidity;
        private readonly ClientWebSocket socket_orientation;
        private readonly ClientWebSocket socket_acceleration;

        private const string Temp = "temp";
        private const string Pressure = "pressure";
        private const string Humidity = "humidity";
        private const string Orientation = "orientation";
        private const string Acceleration = "acceleration";

        public SeriesCollection SeriesCollection { get; set; }

        static LineSeries line_temp;
        static LineSeries line_pressure;
        static LineSeries line_humidity;
        static LineSeries line_orientation;
        static LineSeries line_acceleration;
        static LineSeries lineX;
        static LineSeries lineY;
        static LineSeries lineZ;
        static LineSeries line_roll;
        static LineSeries line_pitch;
        static LineSeries line_yaw;

        public SensorWindow()
        {
            InitializeComponent();
            socket_temp = new ClientWebSocket();
            socket_pressure = new ClientWebSocket();
            socket_humidity = new ClientWebSocket();
            socket_acceleration = new ClientWebSocket();
            socket_orientation = new ClientWebSocket();

            string curDir = System.IO.Directory.GetCurrentDirectory();
            this.serverStatus.Visibility = Visibility.Hidden;

            line_temp = new LineSeries
            {
                Title = "Temperature",
                Values = new ChartValues<double> {  }
            };
            line_pressure = new LineSeries
            {
                Title = "Pressure",
                Values = new ChartValues<double> {  }
            };
            line_humidity = new LineSeries
            {
                Title = "Humidity",
                Values = new ChartValues<double> {  }
            };

            lineX = new LineSeries
            {
                Title = "AccX",
                Values = new ChartValues<double> { }
            };
            lineY = new LineSeries
            {
                Title = "AccY",
                Values = new ChartValues<double> { }
            };
            lineZ = new LineSeries
            {
                Title = "AccZ",
                Values = new ChartValues<double> { }
            };

            line_roll = new LineSeries
            {
                Title = "Roll",
                Values = new ChartValues<double> { }
            };
            line_pitch = new LineSeries
            {
                Title = "Pitch",
                Values = new ChartValues<double> { }
            };
            line_yaw = new LineSeries
            {
                Title = "Yaw",
                Values = new ChartValues<double> { }
            };


            //tempChart.Series.Add(line_temp);
            //presChart.Series.Add(line_pressure);
            //humChart.Series.Add(line_humidity);
            //accXChart.Series.Add(lineX);
            //accYChart.Series.Add(lineY);
            //accZChart.Series.Add(lineZ);

            rollChart.Series.Add(line_roll);
            pitchChart.Series.Add(line_pitch);
            yawChart.Series.Add(line_yaw);

            //BrowserMT.Address = new Uri(String.Format("file:///{0}/Views/marinetrafficmap.html", curDir)).ToString();

            //StartReceiveFromServer(this, socket_pressure, Pressure);
            //StartReceiveFromServer(this, socket_temp, Temp);
            //StartReceiveFromServer(this, socket_humidity, Humidity);

            //StartReceiveFromServer(this, socket_acceleration, Acceleration);

            StartReceiveFromServer(this, socket_orientation, Orientation);

        }

        private static async Task StartReceiveFromServer(SensorWindow window, ClientWebSocket socket, string measurement)
        {
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
            


            byte[] recvBuf;
            bool receiving = false;
            int keepRecords = 40;
            
            Uri uri = new Uri(UrlBase + measurement);

            try
            {
                await socket.ConnectAsync(uri, System.Threading.CancellationToken.None);
                window.serverStatus.Visibility = Visibility.Visible;
                window.serverStatus.Foreground = new SolidColorBrush(Colors.Green);
                window.serverStatus.Content = "Connected to server";
                receiving = true;
            } catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
                window.serverStatus.Content = e.Message;
                window.serverStatus.Visibility = Visibility.Visible;
                
            }
            
            if(measurement == Acceleration || measurement == Orientation)
            {
                recvBuf = new byte[128];
            }
            else
            {
                recvBuf = new byte[32];
            }
            
            var recvSeg = new ArraySegment<byte>(recvBuf);

            var sendBuf = new byte[16];
            
            sendBuf = GetBytes(Start); //HACK
            
            var sendSeg = new ArraySegment<byte>(sendBuf);

            await socket.SendAsync(sendSeg, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);


            while (receiving)
            {
                string stringResult = "";
                var result = await socket.ReceiveAsync(recvSeg, System.Threading.CancellationToken.None);
                var resultArray = recvSeg.Take(recvSeg.Count).ToArray();
                resultArray = RemoveTrailingZeros(resultArray);
                stringResult += Encoding.UTF8.GetString(resultArray);
                if (measurement == Orientation || measurement == Acceleration)
                {
                    var res = SplitXYZ(stringResult);
                    if(measurement == Orientation)
                    {
                        var pitch = res[0]; //TODO: Need to confirm order
                        var roll = res[1];
                        var yaw = res[2];

                        

                        //line_yaw.Values.Remove(first_yaw);

                        if (line_roll.Values.Count < keepRecords)
                        {
                            line_roll.Values.Add(Convert.ToDouble(roll, CultureInfo.InvariantCulture));
                            line_pitch.Values.Add(Convert.ToDouble(pitch, CultureInfo.InvariantCulture));
                            line_yaw.Values.Add(Convert.ToDouble(yaw, CultureInfo.InvariantCulture));
                        }
                        if (line_roll.Values.Count > keepRecords - 1)
                        {
                            var first_roll = line_roll.Values[0]; //TODO: Thread-safe?
                            var first_pitch = line_pitch.Values[0];
                            var first_yaw = line_yaw.Values[0];
                            //var first_yaw = line_yaw.Values.DefaultIfEmpty(0).FirstOrDefault();

                            line_roll.Values.Remove(first_roll);
                            line_pitch.Values.Remove(first_pitch);
                            line_yaw.Values.Remove(first_yaw);
                        }
                    }
                    else //Acceleration
                    {
                        var x = res[0];
                        var y = res[1];
                        var z = res[2];

                        
                        if(lineX.Values.Count < keepRecords)
                        {
                            lineX.Values.Add(Convert.ToDouble(x, CultureInfo.InvariantCulture));
                            lineY.Values.Add(Convert.ToDouble(y, CultureInfo.InvariantCulture));
                            lineZ.Values.Add(Convert.ToDouble(z, CultureInfo.InvariantCulture));
                        }
                        if(lineX.Values.Count > keepRecords - 1)
                        {
                            var first_X = lineX.Values[0]; //TODO: Thread-safe?
                            var first_Y = lineY.Values[0];
                            var first_Z = lineZ.Values[0];

                            lineX.Values.Remove(first_X);
                            lineY.Values.Remove(first_Y);
                            lineZ.Values.Remove(first_Z);
                        }
                        

                    }
                }
                else
                {
                    line.Values.Add(Convert.ToDouble(stringResult, CultureInfo.InvariantCulture));
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
                    return line_orientation;
                case Acceleration:
                    return line_acceleration;
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
    }
}
