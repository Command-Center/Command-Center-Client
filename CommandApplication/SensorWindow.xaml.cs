using CommandApplication.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CommandApplication
{
    /// <summary>
    /// Interaction logic for SensorWindow.xaml
    /// </summary>
    public partial class SensorWindow : Window
    {
        string curDir = System.IO.Directory.GetCurrentDirectory();
        public SensorWindow()
        {
            InitializeComponent();
            GetAISData();
            Browser.Address = new Uri(String.Format("file:///{0}/Views/openlayermap.html", curDir)).ToString();
        }
        public void GetAISData()
        {
            Trace.WriteLine("AIS!");
            
            //string decoderAddress = String.Format("file:///{0}/aisdecode.jar", curDir);
            Process p = new Process();
            //p.StartInfo.WorkingDirectory = curDir;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/c java.exe -jar aisdecode.jar";
            p.StartInfo.RedirectStandardError = false;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            string message;
            while ((message = p.StandardOutput.ReadLine()) != null)
            {
                if(message.Contains("Received AIS message:"))
                {
                    AIS ais = new AIS(message.Trim().Substring(22));
                    Trace.WriteLine(ais.res);
                }
                
            }
            p.WaitForExit();

        }
    }
}
