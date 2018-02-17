namespace Stubbl.Api.Core.Versioning
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Versioning;

    public class AcceptHeaderApiVersionReader : IApiVersionReader
    {
        private static readonly Regex s_versionRegex = new Regex(@"^application\/vnd\.stubbl.v(\d+)\+json$");

        public void AddParameters(IApiVersionParameterDescriptionContext context)
        {
        }

        public string Read(HttpRequest request)
        {
            var acceptHeader = request.Headers.ContainsKey("Accept") ? request.Headers["Accept"].First() : "";
            var match = s_versionRegex.Match(acceptHeader);

            if (!match.Success)
            {
                return null;
            }

            return !int.TryParse(match.Groups[1].Value, out int version) ? null : version.ToString();
        }
    }
}