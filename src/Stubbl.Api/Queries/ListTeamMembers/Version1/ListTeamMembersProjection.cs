using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;
using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.ListTeamMembers.Version1
{
    public class ListTeamMembersProjection : IProjection
    {
        public ListTeamMembersProjection(IReadOnlyCollection<Member> members, Paging paging)
        {
            Members = members;
            Paging = paging;
        }

        public IReadOnlyCollection<Member> Members { get; }
        public Paging Paging { get; }
    }
}