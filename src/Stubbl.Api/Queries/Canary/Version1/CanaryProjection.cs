using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.Canary.Version1
{
    public class CanaryProjection : IProjection
    {
        public CanaryProjection(string database)
        {
            Database = database;
        }

        public string Database { get; }
    }
}