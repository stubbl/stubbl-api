namespace Stubbl.Api.Core.Data
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using Stubbl.Api.Core.Data.Collections.DefaultRoles;
    using Stubbl.Api.Core.Data.Collections.Invitations;
    using Stubbl.Api.Core.Data.Collections.Logs;
    using Stubbl.Api.Core.Data.Collections.Members;
    using Stubbl.Api.Core.Data.Collections.Migrations;
    using Stubbl.Api.Core.Data.Collections.Stubs;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Team = Collections.Teams.Team;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDB(this IServiceCollection extended, MongoDBSettings mongoDBSettings)
        {
            extended.AddTransient(sp =>
            {
                var url = new MongoUrl(mongoDBSettings.ConnectionString);

                if (url.DatabaseName == null)
                {
                    throw new ArgumentException("The connection string must contain a database name.", mongoDBSettings.ConnectionString);
                }

                return url;
            });

            extended.AddTransient(sp =>
            {
                return new MongoClient(sp.GetRequiredService<MongoUrl>());
            });

            extended.AddSingleton<MongoDBMigrationsRunner>();

            var assemblyTypes = typeof(ServiceCollection).Assembly.GetTypes();

            foreach (var type in assemblyTypes.Where(t => typeof(IMongoDBMigration).IsAssignableFrom(t)))
            {
                extended.AddSingleton(typeof(IMongoDBMigration), type);
            }

            extended.AddMongoDBCollection(assemblyTypes, typeof(IMongoCollection<DefaultRole>), CollectionNames.DefaultRoles);
            extended.AddMongoDBCollection(assemblyTypes, typeof(IMongoCollection<Invitation>), CollectionNames.Invitations);
            extended.AddMongoDBCollection(assemblyTypes, typeof(IMongoCollection<Log>), CollectionNames.Logs);
            extended.AddMongoDBCollection(assemblyTypes, typeof(IMongoCollection<Member>), CollectionNames.Members);
            extended.AddMongoDBCollection(assemblyTypes, typeof(IMongoCollection<Migration>), CollectionNames.Migrations);
            extended.AddMongoDBCollection(assemblyTypes, typeof(IMongoCollection<Stub>), CollectionNames.Stubs);
            extended.AddMongoDBCollection(assemblyTypes, typeof(IMongoCollection<Team>), CollectionNames.Teams);

            return extended;
        }

        private static void AddMongoDBCollection(this IServiceCollection extended, IReadOnlyCollection<Type> assemblyTypes, Type collectionType, string collectionName)
        {
            foreach (var type in assemblyTypes.Where(t => collectionType.IsAssignableFrom(t)))
            {
                extended.AddSingleton(typeof(IMongoCollection<DefaultRole>), sp => sp.GetRequiredService<MongoClient>()
                    .GetDatabase(sp.GetRequiredService<MongoUrl>().DatabaseName)
                    .GetCollection<DefaultRole>(CollectionNames.DefaultRoles));
            }
        }
    }
}
