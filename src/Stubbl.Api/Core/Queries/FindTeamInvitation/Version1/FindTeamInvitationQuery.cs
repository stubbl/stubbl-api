namespace Stubbl.Api.Core.Queries.FindTeamInvitation.Version1
{
   using CodeContrib.Queries;
   using MongoDB.Bson;

   public class FindTeamInvitationQuery : IQuery<FindTeamInvitationProjection>
   {
      public FindTeamInvitationQuery(ObjectId teamId, ObjectId invitationId)
      {
         TeamId = teamId;
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
      public ObjectId TeamId { get; }
   }
}
