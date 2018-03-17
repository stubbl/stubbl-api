using MongoDB.Bson;

namespace Stubbl.Api.Data.Collections.Invitations
{
    public class Role
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}