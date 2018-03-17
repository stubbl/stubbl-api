using MongoDB.Bson;

namespace Stubbl.Api.Data.Collections.Teams
{
    public class Member
    {
        public string EmailAddress { get; set; }
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public Role Role { get; set; }
    }
}