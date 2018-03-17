using System;
using MongoDB.Bson;

namespace Stubbl.Api.Data.Collections.Migrations
{
    public class Migration
    {
        public DateTime CreatedAt { get; set; }
        public ObjectId Id { get; set; }
        public string Name { get; set; }
    }
}