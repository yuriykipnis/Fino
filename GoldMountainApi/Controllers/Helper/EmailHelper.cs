using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using GoldMountainShared.Storage.Documents;

namespace GoldMountainApi.Controllers.Helper
{
    public class EmailHelper : IEmailHelper
    {
        public void SendMessage(ContactMessage message)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("fromAddr");
                msg.To.Add("toAddr");
                msg.Subject = "Subj";
                msg.IsBodyHtml = true;
                msg.BodyEncoding = Encoding.ASCII;
                msg.Body = "Body";
                SendMail(msg);
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }

        private void SendMail(MailMessage msg)
        {
            string username = "username";  //email address or domain user for exchange authentication
            string password = "password";  //password
            SmtpClient mClient = new SmtpClient();
            mClient.Host = "mailex.company.us";
            mClient.Credentials = new NetworkCredential(username, password);
            mClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            mClient.Timeout = 100000;
            mClient.Send(msg);
        }
    }
}
