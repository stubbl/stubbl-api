namespace Stubbl.Api.Core.Data
{
   using System.Reflection;
   using Collections.DefaultRoles;
   using Collections.Invitations;
   using Collections.Logs;
   using Collections.Members;
   using Collections.Migrations;
   using Collections.Stubs;
   using Autofac;
   using Microsoft.Extensions.Options;
   using MongoDB.Driver;
   using Module = Autofac.Module;
   using Team = Collections.Teams.Team;

   public class MongoDBModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.Register(cc =>
         {
            var mongoDbOptions = cc.Resolve<IOptions<MongoDBOptions>>();

            return new MongoClient(mongoDbOptions.Value.ConnectionString);
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
               .GetDatabase(MongoDBConfig.DatabaseName)
               .GetCollection<DefaultRole>(MongoDBConfig.CollectionNames.DefaultRoles))
            .As<IMongoCollection<DefaultRole>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(MongoDBConfig.DatabaseName)
               .GetCollection<Invitation>(MongoDBConfig.CollectionNames.Invitations))
            .As<IMongoCollection<Invitation>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(MongoDBConfig.DatabaseName)
               .GetCollection<Log>(MongoDBConfig.CollectionNames.Logs))
            .As<IMongoCollection<Log>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(MongoDBConfig.DatabaseName)
               .GetCollection<Member>(MongoDBConfig.CollectionNames.Members))
            .As<IMongoCollection<Member>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(MongoDBConfig.DatabaseName)
               .GetCollection<Migration>(MongoDBConfig.CollectionNames.Migrations))
            .As<IMongoCollection<Migration>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(MongoDBConfig.DatabaseName)
               .GetCollection<Stub>(MongoDBConfig.CollectionNames.Stubs))
            .As<IMongoCollection<Stub>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(MongoDBConfig.DatabaseName)
               .GetCollection<Team>(MongoDBConfig.CollectionNames.Teams))
            .As<IMongoCollection<Team>>()
            .InstancePerDependency();
      }
   }
}