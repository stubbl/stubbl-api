﻿using System;
using System.Collections.Generic;
using Gunnsoft.Cqs.Queries;
using Stubbl.Api.Queries.Shared.Version1;

namespace Stubbl.Api.Queries.FindTeamLog.Version1
{
    public class FindTeamLogProjection : IProjection
    {
        public FindTeamLogProjection(string id, string teamId, IReadOnlyCollection<string> stubIds, RequestLog request,
            ResponseLog response, DateTime loggedAt)
        {
            Id = id;
            TeamId = teamId;
            StubIds = stubIds;
            Request = request;
            Response = response;
            LoggedAt = loggedAt;
        }

        public string Id { get; }
        public DateTime LoggedAt { get; }
        public RequestLog Request { get; }
        public ResponseLog Response { get; }
        public IReadOnlyCollection<string> StubIds { get; }
        public string TeamId { get; }
    }
}