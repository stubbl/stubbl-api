using MimeKit;
using MongoDB.Bson;
using Stubbl.Api.Commands.SendEmail.Version1;

namespace Stubbl.Api.Notifications.Email.TeamInvitation.Version1
{
    public class TeamInvitationEmail : IEmail
    {
        public TeamInvitationEmail(string toEmailAddress, string teamName, string authenticatedUserName,
            ObjectId invitationId)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Stubbl", "noreply@stubbl.it"));
            message.To.Add(new MailboxAddress(toEmailAddress));
            message.Subject = $"Stubbl: You've been invited to join team '{teamName}'";
            message.Body = new TextPart("html")
            {
                Text =
                    $@"<p><strong>{
                            authenticatedUserName
                        }</strong> has invited you to join Stubbl team '<strong><a href=""http://stubbl.it/invitations/{
                            invitationId
                        }"">{teamName}</a></strong>'.</p><p>Stubbl <3</p>"
            };

            Message = message;
        }

        public MimeMessage Message { get; }
    }
}