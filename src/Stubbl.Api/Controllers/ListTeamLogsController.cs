namespace Stubbl.Api.Controllers
{
   using System;
   using System.Collections.Generic;
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Queries;
   using Core.Queries.ListTeamLogs.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/logs/list", Name = "ListTeamLogs")]
   public class ListTeamLogsController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public ListTeamLogsController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(ListTeamLogsProjection), 200)]
      public async Task<IActionResult> ListTeamLogs([FromRoute] string teamId, [FromQuery] IReadOnlyCollection<string> stubId,
         [FromQuery] int? pageNumber, [FromQuery] int? pageSize, CancellationToken cancellationToken)
      {
         var stubIds = ParseStubId(stubId);

         var query = new ListTeamLogsQuery
         (
            ObjectId.Parse(teamId),
            stubIds,
            pageNumber.GetValueOrDefault(1),
            pageSize.GetValueOrDefault(10)
         );

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }

      private IReadOnlyCollection<ObjectId> ParseStubId(IReadOnlyCollection<string> rawStubIds)
      {
         var stubIds = new List<ObjectId>();

         if (rawStubIds != null)
         {
            foreach (var rawStubId in rawStubIds)
            {
               if (ObjectId.TryParse(rawStubId, out var stubId))
               {
                  stubIds.Add(stubId);
               }
            }
         }

         return stubIds;
      }
   }
}