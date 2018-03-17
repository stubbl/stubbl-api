using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.StubTester.Version1
{
    public class StubTesterProjection : IProjection
    {
        public StubTesterProjection(IReadOnlyCollection<Stub> stubs)
        {
            Stubs = stubs;
        }

        public IReadOnlyCollection<Stub> Stubs { get; }
    }
}