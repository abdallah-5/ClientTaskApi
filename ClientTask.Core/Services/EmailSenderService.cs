using ClientTask.Core.Interfaces.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ClientTask.Core.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        
        public async Task SendMailAsync(string fromAddress, string toAddress, string mailSubject, string mailBody)
        {
            MailMessage mail = new MailMessage();
            MailMessage mailMsg = new MailMessage(fromAddress, toAddress, mailSubject, mailBody);
            mailMsg.IsBodyHtml = true;
            mailMsg.Priority = MailPriority.Normal;
            SmtpClient smtp = new SmtpClient();

            smtp.EnableSsl = true;

            smtp.Credentials = new NetworkCredential("testtittest40@gmail.com", "rval zjjb mxth ceoe");
            smtp.Host = "smtp.gmail.com ";
            smtp.Port = 587;

            smtp.Send(mailMsg);
        }
    }
}
