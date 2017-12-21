namespace Stubbl.Api.Models.CreateTeamRole.Version1
{
   using System.Collections.Generic;

   public class CreateTeamRoleRequest
   {
      public string Name { get; set; }
      public IReadOnlyCollection<string> Permissions { get; set; }
   }
}