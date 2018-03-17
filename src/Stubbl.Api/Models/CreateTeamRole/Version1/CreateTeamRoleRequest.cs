using System.Collections.Generic;

namespace Stubbl.Api.Models.CreateTeamRole.Version1
{
    public class CreateTeamRoleRequest
    {
        public string Name { get; set; }
        public IReadOnlyCollection<string> Permissions { get; set; }
    }
}