namespace Stubbl.Api.Core.Exceptions.AdministratorRoleNotFound.Version1
{
   using System;

   public class AdministratorRoleNotFoundException : Exception
   {
      public AdministratorRoleNotFoundException()
         : base("Administrator role not found.")
      {
      }
   }
}