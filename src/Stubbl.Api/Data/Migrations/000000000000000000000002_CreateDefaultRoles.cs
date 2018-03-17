using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.DefaultRoles;
using Stubbl.Api.Data.Collections.Shared;

namespace Stubbl.Api.Data.Migrations
{
    public class _000000000000000000000002_CreateDefaultRoles : IMongoMigration
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