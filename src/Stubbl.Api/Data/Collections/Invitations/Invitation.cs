using MongoDB.Bson;

namespace Stubbl.Api.Data.Collections.Invitations
{
    public class Invitation
    {
        public string EmailAddress { get; set; }
        public ObjectId Id { get; set; }
        public Role Role { get; set; }
        public InvitationStatus Status { get; set; }
        public Team Team { get; set; }
    }
}