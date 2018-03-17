namespace Stubbl.Api.Models.CreateTeamInvitation.Version1
{
    public class CreateTeamInvitationResponse
    {
        public CreateTeamInvitationResponse(string invitationId)
        {
            InvitationId = invitationId;
        }

        public string InvitationId { get; }
    }
}