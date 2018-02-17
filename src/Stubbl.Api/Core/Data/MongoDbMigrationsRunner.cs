namespace Stubbl.Api.Core.Data
{
    using Collections.Migrations;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class MongoDBMigrationsRunner
    {
        private readonly IReadOnlyCollection<IMongoDBMigration> _migrations;
        private readonly IMongoCollection<Migration> _migrationsCollection;

        public MongoDBMigrationsRunner(IReadOnlyCollection<IMongoDBMigration> migrations, IMongoCollection<Migration> migrationsCollection)
        {
            _migrations = migrations;
            _migrationsCollection = migrationsCollection;
        }

        public async Task RunAsync(CancellationToken cancellationToken = default(CancellationToken))
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
                await migration.ExecuteAsync(cancellationToken);

                var document = new Migration
                {
                    Id = migration.Id,
                    Name = migration.Name,
                    CreatedAt = DateTime.UtcNow
                };

                await _migrationsCollection.InsertOneAsync(document, cancellationToken: cancellationToken);
            }
        }
    }
}
