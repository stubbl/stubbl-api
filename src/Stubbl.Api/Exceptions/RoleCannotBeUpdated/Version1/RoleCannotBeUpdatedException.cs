using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.RoleCannotBeUpdated.Version1
{
    public class RoleCannotBeUpdatedException : Exception
    {
        public RoleCannotBeUpdatedException(ObjectId roleId, ObjectId teamId)
            : base($"Role cannot be updated. RoleID='{roleId}' TeamID='{teamId}'")
        {
            RoleId = roleId;
            TeamId = teamId;
        }

        public ObjectId RoleId { get; }
        public ObjectId TeamId { get; }
    }
}