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
            //this.serverStatus.Visibility = Visibility.Hidden;

            //BrowserMT.Address = new Uri(String.Format("file:///{0}/Views/marinetrafficmap.html", curDir)).ToString();

            NewMethod(this);
            
        }

        private static async Task NewMethod(SensorWindow window)
        {
            ClientWebSocket socket = new ClientWebSocket();
            Uri uri = new Uri("ws://129.242.174.142:8080/temp");
            try
            {
                await socket.ConnectAsync(uri, System.Threading.CancellationToken.None);
            } catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.Message);
                window.serverStatus.Name = e.Message;
                window.serverStatus.Visibility = Visibility.Visible;
                
            }
            
            var buffer = new byte[1024];
            var segment = new ArraySegment<byte>(buffer);
            var res = await socket.ReceiveAsync(segment, System.Threading.CancellationToken.None);
            System.Diagnostics.Trace.WriteLine("Resultatet er: " + res);
        }

    }
}
