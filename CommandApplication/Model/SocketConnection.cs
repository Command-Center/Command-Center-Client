using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace CommandApplication.Model
{
    class SocketConnection
    {
        private const string Start = "START";
        private const string Stop = "STOP";
        private readonly ClientWebSocket Socket;
        public bool Connected = false;
        public bool Receiving = false;
        public delegate void MyFunc(string s);

        public SocketConnection()
        {
            Socket = new ClientWebSocket();
        }

        public async Task<bool> Connect(Uri uri)
        {
            try
            {
                await Socket.ConnectAsync(uri, System.Threading.CancellationToken.None);
                Connected = true;
                Receiving = true;
                
            } catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                Connected = false;
            }
            return Connected;
        }
        public async Task<bool> SendMessage(string msg)
        {
            
            var SendBuffer = new byte[16];
            SendBuffer = GetBytes(msg);
            var SendSegment = new ArraySegment<byte>(SendBuffer);

            
            try
            {
                await Socket.SendAsync(SendSegment, WebSocketMessageType.Text, true, System.Threading.CancellationToken.None);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task Receivemessage(MyFunc callbackResults)
        {
            while (Receiving)
            {
                string StringResult = "";
                var RecvBuf = new byte[128]; //TODO: Should change based on response. GPS needs bigger than rest.
                var RecvSeg = new ArraySegment<byte>(RecvBuf);
                var Result = await Socket.ReceiveAsync(RecvSeg, System.Threading.CancellationToken.None);
                var ResultArray = RecvSeg.Take(RecvSeg.Count).ToArray();
                ResultArray = RemoveTrailingZeros(ResultArray);
                StringResult += Encoding.UTF8.GetString(ResultArray);
                callbackResults(StringResult);
            }
        }
        static byte[] GetBytes(string str)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(str);
            return bytes;
        }
        public static byte[] RemoveTrailingZeros(byte[] input)
        {
            int res = 0;
            for (int i = input.Length - 1; i >= 0; i--)
            {
                if (input[i] != 0)
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
