namespace Stubbl.Api.Core.Data.Migrations
{
   using System.Threading;
   using System.Threading.Tasks;
   using Collections.Stubs;
   using MongoDB.Bson;
   using MongoDB.Driver;

   public class _000000000000000000000001_CreateStubIndex : IMongoDbMigration
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
