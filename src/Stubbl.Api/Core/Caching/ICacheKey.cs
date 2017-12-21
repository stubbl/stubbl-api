namespace Stubbl.Api.Core.Caching
{
   using MongoDB.Bson;

   public interface ICacheKey
   {
      string CountTeamLogs(ObjectId teamId);
      string CountTeamInvitations(ObjectId teamId);
      string CountTeamStubs(ObjectId teamId);
      string FindAuthenticatedMember(string identityId);
      string FindAuthenticatedMemberInvitation(string emailAddress, ObjectId invitationId);
      string FindTeam(ObjectId teamId);
      string FindTeamInvitation(ObjectId teamId, ObjectId invitationId);
      string FindTeamLog(ObjectId teamId, ObjectId logId);
      string FindTeamMember(ObjectId teamId, ObjectId memberId);
      string FindTeamRole(ObjectId teamId, ObjectId roleId);
      string FindTeamStub(ObjectId teamId, ObjectId stubId);
   }
}