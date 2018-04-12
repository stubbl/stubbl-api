using MimeKit;

namespace Stubbl.Api.Services.EmailSender
{
    public interface IEmail
    {
        string Body { get; }
        InternetAddressList From { get; }
        string Subject { get; }
        InternetAddressList To { get; }
    }
}