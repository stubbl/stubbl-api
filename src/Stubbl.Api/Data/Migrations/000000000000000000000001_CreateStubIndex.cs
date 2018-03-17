using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.Stubs;

namespace Stubbl.Api.Data.Migrations
{
    public class _000000000000000000000001_CreateStubIndex : IMongoMigration
    {
        private readonly IMongoCollection<Stub> _stubsCollection;

        public _000000000000000000000001_CreateStubIndex(IMongoCollection<Stub> stubsCollection)
        {
            _stubsCollection = stubsCollection;
        }

        public ObjectId Id => ObjectId.Parse("000000000000000000000001");
        public string Name => "CreateStubIndex";

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var index = Builders<Stub>.IndexKeys
                .Ascending(s => s.Name)
                .Ascending(s => s.Request.Path)
                .Ascending(s => s.Tags);

            await _stubsCollection.Indexes.CreateOneAsync(index, cancellationToken: cancellationToken);
        }
    }
}