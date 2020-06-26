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
        public NetworkStream nwStream;
        public byte[] buffer;
        public int bytesRead;
        public StreamReader STR;
        public StreamWriter STW;
        public string receive;
        public string TextToSend;

            
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
            STW = new StreamWriter(client.GetStream());
            STR = new StreamReader(client.GetStream());
            STW.AutoFlush = true;
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker2.WorkerSupportsCancellation = true;

            #region Alternate Test
            /*nwStream = client.GetStream();
            buffer = new byte[client.ReceiveBufferSize];
            bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            MessagetextBox.AppendText("CLIENT: " + dataReceived + newLine);*/
            #endregion
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            #region Alternate Test
            /*string textToSend = SendChattextBox.Text;
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
            SendChattextBox.Text = "";
            MessagetextBox.AppendText("SENDING: " + textToSend + newLine);
            nwStream.Write(bytesToSend, 0, textToSend.Length);*/
            #endregion

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
            if (client.Connected)
            {
                STW.WriteLine(TextToSend);
                this.MessagetextBox.Invoke(new MethodInvoker(delegate ()
                {
                    MessagetextBox.AppendText("SENDING: " + TextToSend + newLine);
                }));
            }
            backgroundWorker2.CancelAsync();
        }
    }
}
