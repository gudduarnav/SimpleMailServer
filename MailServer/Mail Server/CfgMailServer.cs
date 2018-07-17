using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Xml.Linq;

namespace Mail_Server
{
    /*
     * This is the main class the Mail Server configuration dialog.
     */ 
    public partial class CfgMailServer : Form
    {
        /*
         * This variable holds the POP3 port number.
         */ 
        private int popPort = 110;

        /*
         * This variable holds the SMTP port number.
         */ 
        private int smtpPort = 25;

        /* 
         * This is the default constructor for the CfgMailServer class.
         */ 
        public CfgMailServer()
        {
            InitializeComponent();
        }

        /*
         * This function is called when the form is loaded.
         * In this procedure, we would load the initial POP3 and SMTP
         * values from the Mail Configuration file, and display it in
         * their respective edit boxes.
         */ 
        private void CfgMailServer_Load(object sender, EventArgs e)
        {
            CServerCfg cfg = null;
            try
            {
                cfg = new CServerCfg();

                try
                {
                    popPort = int.Parse(cfg.Root.Element("Pop3Port").Value);
                }
                catch
                {
                    cfg.Root.Add(new XElement("Pop3Port", popPort.ToString()));
                }

                try
                {
                    smtpPort = int.Parse(cfg.Root.Element("SmtpPort").Value);
                }
                catch
                {
                    cfg.Root.Add(new XElement("SmtpPort", smtpPort.ToString()));
                }
            }
            catch
            {
            }
            finally
            {
                if (cfg != null)
                {
                    cfg.Close();
                }
            }

            tbPop.Text = popPort.ToString();
            tbSmtp.Text = smtpPort.ToString();
        }

        /*
         * This function is invoked when "Done" button is clicked.
         * Then save the new value of the POP3 and SMTP port numbers, if modified.
         */ 
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            CServerCfg cfg = null;
            try
            {
                cfg = new CServerCfg();
                cfg.Root.Element("Pop3Port").SetValue(popPort.ToString());
                cfg.Root.Element("SmtpPort").SetValue(smtpPort.ToString());

                MessageBox.Show("Mail Server Configuration updated and saved.",
                    "Mail Server Configuration Updated",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("The Savings could not be updated.",
                    "Mail Server Update failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            finally
            {
                if (cfg != null)
                {
                    cfg.Close();
                }

                this.Dispose();
            }
        }

        /*
         * This function is invoked when the user changes the text in SMTP port edit box.
         * This function verifies if the text entered is valid as a SMTP port number.
         */ 
        private void tbSmtp_TextChanged(object sender, EventArgs e)
        {
            try
            {
                smtpPort = int.Parse(tbSmtp.Text);
            }
            catch
            {
                tbSmtp.Text = smtpPort.ToString();
            }

        }

        /*
         * This function is invoked when the user changes the text in POP3 port edit box.
         * This function verifies if the text entered is valid as a POP3 port number.
         */
        private void tbPop_TextChanged(object sender, EventArgs e)
        {
            try
            {
                popPort = int.Parse(tbPop.Text);
            }
            catch
            {
                tbPop.Text = popPort.ToString();
            }
        }
    }
}
