using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Win.TCPClient
{
    public partial class CommandTest : Form
    {
        byte[] data = new byte[4];
        string input, stringData;
        TcpClient server;


        /// <summary>
        /// 생성자
        /// </summary>
        public CommandTest()
        {
            InitializeComponent();

            data[0] = 0x3C;
            data[1] = 0x00;
            data[2] = 0x00;
            data[3] = 0x3C;
            //data[4] = 0x30;

        } // end CommandTest()

        /// <summary>
        /// Connect
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                //server = new TcpClient("192.168.0.2", 2101);
                //server = new TcpClient("192.168.0.2", 2101);
                server = new TcpClient("127.0.0.1", 9999);
            }
            catch (SocketException)
            {
                lbxSend.Items.Add("Unable to connect to server");
                return;
            }
            NetworkStream ns = server.GetStream();
            //ns.Write(Encoding.ASCII.GetBytes(data), 0, input.Length);
            //ns.Write(data, 0, 5);
            //input = tbxSendMessage.Text;
                ns.Write(data, 0, data.Length);
                ns.Flush();
            ns.Flush();

            try
            {
                data = new byte[1024];
                int recv = ns.Read(data, 0, data.Length);


                //stringData = Encoding.ASCII.GetString(data, 0, recv);
                lbxSend.Items.Add(data[0].ToString());
            }
            catch (Exception ex)
            {
                lbxSend.Items.Add(ex.Message);
            }
            


            //int recv = ns.Read(data, 0, data.Length);
            //stringData = Encoding.ASCII.GetString(data, 0, recv);
            //lbxSend.Items.Add(stringData);

            //while (true)
            //{
            //    input = Console.ReadLine();
            //    if (input == "exit")
            //        break;
            //    ns.Write(Encoding.ASCII.GetBytes(input), 0, input.Length);
            //    ns.Flush();

            //    data = new byte[1024];
            //    recv = ns.Read(data, 0, data.Length);
            //    stringData = Encoding.ASCII.GetString(data, 0, recv);
            //    lbxSend.Items.Add(stringData);
            //}
            //lbxSend.Items.Add("Disconnecting from server...");
            ns.Close();
            server.Close();

        } // end btnConnect_Click

        









    } // end class CommandTest : Form
} // end namespace Win.TCPClient
