using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noc_App.Helpers;
using Noc_App.Models.interfaces;
using Noc_App.Models;
using Noc_App.Models.Payment;
using Noc_App.UtilityService;
using Noc_App.Models.ViewModel;
using static Noc_App.Models.IFMSPayment.DepartmentModel;
using Newtonsoft.Json;
using Noc_App.PaymentUtilities;
using System.Text;
using Noc_App.Models.IFMSPayment;
using RestSharp;
using HttpMethod = System.Net.Http.HttpMethod;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Noc_App.Controllers
{
    [AllowAnonymous]
    public class PaymentController : Controller
    {
        private IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IRepository<GrantPaymentDetails> _repoPayment;
        private readonly IRepository<GrantDetails> _repo;
        private readonly IRepository<ChallanDetails> _repoChallanDetails;
        private readonly IEmailService _emailService;
        public PaymentController(IEmailService emailService, IRepository<GrantDetails> repo, IRepository<GrantPaymentDetails> repoPayment, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IRepository<ChallanDetails> repoChallanDetails)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _repoPayment = repoPayment;
            _repo = repo;
            _repoChallanDetails = repoChallanDetails;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [Obsolete]
        public async Task<IActionResult> ProcessChallan(PaymentRequest _paymentRequest)
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
                string transaction = randomObj.Next(10, 100).ToString();
                string transactionId = DateTime.Now.ToString("yyyyMMddHHmmssff") + transaction;
                var d = HttpContext.Request.Host.Value;
                var scheme=HttpContext.Request.Scheme;
                //var base_url = "@Request.Url.GetLeftPart(UriPartial.Authority)@Url.Content("~/ ")";
                //string domainName = Request.Url.GetLeftPart(UriPartial.Authority);
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
                        ddoCode = _paymentRequest.DdoCode,
                        payLocCode = _paymentRequest.PayLocCode,
                        add1 = _paymentRequest.Hadbast,
                         add2 = _paymentRequest.PlotNo,
                        add3 = _paymentRequest.Address,
                        add4 = "",
                        add5 = "",
                        sURL = scheme+"://" + d + Url.Action("succesfulURL", "Payment"/*, new { id = _paymentRequest.ApplicationId }*/).Replace("https://ifmstg.punjab.gov.in/", d),
                        fURL = scheme + "://" + d + Url.Action("failureURL", "Payment"/*, new { id = _paymentRequest.ApplicationId }*/).Replace("https://ifmstg.punjab.gov.in/", d),//"?failureURL",

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
                            district = _paymentRequest.DistrictId,
                            tehsil = _paymentRequest.TehsilId
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
                ChallanDetails data = new ChallanDetails
                {
                    addLine1 = objPayment.challandata.payee_info.addLine1,
                    addLine2 = objPayment.challandata.payee_info.addLine2,
                    addPincode = objPayment.challandata.payee_info.addPincode,
                    totalAmt = objPayment.challandata.totalAmt,
                    ApplicationId = _paymentRequest.ApplicationId,
                    challanDate = objPayment.challandata.challanDate,
                    companyName = objPayment.challandata.companyName,
                    ddoCode = objPayment.challandata.ddoCode,
                    deptCode = objPayment.challandata.deptCode,
                    deptRefNo = objPayment.challandata.deptRefNo,
                    district = objPayment.challandata.payee_info.district,
                    emailId = objPayment.challandata.payee_info.emailId,
                    expiryDate = objPayment.challandata.expiryDate,
                    fURL = objPayment.challandata.fURL,
                    mobNumber = objPayment.challandata.payee_info.mobNumber,
                    nonTrsyAmt = objPayment.challandata.nonTrsyAmt,
                    noOfTrans = objPayment.challandata.noOfTrans,
                    payerName = objPayment.challandata.payee_info.payerName,
                    payLocCode = objPayment.challandata.payLocCode,
                    sURL = objPayment.challandata.sURL,
                    tehsil = objPayment.challandata.payee_info.tehsil,
                    teleNumber = objPayment.challandata.payee_info.teleNumber,
                    trsyAmt = objPayment.challandata.trsyAmt,
                    RequestStatus = "Fresh Request",
                    add1 = objPayment.challandata.add1,
                    add2 = objPayment.challandata.add2,
                    add3 = objPayment.challandata.add3,
                    add4 = objPayment.challandata.add4,
                    add5 = objPayment.challandata.add5
                };
                await _repoChallanDetails.CreateAsync(data);
                cHeader.ChallanId = (await _repoChallanDetails.FindAsync(x => x.deptRefNo == objPayment.challandata.deptRefNo)).FirstOrDefault().Id;
                ViewBag.HtmlStr = PreparePOSTForm("ChallanFormPost", encData, cHeader, _paymentRequest.ApplicationId);
                return View(cHeader);
                //
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

        private string PreparePOSTForm(string url, string encData, IFMSHeader cHeader, string GrantId)
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
                sb.AppendFormat("<input type='hidden' name='ChallanId' value='{0}'/>",
               cHeader.ChallanId);
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
            TempData["Message"] = TempData["Message"];
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChallanFormPost(IFMSHeader data)
        {
            try
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
                    var url = "DistOptions.url = vdir + \"Server/SelectTel?DistCode=\" + $(\"#DistrictCode\").val()";
                    var actual = "DistOptions.url = \"/Payment/TrpSarthi?DistCode=\" + $(\"#DistrictCode\").val()";
                    
                    TempData["Message"] = (content.Replace(url,actual)).Replace("<input type='hidden' name='DeptCode' value='WAS'/>", "<input type='hidden' name='DeptCode' value='WAS'/>@TempData.Keep()");
                    return RedirectToAction("ChallanForm", "Payment");
                }
                else
                {
                    // Handle unsuccessful response
                    return RedirectToAction("Failed", new { id = data.ApplicationId });
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
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
            HttpResponseMessage resMsg = ExecuteAPI(_configuration["IFMSPayOptions:Verify"], reqData);
            string retJson = resMsg.Content.ReadAsStringAsync().Result;
            return retJson;

        }

        private HttpResponseMessage ExecuteAPI(string URL, string PostData)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, URL);
            request.Content = new StringContent(PostData, Encoding.UTF8, "application/json");
            HttpContent inputContent = new StringContent(PostData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(URL, inputContent).Result;
            return response;
        }

        [Obsolete]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> succesfulURL([FromForm] Checkdata model)
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
            IFMS_EncrDecr obj = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
            var detail = obj.Decrypt(model.encData);
            ifms_dataRes ResponseData = Newtonsoft.Json.JsonConvert.DeserializeObject<ifms_dataRes>(detail);
            var challanDetail = (await _repoChallanDetails.FindAsync(x => x.deptRefNo == ResponseData.challandata.deptRefNo /*&& x.RequestStatus == "Fresh Request"*/)).FirstOrDefault();

            string id = challanDetail.ApplicationId;
            if (ResponseData.challandata.bank_Res.status.ToLower() == "failure")
            {
                challanDetail.RequestStatus = "Failed";
                await _repoChallanDetails.UpdateAsync(challanDetail);
                return RedirectToAction("Failed", new { id = challanDetail.ApplicationId });
            }
            else if (ResponseData.challandata.bank_Res.status.ToLower() == "pending")
            {
                challanDetail.RequestStatus = "Pending";
                await _repoChallanDetails.UpdateAsync(challanDetail);
                return RedirectToAction("Failed", new { id = challanDetail.ApplicationId });
            }
            else
            {
                challanDetail.RequestStatus = "Successful";
                GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == id)).FirstOrDefault();
                GrantPaymentDetails payment = new GrantPaymentDetails
                {
                    deptRefNo = challanDetail.deptRefNo,
                    GrantID = grant.Id,
                    PayerEmail = challanDetail.emailId,
                    PayerName = challanDetail.payerName,
                    PaymentOrderId = challanDetail.deptRefNo,
                    TotalAmount = Convert.ToDecimal(challanDetail.totalAmt),
                    CreatedOn = DateTime.Now

                };
                await _repoPayment.CreateAsync(payment);
                await _repoChallanDetails.UpdateAsync(challanDetail);
                var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessageWithPayment(grant.ApplicantName, grant.ApplicationID, ResponseData.challandata.deptRefNo, Convert.ToDecimal(challanDetail.totalAmt)));
                _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");
                return RedirectToAction("Index", "Grant", new { Id = id });
            }
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> failureURL([FromForm] Checkdata model)
        {
            //IFMS_EncrDecr obj = new IFMS_EncrDecr();
            //sendClient data = new sendClient();
            //string encData = obj.Decrypt(model.data);
            //sendClient reqData = JsonConvert.DeserializeObject<sendClient>(encData);
            IFMS_PaymentConfig settings = new IFMS_PaymentConfig(_configuration["IFMSPayOptions:IpAddress"],
                    _configuration["IFMSPayOptions:IntegratingAgency"], _configuration["IFMSPayOptions:clientSecret"],
                    _configuration["IFMSPayOptions:clientId"], _configuration["IFMSPayOptions:ChecksumKey"],
                    _configuration["IFMSPayOptions:edKey"], _configuration["IFMSPayOptions:edIV"], _configuration["IFMSPayOptions:ddoCode"]
                    , _configuration["IFMSPayOptions:companyName"], _configuration["IFMSPayOptions:deptCode"], _configuration["IFMSPayOptions:payLocCode"]
                    , _configuration["IFMSPayOptions:trsyPaymentHead"], _configuration["IFMSPayOptions:PostUrl"], _configuration["IFMSPayOptions:headerClientId"]);

            string ChecksumKey = settings.ChecksumKey;
            string edKey = settings.edKey;
            string edIV = settings.edIV;
            IFMS_EncrDecr obj = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
            if (model.encData != null)
            {
                var detail = obj.Decrypt(model.encData);
                ifms_dataRes ResponseData = Newtonsoft.Json.JsonConvert.DeserializeObject<ifms_dataRes>(detail);
                var challanDetail = (await _repoChallanDetails.FindAsync(x => x.deptRefNo == ResponseData.challandata.deptRefNo /*&& x.RequestStatus == "Fresh Request"*/)).FirstOrDefault();

                string id = challanDetail.ApplicationId;
                if (ResponseData.challandata.bank_Res.desc == "failure")
                {
                    challanDetail.RequestStatus = "Failed";
                    await _repoChallanDetails.UpdateAsync(challanDetail);
                    return RedirectToAction("Failed", new { id = challanDetail.ApplicationId });
                }
                else if (ResponseData.challandata.bank_Res.desc == "Pending")
                {
                    challanDetail.RequestStatus = "Pending";
                    await _repoChallanDetails.UpdateAsync(challanDetail);
                    return RedirectToAction("Failed", new { id = challanDetail.ApplicationId });
                }
                return View(model);
            }
            else
            {
                ViewBag.HtmlContent = TempData["Message"] as string;
                return RedirectToAction("FailureUrl", "Grant");
            }
        }

        [AllowAnonymous]
        public IActionResult Index(PaymentRequest _paymentRequest)
        {
            return View(_paymentRequest);
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
                if (id != null)
                {
                    var challanDetail = (await _repoChallanDetails.FindAsync(x => x.ApplicationId == id && (x.RequestStatus == "Fresh Request" || x.RequestStatus=="Failed"))).FirstOrDefault();

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
                                Message = "Payment is unsuccessful"
                            };
                            if (challanDetail != null)
                            {
                                if (challanDetail.RequestStatus != "Failed")
                                {
                                    challanDetail.RequestStatus = "Failed";
                                    await _repoChallanDetails.UpdateAsync(challanDetail);
                                }
                            }
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
                                Message = "Payment is successful"
                            };
                            challanDetail.RequestStatus = "Successful";
                            await _repoChallanDetails.UpdateAsync(challanDetail);
                            return View(model);
                        }
                    }
                    else
                    {
                        return View(null);
                    }
                }
                else
                {
                    GrantViewModel model = new GrantViewModel
                    {
                        Id = 0,
                        ApplicationID = "0",
                        OrderId = "0",
                        Message = "Payment is unsuccessful"
                    };
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> TrpSarthi(string DistCode)
        {
            try
            {
                var options2 = new RestClientOptions(_configuration["IFMSPayOptions:PostUrl"])
                {
                    MaxTimeout = -1,
                };
                var client2 = new RestClient(options2);
                var request2 = new RestRequest("https://ifmstg.punjab.gov.in/eRctDeptInt/TrpSarthi/SelectTel?DistCode=" + DistCode, Method.Post);
                RestResponse response2 = await client2.ExecuteAsync(request2);
                List<tehsildetailViewModel> ResponseData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<tehsildetailViewModel>>(response2.Content);

                return Json(ResponseData);
            }
            catch (Exception)
            {
                return Json(null);
            }
        }
    }
}
