namespace Stubbl.Api.Core.CommandHandlers
{
   using System;
   using System.IO;
   using System.Threading;
   using System.Threading.Tasks;
   using Common.CommandHandlers;
   using Common.Smtp;
   using Commands.SendEmail.Version1;
   using Events.EmailSent.Version1;
   using MailKit.Net.Smtp;
   using Microsoft.AspNetCore.Hosting;
   using Microsoft.Extensions.Options;

   public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand, EmailSentEvent>
   {
      private readonly IHostingEnvironment _hostingEnvironment;
      private readonly SmtpOptions _smtpOptions;

      public SendEmailCommandHandler(IHostingEnvironment hostingEnvironment, IOptions<SmtpOptions> smtpOptions)
      {
         _hostingEnvironment = hostingEnvironment;
         _smtpOptions = smtpOptions.Value;
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
               await smtpClient.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, cancellationToken: cancellationToken);
               smtpClient.Authenticate(_smtpOptions.Username, _smtpOptions.Password, cancellationToken: cancellationToken);

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
