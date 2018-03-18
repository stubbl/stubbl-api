using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using Stubbl.Api.Data.Collections.DefaultRoles;
using Stubbl.Api.Data.Collections.Invitations;
using Stubbl.Api.Data.Collections.Logs;
using Stubbl.Api.Data.Collections.Members;
using Stubbl.Api.Data.Collections.Migrations;
using Stubbl.Api.Data.Collections.Stubs;
using Team = Stubbl.Api.Data.Collections.Teams.Team;

namespace Stubbl.Api.Data
{
    public static class MongoConfigurator
    {
        public static void Configure()
        {
            const string conventionName = "stubbl-api";
            var conventionPack = new ConventionPack
            {
                new CamelCaseElementNameConvention(),
                new EnumRepresentationConvention(BsonType.String)
            };

            ConventionRegistry.Remove(conventionName);
            ConventionRegistry.Register(conventionName, conventionPack, t => true);

            if (!BsonClassMap.IsClassMapRegistered(typeof(DefaultRole)))
            {
                BsonClassMap.RegisterClassMap<DefaultRole>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Invitation)))
            {
                BsonClassMap.RegisterClassMap<Invitation>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Log)))
            {
                BsonClassMap.RegisterClassMap<Log>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Member)))
            {
                BsonClassMap.RegisterClassMap<Member>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Migration)))
            {
                BsonClassMap.RegisterClassMap<Migration>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Stub)))
            {
                BsonClassMap.RegisterClassMap<Stub>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                    cm.SetIgnoreExtraElements(true);
                });
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Team)))
            {
                BsonClassMap.RegisterClassMap<Team>(cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                    cm.SetIgnoreExtraElements(true);
                });
            }
        }
    }
}