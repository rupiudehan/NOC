using Noc_App.Models.Payment;
using Noc_App.Models;
using Noc_App.Helpers;

namespace Noc_App.UtilityService
{
    public interface IPaymentService
    {
        //Task<GrantNocPaymentOrderDetail> ProcessOrder(PaymentRequest payRequest, IFMS_PaymentConfig _paymentConfig);
        Task<GrantNocPaymentOrderDetail> ProcessMerchantOrder(PaymentRequest payRequest, PaymentConfig _settings);
        OrderCompletionDetail CompleteOrderProcess(IHttpContextAccessor _httpContextAccessor, int grantid, PaymentConfig _settings);
    }
}
