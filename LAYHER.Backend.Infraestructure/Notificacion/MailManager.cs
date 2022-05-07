using LAYHER.Backend.Domain.Notificacion.Interfaces;
using LAYHER.Backend.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net;

namespace LAYHER.Backend.Infraestructure.Notificacion
{
    public class MailManager : IMailManager
    {
        //private readonly IConfiguracionRepository _configuracionRepository;
        private bool _debugMode;
        private string _host;
        private int _port;
        private bool _enableSecurity;
        private string _SSLorTLS;
        private string _fromEmail;
        private string _fromAlias;
        private string _fromPassword;
        private string[] _mailsBlackList;
        private List<string> _mailsDevelopList;
        private readonly ILogger<IMailManager> _logger;
        public MailManager(ILogger<IMailManager> logger, IConfiguration config)
        {
            this._logger = logger;
            var appSettingsSection = config.GetSection("AppSettings");
            if (appSettingsSection != null)
            {
                this._debugMode = appSettingsSection.GetValue<bool>("DebugMode");
            }

            var smtpSection = config.GetSection("SMTP");
            if (smtpSection != null)
            {
                this._host = smtpSection.GetValue<string>("Host");
                this._port = smtpSection.GetValue<int>("Port");
                this._enableSecurity = smtpSection.GetValue<bool>("EnableSecurity");
                this._SSLorTLS = smtpSection.GetValue<string>("SSLorTLS");
                this._fromEmail = smtpSection.GetValue<string>("FromEmail");
                this._fromAlias = smtpSection.GetValue<string>("FromAlias");
                this._fromPassword = smtpSection.GetValue<string>("FromPassword");
                this._mailsBlackList = smtpSection.GetSection("BlackList").Get<string[]>();
                this._mailsDevelopList = smtpSection.GetSection("DevelopList").Get<string[]>().ToList();
            }
        }

        public async Task<StatusResponse> SendEmail(string subject, string content, List<string> mails_to, List<string> mails_cc = null, List<string> mails_co = null, List<Attachment> files = null, bool isHTML = true)
        {
            StatusResponse status = new StatusResponse();

            try
            {
                if (this._debugMode)
                {
                    subject = subject + " (DEVELOPMENT MODE)";
                    mails_to = this._mailsDevelopList;
                    mails_cc = null;
                    mails_co = null;
                }

                if (mails_to == null) mails_to = new List<string>();
                if (mails_cc == null) mails_cc = new List<string>();
                if (mails_co == null) mails_co = new List<string>();

                SmtpClient client = this._port == 0
                        ? new SmtpClient(this._host)
                        : new SmtpClient(this._host, this._port);

                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(this._fromEmail, this._fromPassword);
                if (this._enableSecurity)
                {
                    //valida como enviar TLS o SSL
                    client.EnableSsl = true;
                }

                client.Timeout = 10000;

                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(this._fromEmail, this._fromAlias);

                foreach (var mail in mails_to)
                {
                    if (this._mailsBlackList.SingleOrDefault(i => i == mail) == null)
                        mailMessage.To.Add(mail);
                }
                foreach (var mail in mails_cc)
                {
                    if (this._mailsBlackList.SingleOrDefault(i => i == mail) == null)
                        mailMessage.CC.Add(mail);
                }
                foreach (var mail in mails_co)
                {
                    if (this._mailsBlackList.SingleOrDefault(i => i == mail) == null)
                        mailMessage.Bcc.Add(mail);
                };

                mailMessage.IsBodyHtml = isHTML;
                mailMessage.Body = content.ToString();
                mailMessage.Subject = subject;

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.SubjectEncoding = System.Text.Encoding.UTF8;


                if (files != null && files.Count > 0)
                    foreach (var file in files)
                        mailMessage.Attachments.Add(file);

                await client.SendMailAsync(mailMessage);

                status.Success = true;
                return status;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "No se pudo enviar email");

                status.Success = false;
                status.Title = "No se pudo enviar email";
                return status;
            }
        }
    }
}
