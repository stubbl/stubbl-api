namespace Stubbl.Api.Core.QueryHandlers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.QueryHandlers;
   using Data;
   using MongoDB.Bson;
   using MongoDB.Driver;
   using Queries.Canary.Version1;

   public class CanaryQueryHandler : IQueryHandler<CanaryQuery, CanaryProjection>
   {
      private readonly MongoClient _mongoClient;

      public CanaryQueryHandler(MongoClient mongoClient)
      {
         _mongoClient = mongoClient;
      }

      public async Task<CanaryProjection> HandleAsync(CanaryQuery query, CancellationToken cancellationToken)
      {
         var database = _mongoClient.GetDatabase(DatabaseNames.Stubbl);
         var databaseStatus = "ok";

         try
         {
            await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}", cancellationToken : cancellationToken);
         }
         catch
         {
            databaseStatus = "fault";
         }

         return new CanaryProjection
         (
             databaseStatus
         );
      }
   }
}