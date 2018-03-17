namespace Stubbl.Api.Core.Commands.RemoveTeamMember.Version1
{
   using Gunnsoft.Cqs.Commands;
   using Events.TeamMemberRemoved.Version1;
   using MongoDB.Bson;

   public class RemoveTeamMemberCommand : ICommand<TeamMemberRemovedEvent>
   {
      public RemoveTeamMemberCommand(ObjectId teamId, ObjectId memberId)
      {
         TeamId = teamId;
         MemberId = memberId;
      }

      public ObjectId MemberId { get; }
      public ObjectId TeamId { get; }
   }
}