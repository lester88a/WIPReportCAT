using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WIPReportCat.Configuration
{
    public class Config
    {
        //Thread Safe Singleton without using locks and no lazy instantiation
        private static readonly Config instance = new Config();
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Config() { }
        private Config() { }

        private static string DataSource { get; set; }
        private static string InitialCatalog { get; set; }
        private static string Manufacturer { get; set; }
        private static  string ConfigFile { get; set; }
        private static string FileSavedPath { get; set; }
        //instance variables for email
        private static string Recipient { get; set; }
        private static string RecipientAdmin { get; set; }
        private static string Sender { get; set; }
        private static string SmtpServer { get; set; }
        private static int SmtpPort { get; set; }
        private static string SenderUserName { get; set; }
        private static string SenderUserPass { get; set; }

        //constructor
        public static Config Instance
        {
            get 
            {
                ConfigFile = System.AppDomain.CurrentDomain.BaseDirectory + @"Config.xml";
                XmlDocument doc = new XmlDocument();
                doc.Load(ConfigFile);
                //config for server
                DataSource = doc.DocumentElement.SelectSingleNode("/config/dataSource").InnerText;
                InitialCatalog = doc.DocumentElement.SelectSingleNode("/config/initialCatalog").InnerText;
                Manufacturer = doc.DocumentElement.SelectSingleNode("/config/manafacturer").InnerText;
                FileSavedPath = doc.DocumentElement.SelectSingleNode("/config/fileSavedPath").InnerText;
                //config for email
                Recipient = doc.DocumentElement.SelectSingleNode("/config/email/recipient").InnerText;
                RecipientAdmin = doc.DocumentElement.SelectSingleNode("/config/email/recipientAdmin").InnerText;
                Sender = doc.DocumentElement.SelectSingleNode("/config/email/sender").InnerText;
                SmtpServer = doc.DocumentElement.SelectSingleNode("/config/email/smtpServer").InnerText;
                SmtpPort = Convert.ToInt32(doc.DocumentElement.SelectSingleNode("/config/email/smtpPort").InnerText);
                SenderUserName = doc.DocumentElement.SelectSingleNode("/config/email/senderUserName").InnerText;
                SenderUserPass = doc.DocumentElement.SelectSingleNode("/config/email/senderUserPass").InnerText;

                return instance;
            }
        }
        
        public string GetDataSource()
        {
            return DataSource;
        }

        public string GetInitialCatalog()
        {
            return InitialCatalog;
        }

        public string GetManufacturer()
        {
            return Manufacturer;
        }

        public string GetFileSavedPath()
        {
            return FileSavedPath;
        }

        public string GetRecipient()
        {
            return Recipient;
        }

        public string GetRecipientAdmin()
        {
            return RecipientAdmin;
        }

        public string GetSender()
        {
            return Sender;
        }

        public string GetSmtpServer()
        {
            return SmtpServer;
        }

        public int GetSmtpPort()
        {
            return SmtpPort;
        }

        public string GetSenderUserName()
        {
            return SenderUserName;
        }

        public string GetSenderUserPass()
        {
            return SenderUserPass;
        }
    }
}
