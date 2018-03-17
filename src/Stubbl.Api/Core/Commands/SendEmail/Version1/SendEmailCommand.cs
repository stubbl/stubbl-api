﻿namespace Stubbl.Api.Core.Commands.SendEmail.Version1
{
   using Gunnsoft.Cqs.Commands;
   using Events.EmailSent.Version1;

   public class SendEmailCommand : ICommand<EmailSentEvent>
   {
      public SendEmailCommand(IEmail email)
      {
         Email = email;
      }

      public IEmail Email { get; }
   }
}
