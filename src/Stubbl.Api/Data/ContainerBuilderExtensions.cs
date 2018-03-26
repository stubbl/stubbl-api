using System;
using System.Reflection;
using Autofac;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Data.Collections.Logs;
using Stubbl.Api.Data.Collections.Migrations;
using Stubbl.Api.Data.Collections.Stubs;
using Stubbl.Api.Data.Collections.Users;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.Data
{
    public static class ContainerextendedExtensions
    {
        public static ContainerBuilder AddMongo(this ContainerBuilder extended, MongoSettings mongoSettings)
        {
            extended.Register(cc =>
                {
                    var url = new MongoUrl(mongoSettings.ConnectionString);

                    if (url.DatabaseName == null)
                    {
                        throw new ArgumentException("The connection string must contain a database name.", mongoSettings.ConnectionString);
                    }

                    return url;
                })
                .AsSelf()
                .InstancePerDependency();

            extended.Register(cc => new MongoClient(cc.Resolve<MongoUrl>()))
                .AsSelf()
                .InstancePerDependency();

            extended.RegisterType<MongoMigrationsRunner>()
                .AsSelf()
                .SingleInstance();

            extended.RegisterAssemblyTypes(Assembly.GetEntryAssembly())
                .Where(t => typeof(IMongoMigration).IsAssignableFrom(t))
                .SingleInstance()
                .AsImplementedInterfaces();

            extended.AddMongoCollection<Invitation>(CollectionNames.Invitations);
            extended.AddMongoCollection<Log>(CollectionNames.Logs);
            extended.AddMongoCollection<User>(CollectionNames.Users);
            extended.AddMongoCollection<Migration>(CollectionNames.Migrations);
            extended.AddMongoCollection<Stub>(CollectionNames.Stubs);
            extended.AddMongoCollection<Team>(CollectionNames.Teams);

            return extended;
        }

        private static void AddMongoCollection<TCollection>(this ContainerBuilder extended, string collectionName)
        {
            extended.Register(cc => cc.Resolve<MongoClient>()
                    .GetDatabase(cc.Resolve<MongoUrl>().DatabaseName)
                    .GetCollection<TCollection>(collectionName))
                .As<IMongoCollection<TCollection>>()
                .InstancePerDependency();
        }
    }
}