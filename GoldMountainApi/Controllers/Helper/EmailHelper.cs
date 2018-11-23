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
        const string FinoEmail = "yuriy.kipnis@gmail.com";
        const string SmtpUser = "bewise.demo@gmail.com";
        const string SmtpPassword = "!Q2w3e4r";

        public async Task SendMessage(ContactMessage message)
        {
            try
            {
                MailMessage msg = new MailMessage(message.Email, FinoEmail);
                msg.Subject = message.Subject;
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
            SmtpClient client = new SmtpClient();
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(SmtpUser, SmtpPassword);
            client.Send(msg);
        }
    }
}
