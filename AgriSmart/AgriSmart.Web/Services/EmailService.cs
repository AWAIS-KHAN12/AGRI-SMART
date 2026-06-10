using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AgriSmart.Web.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public bool IsDummy()
        {
            var host = _config["Smtp:Host"];
            var username = _config["Smtp:Username"];
            var password = _config["Smtp:Password"];
            return string.IsNullOrEmpty(host) || 
                   host.Contains("YOUR_GMAIL") || 
                   string.IsNullOrEmpty(username) || 
                   username.Contains("YOUR_GMAIL") ||
                   string.IsNullOrEmpty(password) || 
                   password.Contains("YOUR_APP_PASSWORD_HERE") ||
                   password.Contains("YOUR_GMAIL_APP_PASSWORD");
        }

        public async Task<bool> SendOtpEmailAsync(string toEmail, string otpCode)
        {
            var host = _config["Smtp:Host"];
            var username = _config["Smtp:Username"];
            var password = _config["Smtp:Password"];
            var fromEmail = _config["Smtp:FromEmail"];
            var fromName = _config["Smtp:FromName"] ?? "AgriSmart Pakistan";
            var portVal = _config["Smtp:Port"];
            int port = string.IsNullOrEmpty(portVal) ? 587 : int.Parse(portVal);
            var useSslVal = _config["Smtp:UseSsl"];
            bool useSsl = string.IsNullOrEmpty(useSslVal) || bool.Parse(useSslVal);

            bool isDummy = string.IsNullOrEmpty(host) || 
                           host.Contains("YOUR_GMAIL") || 
                           string.IsNullOrEmpty(username) || 
                           username.Contains("YOUR_GMAIL") ||
                           string.IsNullOrEmpty(password) || 
                           password.Contains("YOUR_APP_PASSWORD_HERE") ||
                           password.Contains("YOUR_GMAIL_APP_PASSWORD");

            if (isDummy)
            {
                _logger.LogWarning("SMTP is not configured in appsettings.json or has placeholder values. DEVELOPER FALLBACK OTP LOGGED:");
                _logger.LogWarning("**************************************************");
                _logger.LogWarning($"* TO: {toEmail}");
                _logger.LogWarning($"* OTP CODE: {otpCode}");
                _logger.LogWarning("**************************************************");
                return true;
            }

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromName, fromEmail));
                message.To.Add(new MailboxAddress(toEmail, toEmail));
                message.Subject = "AgriSmart Verification Code";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #e1e8ed; border-radius: 10px; background-color: #ffffff;'>
                            <div style='text-align: center; margin-bottom: 20px;'>
                                <h2 style='color: #2e7d32; margin: 0;'>AgriSmart Pakistan</h2>
                                <p style='color: #66788a; margin: 5px 0 0 0;'>Empowering Farmers with Technology</p>
                            </div>
                            <hr style='border: 0; border-top: 1px solid #e1e8ed; margin-bottom: 20px;' />
                            <p style='color: #2c3e50; font-size: 16px; line-height: 1.5;'>Hello,</p>
                            <p style='color: #2c3e50; font-size: 16px; line-height: 1.5;'>You are signing into AgriSmart using Email OTP. Please use the following 6-digit verification code to complete your login:</p>
                            <div style='text-align: center; margin: 30px 0;'>
                                <span style='font-size: 32px; font-weight: bold; letter-spacing: 5px; color: #2e7d32; background-color: #f1f8e9; padding: 10px 20px; border-radius: 5px; border: 1px dashed #a5d6a7;'>{otpCode}</span>
                            </div>
                            <p style='color: #e53935; font-size: 14px; font-weight: 500;'>This code will expire in 5 minutes. Do not share this OTP with anyone.</p>
                            <hr style='border: 0; border-top: 1px solid #e1e8ed; margin: 20px 0;' />
                            <p style='color: #8898aa; font-size: 12px; text-align: center; margin: 0;'>If you did not request this code, please ignore this email.</p>
                        </div>"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                var options = SecureSocketOptions.StartTls;
                if (port == 465)
                {
                    options = SecureSocketOptions.SslOnConnect;
                }
                await client.ConnectAsync(host, port, options);
                await client.AuthenticateAsync(username, password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending OTP email to {toEmail}");
                return false;
            }
        }
    }
}
