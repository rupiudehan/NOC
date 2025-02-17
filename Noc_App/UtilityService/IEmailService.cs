using Noc_App.Models;

namespace Noc_App.UtilityService
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel,string subject);
        int SendEmail2(EmailModel emailModel, string subject);
    }
}
