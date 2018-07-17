using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace Mail_Server
{
    /*
     * This is a utility API for loading, saving or updating the Mail configuration.
     */ 
    class CServerCfg
    {
        /*
         * This private variable stores the path to the mail configuration file.
         */ 
        private static string cfgFileName = Application.StartupPath + @"\mailserver.config";
        
        /*
         * This static private variable will store the reference to the XML file
         * that refers to the mail configuration file.
         * To optimize XML performance the XML file is loaded only once.
         */ 
        private static XDocument xdoc = null;

        /* 
         * This is the default constructor for the CServerCfg class.
         * Here, load the XML configuration file, if it is loaded then do nothing.
         * If the file is not loaded then, create the file and parse it.
         */ 
        public CServerCfg()
        {
            if (xdoc == null)
            {
                try
                {
                    LoadDoc();
                }
                catch
                {
                    try
                    {
                        CreateNewDoc();
                    }
                    catch
                    {
                    }
                }
            }
        }

        /*
         * Load the XML file from the disk.
         * But the file must exist.
         */ 
        private void LoadDoc()
        {
            xdoc = XDocument.Load(cfgFileName);
        }

        /*
         * Parse the XML file if the Load of Document failed.
         */
        private void ParseDoc()
        {
            xdoc = XDocument.Parse(cfgFileName);
        }

        /*
         * Create the new file and load it.
         */ 
        private void CreateNewDoc()
        {
            FileStream fs = File.Create(cfgFileName);

            byte[] bufData = System.Text.Encoding.ASCII.GetBytes("<?xml version=\"1.0\" ?>\n");
            fs.Write(bufData, 0, bufData.Length);

            bufData = System.Text.Encoding.ASCII.GetBytes(@"<MailConfig> </MailConfig>");
            fs.Write(bufData, 0, bufData.Length);

            fs.Close();

            try
            {
                LoadDoc();
            }
            catch
            {
                ParseDoc();
            }
        }

        /* 
         * When Close (if called), the XML data is saved to file on disk and updated.
         */ 
        public void Close()
        {
            try
            {
                xdoc.Save(cfgFileName);
            }
            catch
            {
            }
        }

        /*
         * A public read-only property that returns the reference to the XML document
         * interface.
         */ 
        public XDocument Document
        {
            get
            {
                return xdoc;
            }
        }

        /*
         * A public read-only property that returns the reference to the XElement 
         * interface of the root element in the XML file.
         */ 
        public XElement Root
        {
            get
            {
                return xdoc.Element("MailConfig");
            }
        }
    }
}
