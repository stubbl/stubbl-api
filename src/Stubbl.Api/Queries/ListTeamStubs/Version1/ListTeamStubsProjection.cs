using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;
using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.ListTeamStubs.Version1
{
    public class ListTeamStubsProjection : IProjection
    {
        public ListTeamStubsProjection(IReadOnlyCollection<Stub> stubs, Paging paging)
        {
            Stubs = stubs;
            Paging = paging;
        }

        public Paging Paging { get; }
        public IReadOnlyCollection<Stub> Stubs { get; }
    }
}