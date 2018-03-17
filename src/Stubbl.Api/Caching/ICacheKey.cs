using MongoDB.Bson;

namespace Stubbl.Api.Caching
{
    public interface ICacheKey
    {
        string CountTeamLogs(ObjectId teamId);
        string CountTeamInvitations(ObjectId teamId);
        string CountTeamStubs(ObjectId teamId);
        string FindAuthenticatedUser(string identityId);
        string FindAuthenticatedUserInvitation(string emailAddress, ObjectId invitationId);
        string FindTeam(ObjectId teamId);
        string FindTeamInvitation(ObjectId teamId, ObjectId invitationId);
        string FindTeamLog(ObjectId teamId, ObjectId logId);
        string FindTeamMember(ObjectId teamId, ObjectId memberId);
        string FindTeamRole(ObjectId teamId, ObjectId roleId);
        string FindTeamStub(ObjectId teamId, ObjectId stubId);
    }
}