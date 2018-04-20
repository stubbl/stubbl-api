using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using Stubbl.Api.Options;

namespace Stubbl.Api.Services.EmailSender
{
    public class EmailSender : IEmailSender
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly SmtpOptions _smtpOptions;

        public EmailSender(IHostingEnvironment hostingEnvironment, IOptions<SmtpOptions> smtpOptions)
        {
            _hostingEnvironment = hostingEnvironment;
            _smtpOptions = smtpOptions.Value;
        }

        public async Task SendEmailAsync(IEmail email, CancellationToken cancellationToken)
        {
            var message = new MimeMessage();
            message.From.AddRange(email.From);
            message.To.AddRange(email.To);
            message.Subject = email.Subject;
            message.Body = new TextPart("html")
            {
                Text = email.Body
            };

            if (_hostingEnvironment.IsDevelopment())
            {
                await message.WriteToAsync(Path.Combine(AppContext.BaseDirectory, $"{message.To}-{DateTime.UtcNow:yyyy-MM-ddThh-mm-ss}.eml"), cancellationToken);
            }
            else
            {
                using (var smtpClient = new SmtpClient())
                {
                    var host = _smtpOptions.Host;
                    var port = _smtpOptions.Port;
                    var username = _smtpOptions.Username;
                    var password = _smtpOptions.Password;

                    await smtpClient.ConnectAsync(host, port, cancellationToken: cancellationToken);
                    smtpClient.Authenticate(username, password, cancellationToken);

                    await smtpClient.SendAsync(message, cancellationToken);
                    await smtpClient.DisconnectAsync(true, cancellationToken);
                }
            }
        }
    }
}