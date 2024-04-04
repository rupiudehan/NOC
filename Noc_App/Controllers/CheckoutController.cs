using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Noc_App.Clients;
using Noc_App.Helpers;
using Noc_App.Models;
using Noc_App.Models.interfaces;
using System.Numerics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Noc_App.Controllers
{
    [AllowAnonymous]
    public class CheckoutController : Controller
    {
        private readonly IRepository<GrantPaymentDetails> _repo;
        private readonly IRepository<GrantDetails> _repoGrant;
        [TempData]
        public string TotalAmount { get; set; } = null;

        private readonly PaypalClient _paypalClient;
        public CheckoutController(PaypalClient paypalClient, IRepository<GrantPaymentDetails> repo, IRepository<GrantDetails> repoGrant)
        {
            this._paypalClient = paypalClient;
            this._repo = repo;
            this._repoGrant = repoGrant;
        }


        public IActionResult Index()
        {


            ViewBag.ClientId = _paypalClient.ClientId;

            try
            {
                //var cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                //ViewBag.cart = cart;
                //ViewBag.DollarAmount = cart.Sum(item => item.Grant.Khasras.Select(x => x.MarlaOrBiswansi).FirstOrDefault() * item.Quantity);
                ViewBag.total = TempData["TotalAreaAmount"]!=null?(Convert.ToDecimal(TempData["TotalAreaAmount"]) * Convert.ToDecimal(0.012)).ToString():"0";
                ViewBag.Id = TempData["GrantID"];
                double total = Convert.ToDouble(ViewBag.total);
                TotalAmount = total.ToString();
                TempData["TotalAmount"] = TotalAmount;

            }
            catch (Exception)
            {


            }
            return View();

        }
        public IActionResult Processing(string stripeToken, string stripeEmail)
        {
            //var optionCust = new CustomerCreateOptions
            //{
            //    Email = stripeEmail,
            //    Name = "Rizwan Yousaf",
            //    Phone = "338595119"
            //};
            //var serviceCust = new CustomerService();
            //Customer customer = serviceCust.Create(optionCust);
            //var optionsCharge = new ChargeCreateOptions
            //{
            //    Amount = Convert.ToInt64(TempData["TotalAmount"]),
            //    Currency = "USD",
            //    Description="Pet Selling amount",
            //    Source=stripeToken,
            //    ReceiptEmail=stripeEmail

            //};
            //var serviceCharge = new ChargeService();
            //Charge charge = serviceCharge.Create(optionsCharge);
            //if(charge.Status=="successded")
            //{
            //    ViewBag.AmountPaid = charge.Amount;
            //    ViewBag.Customer = customer.Name;
            //}
            return View();


        }
        [HttpPost]
        public async Task<IActionResult> Order(CancellationToken cancellationToken)
        {
            try
            {
                // set the transaction price and currency
                var price = TotalAmount;
                var currency = "USD";

                // "reference" is the transaction key
                var reference = GetRandomInvoiceNumber();// "INV002";

                var response = await _paypalClient.CreateOrder(price, currency, reference);

                return Ok(response);
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }
        public async Task<IActionResult> Capture(string orderId,string payerID,int grantId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderId);
                if (response.purchase_units != null)
                {
                    var reference = response.purchase_units != null ? response.purchase_units[0].reference_id : "ff2323";

                    // Put your logic to save the transaction here
                    // You can use the "reference" variable as a transaction key
                    if (string.IsNullOrEmpty(orderId)) await Task.FromResult(BadRequest("orderId"));
                    if (string.IsNullOrEmpty(payerID)) await Task.FromResult(BadRequest("payerID"));
                    var paymentDetails = new GrantPaymentDetails
                    {
                        CreatedOn = DateTime.Now,
                        TotalAmount = 100,
                        PaymentOrderId = orderId,
                        paymentid = 1,
                        Paymentstatus = "A",
                        sessionid = payerID,
                        referenceId = reference,
                        GrantID = grantId,
                        PayerName=response.payer.name.given_name,
                        PayerEmail=response.payer.email_address,
                        PayerId=payerID

                    };

                    await _repo.CreateAsync(paymentDetails);
                    //return result > 0 ? Ok() : BadRequest("Error while saving");

                    return Ok(response);
                }
                else
                {
                    return BadRequest("Access Denied!");
                }
            }
            catch (Exception e)
            {
                var error = new
                {
                    e.GetBaseException().Message
                };

                return BadRequest(error);
            }
        }
        public static string GetRandomInvoiceNumber()
        {
            return new Random().Next(999999).ToString();
        }

        [AllowAnonymous]
        public IActionResult Success(int id)
        {
            return RedirectToAction("Index", "Grant", new { Id = id });
        }
    }
}
