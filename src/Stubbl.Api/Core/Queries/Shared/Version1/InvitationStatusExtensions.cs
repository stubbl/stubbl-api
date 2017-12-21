namespace Stubbl.Api.Core.Queries.Shared.Version1
{
   using System.Collections.Generic;

   public static class InvitationStatusExtensions
   {
      private static readonly Dictionary<Data.Collections.Invitations.InvitationStatus, InvitationStatus> _mappings;

      static InvitationStatusExtensions()
      {
         _mappings = new Dictionary<Data.Collections.Invitations.InvitationStatus, InvitationStatus>()
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
         return _mappings.ContainsKey(extended) ? _mappings[extended] : InvitationStatus.Unknown;
      }
   }
}