using System.Threading;
using System.Threading.Tasks;

namespace Stubbl.Api.Services.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(IEmail email, CancellationToken cancellationToken);
    }
}
