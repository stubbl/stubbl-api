namespace Stubbl.Api.Core.Data
{
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Collections.Migrations;
   using MongoDB.Bson;
   using MongoDB.Driver;

   public class MongoDBMigrationsRunner
   {
      private readonly IReadOnlyCollection<IMongoDBMigration> _migrations;
      private readonly IMongoCollection<Migration> _migrationsCollection;

      public MongoDBMigrationsRunner(IReadOnlyCollection<IMongoDBMigration> migrations, IMongoCollection<Migration> migrationsCollection)
      {
         _migrations = migrations;
         _migrationsCollection = migrationsCollection;
      }

      public async Task Run(CancellationToken cancellationToken = default(CancellationToken))
      {
         if (!_migrations.Any())
         {
            return;
         }

         var filter = Builders<Migration>.Filter.Empty;
         var executedMigrationIds = await _migrationsCollection.Find(filter)
            .Project(m => m.Id)
            .ToListAsync(cancellationToken);

         foreach (var migration in _migrations.Where(m => !executedMigrationIds.Contains(m.Id)))
         {
            await UpdateMigrations(migration.Id, migration.Name, cancellationToken);

            await migration.ExecuteAsync(cancellationToken);
         }
      }

      private async Task UpdateMigrations(ObjectId migrationId, string migrationName, CancellationToken cancellationToken)
      {
         var migration = new Migration
         {
            Id = migrationId,
            Name = migrationName
         };

         await _migrationsCollection.InsertOneAsync(migration, cancellationToken: cancellationToken);
      }
   }
}
