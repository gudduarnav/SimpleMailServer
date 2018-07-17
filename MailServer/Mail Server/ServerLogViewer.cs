using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mail_Server
{
    /* 
     * This is the main class for Server Log Viewer dialog.
     */ 
    public partial class ServerLogViewer : Form
    {
        /*
         * Default constructor for the Server Log Viewer class
         */ 
        public ServerLogViewer()
        {
            InitializeComponent();
        }

        /*
         * This function is invoked when the form loads.
         * We would, in here, just refresh the log XML and reload it
         * and show it to user in a Data grid view.
         */ 
        private void ServerLogViewer_Load(object sender, EventArgs e)
        {
            ShowLog();
        }

        /*
         * This function will be invoked when the user clicks on the "Done" button.
         * Here we will just close this dialog as we are done with it.
         */ 
        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /* 
         * This function will be called when the "Clear" button is clicked.
         * This will cause the XML log to clear and delete of all its entries.
         * Then reload and refresh the data grid view.
         */ 
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                CServerLog log = new CServerLog();
                log.ClearLog();
                MessageBox.Show("The Mail Server Log was successfully cleared.",
                    "Mail Server Log Cleared",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("The Mail Server log cannot be cleared now. Please try again later.",
                    "Mail Server Log unable to Clear",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                ShowLog();
            }
        }

        /*
         * This function basically, loads the XML log file and 
         * displays all the loag entry in a data grid view.
         */ 
        private void ShowLog()
        {
            try
            {
                // clear the data grid view
                dgLog.Rows.Clear();

                // load the server log
                CServerLog log = new CServerLog();
                int id = 0;

                // Enumerate all the log entries and display it in the data grid view.
                foreach (ServerLogEntry se in log)
                {
                    id ++;
                    string[] strValues = new string[] { 
                                                           id.ToString(),
                                                           se.eventTime.ToString(),
                                                           se.eventStatus.ToString(),
                                                           se.eventMessage.ToString()
                                                      };
                    dgLog.Rows.Add(strValues);
                }
            }
            catch
            {
                // Some error may happen... but we don't care
            }
        }
    }
}
