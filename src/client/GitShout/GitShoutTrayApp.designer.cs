namespace GitShout
{
    partial class GitShoutTrayApp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GitShoutTrayApp));
            this.lblServer = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.trayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lblPortNumber = new System.Windows.Forms.Label();
            this.txtPortNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkUseDefaultPort = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblServer
            // 
            this.lblServer.AutoSize = true;
            this.lblServer.Location = new System.Drawing.Point(47, 30);
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(38, 13);
            this.lblServer.TabIndex = 0;
            this.lblServer.Text = "Server";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(91, 27);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(108, 20);
            this.txtServer.TabIndex = 1;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(91, 79);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(100, 23);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // trayIcon
            // 
            this.trayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("trayIcon.Icon")));
            this.trayIcon.Text = "notifyIcon1";
            this.trayIcon.Visible = true;
            // 
            // lblPortNumber
            // 
            this.lblPortNumber.AutoSize = true;
            this.lblPortNumber.Location = new System.Drawing.Point(47, 56);
            this.lblPortNumber.Name = "lblPortNumber";
            this.lblPortNumber.Size = new System.Drawing.Size(36, 13);
            this.lblPortNumber.TabIndex = 0;
            this.lblPortNumber.Text = "Port #";
            // 
            // txtPortNumber
            // 
            this.txtPortNumber.Location = new System.Drawing.Point(91, 53);
            this.txtPortNumber.Name = "txtPortNumber";
            this.txtPortNumber.Size = new System.Drawing.Size(108, 20);
            this.txtPortNumber.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(219, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 0;
            // 
            // chkUseDefaultPort
            // 
            this.chkUseDefaultPort.AutoSize = true;
            this.chkUseDefaultPort.Location = new System.Drawing.Point(222, 29);
            this.chkUseDefaultPort.Name = "chkUseDefaultPort";
            this.chkUseDefaultPort.Size = new System.Drawing.Size(134, 17);
            this.chkUseDefaultPort.TabIndex = 5;
            this.chkUseDefaultPort.Text = "Use default port (9898)";
            this.chkUseDefaultPort.UseVisualStyleBackColor = true;
            this.chkUseDefaultPort.CheckedChanged += new System.EventHandler(this.chkUseDefaultPort_CheckedChanged);
            // 
            // GitShoutTrayApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 131);
            this.Controls.Add(this.chkUseDefaultPort);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.txtPortNumber);
            this.Controls.Add(this.lblPortNumber);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblServer);
            this.Name = "GitShoutTrayApp";
            this.Text = "GitShout";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblServer;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.NotifyIcon trayIcon;
        private System.Windows.Forms.Label lblPortNumber;
        private System.Windows.Forms.TextBox txtPortNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkUseDefaultPort;
    }
}

