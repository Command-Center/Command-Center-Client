﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        Mqtt mqtt;
        bool isConnected;
        //ConcurrentQueue<string> incomingMessageQueue;

        public MainViewModel()
        {
            mqtt = new Mqtt();
            //incomingMessageQueue = mqtt.GetIncomingQueue();
        }
        public bool IsConnected
        {
            get { return mqtt.IsConnected(); }
            set
            {
                isConnected = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
