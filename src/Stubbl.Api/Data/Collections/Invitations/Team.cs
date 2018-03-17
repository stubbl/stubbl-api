using MongoDB.Bson;

namespace Stubbl.Api.Data.Collections.Invitations
{
    public class Team
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}