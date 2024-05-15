using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noc_App.Helpers;
using Noc_App.Models.interfaces;
using Noc_App.Models;
using Noc_App.Models.Payment;
using Noc_App.UtilityService;
using Noc_App.Clients;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Noc_App.Models.ViewModel;
using static Noc_App.Models.IFMSPayment.DepartmentModel;
using Newtonsoft.Json;
using Noc_App.PaymentUtilities;
using Microsoft.AspNetCore.DataProtection;
using System.Runtime;
using System.Text;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Noc_App.Models.IFMSPayment;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;
using Razorpay.Api;
using RestSharp;
using HttpMethod = System.Net.Http.HttpMethod;

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
        [Obsolete]
        public IActionResult ProcessChallan(PaymentRequest _paymentRequest)
        {
            try
            {
                IFMS_PaymentConfig settings = new IFMS_PaymentConfig(_configuration["IFMSPayOptions:IpAddress"],
                    _configuration["IFMSPayOptions:IntegratingAgency"], _configuration["IFMSPayOptions:clientSecret"], 
                    _configuration["IFMSPayOptions:clientId"], _configuration["IFMSPayOptions:ChecksumKey"],
                    _configuration["IFMSPayOptions:edKey"], _configuration["IFMSPayOptions:edIV"], _configuration["IFMSPayOptions:ddoCode"]
                    , _configuration["IFMSPayOptions:companyName"], _configuration["IFMSPayOptions:deptCode"], _configuration["IFMSPayOptions:payLocCode"]
                    , _configuration["IFMSPayOptions:trsyPaymentHead"], _configuration["IFMSPayOptions:PostUrl"], _configuration["IFMSPayOptions:headerClientId"]);

                string ChecksumKey = settings.ChecksumKey;
                string edKey = settings.edKey;
                string edIV = settings.edIV;

                DepartmentDl dpt = new DepartmentDl();

                IFMS_EncrDecr obj = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
                Random randomObj = new Random();
                //string transactionId = randomObj.Next(10000000, 100000000).ToString();
                string transactionId = DateTime.Now.ToString("yyyyMMddHHmmssff");

                ifms_data objPayment = new ifms_data
                {
                    challandata = new Challandata()
                    {
                        deptRefNo = transactionId,
                        receiptNo = "",
                        clientId = settings.clientId,
                        challanDate = DateTime.Now.ToString("yyyy-MM-ddT00:00:00"),
                        expiryDate = DateTime.Now.ToString("yyyy-MM-ddT00:00:00"),
                        companyName = settings.companyName,
                        deptCode = settings.deptCode,
                        totalAmt = _paymentRequest.Amount.ToString(),
                        trsyAmt = _paymentRequest.Amount.ToString(),
                        nonTrsyAmt = "0",
                        noOfTrans = "1",
                        ddoCode = settings.ddoCode,
                        payLocCode = settings.payLocCode,
                        add1 = _paymentRequest.Hadbast,
                       // add2 = _paymentRequest.PlotNo,
                        add2 = "",
                        add3 = _paymentRequest.Address,
                        add4 = "",
                        add5 = "",
                        sURL = Url.Action("succesfulURL", "Payment", new { id = _paymentRequest.ApplicationId }),
                        fURL = Url.Action("succesfulURL", "Payment", new { id = _paymentRequest.ApplicationId }),//"?failureURL",

                        payee_info = new PayeeInfo()
                        {
                            payerName = _paymentRequest.PayerName,
                            teleNumber = _paymentRequest.MobileNo,
                            mobNumber = _paymentRequest.MobileNo,
                            emailId = _paymentRequest.Email,
                            //teleNumber = "9478215852",
                            //mobNumber = "9478215852",
                            //emailId = "rupi.udhehan@gmail.com",
                            addLine1 = _paymentRequest.Village,
                            addLine2 = _paymentRequest.Tehsil,
                            addPincode = _paymentRequest.Pincode,
                            district = 28.ToString(),//_paymentRequest.DistrictId,
                            tehsil = 245.ToString(),//_paymentRequest.TehsilId
                        },
                        trsyPayments = new List<trsyPayments>()
                                         {
                                         new trsyPayments(){
                                         Head= settings.trsyPaymentHead,
                                         amt= _paymentRequest.Amount.ToString()}
                                         }
                        
                    }
                };
                //objPayment.challandata = dpt.FillChallanData();
                string json = JsonConvert.SerializeObject(objPayment.challandata);
                objPayment.chcksum = obj.CheckSum(json);
                //string jsonCHK = JsonConvert.SerializeObject(objPayment);
                string jsonCHK = JsonConvert.SerializeObject(objPayment);
                string encData = obj.Encrypt(jsonCHK);

                ViewBag.GrantId = _paymentRequest.ApplicationId;
                IFMSHeader cHeader = new IFMSHeader();
                cHeader.clientId = settings.headerClientId;
                cHeader.clientSecret = settings.clientSecret;
                cHeader.transactionID = transactionId;
                cHeader.ipAddress = settings.IpAddress;
                cHeader.integratingAgency = settings.IntegratingAgency;
                cHeader.encdata = encData;
                cHeader.ApplicationId = _paymentRequest.ApplicationId;
                return View(cHeader);
                //ViewBag.HtmlStr = PreparePOSTForm("ChallanFormPost", encData, cHeader, _paymentRequest.ApplicationId);
               // hrmsView pay = new hrmsView();
                //pay.encdata = encData;
                //return View();
            }
            catch (Exception ex)
            {

               // return ex.Message;
            }
            return View();
        }

        private string PreparePOSTForm(string url, string encData, IFMSHeader cHeader,string GrantId)
        // post form
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<html><head>");
                sb.AppendFormat(@"<meta name='ContentType' content='application/json' />");
                sb.AppendFormat(@"<meta name='Accept' content='application/json' />");
                sb.AppendFormat(@"</head>");
                string formID = "PostForm";
                sb.Append(@"<body><form id=\""" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">").Replace("id=\\\"PostForm\"", "id =\"PostForm");
                sb.AppendFormat("<input type='hidden' name='encData' value='{0}'/>",
               encData);
                sb.AppendFormat("<input type='hidden' name='clientId' value='{0}'/>",
               cHeader.clientId);
                sb.AppendFormat("<input type='hidden' name='ApplicationId' value='{0}'/>",
               GrantId);
                sb.AppendFormat("<input type='hidden' name='clientSecret' value='{0}'/>",
               cHeader.clientSecret);
                sb.AppendFormat("<input type='hidden' name='integratingAgency' value = '{0}' /> ", cHeader.integratingAgency);

                sb.AppendFormat("<input type='hidden' name='ipAddress' value='{0}'/>",
               cHeader.ipAddress);
                sb.AppendFormat("<input type='hidden' name='transactionID' value='{0}'/>",
               cHeader.transactionID);
                sb.AppendFormat("</form>");
                StringBuilder strScript = new StringBuilder();
                strScript.Append("<script language='javascript'>document.addEventListener(\"load\", (event) => {");
                strScript.Append("var v" + formID + " = document.forms['" + formID + "'];");
                strScript.Append("v" + formID + ".submit();");
                strScript.Append("});</script>");
                sb.Append(strScript);
                sb.AppendFormat("</body></html>");
                return sb.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
                ;
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ChallanForm()
        {
            ViewBag.HtmlContent = TempData["Message"] as string;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChallanFormPost(IFMSHeader data)
        {
            var options = new RestClientOptions(_configuration["IFMSPayOptions:PostUrl"])
            {
                MaxTimeout = -1,
            };
            var client = new RestSharp.RestClient(options);
            var request = new RestRequest(_configuration["IFMSPayOptions:PostUrl"], Method.Post);
            request.AddHeader("Cookie", ".AspNetCore.Antiforgery.FHElugoL8ws=CfDJ8ML4BLgAq9lJmJpxAKvSYEz0j8fWp731M3vXH0c2v3Rn5rwpkNkVhJ1RkvwgBgl2tLfMj2P4zhVDSLUqY8aOaI2zuVuLKncGT5ZsYLkzC9a7ap59SkEvg5wKDcGkor04CFb9gedXetHF7x4zh6m1Eog");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("encData", data.encdata);
            request.AddParameter("clientId", data.clientId);
            request.AddParameter("clientSecret", data.clientSecret);
            request.AddParameter("integratingAgency", data.integratingAgency);
            request.AddParameter("ipAddress", data.ipAddress);
            request.AddParameter("transactionID", data.transactionID);
            RestResponse response = await client.ExecuteAsync(request);
            if (response.IsSuccessful)
            {
                var content = response.Content.Replace("/eRctDeptInt", "https://ifmstg.punjab.gov.in/eRctDeptInt");
                //string resultContent = await ChallanVerify(cHeader);
                //ResponseDetail root = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseDetail>(resultContent);
                //if(root.statusCode!= "SC300") return RedirectToAction("Failed", new { id = data.ApplicationId });
                //else
                //return RedirectToAction("ChallanFormPost", new { content = msg });
                TempData["Message"] = content;
                //ViewBag.HtmlContent = content;
                return RedirectToAction("ChallanForm");
                //return View();
            }
            else
            {
                // Handle unsuccessful response
                return RedirectToAction("Failed", new { id = data.ApplicationId });
            }
            //Checkdata cHeader = new Checkdata { 
            //    clientId=data.clientId,
            //    clientSecret=data.clientSecret,
            //    encData=data.encdata,
            //    integratingAgency=data.integratingAgency,
            //    ipAddress= data.ipAddress,
            //    transactionID=data.transactionID
            //};
            //var reqData = JsonConvert.SerializeObject(cHeader);
            //HttpResponseMessage resMsg = await ExecuteAPI(_configuration["IFMSPayOptions:PostUrl"],reqData);
            //if (resMsg.IsSuccessStatusCode)
            //{
            //    var msg= resMsg.Content.ReadAsStringAsync().Result;
            //    //string resultContent = await ChallanVerify(cHeader);
            //    //ResponseDetail root = Newtonsoft.Json.JsonConvert.DeserializeObject<ResponseDetail>(resultContent);
            //    //if(root.statusCode!= "SC300") return RedirectToAction("Failed", new { id = data.ApplicationId });
            //    //else
            //    return RedirectToAction("Success", new { GrantId = data.ApplicationId });
            //}
            //else
            //{
            //    // Handle unsuccessful response
            //    return RedirectToAction("Failed", new { id = data.ApplicationId });
            //}
        }

        public string ChallanVerify(/*ifms_data dataFrm*/Checkdata cHeader)
        {
            //DepartmentDl dpt = new DepartmentDl();
            //ifms_data data = new ifms_data();
            //IFMS_EncrDecr obj = new IFMS_EncrDecr(_configuration["IFMSPayOptions:ChecksumKey"], _configuration["IFMSPayOptions:edKey"], _configuration["IFMSPayOptions:edIV"]);
            //data.challandata = new Challandata()
            //{
            //    deptRefNo = "128",
            //    clientId = _configuration["IFMSPayOptions:clientId"],
            //    deptCode = "WAS",
            //    challanDate = DateTime.Now
            //};
            //string json = JsonConvert.SerializeObject(data.challandata);
            //data.chcksum = obj.CheckSum(json);//CheckSum of Chllan data 
            //string jsonCHK = JsonConvert.SerializeObject(data);
            //string encData = obj.Encrypt(jsonCHK);

            //Checkdata cHeader = new Checkdata()
            //{
            //    encData = encData,
            //    clientId = _configuration["IFMSPayOptions:clientId"],
            //    clientSecret = _configuration["IFMSPayOptions:clientSecret"],
            //    transactionID = new Random().Next(100000, 999999).ToString(),
            //    ipAddress = _configuration["IFMSPayOptions:ipAddress"],
            //    integratingAgency = _configuration["IFMSPayOptions:integratingAgency"]
            //};
            var reqData = JsonConvert.SerializeObject(cHeader);
            HttpResponseMessage resMsg = ExecuteAPI(_configuration["IFMSPayOptions:Verify"],reqData);
            string retJson = resMsg.Content.ReadAsStringAsync().Result;
            return retJson;

        }

        private HttpResponseMessage ExecuteAPI(string URL, string PostData)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
            request.Content = new StringContent(PostData, Encoding.UTF8,"application/json");
            HttpContent inputContent = new StringContent(PostData, Encoding.UTF8,"application/json");
            HttpResponseMessage response = client.PostAsync(URL, inputContent).Result;
            return response;
        }


        [HttpPost]
        public ActionResult succesfulURL([FromForm] string id)
        {
            ////IFMS_EncrDecr obj = new IFMS_EncrDecr();
            ////sendClient data = new sendClient();
            ////string encData = obj.Decrypt(model.data);
            ////sendClient reqData = JsonConvert.DeserializeObject<sendClient>(encData);
            //ViewBag.paymentid = "";
            //ViewBag.amount = "";
            //ViewBag.status = "success";
            //return View();
            return RedirectToAction("Index", "Grant", new { Id = id });
        }
        [HttpPost]
        public ActionResult failureURL([FromForm] Checkdata model)
        {
            //IFMS_EncrDecr obj = new IFMS_EncrDecr();
            //sendClient data = new sendClient();
            //string encData = obj.Decrypt(model.data);
            //sendClient reqData = JsonConvert.DeserializeObject<sendClient>(encData);
            ViewBag.paymentid = "";
            ViewBag.amount = "";
            ViewBag.status = "failure";
            return View();
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
            var detail = _service.CompleteOrderProcess(_httpContextAccessor, Itemid, settings);
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
                return RedirectToAction("Failed", new { id = grant.ApplicationID });
            }
        }
        [AllowAnonymous]
        public IActionResult Success(string GrantId)
        {
            return RedirectToAction("Index", "Grant", new { Id = GrantId });
        }
        [AllowAnonymous]
        public async Task<IActionResult> Failed(string id)
        {
            try
            {
                GrantDetails obj = (await _repo.FindAsync(x => x.ApplicationID.ToLower() == id.ToLower())).FirstOrDefault();
                if (obj != null)
                {
                    var payment = await _repoPayment.FindAsync(x => x.GrantID == obj.Id);
                    if (payment == null || payment.Count() == 0)
                    {
                        GrantViewModel model = new GrantViewModel
                        {
                            Id = obj.Id,
                            ApplicationID = obj.ApplicationID,
                            OrderId = "0",
                            Message = "Payment is unsuccessfull"
                        };
                        return View(model);
                    }
                    else
                    {
                        GrantPaymentDetails objPyment = (payment).FirstOrDefault();
                        GrantViewModel model = new GrantViewModel
                        {
                            Id = obj.Id,
                            ApplicationID = obj.ApplicationID,
                            OrderId = objPyment.PaymentOrderId,
                            Message = "Payment is successfull"
                        };
                        return View(model);
                    }
                }
                else
                {
                    return View(null);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
    }
}
