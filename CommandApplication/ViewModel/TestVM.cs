﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandApplication.Model;

namespace CommandApplication.ViewModel
{
    class TestVM : INotifyPropertyChanged
    {
        private static string UrlBase = "ws://" + Constants.ServerAddress + ":" + Constants.ServerPort + "/";
        public delegate string MyFunc(string s);
        

        SocketConnection TemperatureSocket;
        SocketConnection PressureSocket;

        public TestVM()
        {
            TemperatureSocket = ConnectToSocket(new Uri(UrlBase + "temp"), TemperatureSocket);
            PressureSocket = ConnectToSocket(new Uri(UrlBase + "pressure"), PressureSocket);


            SendMessage(TemperatureSocket);

            SendMessage(PressureSocket);

        }

        private SocketConnection ConnectToSocket(Uri Uri, SocketConnection connection)
        {
            connection = new SocketConnection();
            connection.Connect(Uri);
            
            return connection;
        }

        private async Task SendMessage(SocketConnection connection)
        {
            int TimeOutCount = 0;
            
            while (connection.GetSocketConnection().State != System.Net.WebSockets.WebSocketState.Open)
            {
                Thread.Sleep(1000);
                TimeOutCount++;
                if (TimeOutCount > 10)
                {
                    System.Diagnostics.Trace.WriteLine("Timed out");
                    break;
                }
            }
            if (connection.GetSocketConnection().State == System.Net.WebSockets.WebSocketState.Open)
            {
                System.Diagnostics.Trace.WriteLine("Connected");
                
                await connection.SendMessage("START");
                await connection.Receivemessage(UpdateCallback);
            }
        }

        public void UpdateCallback(string s)
        {
            System.Diagnostics.Trace.WriteLine("Callback: " + s);
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
