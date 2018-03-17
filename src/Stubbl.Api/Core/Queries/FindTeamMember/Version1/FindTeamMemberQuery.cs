namespace Stubbl.Api.Core.Queries.FindTeamMember.Version1
{
   using Gunnsoft.Cqs.Queries;
   using MongoDB.Bson;

   public class FindTeamMemberQuery : IQuery<FindTeamMemberProjection>
   {
      public FindTeamMemberQuery(ObjectId teamId, ObjectId memberId)
      {
         TeamId = teamId;
         MemberId = memberId;
      }

      public ObjectId MemberId { get; }
      public ObjectId TeamId { get; }
   }
}