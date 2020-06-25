using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace TCPIP_Server_Form
{
    public partial class Server : Form
    {
        string newLine = Environment.NewLine;
        public TcpListener listener;
        public TcpClient client;
        public NetworkStream nwStream;
        public byte[] buffer;
        public int bytesRead;
            
        public Server()
        {
            InitializeComponent();
            this.Text = "SERVER";
            
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            IPAddress localAdd = IPAddress.Parse(ServerIPtextBox.Text);
            listener = new TcpListener(localAdd, int.Parse(ServerPorttextBox.Text));
            StartButton.Enabled = false;
            MessagetextBox.AppendText("Listening..." + newLine);
            listener.Start();
            client = listener.AcceptTcpClient();
            nwStream = client.GetStream();
            buffer = new byte[client.ReceiveBufferSize];
            bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            MessagetextBox.AppendText("CLIENT: " + dataReceived + newLine);
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string textToSend = SendChattextBox.Text;
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
            MessagetextBox.AppendText("SENDING: " + textToSend + newLine);
            nwStream.Write(bytesToSend, 0, textToSend.Length);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            MessagetextBox.AppendText("Connection Terminated");
            client.Close();
            listener.Stop();
            this.Close();
        }
    }
}
