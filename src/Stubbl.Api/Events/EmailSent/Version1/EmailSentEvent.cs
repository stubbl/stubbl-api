using Gunnsoft.Cqs.Events;
using MimeKit;

namespace Stubbl.Api.Events.EmailSent.Version1
{
    public class EmailSentEvent : IEvent
    {
        public EmailSentEvent(MimeMessage message)
        {
            Message = message;
        }

        public MimeMessage Message { get; }
    }
}