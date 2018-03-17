using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.DefaultRoles;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Data.Collections.Logs;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Data.Collections.Migrations;
using Stubbl.Api.Data.Collections.Stubs;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection extended, MongoSettings mongoSettings)
        {
            extended.AddTransient(sp =>
            {
                var url = new MongoUrl(mongoSettings.ConnectionString);

                if (url.DatabaseName == null)
                {
                    throw new ArgumentException("The connection string must contain a database name.",
                        mongoSettings.ConnectionString);
                }

                return url;
            });

            extended.AddTransient(sp => new MongoClient(sp.GetRequiredService<MongoUrl>()));

            extended.AddSingleton<MongoMigrationsRunner>();

            var assemblyTypes = typeof(ServiceCollectionExtensions).Assembly.GetTypes();

            foreach (var type in assemblyTypes.Where(t =>
                !t.IsAbstract && t.IsClass && typeof(IMongoMigration).IsAssignableFrom(t)))
            {
                extended.AddSingleton(typeof(IMongoMigration), type);
            }

            extended.AddMongoDbCollection<DefaultRole>(CollectionNames.DefaultRoles);
            extended.AddMongoDbCollection<Invitation>(CollectionNames.Invitations);
            extended.AddMongoDbCollection<Log>(CollectionNames.Logs);
            extended.AddMongoDbCollection<Member>(CollectionNames.Members);
            extended.AddMongoDbCollection<Migration>(CollectionNames.Migrations);
            extended.AddMongoDbCollection<Stub>(CollectionNames.Stubs);
            extended.AddMongoDbCollection<Team>(CollectionNames.Teams);

            return extended;
        }

        private static void AddMongoDbCollection<TCollection>(this IServiceCollection extended, string collectionName)
        {
            var mongoCollectionType = typeof(IMongoCollection<>).MakeGenericType(typeof(TCollection));

            extended.AddSingleton(mongoCollectionType, sp => sp.GetRequiredService<MongoClient>()
                .GetDatabase(sp.GetRequiredService<MongoUrl>().DatabaseName)
                .GetCollection<TCollection>(collectionName));
        }
    }
}