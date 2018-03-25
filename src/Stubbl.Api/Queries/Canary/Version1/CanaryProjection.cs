using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.Canary.Version1
{
    public class CanaryProjection : IProjection
    {
        public CanaryProjection(ComponentStatus mongo, ComponentStatus storageAccount)
        {
            Mongo = mongo;
            StorageAccount = storageAccount;
        }

        public ComponentStatus Mongo { get; }
        public ComponentStatus StorageAccount { get; }
    }
}