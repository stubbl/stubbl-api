namespace Stubbl.Api.Core.Data
{
    using Collections.DefaultRoles;
    using Collections.Invitations;
    using Collections.Logs;
    using Collections.Members;
    using Collections.Migrations;
    using Collections.Stubs;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization;
    using MongoDB.Bson.Serialization.Conventions;
    using Team = Collections.Teams.Team;

    public static class MongoConfigurator
    {
        public static void Configure()
        {
            var conventionPack = new ConventionPack
            {
               new CamelCaseElementNameConvention(),
               new EnumRepresentationConvention(BsonType.String)
            };

            ConventionRegistry.Register("stubbl-api", conventionPack, t => true);

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
