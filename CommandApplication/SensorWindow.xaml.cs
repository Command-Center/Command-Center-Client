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

        public SensorWindow()
        {
            InitializeComponent();
            socket_temp = new ClientWebSocket();
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


            tempChart.Series.Add(line_temp);

            //BrowserMT.Address = new Uri(String.Format("file:///{0}/Views/marinetrafficmap.html", curDir)).ToString();

            StartReceiveFromServer(this, socket_temp, Temp);
            
        }

        private static async Task StartReceiveFromServer(SensorWindow window, ClientWebSocket socket, string measurement)
        {
            var line = GetLines(measurement);
            
            bool receiving = false;
            int count = 0;
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
            var recvBuf = new byte[32];
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
                
                //window.listTemp.Items.Add(stringResult.ToString());
                line.Values.Add(Convert.ToDouble(stringResult, CultureInfo.InvariantCulture));
                
                //System.Diagnostics.Trace.WriteLine(Convert.ToDouble(stringResult, CultureInfo.InvariantCulture));
            }
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
