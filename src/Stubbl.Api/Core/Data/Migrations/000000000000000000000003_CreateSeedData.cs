namespace Stubbl.Api.Core.Data.Migrations
{
   using System;
   using System.Linq;
   using System.Threading;
   using System.Threading.Tasks;
   using Collections.Members;
   using Collections.Stubs;
   using Collections.DefaultRoles;
   using Exceptions.AdministratorRoleNotFound.Version1;
   using Exceptions.UserRoleNotFound.Version1;
   using Microsoft.AspNetCore.Hosting;
   using MongoDB.Bson;
   using MongoDB.Driver;
   using Team = Collections.Teams.Team;

   public class _000000000000000000000003_CreateSeedData : IMongoDbMigration
   {
      private readonly IMongoCollection<DefaultRole> _defaultRolesCollection;
      private readonly IHostingEnvironment _hostingEnvironment;
      private readonly IMongoCollection<Member> _membersCollection;
      private readonly IMongoCollection<Stub> _stubsCollection;
      private readonly IMongoCollection<Team> _teamsCollection;

      public _000000000000000000000003_CreateSeedData(IMongoCollection<DefaultRole> defaultRolesCollection,
         IHostingEnvironment hostingEnvironment, IMongoCollection<Member> membersCollection,
         IMongoCollection<Stub> stubsCollection, IMongoCollection<Team> teamsCollection)
      {
         _defaultRolesCollection = defaultRolesCollection;
         _hostingEnvironment = hostingEnvironment;
         _membersCollection = membersCollection;
         _stubsCollection = stubsCollection;
         _teamsCollection = teamsCollection;
      }

      public ObjectId Id => ObjectId.Parse("000000000000000000000003");
      public string Name => "CreateSeedData";

      public async Task ExecuteAsync(CancellationToken cancellationToken)
      {
         if (!_hostingEnvironment.IsDevelopment())
         {
            return;
         }

         var defaultRoles = await _defaultRolesCollection.Find(Builders<DefaultRole>.Filter.Empty)
            .ToListAsync(cancellationToken);
         var administratorRole = defaultRoles.SingleOrDefault(r => string.Equals(r.Name, DefaultRoleNames.Administrator, StringComparison.OrdinalIgnoreCase));

         if (administratorRole == null)
         {
            throw new AdministratorRoleNotFoundException();
         }

         var userRole = defaultRoles.SingleOrDefault(r => string.Equals(r.Name, DefaultRoleNames.User, StringComparison.OrdinalIgnoreCase));

         if (userRole == null)
         {
            throw new UserRoleNotFoundException();
         }

         var administratorRoleId = ObjectId.GenerateNewId();
         var userRoleId = ObjectId.GenerateNewId();

         var member = new Member
         {
            Id = new ObjectId("000000000000000000000001"),
            Name = "Member A",
            EmailAddress = "a@a.com",
            IdentityId = "000000000000-0000-0000-0000-00000001",
            Teams = new[]
            {
               new Collections.Members.Team
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

         await _membersCollection.DeleteOneAsync(m => m.Id == member.Id, cancellationToken);
         await _membersCollection.InsertOneAsync(member, cancellationToken: cancellationToken);

         var team = new Team
         {
            Id = member.Teams.First().Id,
            Name = member.Teams.First().Name,
            Members = new[]
            {
               new Collections.Teams.Member
               {
                  Id = member.Id,
                  Name = member.Name,
                  EmailAddress = member.EmailAddress,
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
               Path = "stuba"
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