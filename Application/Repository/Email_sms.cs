using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Application.Repository
{
    public class Email_sms
    {
        // ====== SMTP Settings (yaha apna server email daalo) ======
        private readonly string _smtpHost ="smtp.hostinger.com";
        private readonly int _smtpPort = 587;
        private readonly bool _enableSsl = true;
        private readonly string _senderEmail = "info@shaheedbhagatsinghhealthandeducation.com";
        private readonly string _senderName = "Shaheedbhagatsinghhealthandeducation.com";
        private readonly string _senderPassword = "Info@2204";

        /// <summary>
        /// Generic Email Send — bas To, Subject, Body do aur bhej do
        /// </summary>
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(_senderEmail, _senderName);
                    mail.To.Add(new MailAddress(toEmail));
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = isHtml;

                    using (var smtp = new SmtpClient(_smtpHost, _smtpPort))
                    {
                        smtp.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                        smtp.EnableSsl = _enableSsl;
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.UseDefaultCredentials = false;
                        smtp.Timeout = 20000;

                        await smtp.SendMailAsync(mail);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}