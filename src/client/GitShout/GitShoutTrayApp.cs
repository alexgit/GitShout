using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace GitShout
{
    public partial class GitShoutTrayApp : Form
    {
        private const string SERVER_CFG_FILE = "serv.cfg";
        private const int DEFAULT_PORT = 9898;
        private const string APP_NAME = "GitShout";
        private const string COULD_NOT_CONNECT_MESSAGE = "Failed to connect to the GitShout server. Check the server name is correct and make sure it accepts connections on port {0}.";
        
        private readonly ContextMenu trayMenu;
        private GitShoutClient gitShoutClient;
        private readonly IMessageFormatter messageFormatter;        
        private IEnumerable<string> commitURLs;
        
        public GitShoutTrayApp(IMessageFormatter messageFormatter)
        {
            InitializeComponent();

            this.messageFormatter = messageFormatter;

            txtServer.Text = GetLastServer();
            txtPortNumber.Text = DEFAULT_PORT.ToString();
            chkUseDefaultPort.Checked = true;

            trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);
            trayIcon.ContextMenu = trayMenu;
            trayIcon.Text = APP_NAME;
            trayIcon.BalloonTipClicked += OpenCommitsInBrowser;                  
        }       

        private void btnConnect_Click(object sender, EventArgs e)
        {
            var server = txtServer.Text;

            int port;
            if (chkUseDefaultPort.Checked)
                port = DEFAULT_PORT;
            else
                Int32.TryParse(txtPortNumber.Text, out port);

            try
            {
                SaveServer(server);
                ConnectToServer(server, port);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format(COULD_NOT_CONNECT_MESSAGE, port),
                        "Could not connect to GitShout server.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                
            }
        }

        private void SaveServer(string server)
        {
            try
            {
                File.WriteAllText(SERVER_CFG_FILE, server);
            }
            catch (Exception e) 
            {
                CouldntSaveServer(string.Format("({0})", e.Message));
            }            
        }

        private string GetLastServer()
        {
            string result = "";
            try { result = File.ReadAllText(SERVER_CFG_FILE); } catch (Exception) { }

            return result;
        }

        private void CouldntSaveServer(string reason) 
        {
            MessageBox.Show(this, string.Format("Couldn't save the the server for next time. {0}", reason),
                        "Couldn't save server.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void ConnectToServer(string server, int port)
        {
            gitShoutClient = new GitShoutClient(server, port);
            gitShoutClient.OnCommit(ShowBalloonTip);

            try
            {
                gitShoutClient.Start();
            }
            catch (InvalidMessageFormatException e) 
            {
                //TODO: log this
            }

            this.Visible = false;
        }
        
        private void OnExit(object source, EventArgs args)
        {
            trayIcon.Dispose();
            Application.Exit();            
        }

        private void ShowBalloonTip(CommitMessage commitMessage)
        {
            var formattedMessage = messageFormatter.Format(commitMessage);
            commitURLs = commitMessage.Commits.Select(x => x.Url);

            trayIcon.BalloonTipText = formattedMessage;
            trayIcon.ShowBalloonTip(30 * 1000);
        }

        private void OpenCommitsInBrowser(object source, EventArgs eventArgs)
        {
            foreach (var url in commitURLs)
            {
                // This opens the URL using the default browser. Security hole? :)
                System.Diagnostics.Process.Start(url);   
            }
        }

        private void chkUseDefaultPort_CheckedChanged(object sender, EventArgs e)
        {
            // ugh!
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
}
