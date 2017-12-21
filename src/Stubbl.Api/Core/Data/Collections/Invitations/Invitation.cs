namespace Stubbl.Api.Core.Data.Collections.Invitations
{
   using MongoDB.Bson;

   public class Invitation
   {
      public string EmailAddress { get; set; }
      public ObjectId Id { get; set; }
      public Role Role { get; set; }
      public InvitationStatus Status { get; set; }
      public Team Team { get; set; }
   }
}