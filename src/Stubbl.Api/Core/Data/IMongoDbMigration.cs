namespace Stubbl.Api.Core.Data
{
   using System.Threading;
   using System.Threading.Tasks;
   using MongoDB.Bson;

   public interface IMongoDbMigration
   {
      ObjectId Id { get; }
      string Name { get; }

      Task ExecuteAsync(CancellationToken cancellationToken);
   }
}
