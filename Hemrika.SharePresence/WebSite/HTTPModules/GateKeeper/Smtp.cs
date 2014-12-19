using System;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using Microsoft.SharePoint.Administration;

namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    public class Smtp
    {
        /// <summary>
        /// Sends a notification when honeypot has been violated
        /// and the IP Address has been added to the Deny IP Address list
        /// </summary>
        public static void SendNotification(HttpContext current)
        {
            //GateKeeperModule.log.Debug("Sending notification");

            string ipaddress = current.Request.UserHostAddress.ToString();
            string userAgent = current.Request.UserAgent == null ? "Empty" : current.Request.UserAgent;
            string referrer = current.Request.UrlReferrer == null ? "Empty" : current.Request.UrlReferrer.AbsoluteUri;

            //GateKeeperModule.log.DebugFormat("IPAddress : [{0}]", ipaddress);
            //GateKeeperModule.log.DebugFormat("userAgent : [{0}]", userAgent);
            //GateKeeperModule.log.DebugFormat("referrer : [{0}]", referrer);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div>");
            sb.AppendLine("<h2 style=\"color: maroon; padding: 15px 0;\">Access has been denied to a suspected SPAM-Bot</h2>");
            sb.AppendLine(string.Format("<p>IP Address: {0}</p>", ipaddress));
            sb.AppendLine(string.Format("<p>UserAgent: {0}</p>", userAgent));
            sb.AppendLine(string.Format("<p>Referrer: {0}</p>", referrer));
            sb.AppendLine(string.Format("<p>{0}</p>", GateKeeperModule.config.SmtpMessageBody));
            sb.AppendLine("</div>");

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(GateKeeperModule.config.SmtpEmailAddress);
            mail.To.Add(GateKeeperModule.config.SmtpEmailAddress);

            mail.Subject = GateKeeperModule.config.SmtpMessageSubject;
            mail.Body = sb.ToString();

            try
            {
                //GateKeeperModule.log.Debug("Sending message");
                SendMailMessageAsync(mail); 
            }
            catch (Exception)
            {
                //GateKeeperModule.log.WarnFormat("SendNotification : [{0}]", ex.Message);
                //GateKeeperModule.log.Error(ex.StackTrace);
                throw;
            }
        }

        private static void SendMailMessage(MailMessage message)
        {
            try
            {
                if (message == null)
                    throw new ArgumentNullException("message");

                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;
                SmtpClient smtp = new SmtpClient(GateKeeperModule.config.SmtpServerName);

                if (!string.IsNullOrEmpty(GateKeeperModule.config.SmtpUserName))
                {
                    string password = GateKeeperModule.config.SmtpPassword;
                    if (GateKeeperModule.config.StorePasswordEncrypted)
                    {
                        password = Encryption.Decrypt(GateKeeperModule.config.SmtpPassword);
                    }
                    smtp.Credentials = new System.Net.NetworkCredential(GateKeeperModule.config.SmtpUserName, password); 
                }

                smtp.Port = GateKeeperModule.config.SmtpServerPort;
                smtp.EnableSsl = GateKeeperModule.config.SmtpEnableSSL;
                smtp.Send(message);
                OnEmailSent(message);
            }
            catch (SmtpException ex)
            {
                SPDiagnosticsService.Local.WriteTrace(0, new SPDiagnosticsCategory(ex.Source, TraceSeverity.High, EventSeverity.Error), TraceSeverity.High, ex.Message, ex.Data);
                //GateKeeperModule.log.WarnFormat("SMTP SendMailMessage : [{0}]", ex.Message);
                //GateKeeperModule.log.Error(ex.StackTrace);
                OnEmailFailed(message); 
            }
            finally
            {
                message.Dispose();
                message = null;
            }
        }

        /// <summary>
        /// Sends the mail message asynchronously in another thread.
        /// </summary>
        /// <param name="message">The message to send.</param>
        public static void SendMailMessageAsync(MailMessage message)
        {
            ThreadPool.QueueUserWorkItem(delegate { SendMailMessage(message); });
        }

        /// <summary>
        /// Occurs after an e-mail has been sent. The sender is the MailMessage object.
        /// </summary>
        public static event EventHandler<EventArgs> EmailSent;
        private static void OnEmailSent(MailMessage message)
        {
            if (EmailSent != null)
            {
                //GateKeeperModule.log.Debug("Executing EmailSent eventHandler");
                EmailSent(message, new EventArgs()); 
            }
        }

        /// <summary>
        /// Occurs after an e-mail has been sent. The sender is the MailMessage object.
        /// </summary>
        public static event EventHandler<EventArgs> EmailFailed;
        private static void OnEmailFailed(MailMessage message)
        {
            if (EmailFailed != null)
            {
                //GateKeeperModule.log.Debug("Executing EmailFailed eventHandler");
                EmailFailed(message, new EventArgs()); 
            }
        }

    }
}
