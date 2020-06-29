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

namespace TCPIP_Server_Form
{
    public partial class Server : Form
    {
        string newLine = Environment.NewLine;
        public TcpListener listener;
        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        public string TextToSend;
        public const string SERVER_IP = "127.0.0.1";
        public const int PORT_NO = 5000;


        public Server()
        {
            InitializeComponent();
            this.Text = "SERVER";
            ServerIPtextBox.Text = SERVER_IP;
            ServerPorttextBox.Text = PORT_NO.ToString();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                IPAddress localAdd = IPAddress.Parse(ServerIPtextBox.Text); 
                listener = new TcpListener(IPAddress.Any, int.Parse(ServerPorttextBox.Text));
            }
            catch (Exception ex)
            {
                MessagetextBox.AppendText(ex.Message.ToString());
                MessageBox.Show(ex.Message.ToString());
                this.Close();
            } 
            
            StartButton.Enabled = false;
            MessagetextBox.AppendText("Listening..." + newLine);
            listener.Start();
            client = listener.AcceptTcpClient();
            if (client.Connected)
            {
                MessagetextBox.AppendText("Client connected." + newLine);
            }
            STW = new StreamWriter(client.GetStream());
            STR = new StreamReader(client.GetStream());
            STW.AutoFlush = true;
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.WorkerSupportsCancellation = true; 
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
            listener.Stop();
            this.Close();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (client.Connected)
            {
                receive = STR.ReadLine();
                this.MessagetextBox.Invoke(new MethodInvoker(delegate ()
                {
                    MessagetextBox.AppendText("CLIENT: " + receive + newLine);
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
                MessageBox.Show("No client detected");
                this.Close();
            }
            
            backgroundWorker2.CancelAsync();
        }
    }
}
