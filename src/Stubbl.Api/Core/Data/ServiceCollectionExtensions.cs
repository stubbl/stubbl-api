namespace Stubbl.Api.Core.Data
{
    using System;
    using System.Linq;
    using Collections.DefaultRoles;
    using Collections.Invitations;
    using Collections.Logs;
    using Collections.Members;
    using Collections.Migrations;
    using Collections.Stubs;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;
    using Team = Collections.Teams.Team;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection extended, MongoSettings mongoSettings)
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

            foreach (var type in assemblyTypes.Where(t => !t.IsAbstract && t.IsClass && typeof(IMongoMigration).IsAssignableFrom(t)))
            {
                extended.AddSingleton(typeof(IMongoMigration), type);
            }

            extended.AddMongoDBCollection<DefaultRole>(CollectionNames.DefaultRoles);
            extended.AddMongoDBCollection<Invitation>(CollectionNames.Invitations);
            extended.AddMongoDBCollection<Log>(CollectionNames.Logs);
            extended.AddMongoDBCollection<Member>(CollectionNames.Members);
            extended.AddMongoDBCollection<Migration>(CollectionNames.Migrations);
            extended.AddMongoDBCollection<Stub>(CollectionNames.Stubs);
            extended.AddMongoDBCollection<Team>(CollectionNames.Teams);

            return extended;
        }

        private static void AddMongoDBCollection<TCollection>(this IServiceCollection extended, string collectionName)
        {
            var mongoCollectionType = typeof(IMongoCollection<>).MakeGenericType(typeof(TCollection));

            extended.AddSingleton(mongoCollectionType, sp => sp.GetRequiredService<MongoClient>()
                .GetDatabase(sp.GetRequiredService<MongoUrl>().DatabaseName)
                .GetCollection<TCollection>(collectionName));
        }
    }
}