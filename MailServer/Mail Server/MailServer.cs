using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Collections;

namespace Mail_Server
{
    /*
     * This is the main Windows Form class for the Mail Server Dialog.
     * This dialog is the entry dialog of the Mail Server
     */ 
    public partial class MailServer : Form
    {
        /*
         * The Default constructor for the MailServer class
         */
        public MailServer()
        {
            InitializeComponent();
        }

        /*
         * This function is invoked when the "Configure POP3/SMTP link is clicked.
         * This function is responsible for showing and updating the POP3 and SMTP
         * port configuration with the help of the configuration dialog.
         */
        private void lnkCfgMail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                CfgMailServer dlg = new CfgMailServer();
                dlg.ShowDialog(this);
            }
            catch
            {
            }
        }

        /*
         * This function is invoked when the "Configure E-Mail Domains" link is clicked.
         * The function is responsible for showing the Mail Domains and is also responsible
         * for adding and removing the Mail domains using the help of the configuration dialog.
         */ 
        private void lnkCfgDomain_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                CfgDomain cfg = new CfgDomain();
                cfg.ShowDialog(this);
            }
            catch
            {
            }
        }

        /*
         * This function is invoked when the "Configure the E-Mail Users" link is clicked.
         * The function is responsible for viewing, adding, updating or removing an E-Mail 
         * User and his/her account along with his Mail Inbox (if there is any, in particular).
         */ 
        private void lnkCfgUser_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                CfgUser cfg = new CfgUser();
                cfg.ShowDialog(this);
            }
            catch
            {
            }
        }

        /*
         * This function is invoked when the form is loading.
         * It will just be used to execute any loadtime configuration.
         */ 
        private void MailServer_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            CServerLog.LogEntry("INFORMATION", "Mail Server application started.");

            // The Server Listeners are not started. So why do we need the Stop now? Just disable it.
            btnStop.Enabled = false;
        }

        /* 
         * This function is invoked when the "View Log" link is clicked.
         * This function will open up a dialog box where the XML based Mail Log file will be
         * displayed.
         */ 
        private void lnkViewLog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ServerLogViewer viewer = new ServerLogViewer();
                viewer.ShowDialog(this);
            }
            catch
            {
            }
        }

        /* 
         * This function will be invoked when the MailServer dialog based Form will be closed.
         * When this function is invoked, will indicate that this application is ending.
         * So, just execute the task that we need to do, like saying Bye Bye!!!
         */ 
        private void MailServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            CServerLog.LogEntry("INFORMATION", "Mail Server application ended.");
        }

        /* 
         * This function will be invoked when the button "About" is clicked.
         * This function will show the following informations in a MessageBox.
         * The information displayed is generally about the project itself.
         */ 
        private void btnAbout_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Mail Server:\n");
            sb.Append("     A small scale and simple mail server made for home users and small office users.\n");
            sb.Append("\n");
            sb.Append("         Application written by Arnav Mukhopadhyay.\n");
            sb.Append("         BCA Semester    -   6 (Project) [Session Jan 2010]\n");
            sb.Append("         Roll Number     -   510728180\n");
            sb.Append("         LC Code         -   02707\n");
            sb.Append("\n");
            sb.Append("     Requirements:\n");
            sb.Append("         Microsoft .NET Framework 3.5\n");
            sb.Append("         Microsoft Windows XP, Windows XP x64, Windows Vista, Windows Vista x64, Windows 7 or above OS\n");
            sb.Append("\n");
            sb.Append("     Project Development Environment:\n");
            sb.Append("         Written using Visual C# and Microsoft .NET Framework 3.5\n");
            sb.Append("         Compiled with Microsoft Visual Studio 2008\n");
            sb.Append("         Uses Thread, Sockets / WCF, LINQ, XML, Windows Forms\n\n");

            MessageBox.Show(sb.ToString(),
                "About Mail Server",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /*
         * This function is invoked when the button "Start" is clicked.
         * This will start the POP3 and SMTP server.
         */ 
        private void btnStart_Click(object sender, EventArgs e)
        {
            OnStartServer();
        }

        /* 
         * This function is invoked when the button "Stop" is clicked.
         * This will stop the POP3 and SMTP server.
         */ 
        private void btnStop_Click(object sender, EventArgs e)
        {
            OnStopServer();
        }

        /* 
         * This boolean variable will save the running state of the Server.
         * A "true" variable indicate that the server is running.
         */ 
        private bool bStarted = false;

        /*
         * This variable carries the reference to the Thread class that holds the 
         * POP3 server instance.
         */ 
        private Thread thPop3 = null;

        /* 
         * This variable carries the reference to the Thread class that holds the
         * SMTP server instance.
         */ 
        private Thread thSmtp = null;

        /*
         * This function is originally responsible for starting the server instance.
         * This will initialize the Threads that actually starts the POP3 and SMTP
         * listener and bind it to All available Network and wait for the connections
         * that the client calls for on the Mail server.
         */ 
        private void OnStartServer()
        {
            if (bStarted == true)
            {
                return;
            }

            try
            {
                // Create a thread that will listen for client on POP3 port
                thPop3 = new Thread(Pop3ThreadFunc);
                thPop3.Start();

                // Create a thread that will listen for client on Smtp port
                thSmtp = new Thread(SmtpThreadFunc);
                thSmtp.Start();
            }
            catch (Exception ex)
            {
                CServerLog.LogEntry("EXCEPTION", ex.ToString());
                MessageBox.Show("Exception occurred during starting of Server. Please re-start the application.",
                    "Exception occurred",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            btnStart.Enabled = false;
            btnStop.Enabled = true;
            lnkCfgMail.Enabled = false;
            bStarted = true;
            CServerLog.LogEntry("SUCCESS", "Mail Server Started successfully.\nIt is now listening for incoming mail connections on POP3 and SMTP port.");
        }

        /* 
         * This function is originally responsible for Stopping the Threads responsible
         * for listening to POP3 and SMTP clients on the network.
         */ 
        private void OnStopServer()
        {
            if (bStarted == false)
            {
                return;
            }

            if (thPop3 != null)
            {
                if (thPop3.IsAlive == true)
                {
                    thPop3.Abort();
                    thPop3.Join();
                }
            }

            if (thSmtp != null)
            {
                if (thSmtp.IsAlive == true)
                {
                    thSmtp.Abort();
                    thSmtp.Join();
                }
            }

            btnStart.Enabled = true;
            btnStop.Enabled = false;
            lnkCfgMail.Enabled = true;
            bStarted = false;
            CServerLog.LogEntry("SUCCESS", "Mail Server was Stopped successfully.\nIt will not interchange mails on any POP3 or SMTP ports.");
        }

        /* 
         * This is the function invoked on an independent Thread. 
         * This function initializes the POP3 listener TCP Socket and waits for the 
         * incoming connection to be made by Mail clients.
         * On accepting a client, the server spawns child threads, each of which is responsible
         * for handling individual client's request.
         */ 
        private void Pop3ThreadFunc()
        {
            TcpListener lis = null;
            try
            {
                CServerLog.LogEntry("INFORMATION", "Pop3 server starting...");

                try
                {
                    CServerCfg cfg = new CServerCfg();
                    
                    // Read the POP3 listener port from the Mail Sever configuration file.
                    int pop3Port = int.Parse(cfg.Root.Element("Pop3Port").Value);
                    
                    // Create a Tcp Listener to listen to the POP3 port on all available
                    // Network interface.
                    lis = new TcpListener(IPAddress.Parse("0.0.0.0"), pop3Port);

                    // Start the Listener Socket and bind it to the Network interfaces.
                    lis.Start();
                    CServerLog.LogEntry("SUCCESS", "POP3 server started and waiting for client to connect.");
                }
                catch (Exception ex)
                {
                    // Oops!!! I think something went wrong.... What to do now????

                    // Show the Exception to user and Log it.
                    MessageBox.Show("Exception starting POP3 server: " + ex.ToString(),
                        "POP3 Server cannot be started",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    CServerLog.LogEntry("FAILED", "Exception starting POP3 Server: " + ex.ToString());
                  
                    // Rethrow it... So that we get out of this mess.
                    throw ex;
                }

                // Wait for listeners till eternity or until the Thread is aborted.
                while (true)
                {
                    // Poll the POP3 TCP listener  for any client
                    // If we get true result, then we have a client waiting to be accepted.
                    if (lis.Server.Poll(1000, SelectMode.SelectRead) == true)
                    {
                        // Log the client
                        CServerLog.LogEntry("INFORMATION", "A POP3 client is waiting for connection.");
                        
                        // get the client TCP Client socket interface and spawn a new thread
                        // to service the POP3 client
                        TcpClient client = lis.AcceptTcpClient();
                        Thread th = new Thread(Pop3Client);
                        th.Start(client);
                    }
                    else
                    {
                        // Wait for 100ms before we re-poll
                        Thread.Sleep(100);
                    }
                }
            }
            catch
            {
                // Some exception occurred.... lets do nothing here
            }
            finally
            {
                // so now its done 
                // let's stop the listener if there is one and that's it.
                if (lis != null)
                {
                    lis.Stop();
                }
            }
        }

        /*
         * This function is invoked by the POP3 listener, once for every client request 
         * made, and is on a new thread.
         * This enables to invoke multiple independent instance of this function, thus 
         * enabling the handling of multiple clients at once.
         */ 
        private void Pop3Client(object o)
        {
            TcpClient client = null;
            EndPoint remote = null;
            try
            {
                string userName = "";
                string password = "";
                string mailFolder = "";

                // Use the client as TCP Socket and gets it's value from the object parameter
                client = o as TcpClient;

                // Retrieve the respective streams from the TCP client socket to perform 
                // buffered Network IO
                NetworkStream ns = client.GetStream();
                StreamReader sin = new StreamReader(ns, System.Text.Encoding.ASCII, true);
                StreamWriter sout = new StreamWriter(ns, System.Text.Encoding.ASCII);

                // Show the Welcome Message to POP3 client.
                sout.WriteLine("+OK POP3 ready at " + Environment.MachineName +"\r");
                sout.Flush();

                // Get the Remote End Point IP
                remote = client.Client.RemoteEndPoint;
                CServerLog.LogEntry("POP3 CLIENT", "A POP3 client connected from " + remote.ToString());

                string cmd;
                // Poll the POP3 client for commands and reply to them with requisite request
                do
                {
                    // Poll this client socket to see if there exist any data to be read
                    // If the return is true, the read the input command sent by client
                    if (client.Client.Poll(1000, SelectMode.SelectRead) == true)
                    {
                        // Read a line of command from the Network input stream
                        cmd = sin.ReadLine().Trim();

                        // Client wants to QUIT and so do Bye Bye to the client.
                        if (cmd.ToUpper() == "QUIT")
                        {
                            sout.WriteLine("+OK Bye Bye!!!\r");
                            sout.Flush();
                            break;
                        }
                        // It is the "USER" command, so get the user name the client want to 
                        // authenticate for use
                        else if (cmd.Substring(0,Math.Min(4, cmd.Length)).ToUpper() == "USER")
                        {
                            userName = cmd.Substring(5);
                            sout.WriteLine("+OK Please enter the password.\r");
                            sout.Flush();
                            CServerLog.LogEntry("POP3 CLIENT AUTH USER", "The client at " + remote.ToString() + " authorizing with user ID " + userName + ".");
                        }
                        // The client sent the "PASS" command, so get the password and 
                        // authenticate the account.
                        else if (cmd.Substring(0, Math.Min(4, cmd.Length)).ToUpper() == "PASS")
                        {
                            password = cmd.Substring(5);
                            // Verify the User name and his password and in turn also
                            // get his inbox location on the server. 
                            // Everything is good, if this return true. false value
                            // indicates a access violation or some kind of error.
                            if (VerifyAccount(userName, password, ref mailFolder) == true)
                            {
                                sout.WriteLine("+OK The Account has been verified successfully.\r");
                                CServerLog.LogEntry("POP3 CLIENT AUTH", "The client at " + remote.ToString() + " authorized with user ID " + userName + " successfully.");
                            }
                            else
                            {
                                CServerLog.LogEntry("POP3 CLIENT AUTH FAILED", "The client at " + remote.ToString() + " authorization with user ID " + userName + " failed.");
                                sout.WriteLine("-ERR The User name or Password is invalid. Please try again.\r");
                            }
                            sout.Flush();                            
                        }
                        // Client sent the "STAT" command, so send it the mail statistics.
                        // The mail statistics is the number of mails on server and the total
                        // size of the mails on server in octet.
                        // How to do it???
                        // List the files with extension .eml and get their count and total size
                        // and send this values to the client.
                        else if (cmd.Trim('\r').ToUpper() == "STAT")
                        {
                            try
                            {
                                if (mailFolder == "")
                                {
                                    sout.WriteLine("-ERR Mailbox not found on Server\r");
                                    sout.Flush();
                                    break;
                                }

                                DirectoryInfo dinfo = new DirectoryInfo(Application.StartupPath + @"\" + mailFolder);
                                if (dinfo.Exists == false)
                                {
                                    sout.WriteLine("-ERR Server lost the Mailbox\r");
                                    sout.Flush();
                                    break;
                                }

                                FileInfo[] finfoList = dinfo.GetFiles("*.eml");

                                int count = 0;
                                long countSize = 0;
                                if (finfoList != null && finfoList.Length > 0)
                                {
                                    foreach (FileInfo finfo in finfoList)
                                    {
                                        if (finfo.Exists)
                                        {
                                            count++;
                                            countSize += finfo.Length;
                                        }
                                    }
                                }
                                sout.WriteLine("+OK {0} {1}\r", count, countSize);
                                sout.Flush();
                            }
                            catch
                            {
                            }
                            
                        }
                        // The client sent the "LIST" command, so send him the statistics of 
                        // the Mail on server and also the list of the mails and their 
                        // corresponding size to the client.
                        else if (cmd.Trim('\r').ToUpper() == "LIST")
                        {
                            try
                            {
                                if (mailFolder == "")
                                {
                                    sout.WriteLine("-ERR Mailbox not found on Server\r");
                                    sout.Flush();
                                    break;
                                }

                                DirectoryInfo dinfo = new DirectoryInfo(Application.StartupPath + @"\" + mailFolder);
                                if (dinfo.Exists == false)
                                {
                                    sout.WriteLine("-ERR Server lost the Mailbox\r");
                                    sout.Flush();
                                    break;
                                }

                                FileInfo[] finfoList = dinfo.GetFiles("*.eml");

                                int count = 0;
                                long countSize = 0;
                                if (finfoList != null && finfoList.Length > 0)
                                {
                                    foreach (FileInfo finfo in finfoList)
                                    {
                                        if (finfo.Exists)
                                        {
                                            count++;
                                            countSize += finfo.Length;
                                        }
                                    }
                                }
                                sout.WriteLine("+OK {0} messages  ({1} octets)\r", count, countSize);
                                sout.Flush();

                                if (finfoList != null && finfoList.Length > 0)
                                {
                                    foreach (FileInfo finfo in finfoList)
                                    {
                                        if (finfo.Exists)
                                        {
                                            sout.WriteLine("{0} {1}\r", finfo.Name.Substring(0, finfo.Name.IndexOf(".eml")).Trim(), finfo.Length);
                                            sout.Flush();
                                        }
                                    }
                                }

                                sout.WriteLine(".\r");
                                sout.Flush();
                            }
                            catch
                            {
                            }

                        }
                        // Client sent the "DELE" command, so lets delete the EMAIL file number
                        // on the server
                        else if (cmd.Substring(0, Math.Min(4, cmd.Length)).ToUpper() == "DELE")
                        {
                            try
                            {
                                string fname = cmd.Substring(5);
                                if (mailFolder == "")
                                {
                                    sout.WriteLine("-ERR Mailbox not found on Server\r");
                                    sout.Flush();
                                    break;
                                }
                                FileInfo finfo = new FileInfo(Application.StartupPath + @"\" + mailFolder + @"\" + fname + ".eml");
                                if (finfo.Exists)
                                {
                                    finfo.Delete();
                                }

                                sout.WriteLine("+OK Message {0} deleted\r", fname);
                                sout.Flush();
                            }
                            catch
                            {
                            }

                        }
                        // Client sent the "RETR" command, so lets send it the email file
                        // from the server as requested.
                        else if (cmd.Substring(0, Math.Min(4, cmd.Length)).ToUpper() == "RETR")
                        {
                            try
                            {
                                string fname = cmd.Substring(5);
                                if (mailFolder == "")
                                {
                                    sout.WriteLine("-ERR Mailbox not found on Server\r");
                                    sout.Flush();
                                    break;
                                }
                                int fileID = int.Parse(fname.Trim());
                                FileInfo finfo = new FileInfo(Application.StartupPath + @"\" + mailFolder + @"\" + fileID.ToString() + ".eml");
                                if (finfo.Exists)
                                {
                                    sout.WriteLine("+OK {0} octets\r", finfo.Length);
                                    sout.Flush();

                                    FileStream fs = finfo.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                    int writeSize = (int) fs.Length;
                                    byte[] buffer = new byte[writeSize];
                                    writeSize = fs.Read(buffer, 0, writeSize);
                                    fs.Close();

                                    ns.Write(buffer, 0, writeSize);
                                    ns.Flush();
                                }
                                else
                                {
                                    sout.WriteLine("-ERR E-Mail lost on server\r");
                                    sout.Flush();
                                    break;
                                }

                            }
                            catch
                            {
                            }

                        }
                        // What is the client sending???? I don't know it.
                        // Send the client a ERROR message....
                        else
                        {
                            sout.WriteLine("-ERR The POP3 Server cannot understand the command.\r");
                            sout.Flush();
                        }
                    }
                    // So no command sent.... Wait for 100 ms and re-poll.
                    else
                    {
                        Thread.Sleep(100);
                        cmd = "";
                    }
                }
                while (client.Connected == true);
            }
            catch
            {
                // Something wrong happened.... I don't want to know....
            }
            finally
            {
                // So we are done with Client anyway....
                // So close the client connection and log it, and say bye bye to client.
                if (client != null)
                {
                    client.Close();
                    CServerLog.LogEntry("POP3 CLIENT CLOSED", "The Client at " + remote.ToString() + " was closed.");
                }
            }
        }

        /* 
         * This is the function solely reponsible for starting the SMTP server.
         * This function creates the SMTP TCP listener and bind to all available 
         * network adapters and listens for SMTP clients to connect.
         * For eache client request it accepts, it spawns a new thread to cater to
         * the service of the client.
         */ 
        private void SmtpThreadFunc()
        {
            TcpListener lis = null;
            try
            {
                CServerLog.LogEntry("INFORMATION", "Smtp server starting...");
                try
                {
                    CServerCfg cfg = new CServerCfg();

                    // Read the SMTP port configuration from the XML based Mail comfiguration file
                    int smtpPort = int.Parse(cfg.Root.Element("SmtpPort").Value);

                    // Start the SMTP TCP listener interface and bind it to 
                    // all network interfaces available on the assigned SMTP port
                    lis = new TcpListener(IPAddress.Parse("0.0.0.0"), smtpPort);

                    // Start the listening job
                    lis.Start();
                    CServerLog.LogEntry("SUCCESS", "SMTP server started and waiting for client to connect.");
                }
                catch (Exception ex)
                {
                    // Something wrong just happened
                    // Display it to user and log it
                    MessageBox.Show("Exception starting SMTP server: " + ex.ToString(),
                        "SMTP Server cannot be started",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop);
                    CServerLog.LogEntry("FAILED", "Exception starting SMTP Server: " + ex.ToString());
                   
                    // Rethrow the exception, so we can get out of this mess
                    throw ex;
                }

                // start to listen till eternity or till this thread is aborted
                while (true)
                {
                    // Poll this socket for readable data which will return true
                    // when there is client to be accepted.
                    if (lis.Server.Poll(1000, SelectMode.SelectRead) == true)
                    {
                        CServerLog.LogEntry("INFORMATION", "A SMTP client is waiting for connection.");
                        
                        // Accept the new SMTP client and spawn a new child thread
                        // and let it cater the new client.
                        TcpClient client = lis.AcceptTcpClient();
                        Thread th = new Thread(SmtpClient);
                        th.Start(client);
                    }
                    else
                    {
                        // No client is available... So just wait for 100ms and repoll again
                        Thread.Sleep(100);
                    }
                }
            }
            catch
            {
                // Something wrong happened... We don't want to know
            }
            finally
            {
                // So we are done with the listener....
                // Lets stop it and free the network adapters.
                if (lis != null)
                {
                    lis.Stop();
                }
            }

        }

        /* 
         * This is the function that caters the SMTP client request.
         * This function is invoked as a result of the child thread created 
         * for each SMTP client that connects.
         */ 
        private void SmtpClient(object o)
        {
            TcpClient client = null;
            EndPoint remote = null;
            try
            {
                // Create a place holder to hold the inbox folders, that corresponds 
                // to the mail being send to by the client.
                ArrayList folderList = new ArrayList();

                // Get the TcpClient interface from the object
                client = o as TcpClient;

                // Retrieve the respective Network Stream from the Tcp Client socket
                NetworkStream ns = client.GetStream();
                StreamReader sin = new StreamReader(ns, System.Text.Encoding.ASCII, true);
                StreamWriter sout = new StreamWriter(ns, System.Text.Encoding.ASCII);

                // Show the Welcome message to client and flag it that the server is
                // ready to interchange messages.
                sout.WriteLine("220 " + Environment.MachineName + " SMTP ready.\r");
                sout.Flush();

                // Get the remote end IP address.
                remote = client.Client.RemoteEndPoint;
                CServerLog.LogEntry("SMTP CLIENT", "A SMTP client connected from " + remote.ToString());

                string cmd;

                // Poll the SMTP client for command and service the request.
                do
                {
                    // Poll this client socket for data and if the return result
                    // is a true value, then there is command waiting to be read.
                    if (client.Client.Poll(1000, SelectMode.SelectRead) == true)
                    {
                        // Read in the command sent by client.
                        cmd = sin.ReadLine().Trim();

                        // Client wants to quit so Bye Bye it.
                        if (cmd.ToUpper() == "QUIT")
                        {
                            sout.WriteLine("221 Bye Bye!!!\r");
                            sout.Flush();
                            break;
                        }
                        // Client sent in "HELO" command, so log him at server.
                        // This marks client as a valid SMTP client.
                        else if (cmd.Substring(0, Math.Min(4, cmd.Length)).ToUpper() == "HELO") 
                        {
                            string domainName = cmd.Substring(5).Trim().Trim('\r');
                            sout.WriteLine("250 " + Environment.MachineName + "\r");
                            sout.Flush();
                            CServerLog.LogEntry("SMTP CLIENT DOMAIN", "The client at " + remote.ToString() + " is connected from " + domainName + ".");
                        }
                        // The client is an ESMTP client as it sent "EHLO". I don't know 
                        // ESMTP protocol... So please retry by sending in "HELO"
                        else if (cmd.Substring(0, Math.Min(4, cmd.Length)).ToUpper() == "EHLO")
                        {
                            string domainName = cmd.Substring(5).Trim().Trim('\r');
                            sout.WriteLine("500 Not supported. Use HELO\r");
                            sout.Flush();
                            CServerLog.LogEntry("ESMTP CLIENT DOMAIN", "The client at " + remote.ToString() + " is connecting from " + domainName + ".");
                        }
                        // Client sent "MAIL-FROM:" command, so validate the client
                        // Check to see if the sender belongs to this server. 
                        // I don't want spammer or relay services to send in mail here.
                        // Also clear the receiver folder path place holder array list as
                        // this marks the beginning of the new mail.
                        else if (cmd.Substring(0, Math.Min(10, cmd.Length)).ToUpper() == "MAIL FROM:")
                        {
                            string senderEmail = cmd.Substring(11);
                            if (VerifyEMail(senderEmail) == true)
                            {
                                sout.WriteLine("250 OK\r");
                                folderList.Clear();
                            }
                            else
                            {
                                sout.WriteLine("500 ERR Email ID " + senderEmail + " not found.\r");
                                sout.Flush();
                                break;
                            }
                            sout.Flush();
                        }
                        // Client sent "RCPT TO:", so check the attached email addresses that 
                        // they belong on this server, if not found, then report the absence to
                        // the client and abort further mail transaction.
                        // If found, then get the inbox folder path on the server and add to 
                        // the folder place holder array list we created before.
                        else if (cmd.Substring(0, Math.Min(8, cmd.Length)).ToUpper() == "RCPT TO:")
                        {
                            string rcptEmail = cmd.Substring(9);
                            string inboxPath = "";
                            if (getInbox(rcptEmail, ref inboxPath) == true)
                            {
                                sout.WriteLine("250 OK\r");
                                folderList.Add(inboxPath);
                            }
                            else
                            {
                                sout.WriteLine("500 ERR Email ID " + rcptEmail + " not found.\r");
                                sout.Flush();
                                break;
                            }
                            sout.Flush();
                        }
                        // Client sent in "DATA" command, which indicates that the client
                        // wants to send in the email data. So hold it and then copy to 
                        // a new .eml file in the receiver's email folder path.
                        else if (cmd.ToUpper() == "DATA")
                        {
                            // Flag the client with a message of how to send in the mail
                            sout.WriteLine("354 Start mail input; end with <CRLF>.<CRLF>\r");
                            sout.Flush();

                            // create a array list object to act as email read-in buffer.
                            ArrayList dataList = new ArrayList();
                            byte ch;

                            // Read in the data and detect the end of mail data. Otherwise,
                            // save it in the dataList buffer.
                            do
                            {
                                ch = (byte)ns.ReadByte();
                                dataList.Add(ch);

                                if (ch == '\n')
                                {
                                    ch = (byte)ns.ReadByte();
                                    dataList.Add(ch);

                                    if (ch == '.')
                                    {
                                        ch = (byte)ns.ReadByte();
                                        dataList.Add(ch);

                                        if (ch == '\r')
                                        {
                                            ch = (byte)ns.ReadByte();
                                            dataList.Add(ch);

                                            if (ch == '\n')
                                            {
                                                // End of Mail data
                                                break;
                                            }
                                        }
                                    }
                                }
                                
                            }
                            while (true);

                            // now retrieve the Array of Client Inbox folder names
                            // and save the email data in this folder as a new .eml
                            // file.
                            Array dataArray = dataList.ToArray();
                            foreach (string folderName in folderList)
                            {
                                SaveData(folderName, dataArray);
                            }

                            // we are done saving so.... tell the client.
                            sout.WriteLine("250 OK\r");
                            sout.Flush();

                        }
                        // The user sent a blank command... We have nothing to do
                        else if (cmd.Trim().Trim(new char[] { '\r', '\n' }) == "")
                        {

                        }
                        // Cannot understand what the user just sent in.
                        else
                        {
                            sout.WriteLine("500 The SMTP Server cannot understand the command.\r");
                            sout.Flush();
                            break;
                        }
                    }
                    // There is no data to be read so... just wait for 100 ms
                    else
                    {
                        Thread.Sleep(100);
                        cmd = "";
                    }
                }
                while (client.Connected == true);
            }
            catch
            {
                // Some exception just occurred.... We don't want to know
            }
            finally
            {
                // We are done with the client. So close it now.
                if (client != null)
                {
                    client.Close();
                    CServerLog.LogEntry("SMTP CLIENT CLOSED", "The Client at " + remote.ToString() + " was closed.");
                }
            }

        }

        /*
         * This function is called when the form is about to close.
         * So lets stop the server before this form closes.
         */
        private void MailServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnStopServer();
        }

        /*
         * Called to verify the email account against the internal user database.
         * This requires the userName which is in format EMAIL@DOMAIN.XYZ format,
         * along with the valid password for the acoount.
         * If the function returns true, then the mailFolder variable holds the
         * EMAIL inbox path on the server, and also that the account is authenticated
         * and valid for the transaction to proceed.
         * Otherwise, don't proceed with it... It is invalid.
         */ 
        private bool VerifyAccount(string userName, string password, ref string mailFolder)
        {
            try
            {
                CServerCfg cfg = new CServerCfg();
                XElement elUsers = cfg.Root.Element("Users");
                foreach (XElement elDomain in elUsers.Nodes())
                {
                    foreach (XElement elUser in elDomain.Nodes())
                    {
                        string emailID = elUser.Name.LocalName + "@" + elDomain.Name.LocalName;
                        string pass = elUser.Element("pass").Value;
                        string folder = elUser.Element("folder").Value;

                        if (emailID.ToLower() == userName.ToLower() &&
                           pass == password)
                        {
                            mailFolder = folder;
                            return true;
                        }
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        /*
         * Called to verify the email account against the internal user database.
         * This requires the userName which is in format EMAIL@DOMAIN.XYZ format.
         * If the function returns true, the account is authenticated
         * and valid for the transaction to proceed.
         * Otherwise, don't proceed with it... It is invalid.
         */
        private bool VerifyEMail(string mailAddress)
        {
            try
            {
                mailAddress = mailAddress.ToLower().Trim();
                mailAddress = mailAddress.Trim(new char[] { '<', '>'});
                CServerCfg cfg = new CServerCfg();
                XElement elUsers = cfg.Root.Element("Users");
                foreach (XElement elDomain in elUsers.Nodes())
                {
                    foreach (XElement elUser in elDomain.Nodes())
                    {
                        string emailID = elUser.Name.LocalName + "@" + elDomain.Name.LocalName;

                        if (emailID.ToLower() == mailAddress)
                        {
                               return true;
                        }
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        /*
         * Called to retreive the email account inbox path on server from the internal user database.
         * This requires the mailAddress in format EMAIL@DOMAIN.XYZ format.
         * If the function returns true, then the mailFolder variable holds the
         * EMAIL inbox path on the server, and also that the account is authenticated
         * and valid for the transaction to proceed.
         * Otherwise, don't proceed with it... It is invalid.
         */ 
        private bool getInbox(string mailAddress, ref string mailFolder)
        {
            try
            {
                mailAddress = mailAddress.ToLower().Trim();
                mailAddress = mailAddress.Trim(new char[] { '<', '>' });

                CServerCfg cfg = new CServerCfg();
                XElement elUsers = cfg.Root.Element("Users");
                foreach (XElement elDomain in elUsers.Nodes())
                {
                    foreach (XElement elUser in elDomain.Nodes())
                    {
                        string emailID = elUser.Name.LocalName + "@" + elDomain.Name.LocalName;
                        string folder = elUser.Element("folder").Value;

                        if (emailID.ToLower() == mailAddress)
                        {
                            mailFolder = folder;
                            return true;
                        }
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        /*
         * This function saves the char data in the Array reference in mailData
         * to a new .eml file in inboxName path on the server.
         */ 
        private void SaveData(string inboxName, Array mailData)
        {
            // detect a filename to use
            string fileName;
            FileInfo finfo;

            for (int i = 1; ; i++)
            {
                fileName = Application.StartupPath + @"\" + inboxName + @"\" + i.ToString() + ".eml";
                finfo = new FileInfo(fileName);
                if (finfo.Exists == false)
                {
                    // got the required file
                    FileStream fs = finfo.Create();
                    foreach (byte ch in mailData)
                    {
                        fs.WriteByte(ch);
                    }
                    fs.Close();
                    
                    // done 
                    return;
                }
            }
        }

        /*
         * This function is called when the notification icon is called.
         * If the Mail Server dialog is visible, then it is hidden from view,
         * otherwise, it is hidden from the view.
         */ 
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible == true)
            {
                this.Hide();
            }
            else
            {
                this.Show();
            }
        }
    }
}
