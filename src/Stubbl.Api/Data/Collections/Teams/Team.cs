using System.Collections.Generic;
using MongoDB.Bson;

namespace Stubbl.Api.Data.Collections.Teams
{
    public class Team
    {
        public Team()
        {
            Members = new Member[0];
            Roles = new Role[0];
        }

        public ObjectId Id { get; set; }
        public IReadOnlyCollection<Member> Members { get; set; }
        public string Name { get; set; }
        public IReadOnlyCollection<Role> Roles { get; set; }
    }
}