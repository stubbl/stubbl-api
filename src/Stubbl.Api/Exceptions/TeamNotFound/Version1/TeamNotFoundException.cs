using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.TeamNotFound.Version1
{
    public class TeamNotFoundException : Exception
    {
        public TeamNotFoundException(ObjectId teamId)
            : base($"Team cannot be found. TeamID='{teamId}'")
        {
            TeamId = teamId;
        }

        public ObjectId TeamId { get; }
    }
}