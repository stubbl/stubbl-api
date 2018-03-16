namespace Stubbl.Api.Core.Queries.FindAuthenticatedUserInvitation.Version1
{
   using CodeContrib.Queries;
   using MongoDB.Bson;

   public class FindAuthenticatedUserInvitationQuery : IQuery<FindAuthenticatedUserInvitationProjection>
   {
      public FindAuthenticatedUserInvitationQuery(ObjectId invitationId)
      {
         InvitationId = invitationId;
      }

      public ObjectId InvitationId { get; }
   }
}