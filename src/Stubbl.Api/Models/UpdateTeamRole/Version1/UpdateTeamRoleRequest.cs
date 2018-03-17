using System.Collections.Generic;

namespace Stubbl.Api.Models.UpdateTeamRole.Version1
{
    public class UpdateTeamRoleRequest
    {
        public string Name { get; set; }
        public IReadOnlyCollection<string> Permissions { get; set; }
    }
}