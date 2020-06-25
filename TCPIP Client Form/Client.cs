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

namespace TCPIP_Client_Form
{
    public partial class Client : Form
    {
        string newLine = Environment.NewLine;
        public TcpListener listener;
        public TcpClient client;
        public NetworkStream nwStream;
        const string SERVER_IP = "127.0.0.1";
        const int PORT_NO = 5000;

        public Client()
        {
            InitializeComponent();
            this.Text = "CLIENT";
            ClientIPtextBox.Text = SERVER_IP;
            ClientPorttextBox.Text = PORT_NO.ToString();
        }
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            try
            {
                client = new TcpClient(SERVER_IP, PORT_NO);
            }
            catch(Exception)
            {
                MessagetextBox.AppendText("ERROR! Server not found.");
                MessageBox.Show("ERROR! Server not found.");
                this.Close();
            }
            
            nwStream = client.GetStream();

            if (client.Connected)
            {
                MessagetextBox.AppendText("Connected to server!" + newLine);
                ConnectButton.Enabled = false;
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            string textToSend = SendChattextBox.Text;
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
            MessagetextBox.AppendText("SENDING: " + textToSend + newLine);
            try
            {
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            }
            catch(Exception)
            {
                MessagetextBox.AppendText("ERROR! Not connected to a server." + newLine);
                MessageBox.Show("ERROR! Not connected to a server.");
                this.Close();
            }

            byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
            MessagetextBox.AppendText("SERVER: " + Encoding.ASCII.GetString(bytesToRead, 0, bytesRead) + newLine);
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            MessagetextBox.AppendText("Connection Terminated");
            client.Close();
            this.Close();
        } 
    }
}
