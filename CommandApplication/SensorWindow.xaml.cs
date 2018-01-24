using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.WebSockets;

namespace CommandApplication
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SensorWindow : Window
    {
        

        public SensorWindow()
        {
            InitializeComponent();
            string curDir = System.IO.Directory.GetCurrentDirectory();
            this.serverStatus.Visibility = Visibility.Hidden;

            //BrowserMT.Address = new Uri(String.Format("file:///{0}/Views/marinetrafficmap.html", curDir)).ToString();

            NewMethod(this);
            
        }

        private static async Task NewMethod(SensorWindow window)
        {
            ClientWebSocket socket = new ClientWebSocket();
            
            bool receiving = false;
            int count = 0;
            Uri uri = new Uri("ws://129.242.174.142:8080/temp");

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
            var sendSeg = new ArraySegment<byte>(sendBuf);

            await socket.SendAsync(sendSeg, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);

            while (receiving)
            {
                string stringResult = "";
                var result = await socket.ReceiveAsync(recvSeg, System.Threading.CancellationToken.None);
                var resultArray = recvSeg.Take(recvSeg.Count).ToArray();
                resultArray = window.RemoveTrailingZeros(resultArray);
                stringResult += Encoding.UTF8.GetString(resultArray);
                count++;
                if(count > 10)
                {
                    receiving = false;
                }
                //System.Diagnostics.Trace.WriteLine("Resultatet er: " + stringResult.ToString() + "\n");
                window.listTemp.Items.Add(stringResult.ToString());
            }
            


            
        }
        private byte[] RemoveTrailingZeros(byte[] input)
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
    }
}
