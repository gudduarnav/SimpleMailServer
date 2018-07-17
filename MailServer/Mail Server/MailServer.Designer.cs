namespace Mail_Server
{
    partial class MailServer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MailServer));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lnkViewLog = new System.Windows.Forms.LinkLabel();
            this.lnkCfgUser = new System.Windows.Forms.LinkLabel();
            this.lnkCfgDomain = new System.Windows.Forms.LinkLabel();
            this.lnkCfgMail = new System.Windows.Forms.LinkLabel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnAbout = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.lnkViewLog);
            this.groupBox1.Controls.Add(this.lnkCfgUser);
            this.groupBox1.Controls.Add(this.lnkCfgDomain);
            this.groupBox1.Controls.Add(this.lnkCfgMail);
            this.groupBox1.ForeColor = System.Drawing.Color.MediumBlue;
            this.groupBox1.Location = new System.Drawing.Point(270, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(356, 216);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Mail Server Configuration";
            // 
            // lnkViewLog
            // 
            this.lnkViewLog.AutoSize = true;
            this.lnkViewLog.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkViewLog.LinkColor = System.Drawing.Color.Navy;
            this.lnkViewLog.Location = new System.Drawing.Point(31, 153);
            this.lnkViewLog.Name = "lnkViewLog";
            this.lnkViewLog.Size = new System.Drawing.Size(140, 22);
            this.lnkViewLog.TabIndex = 3;
            this.lnkViewLog.TabStop = true;
            this.lnkViewLog.Text = "View Log File";
            this.lnkViewLog.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkViewLog_LinkClicked);
            // 
            // lnkCfgUser
            // 
            this.lnkCfgUser.AutoSize = true;
            this.lnkCfgUser.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCfgUser.LinkColor = System.Drawing.Color.Navy;
            this.lnkCfgUser.Location = new System.Drawing.Point(31, 113);
            this.lnkCfgUser.Name = "lnkCfgUser";
            this.lnkCfgUser.Size = new System.Drawing.Size(230, 22);
            this.lnkCfgUser.TabIndex = 2;
            this.lnkCfgUser.TabStop = true;
            this.lnkCfgUser.Text = "Configure E-Mail Users";
            this.lnkCfgUser.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCfgUser_LinkClicked);
            // 
            // lnkCfgDomain
            // 
            this.lnkCfgDomain.AutoSize = true;
            this.lnkCfgDomain.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCfgDomain.LinkColor = System.Drawing.Color.Navy;
            this.lnkCfgDomain.Location = new System.Drawing.Point(31, 68);
            this.lnkCfgDomain.Name = "lnkCfgDomain";
            this.lnkCfgDomain.Size = new System.Drawing.Size(250, 22);
            this.lnkCfgDomain.TabIndex = 1;
            this.lnkCfgDomain.TabStop = true;
            this.lnkCfgDomain.Text = "Configure E-Mail Domains";
            this.lnkCfgDomain.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCfgDomain_LinkClicked);
            // 
            // lnkCfgMail
            // 
            this.lnkCfgMail.AutoSize = true;
            this.lnkCfgMail.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkCfgMail.ForeColor = System.Drawing.Color.MediumBlue;
            this.lnkCfgMail.LinkColor = System.Drawing.Color.Navy;
            this.lnkCfgMail.Location = new System.Drawing.Point(31, 26);
            this.lnkCfgMail.Name = "lnkCfgMail";
            this.lnkCfgMail.Size = new System.Drawing.Size(290, 22);
            this.lnkCfgMail.TabIndex = 0;
            this.lnkCfgMail.TabStop = true;
            this.lnkCfgMail.Text = "Configure POP3 and SMTP Port";
            this.lnkCfgMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkCfgMail_LinkClicked);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.Transparent;
            this.groupBox3.Controls.Add(this.btnAbout);
            this.groupBox3.Controls.Add(this.btnStop);
            this.groupBox3.Controls.Add(this.btnStart);
            this.groupBox3.ForeColor = System.Drawing.Color.DarkBlue;
            this.groupBox3.Location = new System.Drawing.Point(13, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(251, 211);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Server Startup / Shutdown";
            // 
            // btnAbout
            // 
            this.btnAbout.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAbout.Location = new System.Drawing.Point(22, 21);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(205, 43);
            this.btnAbout.TabIndex = 2;
            this.btnAbout.Text = "About";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStop.Location = new System.Drawing.Point(22, 136);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(205, 47);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStart.Location = new System.Drawing.Point(22, 87);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(205, 43);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.notifyIcon1.BalloonTipText = "Mail Server";
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Mail Server";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // MailServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.BackgroundImage = global::Mail_Server.Properties.Resources.Untitled_2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(642, 242);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MailServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mail Server";
            this.Load += new System.EventHandler(this.MailServer_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MailServer_FormClosed);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MailServer_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.LinkLabel lnkCfgUser;
        private System.Windows.Forms.LinkLabel lnkCfgDomain;
        private System.Windows.Forms.LinkLabel lnkCfgMail;
        private System.Windows.Forms.LinkLabel lnkViewLog;
        private System.Windows.Forms.Button btnAbout;
        private System.Windows.Forms.NotifyIcon notifyIcon1;

    }
}

