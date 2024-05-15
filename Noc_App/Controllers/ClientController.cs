using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Noc_App.PaymentUtilities;
using static Noc_App.Models.IFMSPayment.DepartmentModel;
using System.Text;
using Noc_App.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace Noc_App.Controllers
{
    [AllowAnonymous]
    public class ClientController : Controller
    {
        private readonly IConfiguration _configuration;
        public ClientController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [AllowAnonymous]
        [Obsolete]
        [HttpGet]
        public ActionResult ChallanView()
        {
            IFMS_PaymentConfig _settings = new IFMS_PaymentConfig(_configuration["IFMSPayOptions:IpAddress"],
                    _configuration["IFMSPayOptions:IntegratingAgency"], _configuration["IFMSPayOptions:clientSecret"],
                    _configuration["IFMSPayOptions:clientId"], _configuration["IFMSPayOptions:ChecksumKey"],
                    _configuration["IFMSPayOptions:edKey"], _configuration["IFMSPayOptions:edIV"], _configuration["IFMSPayOptions:ddoCode"]
                    , _configuration["IFMSPayOptions:companyName"], _configuration["IFMSPayOptions:deptCode"], _configuration["IFMSPayOptions:payLocCode"]
                    , _configuration["IFMSPayOptions:trsyPaymentHead"], _configuration["IFMSPayOptions:PostUrl"], _configuration["IFMSPayOptions:headerClientId"]);
            DepartmentDl dpt = new DepartmentDl();

            IFMS_EncrDecr obj = new IFMS_EncrDecr(_configuration["IFMSPayOptions:ChecksumKey"],
                    _configuration["IFMSPayOptions:edKey"], _configuration["IFMSPayOptions:edIV"]);
            ifms_data data = new ifms_data();
            data.challandata = dpt.FillChallanData();
            string json = JsonConvert.SerializeObject(data.challandata);
            data.chcksum = obj.CheckSum(json);
            string jsonCHK = JsonConvert.SerializeObject(data);
            string encData = obj.Encrypt(jsonCHK);
            IFMSHeader cHeader = new IFMSHeader();
            cHeader.clientId = _settings.headerClientId;
            cHeader.clientSecret = _settings.clientSecret;
            cHeader.transactionID = new Random().Next(100000, 999999).ToString();
            cHeader.ipAddress = _settings.IpAddress;
            cHeader.integratingAgency = _settings.IntegratingAgency;
            string d = PreparePOSTForm("ChallanForm", encData, cHeader);
            ViewBag.HtmlStr = d;
            hrmsView pay = new hrmsView();
            pay.encdata = encData;
            return View();
        }
        [AllowAnonymous]
        private string PreparePOSTForm(string url, string encData, IFMSHeader cHeader)
        // post form
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><head>");
            sb.AppendFormat(@"<meta name='ContentType' content='application/json' />");
            sb.AppendFormat(@"<meta name='Accept' content='application/json' />");
            sb.AppendFormat(@"</head>");
            string formID = "PostForm";
            sb.Append(@"<body><form id=\"""+ formID+"\" name=\"" + formID + "\" action =\"" + url + "\" method=\"POST\">").Replace("id=\\\"PostForm\"", "id =\"PostForm\"");
            sb.AppendFormat("<input type='hidden' name='encData' value='{0}'/>",
           encData);
            sb.AppendFormat("<input type='hidden' name='clientId' value='{0}'/>",
           cHeader.clientId);
            sb.AppendFormat("<input type='hidden' name='clientSecret' value='{0}'/>",
           cHeader.clientSecret);
            sb.AppendFormat("<input type='hidden' name='integratingAgency' value = '{0}' /> ", cHeader.integratingAgency);
           
            sb.AppendFormat("<input type='hidden' name='ipAddress' value='{0}'/>",
           cHeader.ipAddress);
            sb.AppendFormat("<input type='hidden' name='transactionID' value='{0}'/>",
           cHeader.transactionID);
            //sb.AppendFormat("</form></body></html>");
            //StringBuilder strScript = new StringBuilder();
            //strScript.Append("<script language='javascript'>");
            //strScript.Append("var v" + formID + " = document." + formID + ";");
            //strScript.Append("v" + formID + ".submit();");
            //strScript.Append("</script>");
            sb.AppendFormat("</form>");
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." + formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            sb.AppendFormat(strScript.ToString());
            sb.AppendFormat("</body></html>");
            //return sb.ToString() + strScript.ToString();
            return sb.ToString();
        }
        [HttpPost]
        public ActionResult succesfulURL([FromForm] Checkdata model)
        {
            //IFMS_EncrDecr obj = new IFMS_EncrDecr();
            //sendClient data = new sendClient();
            //string encData = obj.Decrypt(model.data);
            //sendClient reqData = JsonConvert.DeserializeObject<sendClient>(encData);
            ViewBag.paymentid = "";
            ViewBag.amount = "";
            ViewBag.status = "success";
            return View();
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
    }
}
