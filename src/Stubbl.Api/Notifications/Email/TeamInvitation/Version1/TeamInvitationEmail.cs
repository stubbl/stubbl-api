using MimeKit;
using MongoDB.Bson;
using Stubbl.Api.Services.EmailSender;

namespace Stubbl.Api.Notifications.Email.TeamInvitation.Version1
{
    public class TeamInvitationEmail : IEmail
    {
        public TeamInvitationEmail(string to, string teamName, string username,
            ObjectId invitationId)
        {
            From = new InternetAddressList {new MailboxAddress("Stubbl", "noreply@stubbl.it")};
            To = new InternetAddressList {new MailboxAddress(to)};
            Subject = $"Stubbl: You've been invited to join team '{teamName}'";
            Body =
                $@"<p><strong>{
                        username
                    }</strong> has invited you to join Stubbl team '<strong><a href=""http://stubbl.it/invitations/{
                        invitationId
                    }"">{teamName}</a></strong>'.</p><p>Stubbl <3</p>";
        }

        public string Body { get; }
        public InternetAddressList From { get; }
        public string Subject { get; }
        public InternetAddressList To { get; }
    }
}