namespace Stubbl.Api.Core.Queries.CountTeamInvitations.Version1
{
   using Common.Queries;
   using MongoDB.Bson;

   public class CountTeamInvitationsQuery : IQuery<CountTeamInvitationsProjection>
   {
      public CountTeamInvitationsQuery(ObjectId teamId)
      {
         TeamId = teamId;
      }

      public ObjectId TeamId { get; }
   }
}