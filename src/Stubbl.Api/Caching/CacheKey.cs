using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using MongoDB.Bson;

namespace Stubbl.Api.Caching
{
    public class CacheKey : ICacheKey
    {
        public string CountTeamLogs(ObjectId teamId)
        {
            return BuildCacheKey(new object[] {teamId});
        }

        public string CountTeamInvitations(ObjectId teamId)
        {
            return BuildCacheKey(new object[] {teamId});
        }

        public string CountTeamStubs(ObjectId teamId)
        {
            return BuildCacheKey(new object[] {teamId});
        }

        public string FindAuthenticatedUser(string sub)
        {
            return BuildCacheKey(new object[] {sub});
        }

        public string FindTeam(ObjectId teamId)
        {
            return BuildCacheKey(new object[] {teamId});
        }

        public string FindTeamInvitation(ObjectId teamId, ObjectId invitationId)
        {
            return BuildCacheKey(new object[] {teamId, invitationId});
        }

        public string FindTeamLog(ObjectId teamId, ObjectId logId)
        {
            return BuildCacheKey(new object[] {teamId, logId});
        }

        public string FindTeamMember(ObjectId teamId, ObjectId memberId)
        {
            return BuildCacheKey(new object[] {teamId, memberId});
        }

        public string FindTeamRole(ObjectId teamId, ObjectId roleId)
        {
            return BuildCacheKey(new object[] {teamId, roleId});
        }

        public string FindTeamStub(ObjectId teamId, ObjectId stubId)
        {
            return BuildCacheKey(new object[] {teamId, stubId});
        }

        private static string BuildCacheKey(IReadOnlyCollection<object> values,
            [CallerMemberName] string callerMemberName = null)
        {
            var cacheKey = callerMemberName;

            return values != null && values.Any() ? $"{cacheKey}_{string.Join("_", values)}" : cacheKey;
        }
    }
}