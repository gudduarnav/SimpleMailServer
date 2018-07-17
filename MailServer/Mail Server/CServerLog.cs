using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Windows.Forms;
using System.Collections;

namespace Mail_Server
{
    /*
     * This is a structure, that contains the representation for each event in the XML
     * based Log file.
     */ 
    struct ServerLogEntry
    {
        public DateTime eventTime;
        public string eventStatus;
        public string eventMessage;
    }

    /*
     * The CServerLog class is the abstract representation for the Mail Server log file.
     */ 
    class CServerLog : IDisposable, IEnumerable
    {
        /* 
         * private static variable that holds the reference to the XML document of
         * the server log.
         */ 
        private static XDocument doc = null;
        
        /*
         * private static variable that represents the path to the Mail Server log file.
         */ 
        private static string logPath = Application.StartupPath + @"\mailserver.log";

        /*
         * The default constructor of CServerLog file, which is responsible to
         * open / create the server log file once, for reading (as optimization).
         */ 
        public CServerLog()
        {
            if (doc == null)
            {
                OpenDoc();
            }
        }

        /* 
         * This private function creates / opens the server log file.
         */ 
        private void OpenDoc()
        {
            try
            {
                FileInfo finfo = new FileInfo(logPath);
                if (finfo.Exists)
                {
                    OpenXml();
                }
                else
                {
                    FileStream fs = finfo.Create();
                    StreamWriter sout = new StreamWriter(fs);
                    sout.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                    sout.WriteLine("<log>");
                    sout.WriteLine("</log>");
                    sout.Flush();
                    fs.Close();

                    OpenXml();
                }
            }
            catch
            {
            }
        }
        
        /*
         * This private function loads or parse the XML document.
         */ 
        private void OpenXml()
        {
            try
            {
                doc = XDocument.Load(logPath);
                if (doc == null)
                    throw new Exception();
            }
            catch
            {
                try
                {
                    doc = XDocument.Parse(logPath);
                }
                catch
                {
                }
            }
        }

        /*
         * This is the Dispose() method, which is called by the garbage collector when
         * there is no reference to the object.
         */ 
        void IDisposable.Dispose()
        {
            Close();
        }

        /*
         * This function if called, whence the XML file is updated with new Log
         * data.
         */ 
        public void Close()
        {
            if (doc == null)
            {
                return;
            }

            try
            {
                doc.Save(logPath);
            }
            catch
            {
            }
        }

        /* 
         * This function is called to add a Log entry to the XML Mail server log file.
         */ 
        public void WriteLog(string severityString,
                             string eventString)
        {
            if (doc == null)
            {
                return;
            }

            try
            {
                XElement el = new XElement("message");
                el.Add(new XElement("date", DateTime.Now.ToString()));
                el.Add(new XElement("status", severityString));
                el.Add(new XElement("event", eventString));
                doc.Element("log").Add(el);
            }
            catch
            {
            }
        }

        /*
         * This function will be used by foreach to enumerate the log entries.
         */ 
        IEnumerator IEnumerable.GetEnumerator()
        {
            if (doc == null)
            {
                yield break;
            }

            XElement elRoot = doc.Element("log");
            foreach (XElement el in elRoot.Nodes())
             {
                  ServerLogEntry se;
                  se.eventMessage = el.Element("event").Value;
                  se.eventStatus = el.Element("status").Value;
                  se.eventTime = DateTime.Parse(el.Element("date").Value);
                  yield return se;
              }
        }

        /*
         * This function is called to clear all the log entries in the XML log 
         * file.
         */ 
        public void ClearLog()
        {
            if (doc == null)
            {
                return;
            }

            try
            {
                Close();

                XElement elRoot = doc.Element("log");
                elRoot.RemoveAll();
            }
            catch
            {
            }
            finally
            {
                Close();
            }
        }

        /* 
         * This static function is called to add a log entry without creating a instance 
         * of this class.
         * The function takes in the log entry and automatically saves it to the
         * Log XML and updates it.
         */ 
        public static void LogEntry(string severityString,
                                    string eventString)
        {
            CServerLog log = null;
            try
            {
                log = new CServerLog();
                log.WriteLog(severityString, eventString);
            }
            catch
            {
            }
            finally
            {
                if (log != null)
                {
                    log.Close();
                }
            }
        }
    }
}
