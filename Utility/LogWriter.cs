using System;
using System.Configuration;
using System.IO;

namespace Utility
{
    public static class LogWriter
    {
        public static object _locked = new object();
        public static void LogWrite(string logMessage,string path)
        {
            //m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string logfileName = ConfigurationManager.AppSettings["logfileName"].ToString();
            if (!File.Exists(path + "\\"+ logfileName))
                File.Create(path + "\\"+ logfileName);

            try
            {
                
                using (StreamWriter w = File.AppendText(path + "\\" + logfileName))
                    AppendLog(logMessage, w);
            }
            catch (Exception ex)
            {
                lock (_locked)
                {
                    File.AppendAllText(Path.Combine(path, logfileName), logMessage + Environment.NewLine);
                }
            }

        }

        private static void AppendLog(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
    
}