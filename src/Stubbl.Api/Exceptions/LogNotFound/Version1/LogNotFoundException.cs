using System;
using MongoDB.Bson;

namespace Stubbl.Api.Exceptions.LogNotFound.Version1
{
    public class LogNotFoundException : Exception
    {
        public LogNotFoundException(ObjectId logId, ObjectId teamId)
            : base($"Log not found. LogID='{logId}' TeamID='{teamId}'")
        {
            LogId = logId;
            TeamId = teamId;
        }

        public ObjectId LogId { get; }
        public ObjectId TeamId { get; }
    }
}