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
using Org.BouncyCastle.Asn1.Ocsp;
using System.IO.Compression;

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

                int istest = Convert.ToInt16(_configuration["Testing"].ToString());
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
                        receiptNo = istest==0?"": transactionId,
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
                        sURL = //scheme+
                        "https://" + d + Url.Action("succesfulURL", "Payment"/*, new { id = _paymentRequest.ApplicationId }*/).Replace("https://ifmstg.punjab.gov.in/", d),
                        fURL = //scheme + 
                        "https://" + d + Url.Action("failureURL", "Payment"/*, new { id = _paymentRequest.ApplicationId }*/).Replace("https://ifmstg.punjab.gov.in/", d),//"?failureURL",

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
                if (istest == 1) {
                    Checkdata cHeaderR = new Checkdata();
                    cHeaderR.clientId = settings.headerClientId;
                    cHeaderR.clientSecret = settings.clientSecret;
                    cHeaderR.transactionID = transactionId;
                    cHeaderR.ipAddress = settings.IpAddress;
                    cHeaderR.integratingAgency = settings.IntegratingAgency;
                    cHeaderR.encData = encData;
                    succesfulURL(cHeaderR);
                }
                else {
                    ViewBag.HtmlStr = PreparePOSTForm("ChallanFormPost", encData, cHeader, _paymentRequest.ApplicationId);
                    return View(cHeader);
                }
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
            //TempData["Message"] = TempData["Message"];
            return View();
        }
        [HttpPost]
        [Obsolete]
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
                    var content = response.Content.Replace("/eRctDeptInt", _configuration["IFMSPayOptions:replace"]);
                    if (content.Contains("An error occurred while processing your request."))
                    {
                        return RedirectToAction("Failed", new { id = data.ApplicationId });
                    }
                    else
                    {
                        var url = "DistOptions.url = vdir + \"Server/SelectTel?DistCode=\" + $(\"#DistrictCode\").val()";
                        var actual = "DistOptions.url = \"/Payment/TrpSarthi?DistCode=\" + $(\"#DistrictCode\").val()";

                        //TempData["Message"] 
                        string formdata = (content.Replace(url, actual)).Replace("<input type='hidden' name='DeptCode' value='WAS'/>", "<input type='hidden' name='DeptCode' value='WAS'/>@TempData.Keep()");
                        
                        byte[] compressedData = CompressData(System.Text.Encoding.UTF8.GetBytes(formdata));
                        string encData = Convert.ToBase64String(compressedData);
                        TempData["Message"] = encData;

                        return RedirectToAction("ChallanForm", "Payment");
                    }
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
        private byte[] CompressData(byte[] data)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    gzipStream.Write(data, 0, data.Length); // Write the input data into the GZipStream
                }
                return memoryStream.ToArray(); // Get the compressed data from the memory stream
            }
        }

        // New DecompressData method
        private byte[] DecompressData(byte[] compressedData)
        {
            using (var compressedStream = new MemoryStream(compressedData))
            {
                using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        gzipStream.CopyTo(resultStream);
                        return resultStream.ToArray();
                    }
                }
            }
        }
        [HttpGet]
        public IActionResult VerifyPayment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyPayment(string transId,string challandate) {
            try
            {
                IFMS_PaymentConfig settings = new IFMS_PaymentConfig(_configuration["IFMSPayOptions:IpAddress"],
                  _configuration["IFMSPayOptions:IntegratingAgency"], _configuration["IFMSPayOptions:clientSecret"],
                  _configuration["IFMSPayOptions:clientId"], _configuration["IFMSPayOptions:ChecksumKey"],
                  _configuration["IFMSPayOptions:edKey"], _configuration["IFMSPayOptions:edIV"], _configuration["IFMSPayOptions:ddoCode"]
                  , _configuration["IFMSPayOptions:companyName"], _configuration["IFMSPayOptions:deptCode"], _configuration["IFMSPayOptions:payLocCode"]
                  , _configuration["IFMSPayOptions:trsyPaymentHead"], _configuration["IFMSPayOptions:PostUrl"], _configuration["IFMSPayOptions:headerClientId"]);

                ifms_verifydata data = new ifms_verifydata();
                IFMS_EncrDecr obj = new IFMS_EncrDecr(settings.ChecksumKey, settings.edKey, settings.edIV);
                //var challanDetail = (await _repoChallanDetails.FindAsync(x => x.receiptNo == transId)).OrderByDescending(x=>x.Id).FirstOrDefault();
                data.challandata = new Challanverifydata()
                {
                    deptRefNo = transId,//challanDetail.deptRefNo,
                    clientId = settings.clientId,
                    deptCode = settings.deptCode,
                    challanDate = challandate,//DateTime.Now.ToString("yyyy-MM-ddT00:00:00")
                };
                string json = JsonConvert.SerializeObject(data.challandata);
                data.chcksum = obj.CheckSum(json);//CheckSum of Chllan data 
                string jsonCHK = JsonConvert.SerializeObject(data);
                string encData = obj.Encrypt(jsonCHK);

                Checkdata cHeader = new Checkdata()
                {
                    encData = encData,
                    clientId = settings.headerClientId,
                    clientSecret = settings.clientSecret,
                    transactionID = new Random().Next(100000,999999).ToString(),
                    ipAddress = settings.IpAddress,
                    integratingAgency = settings.IntegratingAgency,
                    dateTime=null
                };
                string result=ChallanVerify(cHeader);
            }
            catch (Exception ex)
            {

                throw;
            }
            return View();
        }

        public string ChallanVerify(/*ifms_data dataFrm*/Checkdata cHeader)
        {
            var reqData = JsonConvert.SerializeObject(cHeader);
            HttpResponseMessage resMsg = ExecuteAPI(_configuration["IFMSPayOptions:Verify"], reqData);
            //ExecuteAPIRequest(_configuration["IFMSPayOptions:Verify"], reqData);
            string retJson = resMsg.Content.ReadAsStringAsync().Result;
            return retJson;

        }

        private async void ExecuteAPIRequest(string URL, string PostData)
        {
            try
            {
                var options = new RestClientOptions(URL)
                {
                    MaxTimeout = -1,
                };
                var client = new RestClient(options);
                var request = new RestRequest(URL, Method.Post);
                request.AddHeader("Content-Type", "application/json");
                var body =  @"{""encData"":""Asrv4OIbe5hiUGCJk84Je0nWdt+22svrDtZNFROBK7eVkePkm6exLx9mWNnFQdjXWGca8+GOfXh3adMAYBZUH7djCRRumKuww8ZBZhI1kNkwok0hMn/Ulw+4QB/J81L7h/uwjVFIZ+nJ15DCfNDBXGd01aq00Qwd3cLThZgoj3dRw8QgkdBNsVGCsfU6sIW4VRXKfZIDSbFmWor243sj24q2gwhN88ZM9kPihA1RJsqQWulk//QNZGxLb3qo6ijCElgutqeS9g3XMB7EQghjwMuI4ao++myMq39UrRRO0HYmcLTcMcXEOfrr495KPaEOarVTjRdRRvwQcc7SuO7WBTYocqiF2bH+uaMf2owII/E="",""clientId"":""DWR002"",""clientSecret"":""5bd6f6c874b9479fb7ed09f760faa709"",""integratingAgency"":""DWR002"",""ipAddress"":""112.196.25.180"",""transactionID"":""202408051023157035""}";
                request.AddStringBody(body, DataFormat.Json);
                RestResponse response = await client.ExecuteAsync(request);
                var responseData = response.Content;
                Console.WriteLine(response.Content);
                //return response;
            }
            catch (Exception ex)
            {

            }
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
            var challanDetail = (_repoChallanDetails.Find(x => x.deptRefNo == ResponseData.challandata.deptRefNo /*&& x.RequestStatus == "Fresh Request"*/)).FirstOrDefault();

            try
            {



                string id = challanDetail.ApplicationId;
                int istest = Convert.ToInt16(_configuration["Testing"].ToString());
                if (istest == 1)
                {
                    BankRes bank = new BankRes();
                    bank.BankRefNo = ResponseData.challandata.deptRefNo;
                    bank.CIN = "123";
                    bank.dateOfPay = DateTime.Now.ToString();
                    bank.status = "success";
                    bank.desc = "test";
                    ResponseData.challandata.bank_Res = bank;
                    ResponseData.challandata.receiptNo = ResponseData.challandata.deptRefNo;
                }
                if (ResponseData.challandata.bank_Res.status.ToLower() == "failure")
                {
                    challanDetail.RequestStatus = "Failed";
                    challanDetail.receiptNo = ResponseData.challandata.receiptNo;
                    await _repoChallanDetails.UpdateAsync(challanDetail);
                    GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == id)).FirstOrDefault();
                    var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessageWithPaymentFailure(grant.ApplicantName, grant.ApplicationID, challanDetail.deptRefNo, Convert.ToDecimal(challanDetail.totalAmt)));
                    _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");

                    return RedirectToAction("Failed", new { id = challanDetail.ApplicationId });
                }
                else if (ResponseData.challandata.bank_Res.status.ToLower() == "pending")
                {
                    challanDetail.RequestStatus = "Pending";
                    challanDetail.receiptNo = ResponseData.challandata.receiptNo;
                    await _repoChallanDetails.UpdateAsync(challanDetail);
                    GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == id)).FirstOrDefault();
                    var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessageWithPaymentPending(grant.ApplicantName, grant.ApplicationID, challanDetail.deptRefNo, Convert.ToDecimal(challanDetail.totalAmt), challanDetail.receiptNo));
                    _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");

                    return RedirectToAction("Failed", new { id = challanDetail.ApplicationId });
                }
                else if (ResponseData.challandata.bank_Res.status.ToLower() == "success")
                {
                    challanDetail.RequestStatus = "Successful";
                    challanDetail.receiptNo = ResponseData.challandata.receiptNo;
                    GrantDetails grant = ( _repo.Find(x => x.ApplicationID == id)).FirstOrDefault();
                    GrantPaymentDetails payment = new GrantPaymentDetails
                    {
                        deptRefNo = challanDetail.deptRefNo,
                        GrantID = grant.Id,
                        PayerEmail = challanDetail.emailId,
                        PayerName = challanDetail.payerName,
                        PaymentOrderId = ResponseData.challandata.receiptNo,
                        TotalAmount = Convert.ToDecimal(challanDetail.totalAmt),
                        CreatedOn = DateTime.Now

                    };
                    if (grant != null)
                    {
                        int res=await _repoPayment.CreateWithReturnAsync(payment);
                        if (res >0)
                        {
                            int ures=await _repoChallanDetails.UpdateWithReturnAsync(challanDetail);
                            if (ures >0)
                            {
                                var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessageWithPayment(grant.ApplicantName, grant.ApplicationID, challanDetail.deptRefNo, Convert.ToDecimal(challanDetail.totalAmt), challanDetail.receiptNo));
                                int r = _emailService.SendEmail2(emailModel, "Department of Water Resources, Punjab");
                                return RedirectToAction("Index", "Grant", new { Id = id });
                            }
                        }
                    }
                        //challanDetail.RequestStatus = ResponseData.challandata.bank_Res.status;
                        //challanDetail.receiptNo = ResponseData.challandata.receiptNo;
                        //await _repoChallanDetails.UpdateAsync(challanDetail);
                        //GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == id)).FirstOrDefault();
                        var emailModelF = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessageWithPaymentFailure(grant.ApplicantName, grant.ApplicationID, challanDetail.deptRefNo, Convert.ToDecimal(challanDetail.totalAmt)));
                        _emailService.SendEmail(emailModelF, "Department of Water Resources, Punjab");

                        return RedirectToAction("Failed", new { id = challanDetail.ApplicationId });
                   
                }
                else
                {
                    challanDetail.RequestStatus = ResponseData.challandata.bank_Res.status;
                    challanDetail.receiptNo = ResponseData.challandata.receiptNo;
                    await _repoChallanDetails.UpdateAsync(challanDetail);
                    GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == id)).FirstOrDefault();
                    var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessageWithPaymentFailure(grant.ApplicantName, grant.ApplicationID, challanDetail.deptRefNo, Convert.ToDecimal(challanDetail.totalAmt)));
                    _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");

                    return RedirectToAction("Failed", new { id = challanDetail.ApplicationId });
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Failed", new { id = challanDetail.ApplicationId });
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
                    challanDetail.receiptNo = ResponseData.challandata.receiptNo;
                    await _repoChallanDetails.UpdateAsync(challanDetail);
                    GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == id)).FirstOrDefault();
                    var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessageWithPaymentFailure(grant.ApplicantName, grant.ApplicationID, challanDetail.deptRefNo, Convert.ToDecimal(challanDetail.totalAmt)));
                    _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");

                    return RedirectToAction("Failed", new { id = challanDetail.ApplicationId });
                }
                else if (ResponseData.challandata.bank_Res.desc == "Pending")
                {
                    challanDetail.RequestStatus = "Pending";
                    challanDetail.receiptNo = ResponseData.challandata.receiptNo;
                    await _repoChallanDetails.UpdateAsync(challanDetail);
                    GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == id)).FirstOrDefault();
                    var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessageWithPaymentPending(grant.ApplicantName, grant.ApplicationID, challanDetail.deptRefNo, Convert.ToDecimal(challanDetail.totalAmt), challanDetail.receiptNo));
                    _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");

                    return RedirectToAction("Failed", new { id = challanDetail.ApplicationId });
                }
                else
                {
                    challanDetail.RequestStatus = ResponseData.challandata.bank_Res.status;
                    challanDetail.receiptNo = ResponseData.challandata.receiptNo;
                    await _repoChallanDetails.UpdateAsync(challanDetail);
                    GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == id)).FirstOrDefault();
                    var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessageWithPaymentFailure(grant.ApplicantName, grant.ApplicationID, challanDetail.deptRefNo, Convert.ToDecimal(challanDetail.totalAmt)));
                    _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");

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
                    if(challanDetail==null)
                    challanDetail = (await _repoChallanDetails.FindAsync(x => x.ApplicationId == id && (x.RequestStatus == "Successful"))).FirstOrDefault();

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
                            if (challanDetail.RequestStatus != "Successful")
                            {
                                challanDetail.RequestStatus = "Successful";
                                challanDetail.receiptNo = objPyment.PaymentOrderId;
                                int res = await _repoChallanDetails.UpdateWithReturnAsync(challanDetail);
                                if (res > 0)
                                {
                                    var emailModel = new EmailModel(obj.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessageWithPayment(obj.ApplicantName, obj.ApplicationID, challanDetail.deptRefNo, Convert.ToDecimal(challanDetail.totalAmt), challanDetail.receiptNo));
                                    int r = _emailService.SendEmail2(emailModel, "Department of Water Resources, Punjab");
                                    return RedirectToAction("Index", "Grant", new { Id = id });
                                }
                            }
                            else
                            {
                                //var emailModel = new EmailModel(obj.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessageWithPayment(obj.ApplicantName, obj.ApplicationID, challanDetail.deptRefNo, Convert.ToDecimal(challanDetail.totalAmt), challanDetail.receiptNo));
                                //int r = _emailService.SendEmail2(emailModel, "Department of Water Resources, Punjab");
                                return RedirectToAction("Index", "Grant", new { Id = id });
                            }
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
                string url = _configuration["IFMSPayOptions:replace"].ToString();
                var request2 = new RestRequest(url+"/TrpSarthi/SelectTel?DistCode=" + DistCode, Method.Post);
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
