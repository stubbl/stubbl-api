namespace Stubbl.Api.Controllers
{
   using System.Threading;
   using System.Threading.Tasks;
   using Common.Queries;
   using Core.Queries.ListTeamMembers.Version1;
   using Microsoft.AspNetCore.Mvc;
   using MongoDB.Bson;

   [ApiVersion("1")]
   [Route("teams/{teamId:ObjectId}/members/list", Name = "ListTeamMembers")]
   public class ListTeamMembersController : Controller
   {
      private readonly IQueryDispatcher _queryDispatcher;

      public ListTeamMembersController(IQueryDispatcher queryDispatcher)
      {
         _queryDispatcher = queryDispatcher;
      }

      [HttpGet]
      [ProducesResponseType(typeof(ListTeamMembersProjection), 200)]
      public async Task<IActionResult> ListTeamMembers([FromRoute] string teamId, [FromQuery] int? pageNumber,
         [FromQuery] int? pageSize, CancellationToken cancellationToken)
      {
         var query = new ListTeamMembersQuery
         (
            ObjectId.Parse(teamId),
            pageNumber.GetValueOrDefault(1),
            pageSize.GetValueOrDefault(10)
         );

         var projection = await _queryDispatcher.DispatchAsync(query, cancellationToken);

         return StatusCode(200, projection);
      }
   }
}