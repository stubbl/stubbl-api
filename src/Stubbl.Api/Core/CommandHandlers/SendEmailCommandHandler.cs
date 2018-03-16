namespace Stubbl.Api.Core.CommandHandlers
{
    using CodeContrib.CommandHandlers;
    using Commands.SendEmail.Version1;
    using Events.EmailSent.Version1;
    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand, EmailSentEvent>
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public SendEmailCommandHandler(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<EmailSentEvent> HandleAsync(SendEmailCommand command, CancellationToken cancellationToken)
        {
            if (_hostingEnvironment.IsDevelopment())
            {
                command.Email.Message.WriteTo(Path.Combine(AppContext.BaseDirectory, $"{Guid.NewGuid()}.eml"));
            }
            else
            {
                using (var smtpClient = new SmtpClient())
                {
                    var configurationSection = _configuration.GetSection("Smtp");
                    var host = configurationSection.GetValue<string>("Host");
                    var port = configurationSection.GetValue<int>("Port");
                    var username = configurationSection.GetValue<string>("Username");
                    var password = configurationSection.GetValue<string>("Password");

                    await smtpClient.ConnectAsync(host, port, cancellationToken: cancellationToken);
                    smtpClient.Authenticate(username, password, cancellationToken: cancellationToken);

                    await smtpClient.SendAsync(command.Email.Message, cancellationToken);
                    await smtpClient.DisconnectAsync(true, cancellationToken);
                }
            }

            return new EmailSentEvent
            (
               command.Email.Message
            );
        }
    }
}
