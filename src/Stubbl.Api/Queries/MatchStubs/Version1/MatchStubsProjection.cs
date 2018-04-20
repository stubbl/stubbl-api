using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.MatchStubs.Version1
{
    public class MatchStubsProjection : IProjection
    {
        public MatchStubsProjection(IReadOnlyCollection<Stub> stubs)
        {
            Stubs = stubs;
        }

        public IReadOnlyCollection<Stub> Stubs { get; }
    }
}