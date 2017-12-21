namespace Stubbl.Api.Core.Queries.Shared.Version1
{
   public class Permission
   {
      public Permission(string name, string description)
      {
         Name = name;
         Description = description;
      }

      public string Description { get; }
      public string Name { get; }
   }
}