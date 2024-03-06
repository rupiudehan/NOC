using Noc_App.Models;

namespace Noc_App.UtilityService
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel,string subject);
    }
}
