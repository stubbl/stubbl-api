namespace Stubbl.Api.Core.Data.Migrations
{
   using System;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Collections.DefaultRoles;
   using Collections.Shared;
   using MongoDB.Bson;
   using MongoDB.Driver;

   public class _000000000000000000000002_CreateDefaultRoles : IMongoDBMigration
   {
      private readonly IMongoCollection<DefaultRole> _defaultRolesCollection;

      public _000000000000000000000002_CreateDefaultRoles(IMongoCollection<DefaultRole> defaultRolesCollection)
      {
         _defaultRolesCollection = defaultRolesCollection;
      }

      public ObjectId Id => ObjectId.Parse("000000000000000000000002");
      public string Name => "CreateDefaultRoles";

      public async Task ExecuteAsync(CancellationToken cancellationToken)
      {
         var administratorRole = new DefaultRole
         {
            Name = DefaultRoleNames.Administrator,
            Permissions = Enum.GetValues(typeof(Permission))
               .Cast<Permission>()
               .Where(p => p > 0)
               .ToList()
         };

         var userRole = new DefaultRole
         {
            Name = DefaultRoleNames.User
         };

         var roles = new[]
         {
            administratorRole,
            userRole
         };

         await _defaultRolesCollection.InsertManyAsync(roles, cancellationToken: cancellationToken);
      }
   }
}