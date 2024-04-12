using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noc_App.Helpers;
using Noc_App.Models.interfaces;
using Noc_App.Models;
using Noc_App.Models.Payment;
using Noc_App.UtilityService;
using Noc_App.Clients;

namespace Noc_App.Controllers
{
    [AllowAnonymous]
    public class PaymentController : Controller
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _service;
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IRepository<GrantPaymentDetails> _repoPayment;
        private readonly IRepository<GrantDetails> _repo;
        public PaymentController(ILogger<PaymentController> logger, IRepository<GrantDetails> repo, IRepository<GrantPaymentDetails> repoPayment, IPaymentService service, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _logger = logger;
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _repoPayment = repoPayment;
            _repo=repo;
        }
        [AllowAnonymous]
        public IActionResult Index(PaymentRequest _paymentRequest)
        {
            return View(_paymentRequest);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ProcessRequestOrder(PaymentRequest _paymentRequest)
        {
            PaymentConfig settings = new PaymentConfig(_configuration["RazorPayOptions:ClientId"], _configuration["RazorPayOptions:ClientSecret"]);
            
            GrantNocPaymentOrderDetail _orderDetail = await _service.ProcessMerchantOrder(_paymentRequest, settings);
            ViewBag.GrantId=_orderDetail.GrantId;
            return View("Payment", _orderDetail);
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CompleteOrderProcess(int Itemid)
        {
            PaymentConfig settings = new PaymentConfig(_configuration["RazorPayOptions:ClientId"], _configuration["RazorPayOptions:ClientSecret"]);
            GrantDetails grant= await _repo.GetByIdAsync(Itemid);
            var detail = await _service.CompleteOrderProcess(_httpContextAccessor, Itemid, settings);
            var paymentDetails = new GrantPaymentDetails
            {
                CreatedOn = DateTime.Now,
                TotalAmount = Convert.ToDecimal(detail.Amount),
                PaymentOrderId = detail.OrderId,
                paymentid = detail.PaymentId,
                Paymentstatus = "A",
                GrantID=detail.GrantId
            };

            await _repoPayment.CreateAsync(paymentDetails);
            string PaymentMessage = detail.Status;
            if (PaymentMessage == "captured")
            {
                return RedirectToAction("Success", new {id= grant.ApplicationID });
            }
            else
            {
                return RedirectToAction("Failed");
            }
        }
        [AllowAnonymous]
        public IActionResult Success(string id)
        {
            return RedirectToAction("Index", "Grant", new { Id = id });
        }
        [AllowAnonymous]
        public IActionResult Failed()
        {
            return View();
        }
    }
}
