using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;

namespace Stubbl.Api.Queries.FindAuthenticatedUser.Version1
{
    public class FindAuthenticatedUserProjection : IProjection
    {
        public FindAuthenticatedUserProjection(string id, string name, string emailAddress,
            IReadOnlyCollection<Team> teams)
        {
            Id = id;
            Name = name;
            EmailAddress = emailAddress;
            Teams = teams;
        }

        public string Id { get; }
        public string Name { get; }
        public string EmailAddress { get; }
        public IReadOnlyCollection<Team> Teams { get; }
    }
}