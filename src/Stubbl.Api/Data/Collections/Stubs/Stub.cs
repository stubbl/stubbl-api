using System.Collections.Generic;
using MongoDB.Bson;

namespace Stubbl.Api.Data.Collections.Stubs
{
    public class Stub
    {
        public Stub()
        {
            Tags = new string[0];
        }

        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public Request Request { get; set; }
        public Response Response { get; set; }
        public IReadOnlyCollection<string> Tags { get; set; }
        public ObjectId TeamId { get; set; }
    }
}