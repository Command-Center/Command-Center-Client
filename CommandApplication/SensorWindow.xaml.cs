using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Net.WebSockets;

namespace CommandApplication
{
    /// <summary>
    /// Interaction logic for SensorWindow.xaml
    /// </summary>
    public partial class SensorWindow : Window
    {
        private const string Start = "START";
        private const string Stop = "STOP";
        private const string UrlTemp = "ws://129.242.174.142:8080/temp";
        private readonly ClientWebSocket socket;

        public SensorWindow()
        {
            InitializeComponent();
            socket = new ClientWebSocket();
            string curDir = System.IO.Directory.GetCurrentDirectory();
            this.serverStatus.Visibility = Visibility.Hidden;

            //BrowserMT.Address = new Uri(String.Format("file:///{0}/Views/marinetrafficmap.html", curDir)).ToString();

            StartReceiveTemp(this, socket);
            
        }

        private static async Task StartReceiveTemp(SensorWindow window, ClientWebSocket socket)
        {
            
            
            bool receiving = false;
            int count = 0;
            Uri uri = new Uri(UrlTemp);

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
            var recvBuf = new byte[16];
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
                count++;
                if(count > 100)
                {
                    receiving = false;
                }
                //System.Diagnostics.Trace.WriteLine("Resultatet er: " + stringResult.ToString() + "\n");
                window.listTemp.Items.Add(stringResult.ToString());
            }
        }
        private static async Task DisconnectFromServer(SensorWindow window, ClientWebSocket socket)
        {
            var sendBuf = new byte[16];
            sendBuf = GetBytes(Stop);
            var sendSeg = new ArraySegment<byte>(sendBuf);
            await socket.SendAsync(sendSeg, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
        }
        static byte[] GetBytes(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
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
            DisconnectFromServer(this, socket);
        }
    }
}
