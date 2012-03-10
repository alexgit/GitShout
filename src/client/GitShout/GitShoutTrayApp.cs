using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace GitShout
{
    public partial class GitShoutTrayApp : Form
    {
        

        private readonly ContextMenu trayMenu;

        private const int DEFAULT_PORT = 9898;
        private NetworkStream netStream;
        

        /// <summary>
        /// The message header contains the length of the message that follows it.
        /// The the 4 byte array contains a little endian integer representation of the length.
        /// </summary>
        private readonly byte[] messageHeader = new byte[4];
        private byte[] messageBuffer;
        private event MessageProcessedEventHandler MessageProcessed;
        private readonly IMessageFormatter messageFormatter;

        //security hole
        private IEnumerable<string> commitURLs;

        public GitShoutTrayApp(IMessageFormatter messageFormatter)
        {
            InitializeComponent();

            this.messageFormatter = messageFormatter;

            txtServer.Text = "192.168.0.95";
            txtPortNumber.Text = "9898";
            chkUseDefaultPort.Checked = true;

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            trayIcon.Text = "GitShout";
            trayIcon.BalloonTipClicked += OnBalloonClicked;
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Visible = true;
            
            MessageProcessed += ReadMessageHeader;
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            var server = txtServer.Text;

            int port;
            if (chkUseDefaultPort.Checked)
            {
                port = DEFAULT_PORT;                
            } 
            else
            {
                Int32.TryParse(txtPortNumber.Text, out port);
            }

            this.Visible = false;
            TcpClient client = null;
            try
            {
                client = new TcpClient(server, port);
            }
            catch (SocketException ex)
            {
                MessageBox.Show("Could not connect to the server: " + ex.Message);
                this.Visible = true;
                return;
            }

            netStream = client.GetStream();
            Listen();
        }

        private void Listen()
        {
            ReadMessageHeader(this, null);
        }

        private void ReadMessageHeader(object source, EventArgs args)
        {
            netStream.BeginRead(messageHeader, 0, messageHeader.Length, OnMessageHeaderRead, null);
        }

        private void OnMessageHeaderRead(IAsyncResult result)
        {
            netStream.EndRead(result);
            var messageLength = BitConverter.ToInt32(messageHeader, 0);            

            ReadMessageBody(messageLength);
        }
        
        private void ReadMessageBody(int messageLength)
        {
            messageBuffer = new byte[messageLength];
            netStream.BeginRead(messageBuffer, 0, messageBuffer.Length, OnBodyRead, null);
        }
        
        public void OnBodyRead(IAsyncResult result)
        {
            netStream.EndRead(result);
            ShowNotification();
            MessageProcessed(this, null);
        }

        private void ShowNotification()
        {           
            var payload = System.Text.Encoding.UTF8.GetString(messageBuffer);
            var commitMessage = JsonConvert.DeserializeObject<CommitMessage>(payload);
            var textToDisplay = messageFormatter.Format(commitMessage);
            commitURLs = commitMessage.Commits.Select(x => x.Url);
            
            trayIcon.BalloonTipText = textToDisplay;
            trayIcon.ShowBalloonTip(30 * 1000);
        }
        
        private void OnExit(object source, EventArgs args)
        {
            trayIcon.Dispose();
            Application.Exit();
        }

        private void OnBalloonClicked(object source, EventArgs eventArgs)
        {
            foreach (var url in commitURLs)
            {
                System.Diagnostics.Process.Start(url);   
            }
        }

        private void chkUseDefaultPort_CheckedChanged(object sender, EventArgs e)
        {
            if(chkUseDefaultPort.Checked)
            {
                txtPortNumber.Hide();
                lblPortNumber.Hide();
            } 
            else
            {
                txtPortNumber.Show();
                lblPortNumber.Show();
            }
        }                
    }

    internal delegate void MessageProcessedEventHandler(object sender, EventArgs args);
}
