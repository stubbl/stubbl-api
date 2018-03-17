using Gunnsoft.Cqs.Commands;
using Stubbl.Api.Events.EmailSent.Version1;

namespace Stubbl.Api.Commands.SendEmail.Version1
{
    public class SendEmailCommand : ICommand<EmailSentEvent>
    {
        public SendEmailCommand(IEmail email)
        {
            Email = email;
        }

        public IEmail Email { get; }
    }
}