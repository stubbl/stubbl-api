using System.Collections.Generic;

namespace Stubbl.Api.Queries.Shared.Version1
{
    public static class InvitationStatusExtensions
    {
        private static readonly Dictionary<Data.Collections.Invitations.InvitationStatus, InvitationStatus> s_mappings;

        static InvitationStatusExtensions()
        {
            s_mappings = new Dictionary<Data.Collections.Invitations.InvitationStatus, InvitationStatus>
            {
                {
                    Data.Collections.Invitations.InvitationStatus.Sent,
                    InvitationStatus.Sent
                },
                {
                    Data.Collections.Invitations.InvitationStatus.Accepted,
                    InvitationStatus.Accepted
                },
                {
                    Data.Collections.Invitations.InvitationStatus.Declined,
                    InvitationStatus.Declined
                }
            };
        }

        public static InvitationStatus ToInvitationStatus(this Data.Collections.Invitations.InvitationStatus extended)
        {
            return s_mappings.ContainsKey(extended) ? s_mappings[extended] : InvitationStatus.Unknown;
        }
    }
}