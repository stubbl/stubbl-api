namespace Stubbl.Api.Core.Versioning
{
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Reflection;
   using System.Text.RegularExpressions;

   public static class Versions
   {
      private static readonly IReadOnlyCollection<int> s_detectedVersions;
      private static readonly Regex s_versionRegex;

      static Versions()
      {
         s_versionRegex = new Regex(@"\.Version([0-9])(?:\.|$)", RegexOptions.Compiled);
         s_detectedVersions = Assembly.Load(new AssemblyName("Stubbl.Api"))
            .GetTypes()
            .Where(IsInVersionedNamespace)
            .Select(t => GetVersionFromNamespace(t).Value)
            .Distinct()
            .OrderBy(v => v)
            .ToList();
      }

      public static int Latest => s_detectedVersions.LastOrDefault();

      public static bool IsInVersionedNamespace(Type type)
      {
         return !string.IsNullOrWhiteSpace(type?.Namespace) && s_versionRegex.IsMatch(type.Namespace);
      }

      public static int? GetVersionFromNamespace(Type type)
      {
         if (!IsInVersionedNamespace(type))
         {
            return null;
         }

         return int.Parse(s_versionRegex.Match(type.Namespace).Groups[1].Value);
      }
   }
}