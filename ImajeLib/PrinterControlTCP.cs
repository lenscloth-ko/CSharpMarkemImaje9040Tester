using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImajeLib
{
    public class PrinterControlTCP : IDisposable
    {
        public const byte ACK = 0x06;
        public const byte NACK = 0x15;
        public const int SecondsToWait = 10000; // 10s

        public static Socket socket;
        public static byte[] getbyte = new byte[1024];
        public static byte[] setbyte = new byte[1024];
        //private readonly IPAddress _port;
        //private readonly AutoResetEvent _ar;
        private object _lastResponse;
        private bool _disposed;
        //for TCP
        private string _Ip;
        private int _port;

        public PrinterControlTCP(string serverIp, int serverPort)
        {
            this._Ip = serverIp;
            this._port = serverPort;

            IPAddress serverIP = IPAddress.Parse(this._Ip);
            IPEndPoint serverEndPoint = new IPEndPoint(serverIP, this._port);

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect(serverEndPoint);
        } // end PrinterControlTCP

        public void SendMessage(Message message)
        {
            var request = new EnqRequest();
            request.Ident = Identifiers.TransmitNonLibraryMessage;
            request.Data = message.GetBytes();

            var response = this.InternalSendRequest(request);
            this.CheckAck(response);
        }

        private AckResponse InternalSendRequest(EnqRequest request)
        {
            int getValueLength = 0;
            string getstring = null;

            var bytes = request.GetBytes();

            socket.Receive(getbyte, 0
                , getbyte.Length, SocketFlags.None);
            getValueLength = byteArrayDefrag(getbyte);
            getstring = Encoding.UTF7.GetString(getbyte, 0
                , getValueLength + 1);

            //this._port.Write(bytes, 0, bytes.Length);

            //Thread.VolatileWrite(ref this._lastResponse, null);
            this._lastResponse = getbyte;
            //this._ar.WaitOne(SecondsToWait);

            if (this._lastResponse == null)
            {
                throw new Exception("Cannot connect to port!");
            }

            return (AckResponse)this._lastResponse;
        }

        private static int byteArrayDefrag(byte[] getbyte)
        {
            int endLength = 0;

            for (int i = 0; i < getbyte.Length; i++)
            {
                if ((byte)getbyte[i] != (byte)0)
                {
                    endLength = i;
                }
            }

            return endLength;
        }

        private void CheckAck(AckResponse response, string callName = null)
        {
            if (response.Ack == ACK)
            {
                return;
            }
            if (response.Ack == NACK)
            {
                throw new NackException();
            }
            throw new UnknownAckException();
        }

        public void Dispose()
        {
            this.Dispose(true);
        } // end Dispose()

        public virtual void Dispose(bool disposing)
        {
            if (this._disposed)
            {
                return;
            }
            if (disposing)
            {
                if (socket.Connected)
                {
                    socket.Disconnect(true);
                    socket.Dispose();
                }
            }
            this._disposed = true;
        } // end Dispose(bool disposing)
    }
}
