using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Mail_Server
{
    /*
     * This is the main class for User Account configuration dialog.
     */ 
    public partial class CfgUser : Form
    {
        /* 
         * This is the default constructor for CfgUser class.
         */ 
        public CfgUser()
        {
            InitializeComponent();
        }

        /*
         * This function is invoked when the User Account Configuration dialog is invoked.
         * Enumerate the Domain and its Users, and list the registered E-Mail addresses in
         * the list box.
         */ 
        private void CfgUser_Load(object sender, EventArgs e)
        {
            CServerCfg cfg = null;
            try
            {
                cfg = new CServerCfg();
                XElement elDomain = cfg.Root.Element("Domains");
                if (elDomain != null)
                {
                    foreach (XElement el in elDomain.Nodes())
                    {
                        cbDomains.Items.Add(el.Name.LocalName);
                    }
                    if (cbDomains.Items.Count > 0)
                    {
                        cbDomains.SelectedIndex = 0;
                    }
                    else
                    {
                        // No E-Mail Domains specified
                        MessageBox.Show("The E-Mail server has no E-Mail domains configured. Please configure domain names before adding an User Account.",
                            "E-Mail Domain not Configured",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Stop);
                        this.Dispose();
                        return;
                    }
                }

                XElement elUsers = cfg.Root.Element("Users");
                if (elUsers != null)
                {
                    foreach (XElement elDomains in elUsers.Nodes())
                    {
                        foreach (XElement elUser in elDomains.Nodes())
                        {
                            lbUsers.Items.Add(elUser.Name.LocalName + "@" + elDomains.Name.LocalName);
                        }
                    }
                    if (lbUsers.Items.Count > 0)
                    {
                        lbUsers.SelectedIndex = 0;
                        EnableReadOnly();
                    }
                    else
                    {
                        EnableWrite();
                    }
                }
                else
                {
                    // add the provision node for Users
                    cfg.Root.Add(new XElement("Users"));
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
        }

        /* 
         * This function enables those dialog items, that can be enabled only 
         * such that the email accounts can be updated and removed.
         */ 
        private void EnableReadOnly()
        {
            tbUser.ReadOnly = true;
            cbDomains.Enabled = false;
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnRemove.Enabled = true;
        }

        /*
         * This function enables those dialog items, that can be enabled only 
         * when we need to add a new account or create a new account.
         */ 
        private void EnableWrite()
        {
            tbUser.ReadOnly = false;
            cbDomains.Enabled = true;
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnRemove.Enabled = false;
        }

        /*
         * This function is invoked when the "Done" button is clicked.
         * So we are done with account editing, and so we close this form.
         */ 
        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /* 
         * This function is invoked when we want to create a new E-Mail account.
         */ 
        private void btnNew_Click(object sender, EventArgs e)
        {
            tbUser.Clear();
            if (cbDomains.Items.Count > 0)
            {
                cbDomains.SelectedIndex = 0;
            }
            tbPass.Clear();
            tbPassConfirm.Clear();
            EnableWrite();
        }

        /*
         * This function is invoked when we want to add the newly created E-Mail
         * user account to the Mail Configuration file and also to the E-Mail 
         * list box.
         */ 
        private void btnAdd_Click(object sender, EventArgs e)
        {
            tbUser.Text = tbUser.Text.Trim();
            tbPass.Text = tbPass.Text.Trim();

            if (tbUser.Text == "" || tbPass.Text == "")
            {
                MessageBox.Show("User Name or Password cannot be blank.\nPlease enter a valid User Name and Password and try again.",
                    "Error in User Name or Password.",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            if(tbPass.Text != tbPassConfirm.Text)
            {
                MessageBox.Show("The entered passwords do not match. Please enter the correct password and try again.",
                    "Wrong Password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }


            /*
             * <Users>
             * ... ... ...
             *  <domain.xyz>
             *      <email>
             *          <pass>mypassword</pass>
             *          <folder>myemailfolder</folder>
             *      </email>
             *  <domain.xyz>
             * ... ... ...
             * </Users>
             */

            CServerCfg cfg = null;
            try
            {
                cfg = new CServerCfg();
                XElement el = cfg.Root.Element("Users");
                if (el == null)
                {
                    throw new Exception();
                }

                // Create the new folder Guid for email account
                string userdir = Guid.NewGuid().ToString().ToUpper();
                

                DirectoryInfo dirinfo = new DirectoryInfo(Application.StartupPath);
                DirectoryInfo[] dirinfolist = dirinfo.GetDirectories();
                bool bFound;
                do
                {
                    bFound = true;
                    foreach (DirectoryInfo dinfo in dirinfolist)
                    {
                        if (dinfo.Name == userdir)
                        {
                            userdir = Guid.NewGuid().ToString().ToUpper();
                            bFound = false;
                            break;
                        }
                    }
                }
                while (bFound != true);
        
                // now create the unique user directory
                dirinfo = new DirectoryInfo(Application.StartupPath + @"\" + userdir);
                dirinfo.Create();

                // now add this settings to configuration
                XElement elUser = new XElement(tbUser.Text);
                elUser.Add(new XElement("pass", tbPass.Text));
                elUser.Add(new XElement("folder", userdir));

                XElement elDomain = el.Element(cbDomains.Text);
                if (elDomain == null)
                {
                    elDomain = new XElement(cbDomains.Text);
                    elDomain.Add(elUser);
                    el.Add(elDomain);
                }
                else
                {
                    elDomain.Add(elUser);
                }

                lbUsers.SelectedIndex = lbUsers.Items.Add(tbUser.Text + "@" + cbDomains.Text);

                MessageBox.Show("E-Mail Account was successfully created and added to the list.",
                    "E-Mail Account created",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("E-Mail Account cannot be created at this time. Please try again later.",
                    "E-Mail Account cannot be created",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
            }
            finally
            {
                if (cfg != null)
                {
                    cfg.Close();
                }
            }
        }

        /*
         * This function is invoked when the user selects a different user account
         * to update or remove.
         */ 
        private void lbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableReadOnly();

            string usermail = lbUsers.Items[lbUsers.SelectedIndex].ToString();
            string username = usermail.Substring(0, usermail.IndexOf("@"));
            string userdomain = usermail.Substring(usermail.IndexOf("@") + 1);

            CServerCfg cfg = null;
            try
            {
                cfg = new CServerCfg();
                XElement elDomain = cfg.Root.Element("Users").Element(userdomain);
                if (elDomain == null)
                {
                    throw new Exception();
                }

                XElement elUser = elDomain.Element(username);
                if (elUser == null)
                {
                    throw new Exception();
                }

                tbUser.Text = elUser.Name.LocalName;
                cbDomains.SelectedIndex = cbDomains.Items.IndexOf(elDomain.Name.LocalName);
                tbPass.Text = elUser.Element("pass").Value;
                tbPassConfirm.Text = tbPass.Text;
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
        }

        /*
         * This function is invoked when the user clicks the "Update"
         * button and thereby, updates the E-Mail account settings in the Mail
         * Configuration file.
         */ 
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Update only updates the password
            tbPass.Text = tbPass.Text.Trim();
            if (tbPass.Text == "")
            {
                MessageBox.Show("Empty password string is not allowed. Please enter a non-empty valid password and retry.",
                    "Invalid password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }
            if (tbPass.Text != tbPassConfirm.Text)
            {
                MessageBox.Show("The entered password does not match. Please enter the correct password before continuing.",
                    "Wrong password",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            CServerCfg cfg = null;
            try
            {
                cfg = new CServerCfg();
                XElement elDomain = cfg.Root.Element("Users").Element(cbDomains.Items[cbDomains.SelectedIndex].ToString());
                XElement elUser = elDomain.Element(tbUser.Text);
                elUser.Element("pass").SetValue(tbPass.Text);

                MessageBox.Show("The E-Mail account was successfully updated.",
                    "E-Mail Account update success",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("The E-Mail account could not be updated at this time. Please try again later.",
                    "E-Mail Account Update Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
            }
            finally
            {
                if (cfg != null)
                {
                    cfg.Close();
                }
            }
            
        }

        /* 
         * This function is called when the user clicks the "Remove" button
         * which will remove the E-Mail account settings from the Mail configuration 
         * file and also removes the user's mail inbox and all the mail content in it.
         */ 
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You are about to delete the E-Mail account. This will delete the E-Mail Inbox and the user will loose all the mail content in it.\nAre you sure with the delete option ?",
                                "Confirm Delete ?",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return;
                }   

            CServerCfg cfg = null;
            try
            {

                cfg = new CServerCfg();

                XElement elDomain = cfg.Root.Element("Users").Element(cbDomains.Items[cbDomains.SelectedIndex].ToString());
                XElement elUser = elDomain.Element(tbUser.Text);
                DirectoryInfo dinfo = new DirectoryInfo(Application.StartupPath + @"\" + elUser.Element("folder").Value);
                if (dinfo.Exists)
                {
                    dinfo.Delete(true);
                }

                elUser.Remove();
                cbDomains.Items.RemoveAt(cbDomains.SelectedIndex);
                if (cbDomains.Items.Count > 0)
                {
                    cbDomains.SelectedIndex = 0;
                }

                MessageBox.Show("The E-Mail account was successfully removed.",
                    "E-Mail Account deleted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("The E-Mail account cannot be removed at this time. Please try again later.",
                    "E-Mail cannot be deleted",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
            }
            finally
            {
                if (cfg != null)
                {
                    cfg.Close();
                }
            }
        }
    }
}
