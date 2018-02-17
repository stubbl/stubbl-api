namespace Stubbl.Api.Core.Data.Collections.Migrations
{
    using MongoDB.Bson;
    using System;

    public class Migration
    {
        public DateTime CreatedAt { get; set; }
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}
