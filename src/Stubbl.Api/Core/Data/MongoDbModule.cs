namespace Stubbl.Api.Core.Data
{
    using Autofac;
    using Collections.DefaultRoles;
    using Collections.Invitations;
    using Collections.Logs;
    using Collections.Members;
    using Collections.Migrations;
    using Collections.Stubs;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using System;
    using System.Reflection;
    using Module = Autofac.Module;
    using Team = Collections.Teams.Team;

    public class MongoDBModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(cc =>
            {
                var mongoDbOptions = cc.Resolve<IOptions<MongoDBOptions>>();
                var connectionString = mongoDbOptions.Value.ConnectionString;
                var url = new MongoUrl(connectionString);

                if (url.DatabaseName == null)
                {
                    throw new ArgumentException("The connection string must contain a database name.", connectionString);
                }

                return url;
            })
            .AsSelf()
            .InstancePerDependency();

            builder.Register(cc =>
            {
                return new MongoClient(cc.Resolve<MongoUrl>());
            })
            .AsSelf()
            .InstancePerDependency();

            builder.RegisterType<MongoDBMigrationsRunner>()
               .AsSelf()
               .SingleInstance();

            builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Stubbl.Api")))
               .Where(t => typeof(IMongoDBMigration).IsAssignableFrom(t))
               .SingleInstance()
               .AsImplementedInterfaces();

            builder.Register(cc => cc.Resolve<MongoClient>()
                  .GetDatabase(cc.Resolve<MongoUrl>().DatabaseName)
                  .GetCollection<DefaultRole>(CollectionNames.DefaultRoles))
               .As<IMongoCollection<DefaultRole>>()
               .InstancePerDependency();

            builder.Register(cc => cc.Resolve<MongoClient>()
                  .GetDatabase(cc.Resolve<MongoUrl>().DatabaseName)
                  .GetCollection<Invitation>(CollectionNames.Invitations))
               .As<IMongoCollection<Invitation>>()
               .InstancePerDependency();

            builder.Register(cc => cc.Resolve<MongoClient>()
                  .GetDatabase(cc.Resolve<MongoUrl>().DatabaseName)
                  .GetCollection<Log>(CollectionNames.Logs))
               .As<IMongoCollection<Log>>()
               .InstancePerDependency();

            builder.Register(cc => cc.Resolve<MongoClient>()
                  .GetDatabase(cc.Resolve<MongoUrl>().DatabaseName)
                  .GetCollection<Member>(CollectionNames.Members))
               .As<IMongoCollection<Member>>()
               .InstancePerDependency();

            builder.Register(cc => cc.Resolve<MongoClient>()
                  .GetDatabase(cc.Resolve<MongoUrl>().DatabaseName)
                  .GetCollection<Migration>(CollectionNames.Migrations))
               .As<IMongoCollection<Migration>>()
               .InstancePerDependency();

            builder.Register(cc => cc.Resolve<MongoClient>()
                  .GetDatabase(cc.Resolve<MongoUrl>().DatabaseName)
                  .GetCollection<Stub>(CollectionNames.Stubs))
               .As<IMongoCollection<Stub>>()
               .InstancePerDependency();

            builder.Register(cc => cc.Resolve<MongoClient>()
                  .GetDatabase(cc.Resolve<MongoUrl>().DatabaseName)
                  .GetCollection<Team>(CollectionNames.Teams))
               .As<IMongoCollection<Team>>()
               .InstancePerDependency();
        }
    }
}