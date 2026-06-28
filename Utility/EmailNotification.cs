using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Utility
{
    public class EmailNotification
    {
        public static bool SendMailConfirmation(string recipientAddress, string recipientName, string emailSubject, string emailBody,string filepath)
        {
            bool result = false;
            try
            {
                
                string emailSender = ConfigurationManager.AppSettings["fromEmail"].ToString();
                string emailSenderPassword = ConfigurationManager.AppSettings["fromPassword"].ToString();
                string emailSenderHost = ConfigurationManager.AppSettings["host"].ToString();
                int emailSenderPort = Convert.ToInt16(ConfigurationManager.AppSettings["port"]);
                string to = ConfigurationManager.AppSettings["mailTo"].ToString();  // Admin mail
                Boolean enableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["enableSsl"].ToString());
                
                //Base class for sending email  
                MailMessage _mailmsg = new MailMessage();
                //Make TRUE because our body text is html  
                _mailmsg.IsBodyHtml = true;
                //Set From Email ID  
                _mailmsg.From = new MailAddress(emailSender);
                //Set To Email ID  
                _mailmsg.To.Add(recipientAddress);
                //Set Bcc  
                _mailmsg.Bcc.Add(to);

                _mailmsg.Subject = emailSubject;   //Set Subject
                _mailmsg.Body = emailBody;  //Set Body Text of Email 
                SmtpClient _smtp = new SmtpClient(emailSenderHost, emailSenderPort);//Now set your SMTP   
                _smtp.UseDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"].ToString());
    
                //Set SSL --> True / False  
                _smtp.EnableSsl = enableSsl;
                _smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                //Set Sender UserEmailID, Password  
                NetworkCredential _network = new NetworkCredential(emailSender, emailSenderPassword);
                _smtp.Credentials = _network;
                _smtp.EnableSsl = enableSsl;
                //Send Method will send your MailMessage create above.  
                _smtp.Send(_mailmsg);
                result = true;
                LogWriter.LogWrite("Mail sent" , filepath);
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                LogWriter.LogWrite(ex.Message, filepath);

            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.Message, filepath);
                //LogWriter.LogWrite(ex.Message, filepath);
            }
            return result;
        }

        public static bool TestEmail(string filepath)
        {
            bool res = true;
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("brajeshkrgaya@gmail.com");
                    mail.To.Add("brajeshkrgaya@gmail.com");
                    mail.Subject = "Hello World";
                    mail.Body = "<h1>Hello</h1>";
                    mail.IsBodyHtml = true;
                   // mail.Attachments.Add(new Attachment("C:\\file.zip"));

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("brajeshkrgaya@gmail.com", "Hello@98529852");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                res = true;
            }
            catch (Exception ex)
            {
                LogWriter.LogWrite(ex.Message, filepath);
            }
            return res;
        }
    }
}