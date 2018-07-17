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
     * This is the main form for Configuring E-Mail domain names.
     */ 
    public partial class CfgDomain : Form
    {
        /* 
         * This is the default constructor for DfgDomain class.
         */ 
        public CfgDomain()
        {
            InitializeComponent();
        }

        /*
         * This function is called when the Domain configuration form is loading.
         * Here we will load the XML configuration file, and enumerate through
         * all the domain entries and display it in a list box on the form for
         * the user to choose from.
         */ 
        private void CfgDomain_Load(object sender, EventArgs e)
        {
            CServerCfg cfg = null;
            try
            {
                cfg = new CServerCfg();
                try
                {
                    XElement el = cfg.Root.Element("Domains");
                    if (el == null)
                        throw new Exception();

                    foreach (XElement dmname in el.Nodes())
                    {
                        lbDomains.Items.Add(dmname.Name.LocalName);
                    }

                    if (lbDomains.Items.Count > 0)
                    {
                        lbDomains.SelectedIndex = 0;
                    }
                }
                catch
                {
                    cfg.Root.Add(new XElement("Domains"));
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
         * This function is invoked when the "Done" button is clicked.
         * So we are done, then destroy the form.
         */
        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /*
         * Add the new domain to the list of list box on the form and also 
         * to the mail configuration file.
         */ 
        private void btnAdd_Click(object sender, EventArgs e)
        {
            tbDomain.Text = tbDomain.Text.Trim();
            if (tbDomain.Text == "")
            {
                return;
            }

            if (lbDomains.Items.IndexOf(tbDomain.Text.ToLower()) >= 0)
            {
                return;
            }

            CServerCfg cfg = null;
            try
            {
                cfg = new CServerCfg();
                cfg.Root.Element("Domains").Add(new XElement(tbDomain.Text.ToLower()));
                lbDomains.SelectedIndex = lbDomains.Items.Add(tbDomain.Text.ToLower());
                MessageBox.Show("The Domain Name has been added to the Domain List.",
                    "Domain Name Added",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("The Domain Name cannot be added to the Domain List.",
                    "Domain Name Added Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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
         * This function will display the selected domain name on the list box in tge
         * domain name text function, an dwill be invoked when the user selects a
         * new domain name from the domain name list.
         */ 
        private void lbDomains_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbDomain.Text = lbDomains.Items[lbDomains.SelectedIndex] as String;
        }

        /*
         * Remove the selected domain from the domain name list and
         * delete the domain name from Mail server configuration file. It also 
         * removes the users and inboxes configured under it.
         */ 
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("You are about to delete the E-Mail Domain. This will delete all the E-Mail Inbox configured with it and the dependent user will loose all the mail content in their respective Inbox.\nAre you sure with the delete option ?",
                                "Confirm Delete of Domain ?",
                                MessageBoxButtons.OKCancel,
                                MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                        return;
                }

            CServerCfg cfg = null;
            try
            {

                cfg = new CServerCfg();
                cfg.Root.Element("Domains").Element(lbDomains.Items[lbDomains.SelectedIndex].ToString()).Remove();
                XElement udomain = cfg.Root.Element("Users").Element(lbDomains.Items[lbDomains.SelectedIndex].ToString());
                if (udomain != null)
                {
                    foreach (XElement users in udomain.Nodes())
                    {
                        try
                        {
                            XElement ufolder = users.Element("folder");
                            DirectoryInfo dinfo = new DirectoryInfo(Application.StartupPath + @"\" + ufolder.Value);
                            dinfo.Delete(true);
                        }
                        catch
                        {
                        }
                    }
                    udomain.Remove();
                }

                try
                {
                    lbDomains.Items.RemoveAt(lbDomains.SelectedIndex);
                }
                catch
                {
                }
                finally
                {
                    if (lbDomains.Items.Count > 0)
                    {
                        lbDomains.SelectedIndex = 0;
                    }
                }

                MessageBox.Show("The Domain name was removed successfully from the Domain List.",
                    "Domain Name Removed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("The Domain Name cannot be removed from the Domain Name list.",
                    "Domain Name Removal Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                cfg.Close();
            }
        }

    }
}
