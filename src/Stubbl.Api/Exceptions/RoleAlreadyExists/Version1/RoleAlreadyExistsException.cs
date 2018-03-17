using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.RoleAlreadyExists.Version1
{
    public class RoleAlreadyExistsException : Exception
    {
        public RoleAlreadyExistsException(string roleName, ObjectId teamId)
            : base($"Role already exists. RoleName='{roleName}' TeamID='{teamId}'")
        {
            RoleName = roleName;
            TeamId = teamId;
        }

        public string RoleName { get; }
        public ObjectId TeamId { get; }
    }
}