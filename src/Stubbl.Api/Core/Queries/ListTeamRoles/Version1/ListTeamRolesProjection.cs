namespace Stubbl.Api.Core.Queries.ListTeamRoles.Version1
{
   using System.Collections.Generic;
   using Common.Queries;

   public class ListTeamRolesProjection : IProjection
   {
      public ListTeamRolesProjection(IReadOnlyCollection<Role> roles)
      {
         Roles = roles;
      }

      public IReadOnlyCollection<Role> Roles { get; }
   }
}