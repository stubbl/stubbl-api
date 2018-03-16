namespace Stubbl.Api.Core.Events.EmailSent.Version1
{
   using CodeContrib.Events;
   using MimeKit;

   public class EmailSentEvent : IEvent
   {
      public EmailSentEvent(MimeMessage message)
      {
         Message = message;
      }

      public MimeMessage Message { get; }
   }
}
