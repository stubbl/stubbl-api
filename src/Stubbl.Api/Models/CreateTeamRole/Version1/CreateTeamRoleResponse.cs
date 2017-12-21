namespace Stubbl.Api.Models.CreateTeamRole.Version1
{
   public class CreateTeamRoleResponse
   {
      public CreateTeamRoleResponse(string roleId)
      {
         RoleId = roleId;
      }

      public string RoleId { get; }
   }
}