namespace Stubbl.Api.Models.UpdateTeamRole.Version1
{
   using System.Collections.Generic;

   public class UpdateTeamRoleRequest
   {
      public string Name { get; set; }
      public IReadOnlyCollection<string> Permissions { get; set; }
   }
}