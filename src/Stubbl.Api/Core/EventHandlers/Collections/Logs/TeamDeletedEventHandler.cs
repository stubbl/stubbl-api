namespace Stubbl.Api.Core.EventHandlers.Collections.Logs
{
   using System.Threading;
   using System.Threading.Tasks;
   using Gunnsoft.Cqs.EventHandlers;
   using Data.Collections.Logs;
   using Events.TeamDeleted.Version1;
   using MongoDB.Driver;

   public class TeamDeletedEventHandler : IEventHandler<TeamDeletedEvent>
   {
      private readonly IMongoCollection<Log> _logsCollection;

      public TeamDeletedEventHandler(IMongoCollection<Log> logsCollection)
      {
         _logsCollection = logsCollection;
      }

      public async Task HandleAsync(TeamDeletedEvent @event, CancellationToken cancellationToken)
      {
         var filter = Builders<Log>.Filter.Where(m => m.TeamId == @event.TeamId);

         await _logsCollection.DeleteManyAsync(filter, cancellationToken);
      }
   }
}
