using MimeKit;

namespace Stubbl.Api.Commands.SendEmail.Version1
{
    public interface IEmail
    {
        MimeMessage Message { get; }
    }
}