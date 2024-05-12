using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTask.Core.Interfaces.IServices
{
    public interface IEmailSenderService
    {
        Task SendMailAsync(string fromAddress, string toAddress, string mailSubject, string mailBody);
    }
}
