using MongoDB.Bson;

namespace Stubbl.Api.Data.Collections.Users
{
    public class Team
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
    }
}