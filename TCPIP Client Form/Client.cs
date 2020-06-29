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
using System.IO;

namespace TCPIP_Client_Form
{
    public partial class Client : Form
    {
        string newLine = Environment.NewLine;
        public TcpClient client;
        const string SERVER_IP = "127.0.0.1";
        const int PORT_NO = 5000;
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        public string TextToSend;

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

            if (client.Connected)
            {
                MessagetextBox.AppendText("Connected to server!" + newLine);
                ConnectButton.Enabled = false;
                STW = new StreamWriter(client.GetStream());
                STR = new StreamReader(client.GetStream());
                STW.AutoFlush = true;
                backgroundWorker1.RunWorkerAsync();
                backgroundWorker2.WorkerSupportsCancellation = true;
            }
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            if (SendChattextBox.Text != "")
            {
                TextToSend = SendChattextBox.Text;
                backgroundWorker2.RunWorkerAsync();
            }
            SendChattextBox.Text = "";
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            MessagetextBox.AppendText("Connection Terminated");
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.CancelAsync();
            client.Close();  
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                receive = STR.ReadLine();
                this.MessagetextBox.Invoke(new MethodInvoker(delegate ()
                {
                    MessagetextBox.AppendText("SERVER: " + receive + newLine);
                }));
                receive = "";
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (client.Connected)
                {
                    STW.WriteLine(TextToSend);
                    this.MessagetextBox.Invoke(new MethodInvoker(delegate ()
                    {
                        MessagetextBox.AppendText("SENDING: " + TextToSend + newLine);
                    }));
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Not connected to a server");
                this.Close();
            }
            backgroundWorker2.CancelAsync();
        }
    }
}
