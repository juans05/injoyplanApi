using LAYHER.Backend.Shared;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LAYHER.Backend.Domain.Notificacion.Interfaces
{
    public interface IMailManager
    {
        Task<StatusResponse> SendEmail(string subject, string content, List<string> mails_to, List<string> mails_cc = null, List<string> mails_co = null, List<Attachment> files = null, bool isHTML = true);
    }
}
