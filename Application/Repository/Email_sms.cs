using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Application.Repository
{
    public class Email_sms
    {
        // ====== SMTP Settings (yaha apna server email daalo) ======
        private readonly string _smtpHost ="smtp.hostinger.com";
        private readonly int _smtpPort = 587;
        private readonly bool _enableSsl = true;
        private readonly string _senderEmail = "no-replay@shaheedbhagatsinghhealthandeducation.com";
        private readonly string _senderName = "Shaheedbhagatsinghhealthandeducation.com";
        private readonly string _senderPassword = "Noreplay@2026";

        /// <summary>
        /// Generic Email Send — bas To, Subject, Body do aur bhej do
        /// </summary>
        public async Task<(bool Success, string Error)> SendEmailAsync(
            string toEmail, string subject, string body, bool isHtml = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toEmail))
                    return (false, "Recipient email is empty");

                if (string.IsNullOrWhiteSpace(_senderEmail) || string.IsNullOrWhiteSpace(_senderPassword))
                    return (false, "SMTP credentials not configured in Email_sms class");

                using var mail = new MailMessage();
                mail.From = new MailAddress(_senderEmail, _senderName);
                mail.To.Add(new MailAddress(toEmail));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = isHtml;

                // ✅ STEP 1: SMTP host ko IPv4 me resolve karo
                //    Isse outbound connection IPv6 se nahi jaayega
                string connectHost = _smtpHost;
                bool usingIp = false;

                try
                {
                    var addresses = await Dns.GetHostAddressesAsync(_smtpHost);
                    var ipv4 = addresses.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
                    if (ipv4 != null)
                    {
                        connectHost = ipv4.ToString();
                        usingIp = true;
                    }
                }
                catch
                {
                    // DNS resolve fail ho to hostname hi use karo
                }

                // ✅ STEP 2: Agar IP se connect kar rahe ho to cert mismatch ignore karo
                //    (Cert hostname ke liye hai, IP ke liye nahi)
                if (usingIp && _enableSsl)
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                        (sender, certificate, chain, sslPolicyErrors) =>
                        {
                            return sslPolicyErrors == System.Net.Security.SslPolicyErrors.None
                                || sslPolicyErrors == System.Net.Security.SslPolicyErrors.RemoteCertificateNameMismatch;
                        };
                }

                // ✅ STEP 3: SmtpClient banao aur bhejo
                using var smtp = new SmtpClient(connectHost, _smtpPort)
                {
                    Credentials = new NetworkCredential(_senderEmail, _senderPassword),
                    EnableSsl = _enableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Timeout = 30000
                };

                await smtp.SendMailAsync(mail);
                return (true, string.Empty);
            }
            catch (SmtpException smtpEx)
            {
                return (false, $"SMTP [{smtpEx.StatusCode}]: {smtpEx.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"{ex.GetType().Name}: {ex.Message}");
            }
        }
    }



}
 