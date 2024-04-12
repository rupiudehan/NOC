using Microsoft.Extensions.Options;
using Noc_App.Helpers;
using Noc_App.Models;
using Noc_App.Models.Payment;

namespace Noc_App.UtilityService
{
    public class PaymentService : IPaymentService
    {
        private PaymentConfig settings;
        public Task<GrantNocPaymentOrderDetail> ProcessMerchantOrder(PaymentRequest payRequest, PaymentConfig _settings)
        {
            try
            {
                settings = _settings;
                string key = _settings.ClientId;// "rzp_test_cwvcfvJWu0dqoc";
                string secret = _settings.ClientSecret;// "VQuu1AZq5aeDvN9BleEEsZCl";
                // Generate random receipt number for order
                Random randomObj = new Random();
                string transactionId = randomObj.Next(10000000, 100000000).ToString();
                //Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_8P7RhnsImxd2OR", "kD8tw7ECYsTTZnx0OyrKI4kh");
                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(key, secret);
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payRequest.Amount * 100);
                options.Add("receipt", transactionId);
                options.Add("currency", "INR");
                options.Add("payment_capture", "0"); // 1 - automatic  , 0 - manual
                //options.Add("Notes", "Test Payment of Merchant");
                Razorpay.Api.Order orderResponse = client.Order.Create(options);
                string orderId = orderResponse["id"].ToString();
                GrantNocPaymentOrderDetail order = new GrantNocPaymentOrderDetail
                {
                    OrderId = orderResponse.Attributes["id"],
                    RazorpayKey = key,//"rzp_test_8P7RhnsImxd2OR",
                    Amount = payRequest.Amount * 100,
                    Currency = "INR",
                    Name = payRequest.Name,
                    Email = payRequest.Email,
                    PhoneNumber = payRequest.PhoneNumber,
                    Address = payRequest.Address,
                    Description = "NOC Payment",
                    GrantId=payRequest.GrantId
                };
                return Task.FromResult(order);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<OrderCompletionDetail> CompleteOrderProcess(IHttpContextAccessor _httpContextAccessor,int grantid, PaymentConfig _settings)
        {
            try
            {
                settings = _settings;
                string key = settings.ClientId;// "rzp_test_cwvcfvJWu0dqoc";
                string secret = settings.ClientSecret;// "VQuu1AZq5aeDvN9BleEEsZCl";
                string paymentId = _httpContextAccessor.HttpContext.Request.Form["rzp_paymentid"];
                // This is orderId
                string orderId = _httpContextAccessor.HttpContext.Request.Form["rzp_orderid"];
                //Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient("rzp_test_8P7RhnsImxd2OR", "kD8tw7ECYsTTZnx0OyrKI4kh");
                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(key, secret);
                Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);
                // This code is for capture the payment 
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payment.Attributes["amount"]);
                Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
                string amt = paymentCaptured.Attributes["amount"];
                OrderCompletionDetail detail = new OrderCompletionDetail { 
                    GrantId=grantid,
                    OrderId=orderId,
                    PaymentId=paymentId,
                    Amount=Convert.ToDouble(amt)/100,
                    Status= paymentCaptured.Attributes["status"]
                };
                return detail;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
