using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using Stubbl.Api.Data.Collections.Stubs;
using Stubbl.Api.Data.Collections.Teams;
using Stubbl.Api.Data.Collections.Users;
using Role = Stubbl.Api.Data.Collections.Users.Role;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.Data.Migrations
{
    public class _000000000000000000000002_CreateSeedData : IMongoMigration
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMongoCollection<Stub> _stubsCollection;
        private readonly IMongoCollection<Team> _teamsCollection;
        private readonly IMongoCollection<User> _usersCollection;

        public _000000000000000000000002_CreateSeedData(IHostingEnvironment hostingEnvironment,
            IMongoCollection<Stub> stubsCollection,
            IMongoCollection<Team> teamsCollection, IMongoCollection<User> usersCollection)
        {
            _hostingEnvironment = hostingEnvironment;
            _stubsCollection = stubsCollection;
            _teamsCollection = teamsCollection;
            _usersCollection = usersCollection;
        }

        public ObjectId Id => ObjectId.Parse("000000000000000000000002");
        public string Name => "CreateSeedData";

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (!_hostingEnvironment.IsDevelopment())
            {
                return;
            }

            var administratorRole = DefaultRoles.Administrator;
            var userRole = DefaultRoles.User;

            var administratorRoleId = ObjectId.GenerateNewId();
            var userRoleId = ObjectId.GenerateNewId();

            var user = new User
            {
                Id = new ObjectId("000000000000000000000001"),
                Name = "Member A",
                EmailAddress = "a@a.com",
                Sub = "000000000000-0000-0000-0000-00000001",
                Teams = new[]
                {
                    new Collections.Users.Team
                    {
                        Id = new ObjectId("000000000000000000000001"),
                        Name = "TeamA",
                        Role = new Role
                        {
                            Id = administratorRoleId,
                            Name = administratorRole.Name,
                            Permissions = administratorRole.Permissions
                        }
                    }
                }
            };

            await _usersCollection.DeleteOneAsync(m => m.Id == user.Id, cancellationToken);
            await _usersCollection.InsertOneAsync(user, cancellationToken: cancellationToken);

            var team = new Team
            {
                Id = user.Teams.First().Id,
                Name = user.Teams.First().Name,
                Members = new[]
                {
                    new Member
                    {
                        Id = user.Id,
                        Name = user.Name,
                        EmailAddress = user.EmailAddress,
                        Role = new Collections.Teams.Role
                        {
                            Id = administratorRoleId,
                            Name = administratorRole.Name,
                            Permissions = administratorRole.Permissions
                        }
                    }
                },
                Roles = new[]
                {
                    new Collections.Teams.Role
                    {
                        Id = administratorRoleId,
                        Name = administratorRole.Name,
                        Permissions = administratorRole.Permissions
                    },
                    new Collections.Teams.Role
                    {
                        Id = userRoleId,
                        Name = userRole.Name,
                        Permissions = userRole.Permissions
                    }
                }
            };

            await _teamsCollection.DeleteManyAsync(t => t.Id == team.Id, cancellationToken);
            await _teamsCollection.InsertOneAsync(team, cancellationToken: cancellationToken);

            var stub = new Stub
            {
                Id = new ObjectId("000000000000000000000001"),
                TeamId = team.Id,
                Name = "Stub A",
                Request = new Request
                {
                    HttpMethod = "GET",
                    Path = "/stuba"
                },
                Response = new Response
                {
                    HttpStatusCode = 200
                },
                Tags = new[]
                {
                    "Tag A"
                }
            };

            await _stubsCollection.DeleteManyAsync(s => s.Id == stub.Id, cancellationToken);
            await _stubsCollection.InsertOneAsync(stub, cancellationToken: cancellationToken);
        }
    }
}