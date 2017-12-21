﻿namespace Stubbl.Api.Core.Queries.FindTeamMember.Version1
{
   using System.Collections.Generic;
   using Shared.Version1;

   public class Role
   {
      public Role(string id, string name, IReadOnlyCollection<Permission> permissions)
      {
         Id = id;
         Name = name;
         Permissions = permissions;
      }

      public string Id { get; }
      public string Name { get; }
      public IReadOnlyCollection<Permission> Permissions { get; }
   }
}