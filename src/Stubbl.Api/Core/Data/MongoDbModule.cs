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

   public class MongoDbModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.Register(cc =>
         {
            var mongoDbOptions = cc.Resolve<IOptions<MongoDbOptions>>();

            return new MongoClient(mongoDbOptions.Value.ConnectionString);
         })
         .AsSelf()
         .InstancePerDependency();

         builder.RegisterType<MongoDbMigrationsRunner>()
            .AsSelf()
            .SingleInstance();

         builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Stubbl.Api")))
            .Where(t => typeof(IMongoDbMigration).IsAssignableFrom(t))
            .SingleInstance()
            .AsImplementedInterfaces();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(DatabaseNames.Stubbl)
               .GetCollection<DefaultRole>(CollectionNames.DefaultRoles))
            .As<IMongoCollection<DefaultRole>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(DatabaseNames.Stubbl)
               .GetCollection<Invitation>(CollectionNames.Invitations))
            .As<IMongoCollection<Invitation>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(DatabaseNames.Stubbl)
               .GetCollection<Log>(CollectionNames.Logs))
            .As<IMongoCollection<Log>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(DatabaseNames.Stubbl)
               .GetCollection<Member>(CollectionNames.Members))
            .As<IMongoCollection<Member>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(DatabaseNames.Stubbl)
               .GetCollection<Migration>(CollectionNames.Migrations))
            .As<IMongoCollection<Migration>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(DatabaseNames.Stubbl)
               .GetCollection<Stub>(CollectionNames.Stubs))
            .As<IMongoCollection<Stub>>()
            .InstancePerDependency();

         builder.Register(cc => cc.Resolve<MongoClient>()
               .GetDatabase(DatabaseNames.Stubbl)
               .GetCollection<Team>(CollectionNames.Teams))
            .As<IMongoCollection<Team>>()
            .InstancePerDependency();
      }
   }
}