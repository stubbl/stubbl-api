using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.ListTeams.Version1
{
    public class ListTeamsProjection : IProjection
    {
        public ListTeamsProjection(IReadOnlyCollection<Team> teams)
        {
            Teams = teams;
        }

        public IReadOnlyCollection<Team> Teams { get; }
    }
}