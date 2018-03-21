using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Gunnsoft.Api.Versioning
{
    public static class Versions
    {
        private static readonly IReadOnlyCollection<int> s_detectedVersions;
        private static readonly Regex s_versionRegex;

        static Versions()
        {
            s_versionRegex = new Regex(@"\.Version([0-9])(?:\.|$)", RegexOptions.Compiled);

            Latest = 1;
        }

        public static uint Latest { get; set; }

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