using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.FindTeam.Version1
{
    public class FindTeamProjection : IProjection
    {
        public FindTeamProjection(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}