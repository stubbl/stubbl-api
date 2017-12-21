namespace Stubbl.Api.Core.Commands.SendEmail.Version1
{
   using MimeKit;

   public interface IEmail
   {
      MimeMessage Message { get; }
   }
}
