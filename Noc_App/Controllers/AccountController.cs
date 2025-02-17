using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Noc_App.Helpers;
using Noc_App.Models;
using Noc_App.Models.interfaces;
using Noc_App.Models.ViewModel;
using Noc_App.UtilityService;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json;
using Noc_App.PaymentUtilities;
using System.Security.Policy;
using System;

namespace Noc_App.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //private readonly IRepository<VillageDetails> _villageRepository;
        private readonly IEmailService _emailService;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepository;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepository;
        private readonly IRepository<DivisionDetails> _divisionRepository;
        private readonly IRepository<UserRoleDetails> _userRolesRepository;
        private readonly IRepository<CircleDetails> _circleRepository;
        private readonly IRepository<CircleDivisionMapping> _circleDivRepository;
        private readonly IRepository<EstablishmentOfficeDetails> _estabOfficeRepository;
        private readonly GoogleCaptchaService _googleCaptchaService;
        private readonly PasswordEncryptionService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly IRepository<UserSessionDetails> _userSessionnRepository;
        private readonly ITokenService _tokenService;


        //private readonly IRepository<DrainDetails> _drainRepo;

        public AccountController(GoogleCaptchaService googleCaptchaService, IRepository<DivisionDetails> divisionRepository, 
            IRepository<SubDivisionDetails> subDivisionRepository, IRepository<TehsilBlockDetails> tehsilBlockRepository, /*IRepository<VillageDetails> villageRepository, */
            IEmailService emailService, IRepository<UserRoleDetails> userRolesRepository, IRepository<CircleDetails> circleRepository, IRepository<UserSessionDetails> userSessionnRepository, ITokenService tokenService
            , IRepository<CircleDivisionMapping> circleDivRepository, IRepository<EstablishmentOfficeDetails> estabOfficeRepository, PasswordEncryptionService passwordService, IConfiguration configuration)
        {
            _divisionRepository = divisionRepository;
            _subDivisionRepository = subDivisionRepository;
            _tehsilBlockRepository = tehsilBlockRepository;
            //_villageRepository = villageRepository;
            _userRolesRepository = userRolesRepository;
            _googleCaptchaService = googleCaptchaService;
            _emailService = emailService;
            _circleRepository = circleRepository;
            _circleDivRepository = circleDivRepository;
            _estabOfficeRepository = estabOfficeRepository;
            _passwordService = passwordService;
            _configuration = configuration;
            _userSessionnRepository = userSessionnRepository;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Clear all cookies
            foreach (var cookie in Request.Cookies.Keys)
            {
                // Expire each cookie by setting an expiration in the past
                Response.Cookies.Append(cookie, "", new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(-1),
                    Path = "/", // Ensure this matches the cookie's path
                    HttpOnly = true, // Ensure this matches the cookie's HttpOnly flag
                    Secure = true // Ensure this matches the cookie's Secure flag (only for HTTPS)
                });

                // Special handling for Antiforgery, Cookies, and Session cookies
                if (cookie.StartsWith(".AspNetCore.Antiforgery"))
                {
                    // Expire the Antiforgery cookie
                    Response.Cookies.Append(cookie, "", new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(-1),
                        Path = "/",
                        HttpOnly = true,
                        Secure = true
                    });
                }
                else if (cookie.StartsWith(".AspNetCore.Cookies"))
                {
                    // Expire the Authentication and Session cookies
                    Response.Cookies.Append(cookie, "", new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(-1),
                        Path = "/",
                        HttpOnly = true,
                        Secure = true
                    });
                }
                else if (cookie.StartsWith(".AspNetCore.Session"))
                {
                    // Expire the Authentication and Session cookies
                    Response.Cookies.Append(cookie, "", new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(-1),
                        Path = "/",
                        HttpOnly = true,
                        Secure = true
                    });
                }
            }
            //// Invalidate the session token
            //var token = HttpContext.Session.GetString("SessionToken");
            //if (!string.IsNullOrEmpty(token))
            //{
            //    _tokenService.InvalidateToken(token);
            //}
            // Clear session data
            HttpContext.Session.Clear();

            // Expire the session cookie explicitly (this may not be necessary, as the session cookie is tied to the session)
            Response.Cookies.Append(".AspNetCore.Session", "", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                Path = "/",
                HttpOnly = true,
                Secure = true
            });


            // Sign out the user from the authentication system
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to login page
            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl)
        {
            ViewData["returnedUrl"] = ReturnUrl;
            string token = _tokenService.GenerateToken();
            HttpContext.Session.SetString("Ses", token);

            IFMS_PaymentConfig settings = new IFMS_PaymentConfig(_configuration["IFMSPayOptions:IpAddress"],
                _configuration["IFMSPayOptions:IntegratingAgency"], _configuration["IFMSPayOptions:clientSecret"],
                _configuration["IFMSPayOptions:clientId"], _configuration["IFMSPayOptions:ChecksumKey"],
                _configuration["IFMSPayOptions:edKey"], _configuration["IFMSPayOptions:edIV"], _configuration["IFMSPayOptions:ddoCode"]
                , _configuration["IFMSPayOptions:companyName"], _configuration["IFMSPayOptions:deptCode"], _configuration["IFMSPayOptions:payLocCode"]
                , _configuration["IFMSPayOptions:trsyPaymentHead"], _configuration["IFMSPayOptions:PostUrl"], _configuration["IFMSPayOptions:headerClientId"]);

            SessionViewModel ses = new SessionViewModel();
            ses.Token=token;
            string ChecksumKey = settings.ChecksumKey;
            string edKey = settings.edKey;
            string edIV = settings.edIV;

            string json = JsonConvert.SerializeObject(ses);

            IFMS_EncrDecr obj = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
            string encData = obj.Encrypt(json);
            ViewBag.S = encData;

            return View();
        }

        [HttpPost]
        [Obsolete]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
        {
            LoginEncViewModel loginEncViewModel = new LoginEncViewModel();

            string token = _tokenService.GenerateToken();

            IFMS_PaymentConfig settings = new IFMS_PaymentConfig(_configuration["IFMSPayOptions:IpAddress"],
                 _configuration["IFMSPayOptions:IntegratingAgency"], _configuration["IFMSPayOptions:clientSecret"],
                 _configuration["IFMSPayOptions:clientId"], _configuration["IFMSPayOptions:ChecksumKey"],
                 _configuration["IFMSPayOptions:edKey"], _configuration["IFMSPayOptions:edIV"], _configuration["IFMSPayOptions:ddoCode"]
                 , _configuration["IFMSPayOptions:companyName"], _configuration["IFMSPayOptions:deptCode"], _configuration["IFMSPayOptions:payLocCode"]
                 , _configuration["IFMSPayOptions:trsyPaymentHead"], _configuration["IFMSPayOptions:PostUrl"], _configuration["IFMSPayOptions:headerClientId"]);

            IFMS_EncrDecr objEnc = new IFMS_EncrDecr(settings.ChecksumKey, settings.edKey, settings.edIV);
            

            string ChecksumKey = settings.ChecksumKey;
            string edKey = settings.edKey;
            string edIV = settings.edIV;

            SessionViewModel ses = new SessionViewModel();
            ses.Token = token;

            string json = JsonConvert.SerializeObject(ses);

            IFMS_EncrDecr obj = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
            string encData = obj.Encrypt(json);
            try
            {

                loginEncViewModel.Success = "0";
                LoginRoleViewModel login = new LoginRoleViewModel();
                string password = _passwordService.DecryptPassword(model.Password);
                var str = password.Split('|');
                password = _passwordService.DecryptPassword(str[0]);

                if (model.Email != str[1])
                {
                    HttpContext.Session.SetString("Ses", token);
                    ViewBag.S = encData;
                    loginEncViewModel.ss = encData;
                    loginEncViewModel.Errors = "Invalid login attempt.";
                    ModelState.AddModelError(string.Empty, loginEncViewModel.Errors);
                    return Json(loginEncViewModel);
                }

                login.Name = model.Email; login.Token = model.Token;
                string decodedDate = objEnc.Decrypt(str[2]);

                SessionViewModel session = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionViewModel>(decodedDate);
                string originalSession = HttpContext.Session.GetString("Ses");
                HttpContext.Session.SetString("Ses", token);

                if (session.Token != originalSession)
                {
                    //HttpContext.Session.SetString("Ses", token);
                    ViewBag.S = encData;
                    loginEncViewModel.ss = encData;
                    loginEncViewModel.Errors = "Invalid login attempt.";
                    ModelState.AddModelError(string.Empty, loginEncViewModel.Errors);
                    return Json(loginEncViewModel);
                }
                else
                {
                    var sessionDetails = _userSessionnRepository.GetAll().FirstOrDefault(x => x.Hrms == model.Email);
                    if (sessionDetails != null)
                    {
                        if (sessionDetails.LastSessionId == session.Token)
                        {
                            //HttpContext.Session.SetString("Ses", token);
                            ViewBag.S = encData;
                            loginEncViewModel.ss = encData;
                            loginEncViewModel.Errors = "Invalid login attempt.";
                            ModelState.AddModelError(string.Empty, loginEncViewModel.Errors);
                            return Json(loginEncViewModel);
                        }
                    }
                }


                var googlereCaptchaResponse = _googleCaptchaService.VerifyreCaptcha(model.Token);
                loginEncViewModel.Success = "0";
                try
                {
                    if (!googlereCaptchaResponse.Result.success && googlereCaptchaResponse.Result.score <= 0.5)
                    {
                        //HttpContext.Session.SetString("Ses", token);
                        ViewBag.S = encData;
                        loginEncViewModel.ss = encData;
                        loginEncViewModel.Errors = "You are not human. Please the refresh page.";
                        ModelState.AddModelError(string.Empty, loginEncViewModel.Errors);
                        return Json(loginEncViewModel);

                    }

                }
                catch (Exception ex)
                {
                    loginEncViewModel.Errors = ex.InnerException.InnerException.Message;// "Captcha is not allowed from the server";//"An error occured while doing login attempt";
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

                //var token = HttpContext.Session.GetString("SessionToken");
                //if (!string.IsNullOrEmpty(token))
                //{
                //    if (!_tokenService.ValidateToken(token))
                //    {
                //        loginEncViewModel.Errors = "Invalid or expired session.";
                //        ModelState.AddModelError(string.Empty, loginEncViewModel.Errors);
                //        return Json(loginEncViewModel);
                //    }
                //}

                HttpContext.Session.Clear();
                string testjson = JsonConvert.SerializeObject(model);
                //if (ModelState.IsValid)
                if (model.Email!=null && model.Token != null && model.Password != null){
                    if (model.Email.Trim() != "" && model.Token.Trim() != "" && model.Password.Trim() != "")
                    {
                        //if ((model.Email == "admin"))
                        if ((model.Email == "xen" || model.Email == "je" || model.Email == "sdo" || model.Email == "se" || model.Email == "dws" || model.Email == "xenho" || model.Email == "ce" || model.Email == "ade" || model.Email == "dd" || model.Email == "ps" || model.Email == "minister" || model.Email == "admin"))
                        {
                            LoginResponseViewModel root = FetchUser().Find(x => x.user_info.EmailId == model.Email && password == "123");
                            if (root != null)
                            {
                                var LocationRoleDetail = (from r in _userRolesRepository.GetAll().AsEnumerable()
                                                          join rr in root.user_info.OfficeWiseRoleID on r.Id equals rr.role
                                                          //join loc in _divisionRepository.GetAll() on rr.office_id equals loc.Id
                                                          //where root.user_info.Role.ToString().Contains(r.RoleName.ToString())
                                                          select new
                                                          {
                                                              Roles = r,
                                                              Location = rr
                                                          }
                                                                     ).ToList();
                                //List<string> roles = root.user_info.RoleID.Split(',').ToList();
                                //var ro = (from rol in _userRolesRepository.GetAll().AsEnumerable()
                                //          join nr in roles on rol.Id.ToString() equals nr
                                //          select new
                                //          {
                                //              roleId = rol.Id.ToString()
                                //          }).FirstOrDefault();

                                //var role = ro == null ? "0" : ro.roleId;
                                //List<UserRoleDetailsViewModel> RoleDetail = (from r in _divisionRepository.GetAll().AsEnumerable()
                                //                                             join rr in LocationRoleDetail on r.Id equals rr.Location.office_id
                                //                                             //join loc in _divisionRepository.GetAll() on rr.office_id equals loc.Id
                                //                                             //where root.user_info.Role.ToString().Contains(r.RoleName.ToString())
                                //                                             where rr.Roles.Id == Convert.ToInt32(role)
                                //                                             select new UserRoleDetailsViewModel
                                //                                             {
                                //                                                 DivisionId = r.Id,
                                //                                                 DivisionName = r.Name,
                                //                                                 AppRoleName = rr.Roles.RoleName,
                                //                                                 Id = rr.Roles.Id,
                                //                                                 RoleLevel = rr.Roles.RoleLevel,
                                //                                                 RoleName = rr.Roles.RoleName
                                //                                             }
                                //                                     ).ToList();

                                //string divisionRolePairs = string.Join(",", RoleDetail.Select(x => x.DivisionId + "-" + x.Id));

                                //login.Roles = RoleDetail.Take(1).ToList();
                                //login.Designation = root.user_info.Designation;
                                //login.DivisionID = root.user_info.DivisionID.ToString();
                                //login.DistrictID = root.user_info.DistrictID.ToString();
                                //login.EmployeeName = root.user_info.EmployeeName;
                                //login.EmpID = root.user_info.EmpID;
                                //loginEncViewModel.Success = "1";
                                //login.Name = root.user_info.Name;
                                //login.RoleID = role;
                                //login.RoleWithOffice = divisionRolePairs;
                                //login.DivisionName = RoleDetail.Take(1).FirstOrDefault().DivisionName;
                                //token = _tokenService.GenerateToken();
                                //ses.Token = token;

                                //string jsonSession = JsonConvert.SerializeObject(ses);

                                //IFMS_EncrDecr objSession = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
                                //string encDataSession = objSession.Encrypt(jsonSession);
                                //login.SessionId = encDataSession;

                                //HttpContext.Session.SetString("Ses", token);
                                //string json2 = JsonConvert.SerializeObject(login);

                                //IFMS_EncrDecr obj2 = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
                                //string encData2 = obj2.Encrypt(json2);
                                //loginEncViewModel.EncData = encData2;
                                //loginEncViewModel.Success = "1";
                                //ViewBag.S = encData;
                                //loginEncViewModel.ss = encData;
                                List<UserRoleDetailsViewModel> RoleDetail = new List<UserRoleDetailsViewModel>();
                                List<string> roles = root.user_info.RoleID.Split(',').ToList();
                                int cnt = 1;
                                foreach (var role1 in roles)
                                {
                                    var ro = (from rol in _userRolesRepository.GetAll().AsEnumerable()
                                              join nr in roles on rol.Id.ToString() equals nr
                                              where nr == role1
                                              select new
                                              {
                                                  roleId = rol.Id.ToString()
                                              }).FirstOrDefault();

                                    var role = ro == null ? "0" : ro.roleId;

                                    //foreach (string role in roles)
                                    //{
                                    //    if (role != "2")
                                    //    {
                                    if (ro == null)
                                    {
                                        if (cnt == roles.Count)
                                        {
                                            //HttpContext.Session.SetString("Ses", token);
                                            ViewBag.S = encData;
                                            loginEncViewModel.ss = encData;
                                            loginEncViewModel.Success = "0";
                                            loginEncViewModel.Errors = "Invalid user.";
                                            ModelState.AddModelError(string.Empty, loginEncViewModel.Errors);
                                            return Json(loginEncViewModel);
                                        }
                                        else
                                        {
                                            cnt++;
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (role == "8" /*|| role == "10" || role == "128" || role == "6"*/)
                                        {
                                            RoleDetail = (from rr in LocationRoleDetail
                                                          join r in _circleRepository.GetAll().AsEnumerable() on rr.Location.office_id equals r.Id
                                                          join loc in _circleDivRepository.GetAll() on rr.Location.office_id equals loc.CircleId
                                                          join div in _divisionRepository.GetAll() on loc.DivisionId equals div.Id
                                                          where rr.Roles.Id == Convert.ToInt32(role)
                                                          select new UserRoleDetailsViewModel
                                                          {
                                                              DivisionId = div.Id,
                                                              DivisionName = div.Name + " (" + r.Name + ")",
                                                              AppRoleName = rr.Roles.RoleName,
                                                              Id = rr.Roles.Id,
                                                              RoleLevel = rr.Roles.RoleLevel,
                                                              RoleName = rr.Roles.RoleName
                                                          }
                                                                         ).ToList();
                                            if (RoleDetail.Count == 0)
                                            {
                                                continue;

                                            }
                                        }
                                        else if (role == "60" || role == "67" || role == "7")
                                        {

                                            RoleDetail = (from r in _divisionRepository.GetAll().AsEnumerable()
                                                          join rr in LocationRoleDetail on r.Id equals rr.Location.office_id
                                                          where rr.Roles.Id == Convert.ToInt32(role)
                                                          select new UserRoleDetailsViewModel
                                                          {
                                                              DivisionId = r.Id,
                                                              DivisionName = r.Name,
                                                              AppRoleName = rr.Roles.RoleName,
                                                              Id = rr.Roles.Id,
                                                              RoleLevel = rr.Roles.RoleLevel,
                                                              RoleName = rr.Roles.RoleName
                                                          }
                                                                         ).ToList();
                                            if (RoleDetail.Count == 0)
                                            {
                                                continue;

                                            }
                                        }
                                        else if (role == "6" || role == "35" || role == "10" || role == "10" || role == "117" || role == "83" || role == "90" || role == "128")
                                        {
                                            RoleDetail = (from r in _estabOfficeRepository.GetAll().AsEnumerable()
                                                          join rr in LocationRoleDetail on r.Id equals rr.Location.office_id
                                                          where rr.Roles.Id == Convert.ToInt32(role)
                                                          select new UserRoleDetailsViewModel
                                                          {
                                                              DivisionId = r.Id,
                                                              DivisionName = r.Name,
                                                              AppRoleName = rr.Roles.RoleName,
                                                              Id = rr.Roles.Id,
                                                              RoleLevel = rr.Roles.RoleLevel,
                                                              RoleName = rr.Roles.RoleName
                                                          }
                                                                         ).ToList();
                                            if (RoleDetail.Count == 0)
                                            {
                                                continue;

                                            }
                                        }
                                    }

                                    //    }
                                    //}

                                    string divisionRolePairs = "";// string.Join(",", RoleDetail.Select(x => x.DivisionId + "-" + x.Id));
                                    foreach (var item in root.user_info.OfficeWiseRoleID)
                                    {
                                        if (item.role != 2)
                                        {

                                            if (divisionRolePairs != "")
                                                divisionRolePairs = divisionRolePairs + "," + item.office_id + "-" + item.role;
                                            else
                                                divisionRolePairs = item.office_id + "-" + item.role;
                                        }
                                    }


                                    login.Roles = RoleDetail.Take(1).ToList();
                                    login.Designation = root.user_info.Designation;
                                    //login.DivisionID = root.user_info.DivisionID.ToString();
                                    login.DistrictID = root.user_info.DistrictID.ToString();
                                    login.EmployeeName = root.user_info.EmployeeName;
                                    login.EmpID = root.user_info.EmpID;
                                    login.Name = root.user_info.Name;
                                    login.RoleID = role;
                                    login.DivisionName = RoleDetail.Count() > 0 ? RoleDetail.Take(1).FirstOrDefault().DivisionName : "";
                                    login.RoleWithOffice = divisionRolePairs;
                                    token = _tokenService.GenerateToken();

                                    ses.Token = token;
                                    string jsonSession = JsonConvert.SerializeObject(ses);

                                    IFMS_EncrDecr objSession = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
                                    string encDataSession = objSession.Encrypt(jsonSession);
                                    login.SessionId = encDataSession;

                                    HttpContext.Session.SetString("Ses", token);
                                    string json2 = JsonConvert.SerializeObject(login);

                                    IFMS_EncrDecr obj2 = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
                                    string encData2 = obj2.Encrypt(json2);
                                    loginEncViewModel.EncData = encData2;
                                    loginEncViewModel.Success = "1";
                                    ViewBag.S = encData;
                                    loginEncViewModel.ss = encData;
                                    break;
                                }

                                return Json(loginEncViewModel);
                            }
                        }
                        else
                        {
                            string baseUrl = "https://wrdpbind.com/api/user_login.php";
                            string salt = "8QCHhk3cJ6OMGfEW";
                            string checksum = "zUOwFCGMqKvJARC1tU6l4r24";
                            string combinedPassword = model.Email + "|" + password + "|" + checksum;

                            string plainText = combinedPassword;

                            var keyBytes = new byte[16];
                            var ivBytes = new byte[16];

                            string key = salt;
                            var keySalt = Encoding.UTF8.GetBytes(key);
                            var pdb = new Rfc2898DeriveBytes(keySalt, keySalt, 1000);

                            Array.Copy(pdb.GetBytes(16), keyBytes, 16);
                            Array.Copy(pdb.GetBytes(16), ivBytes, 16);
                            string encryptedString = NCC_encryptHelper(plainText, key, key);

                            HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = false };
                            HttpClient client = new HttpClient(handler);
                            client.BaseAddress = new Uri(baseUrl);
                            client.DefaultRequestHeaders.Accept.Clear();
                            client.DefaultRequestHeaders.Add("HASHEDDATA", encryptedString);
                            var tokenResponse1 = await client.GetAsync(client.BaseAddress.ToString());
                            string resultContent = tokenResponse1.Content.ReadAsStringAsync().Result;
                            if (resultContent.Contains("An error has occurred"))
                            {
                                //HttpContext.Session.SetString("Ses", token);
                                ViewBag.S = encData;
                                loginEncViewModel.ss = encData;
                                loginEncViewModel.Errors = "An error has occurred.";
                                ModelState.AddModelError(string.Empty, loginEncViewModel.Errors);
                                return Json(loginEncViewModel);
                            }
                            LoginResponseViewModel root = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseViewModel>(resultContent);

                            if (root.Status == "200")
                            {
                                var LocationRoleDetail = (from r in _userRolesRepository.GetAll().AsEnumerable()
                                                          join rr in root.user_info.OfficeWiseRoleID on r.Id equals rr.role
                                                          //join loc in _divisionRepository.GetAll() on rr.office_id equals loc.Id
                                                          //where root.user_info.Role.ToString().Contains(r.RoleName.ToString())
                                                          select new
                                                          {
                                                              Roles = r,
                                                              Location = rr,
                                                              //Division=loc
                                                          }
                                                                      ).ToList();
                                List<UserRoleDetailsViewModel> RoleDetail = new List<UserRoleDetailsViewModel>();
                                List<string> roles = root.user_info.RoleID.Split(',').ToList();
                                int cnt = 1;
                                foreach (var role1 in roles)
                                {
                                    var ro = (from rol in _userRolesRepository.GetAll().AsEnumerable()
                                              join nr in roles on rol.Id.ToString() equals nr
                                              where nr == role1
                                              select new
                                              {
                                                  roleId = rol.Id.ToString()
                                              }).FirstOrDefault();

                                    var role = ro == null ? "0" : ro.roleId;

                                    //foreach (string role in roles)
                                    //{
                                    //    if (role != "2")
                                    //    {
                                    if (ro == null)
                                    {
                                        if (cnt == roles.Count)
                                        {
                                            //HttpContext.Session.SetString("Ses", token);
                                            ViewBag.S = encData;
                                            loginEncViewModel.ss = encData;
                                            loginEncViewModel.Success = "0";
                                            loginEncViewModel.Errors = "Invalid user.";
                                            ModelState.AddModelError(string.Empty, loginEncViewModel.Errors);
                                            return Json(loginEncViewModel);
                                        }
                                        else
                                        {
                                            cnt++;
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (role == "8" /*|| role == "10" || role == "128" || role == "6"*/)
                                        {
                                            RoleDetail = (from rr in LocationRoleDetail
                                                          join r in _circleRepository.GetAll().AsEnumerable() on rr.Location.office_id equals r.Id
                                                          join loc in _circleDivRepository.GetAll() on rr.Location.office_id equals loc.CircleId
                                                          join div in _divisionRepository.GetAll() on loc.DivisionId equals div.Id
                                                          where rr.Roles.Id == Convert.ToInt32(role)
                                                          select new UserRoleDetailsViewModel
                                                          {
                                                              DivisionId = div.Id,
                                                              DivisionName = div.Name + " (" + r.Name + ")",
                                                              AppRoleName = rr.Roles.RoleName,
                                                              Id = rr.Roles.Id,
                                                              RoleLevel = rr.Roles.RoleLevel,
                                                              RoleName = rr.Roles.RoleName
                                                          }
                                                                         ).ToList();
                                            if (RoleDetail.Count == 0)
                                            {
                                                continue;

                                            }
                                        }
                                        else if (role == "60" || role == "67" || role == "7")
                                        {

                                            RoleDetail = (from r in _divisionRepository.GetAll().AsEnumerable()
                                                          join rr in LocationRoleDetail on r.Id equals rr.Location.office_id
                                                          where rr.Roles.Id == Convert.ToInt32(role)
                                                          select new UserRoleDetailsViewModel
                                                          {
                                                              DivisionId = r.Id,
                                                              DivisionName = r.Name,
                                                              AppRoleName = rr.Roles.RoleName,
                                                              Id = rr.Roles.Id,
                                                              RoleLevel = rr.Roles.RoleLevel,
                                                              RoleName = rr.Roles.RoleName
                                                          }
                                                                         ).ToList();
                                            if (RoleDetail.Count == 0)
                                            {
                                                continue;

                                            }
                                        }
                                        else if (role == "6" || role == "35" || role == "10" || role == "10" || role == "117" || role == "83" || role == "90" || role == "128")
                                        {
                                            RoleDetail = (from r in _estabOfficeRepository.GetAll().AsEnumerable()
                                                          join rr in LocationRoleDetail on r.Id equals rr.Location.office_id
                                                          where rr.Roles.Id == Convert.ToInt32(role)
                                                          select new UserRoleDetailsViewModel
                                                          {
                                                              DivisionId = r.Id,
                                                              DivisionName = r.Name,
                                                              AppRoleName = rr.Roles.RoleName,
                                                              Id = rr.Roles.Id,
                                                              RoleLevel = rr.Roles.RoleLevel,
                                                              RoleName = rr.Roles.RoleName
                                                          }
                                                                         ).ToList();
                                            if (RoleDetail.Count == 0)
                                            {
                                                continue;

                                            }
                                        }
                                    }

                                    //    }
                                    //}

                                    string divisionRolePairs = "";// string.Join(",", RoleDetail.Select(x => x.DivisionId + "-" + x.Id));
                                    foreach (var item in root.user_info.OfficeWiseRoleID)
                                    {
                                        if (item.role != 2)
                                        {

                                            if (divisionRolePairs != "")
                                                divisionRolePairs = divisionRolePairs + "," + item.office_id + "-" + item.role;
                                            else
                                                divisionRolePairs = item.office_id + "-" + item.role;
                                        }
                                    }


                                    login.Roles = RoleDetail.Take(1).ToList();
                                    login.Designation = root.user_info.Designation;
                                    //login.DivisionID = root.user_info.DivisionID.ToString();
                                    login.DistrictID = root.user_info.DistrictID.ToString();
                                    login.EmployeeName = root.user_info.EmployeeName;
                                    login.EmpID = root.user_info.EmpID;
                                    login.Name = root.user_info.Name;
                                    login.RoleID = role;
                                    login.DivisionName = RoleDetail.Count() > 0 ? RoleDetail.Take(1).FirstOrDefault().DivisionName : "";
                                    login.RoleWithOffice = divisionRolePairs;
                                    token = _tokenService.GenerateToken();

                                    ses.Token = token;
                                    string jsonSession = JsonConvert.SerializeObject(ses);

                                    IFMS_EncrDecr objSession = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
                                    string encDataSession = objSession.Encrypt(jsonSession);
                                    login.SessionId = encDataSession;

                                    HttpContext.Session.SetString("Ses", token);
                                    string json2 = JsonConvert.SerializeObject(login);

                                    IFMS_EncrDecr obj2 = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
                                    string encData2 = obj2.Encrypt(json2);
                                    loginEncViewModel.EncData = encData2;
                                    loginEncViewModel.Success = "1";
                                    ViewBag.S = encData;
                                    loginEncViewModel.ss = encData;
                                    break;
                                }
                                return Json(loginEncViewModel);
                            }
                        }

                        ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

                    }
                }
                loginEncViewModel.Errors = "All Fields Are Required To Fill";
                //if (ModelState.IsValid)
                //    loginEncViewModel.Errors = testjson+" "+"Invalid Login Attempt2";
                //else
                //{

                //    //var errors = ModelState
                //    //    .Where(ms => ms.Value.Errors.Count > 0) // Only fields with errors
                //    //    .ToDictionary(
                //    //        ms => ms.Key,                        // Field name
                //    //        ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToArray() // Error messages
                //    //    );

                //    //return BadRequest(new { Errors = errors }); // Return as JSON response

                //    loginEncViewModel.Errors = ModelState.Values.Where(v => v.Errors.Count > 0).SelectMany(v=>v.Errors).FirstOrDefault().ErrorMessage;
                //        //string.Join(
                //        //                            "\n",
                //        //                            ModelState.Values
                //        //                                .Where(v => v.Errors.Count > 0) // Fields with errors
                //        //                                .SelectMany(v => v.Errors)      // Select all errors
                //        //                                .Select(e => e.ErrorMessage)    // Extract error messages
                //        //                        );
                //}
            }
            catch (Exception ex)
            {
                loginEncViewModel.Errors = ex.InnerException.InnerException.Message;//"An error occured while doing login attempt";
                ModelState.AddModelError(string.Empty, ex.InnerException.InnerException.Message);
                    //"Token has expired now. Please refresh the page.");
            }
            HttpContext.Session.SetString("Ses", token);
            ViewBag.S = encData;
            loginEncViewModel.ss = encData;
            return Json(loginEncViewModel);
        }

        [HttpPost]
        [Obsolete]
        [AllowAnonymous]
        public async Task<IActionResult> RedirecToLoginRole(/*LoginRoleViewModel model*/LoginEncViewModel model2, string ReturnUrl)
        {
            try
            {

                //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                //HttpContext.Session.Clear(); // Rotate session ID

                // Set session data
                HttpContext.Session.SetString("DeviceId", DeviceIdHelper.GenerateDeviceId(HttpContext));
                HttpContext.Session.SetString("UserIp", HttpContext.Connection.RemoteIpAddress?.ToString());

                string token = HttpContext.Session.GetString("Ses");
                //HttpContext.Session.SetString("SessionToken", token);
                string tokenSession = _tokenService.GenerateToken();

                IFMS_PaymentConfig settings = new IFMS_PaymentConfig(_configuration["IFMSPayOptions:IpAddress"],
                 _configuration["IFMSPayOptions:IntegratingAgency"], _configuration["IFMSPayOptions:clientSecret"],
                 _configuration["IFMSPayOptions:clientId"], _configuration["IFMSPayOptions:ChecksumKey"],
                 _configuration["IFMSPayOptions:edKey"], _configuration["IFMSPayOptions:edIV"], _configuration["IFMSPayOptions:ddoCode"]
                 , _configuration["IFMSPayOptions:companyName"], _configuration["IFMSPayOptions:deptCode"], _configuration["IFMSPayOptions:payLocCode"]
                 , _configuration["IFMSPayOptions:trsyPaymentHead"], _configuration["IFMSPayOptions:PostUrl"], _configuration["IFMSPayOptions:headerClientId"]);

                IFMS_EncrDecr obj = new IFMS_EncrDecr(settings.ChecksumKey, settings.edKey, settings.edIV);


                string ChecksumKey = settings.ChecksumKey;
                string edKey = settings.edKey;
                string edIV = settings.edIV;

                SessionViewModel ses = new SessionViewModel();
                ses.Token = token;

                string json = JsonConvert.SerializeObject(ses);

                IFMS_EncrDecr objSes = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
                string encDataSes = objSes.Encrypt(json);

                string decodedDate = obj.Decrypt(model2.EncData);

                LoginRoleViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginRoleViewModel>(decodedDate);

                string decodedSession = obj.Decrypt(model.SessionId);

                SessionViewModel session = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionViewModel>(decodedSession);

                string originalSession = HttpContext.Session.GetString("Ses");

                if (session.Token != originalSession)
                {

                    HttpContext.Session.SetString("Ses", tokenSession);
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return RedirectToAction("Login", "Account");
                }

                if (model != null)
                {
                    // Generate a session ID and store it
                    var sessionId = HttpContext.Session.GetString("Ses");
                    DateTime loginTime = DateTime.Now;

                    var EmpID = model.EmpID; // Fetch user HRMS or unique identifier here
                    var hrms = model.Name;
                   UserSessionDetails res= _userSessionnRepository.GetAll().Where(x=>x.Hrms==hrms).FirstOrDefault();

                    if (res == null)
                    {


                        // Store session details in the database
                        var userSession = new UserSessionDetails
                        {
                            Id = Guid.NewGuid(),
                            Hrms = hrms,
                            EmpId=EmpID,
                            LastSessionId = sessionId,
                            LastLoginTime = loginTime
                        };

                        //List<UserSessionDetails> sessionDetail = _userSessionnRepository.GetAll()
                        //                                         .Where(u => u.Hrms == hrms && u.LastSessionId== sessionId)
                        //                                         .OrderByDescending(u => u.LastLoginTime)
                        //                                         .ToList();
                        //if (sessionDetail.Count > 0) { 

                        //}
                        //else
                        //{
                        // Save to the database
                        await _userSessionnRepository.CreateAsync(userSession);
                    }
                    else
                    {
                        res.LastSessionId= sessionId;
                        res.LastLoginTime= loginTime;
                        await _userSessionnRepository.UpdateAsync(res);
                    }
                    //}
                    //HttpContext.Session.SetString("SessionId", sessionId);
                    //HttpContext.Session.SetString("SessionTime", loginTime.ToString());

                    string role = "";
                    UserRoleDetails RoleDetail = (await _userRolesRepository.FindAsync(x => model.RoleID == x.Id.ToString())).FirstOrDefault();
                    if (RoleDetail != null)
                    {
                        role = RoleDetail.AppRoleName;
                    }
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, model.Name));
                    //claims.Add(new Claim(ClaimTypes.Name, model.EmployeeName+" ("+model.Name+"-"+model.Designation+")"));
                    claims.Add(new Claim(ClaimTypes.Name, model.EmployeeName + " (" + model.Name + ")"));
                    claims.Add(new Claim(ClaimTypes.Role, role));
                    claims.Add(new Claim(ClaimTypes.Surname, model.RoleWithOffice));
                    claims.Add(new Claim(ClaimTypes.Locality, model.DivisionName));
                    claims.Add(new Claim(ClaimTypes.Country, encDataSes));
                    //claims.Add(new Claim(ClaimTypes.Role, "Dev"));
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(principal);
                    HttpContext.Session.SetString("Username", model.Name);
                    HttpContext.Session.SetString("Empname", model.EmployeeName);
                    HttpContext.Session.SetString("Designation", model.Designation);
                    HttpContext.Session.SetString("Userid", model.EmpID);
                    HttpContext.Session.SetString("Districtid", model.DistrictID.ToString());
                    HttpContext.Session.SetString("Divisionid", model.Roles.FirstOrDefault().DivisionId.ToString());
                    HttpContext.Session.SetString("RoleId", model.RoleID.ToString());
                    HttpContext.Session.SetString("RoleWithOffice", model.RoleWithOffice);
                    //HttpContext.Session.SetString("SubDivisionid", root.user_info.SubDivisionID.ToString());
                    HttpContext.Session.SetString("Rolename", role);
                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl)) return Redirect(ReturnUrl);
                    else
                    {
                        if (role.ToUpper() != "JUNIOR ENGINEER" && role.ToUpper() != "SUB DIVISIONAL OFFICER")
                            return RedirectToAction("Index", "Home");
                        else
                            return RedirectToAction("Index", "ApprovalProcess");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                }
                return View(model2);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model2);

        }
        [HttpPost]
        [Obsolete]
        [AllowAnonymous]
        public async Task<IActionResult> SwitchRole(SwitchRoleSessionViewModel m)
        {
            try
            {
                //var tokenOld = HttpContext.Session.GetString("SessionToken");
                //if (!string.IsNullOrEmpty(tokenOld))
                //{
                //    if (!_tokenService.ValidateToken(tokenOld))
                //    {
                //        ModelState.AddModelError(string.Empty, "Invalid attempt to switch role");
                //        //return View(model);
                //        return RedirectToAction("Login", "Account");
                //    }
                //}

                //string token = _tokenService.GenerateToken();
                //HttpContext.Session.SetString("SessionToken", token);

                string token = HttpContext.Session.GetString("Ses");
                //HttpContext.Session.SetString("SessionToken", token);
                string tokenSession = _tokenService.GenerateToken();
                HttpContext.Session.SetString("Ses", token);
                IFMS_PaymentConfig settings = new IFMS_PaymentConfig(_configuration["IFMSPayOptions:IpAddress"],
                 _configuration["IFMSPayOptions:IntegratingAgency"], _configuration["IFMSPayOptions:clientSecret"],
                 _configuration["IFMSPayOptions:clientId"], _configuration["IFMSPayOptions:ChecksumKey"],
                 _configuration["IFMSPayOptions:edKey"], _configuration["IFMSPayOptions:edIV"], _configuration["IFMSPayOptions:ddoCode"]
                 , _configuration["IFMSPayOptions:companyName"], _configuration["IFMSPayOptions:deptCode"], _configuration["IFMSPayOptions:payLocCode"]
                 , _configuration["IFMSPayOptions:trsyPaymentHead"], _configuration["IFMSPayOptions:PostUrl"], _configuration["IFMSPayOptions:headerClientId"]);

                IFMS_EncrDecr obj = new IFMS_EncrDecr(settings.ChecksumKey, settings.edKey, settings.edIV);


                string ChecksumKey = settings.ChecksumKey;
                string edKey = settings.edKey;
                string edIV = settings.edIV;

                SessionViewModel ses = new SessionViewModel();
                ses.Token = tokenSession;

                string json = JsonConvert.SerializeObject(ses);

                IFMS_EncrDecr objSes = new IFMS_EncrDecr(ChecksumKey, edKey, edIV);
                string encDataSes = objSes.Encrypt(json);

                string decodedData = objSes.Decrypt(m.EncData);

                SwitchRoleViewModel model = Newtonsoft.Json.JsonConvert.DeserializeObject<SwitchRoleViewModel>(decodedData);

                string decodedSession = obj.Decrypt(model.SessionId);

                SessionViewModel session = Newtonsoft.Json.JsonConvert.DeserializeObject<SessionViewModel>(decodedSession);

                string originalSession = HttpContext.Session.GetString("Ses");

                if (session.Token != originalSession)
                {

                    //HttpContext.Session.SetString("Ses", tokenSession);
                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
                    return RedirectToAction("Login", "Account");
                }
                if (model != null)
                {
                    string role = "";
                    UserRoleDetails RoleDetail = (await _userRolesRepository.FindAsync(x => model.RoleID == x.Id.ToString())).FirstOrDefault();
                    if (RoleDetail != null)
                    {
                        role = RoleDetail.AppRoleName;
                    }
                    var currentUserClaims = User.Claims.ToList();

                    // Fetch the specific claim you want to update
                    var nameClaim = currentUserClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                    var roleClaim = currentUserClaims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                    var localityClaim = currentUserClaims.FirstOrDefault(c => c.Type == ClaimTypes.Locality);

                    // Remove the old claim (if necessary)
                    if (nameClaim != null)
                    {
                        currentUserClaims.Remove(nameClaim);
                    }
                    if (roleClaim != null)
                    {
                        currentUserClaims.Remove(roleClaim);
                    }
                    if (localityClaim != null)
                    {
                        currentUserClaims.Remove(localityClaim);
                    }
                    // Add updated claims
                    currentUserClaims.Add(new Claim(ClaimTypes.Name, model.Name));
                    currentUserClaims.Add(new Claim(ClaimTypes.Role, role));
                    currentUserClaims.Add(new Claim(ClaimTypes.Locality, model.Location));
                    currentUserClaims.Add(new Claim(ClaimTypes.Country, encDataSes));

                    // Create a new identity with updated claims
                    var identity = new ClaimsIdentity(currentUserClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Sign in the user again with the new identity (reissue the cookie with updated claims)
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                    //List<Claim> claims = new List<Claim>();
                    //var nameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    //claims.Add(new Claim(ClaimTypes.NameIdentifier, nameIdentifier));
                    //claims.Add(new Claim(ClaimTypes.Name, model.EmployeeName));
                    //claims.Add(new Claim(ClaimTypes.Role, role));
                    //claims.Add(new Claim(ClaimTypes.Surname, model.RoleWithOffice));
                    //claims.Add(new Claim(ClaimTypes.Locality, model.DivisionName));
                    //claims.Add(new Claim(ClaimTypes.Role, "Dev"));
                    //ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    //await HttpContext.SignInAsync(principal);
                   // HttpContext.Session.SetString("Username", model.Name);
                    //HttpContext.Session.SetString("Empname", model.EmployeeName);
                    HttpContext.Session.SetString("Designation", model.Designation);
                   // HttpContext.Session.SetString("Userid", model.EmpID);
                   // HttpContext.Session.SetString("Districtid", model.DistrictID.ToString());
                    HttpContext.Session.SetString("Divisionid", model.DivisionID.ToString());
                    HttpContext.Session.SetString("RoleId", model.RoleID.ToString());
                    //HttpContext.Session.SetString("RoleWithOffice", model.RoleWithOffice);
                    //HttpContext.Session.SetString("SubDivisionid", root.user_info.SubDivisionID.ToString());
                    HttpContext.Session.SetString("Rolename", role);
                    if (role.ToUpper() != "JUNIOR ENGINEER" && role.ToUpper() != "SUB DIVISIONAL OFFICER")
                        return RedirectToAction("Index", "Home");
                    else
                        return RedirectToAction("Index", "ApprovalProcess");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid attempt to switch role"); 
                }
                //return View(model);
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "An error occored while switching role");
            }
            //return View(model);
            return RedirectToAction("Login", "Account");
        }

        private string NCC_encryptHelper(string plainText, string key, string iv)
        {
            // Pad text and encrypt
            string paddedString = _NCC_addpadding(plainText);

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                // Encrypt the data
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(plainText);
                    byte[] encryptedData = encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length);
                    return Convert.ToBase64String(encryptedData);
                }
            }
        }

        private string _NCC_addpadding(string inputString)
        {
            // Implement your padding logic here, if required
            // In this example, we're just using PKCS7 padding
            int paddingSize = 16 - (inputString.Length % 16);
            string paddedString = inputString + new string((char)paddingSize, paddingSize);
            return paddedString;
        }

        private List<LoginResponseViewModel> FetchUser()
        {
            List<LoginResponseViewModel> users = new List<LoginResponseViewModel>();
            user_info user_info1 = new user_info();
            user_info user_info2 = new user_info();
            user_info user_info3 = new user_info();
            user_info user_info4 = new user_info();
            user_info user_info5 = new user_info();
            user_info user_info6 = new user_info();
            user_info user_info7 = new user_info();
            user_info user_info8 = new user_info();
            user_info user_info9 = new user_info();
            user_info user_info10 = new user_info();
            user_info user_info11 = new user_info();
            user_info user_info12 = new user_info();
            user_info user_info13 = new user_info();
            user_info user_info14 = new user_info();
            user_info user_info15 = new user_info();
            user_info user_info16 = new user_info();
            user_info user_info17 = new user_info();
            //user_info1 =  new user_info { OfficeWiseRoleID = new List<OfficeWiseRolesIds> { new OfficeWiseRolesIds { office_id = 54, role = 10 }, new OfficeWiseRolesIds { office_id = 33, role = 60 }}, Name = "Junior Engineer",EmployeeName="N", Designation = "xyz", DesignationID = 1, Role = "Chief Engineer,Junior Engineer", RoleID = "10,60", DivisionID = 54.ToString(), Division = "Executive Engineer Shri Muktsar Sahib Drainage-cum-Mining &amp; Geology Division, WRD, Punjab", DistrictID = 22, District = "Mukatsar", EmailId = "juniorengineer", EmpID = "123", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };

            user_info1 = new user_info
            {
                OfficeWiseRoleID = new List<OfficeWiseRolesIds>
                    {
                        new OfficeWiseRolesIds { office_id = 27, role = 60 },
                        new OfficeWiseRolesIds { office_id = 27, role = 2 }
                    },
                OfficeWiseRoleName = new List<OfficeWiseRolesNames>
                    {
                        new OfficeWiseRolesNames { office_name = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab", role_name = "Junior Engineer" },
                        new OfficeWiseRolesNames { office_name = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab", role_name = "Employee" }
                    },
                Name = "Junior Engineer",
                EmployeeName = "N",
                Designation = "xyz",
                DesignationID = 8,
                Role = "Employee,Junior Engineer",
                RoleID = "2,60",
                DivisionID = "27",
                Division = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab, Chief Engineer Bbmb Nangal, Bbmb Nangal",
                DistrictID = 20,
                District = "Rupnagar",
                EmailId = "je",
                EmpID = "124",
                MobileNo = "231221234",
                SubDivision = "",
                SubDivisionID = 0
            };
            user_info2 = new user_info
            {
                OfficeWiseRoleID = new List<OfficeWiseRolesIds>
                    {
                        new OfficeWiseRolesIds { office_id = 168, role = 3 },
                        new OfficeWiseRolesIds { office_id = 168, role = 7 },
                        new OfficeWiseRolesIds { office_id = 161, role = 3 },
                        new OfficeWiseRolesIds { office_id = 161, role = 7 },
                        new OfficeWiseRolesIds { office_id = 27, role = 3 },
                        new OfficeWiseRolesIds { office_id = 27, role = 7 },
                        new OfficeWiseRolesIds { office_id = 27, role = 2 }
                    },
                OfficeWiseRoleName = new List<OfficeWiseRolesNames>
                    {
                        new OfficeWiseRolesNames { office_name = "Bbmb Nangal", role_name = "DDO" },
                        new OfficeWiseRolesNames { office_name = "Bbmb Nangal", role_name = "Executive Engineer" },
                        new OfficeWiseRolesNames { office_name = "Chief Engineer Bbmb Nangal", role_name = "DDO" },
                        new OfficeWiseRolesNames { office_name = "Chief Engineer Bbmb Nangal", role_name = "Executive Engineer" },
                        new OfficeWiseRolesNames { office_name = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab", role_name = "DDO" },
                        new OfficeWiseRolesNames { office_name = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab", role_name = "Executive Engineer" },
                        new OfficeWiseRolesNames { office_name = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab", role_name = "Employee" }
                    },
                Name = "Executive Engineer",
                EmployeeName = "N",
                Designation = "xyz",
                DesignationID = 8,
                Role = "Employee, DDO, Executive Engineer",
                RoleID = "3,2,7",
                DivisionID = "168,161,27",
                Division = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab, Chief Engineer Bbmb Nangal, Bbmb Nangal",
                DistrictID = 20,
                District = "Rupnagar",
                EmailId = "xen",
                EmpID = "123",
                MobileNo = "231221234",
                SubDivision = "",
                SubDivisionID = 0
            };
            user_info12 = new user_info
            {
                OfficeWiseRoleID = new List<OfficeWiseRolesIds>
                    {
                        new OfficeWiseRolesIds { office_id = 27, role = 67 },
                        new OfficeWiseRolesIds { office_id = 27, role = 2 }
                    },
                OfficeWiseRoleName = new List<OfficeWiseRolesNames>
                    {
                        new OfficeWiseRolesNames { office_name = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab", role_name = "Sub Divisional Officer" },
                        new OfficeWiseRolesNames { office_name = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab", role_name = "Employee" }
                    },
                Name = "Sub Divisional Officer",
                EmployeeName = "N",
                Designation = "xyz",
                DesignationID = 8,
                Role = "Employee,  Sub Divisional Officer",
                RoleID = "2,67",
                DivisionID = "27",
                Division = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab",
                DistrictID = 20,
                District = "Rupnagar",
                EmailId = "sdo",
                EmpID = "125",
                MobileNo = "231221234",
                SubDivision = "",
                SubDivisionID = 0
            };
            user_info3 = new user_info
            {
                OfficeWiseRoleID = new List<OfficeWiseRolesIds>
                    {
                        new OfficeWiseRolesIds { office_id = 209, role = 8 },
                        new OfficeWiseRolesIds { office_id = 209, role = 2 }
                    },
                OfficeWiseRoleName = new List<OfficeWiseRolesNames>
                    {
                        new OfficeWiseRolesNames { office_name = "Superintending Engineer Ropar  Drainage-cum-Mining & Geology Circle, WRD Punjab", role_name = "Superintending Engineer" },
                        new OfficeWiseRolesNames { office_name = "Superintending Engineer Ropar  Drainage-cum-Mining & Geology Circle, WRD Punjab", role_name = "Employee" }
                    },
                Name = "Superintending Engineer",
                EmployeeName = "N",
                Designation = "xyz",
                DesignationID = 8,
                Role = "Employee, Superintending Engineer",
                RoleID = "2,8",
                DivisionID = "209",
                Division = "Superintending Engineer Ropar  Drainage-cum-Mining & Geology Circle, WRD Punjab",
                DistrictID = 20,
                District = "Rupnagar",
                EmailId = "se",
                EmpID = "126",
                MobileNo = "231221234",
                SubDivision = "",
                SubDivisionID = 0
            };
            user_info4 = new user_info
            {
                OfficeWiseRoleID = new List<OfficeWiseRolesIds>
                    {
                        new OfficeWiseRolesIds { office_id = 37, role = 83 },
                        new OfficeWiseRolesIds { office_id = 37, role = 2 }
                    },
                OfficeWiseRoleName = new List<OfficeWiseRolesNames>
                    {
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "XEN/DWS" },
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "Employee" }
                    },
                Name = "XEN/DWS",
                EmployeeName = "N",
                Designation = "xyz",
                DesignationID = 8,
                Role = "Employee, XEN/DWS",
                RoleID = "2,83",
                DivisionID = "37",
                Division = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictID = 20,
                District = "Rupnagar",
                EmailId = "dws",
                EmpID = "127",
                MobileNo = "231221234",
                SubDivision = "",
                SubDivisionID = 0
            };
            user_info5 = new user_info
            {
                OfficeWiseRoleID = new List<OfficeWiseRolesIds>
                    {
                        new OfficeWiseRolesIds { office_id = 37, role = 128 },
                        new OfficeWiseRolesIds { office_id = 37, role = 2 }
                    },
                OfficeWiseRoleName = new List<OfficeWiseRolesNames>
                    {
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "XEN HO Drainage" },
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "Employee" }
                    },
                Name = "XEN HO Drainage",
                EmployeeName = "N",
                Designation = "xyz",
                DesignationID = 8,
                Role = "Employee, XEN HO Drainage",
                RoleID = "2,128",
                DivisionID = "37",
                Division = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictID = 20,
                District = "Rupnagar",
                EmailId = "xenho",
                EmpID = "128",
                MobileNo = "231221234",
                SubDivision = "",
                SubDivisionID = 0
            };
            user_info6 = new user_info
            {
                OfficeWiseRoleID = new List<OfficeWiseRolesIds>
                    {
                        new OfficeWiseRolesIds { office_id = 37, role = 10 },
                        new OfficeWiseRolesIds { office_id = 37, role = 2 }
                    },
                OfficeWiseRoleName = new List<OfficeWiseRolesNames>
                    {
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "Chief Engineer" },
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "Employee" }
                    },
                Name = "Chief Engineer",
                EmployeeName = "N",
                Designation = "xyz",
                DesignationID = 8,
                Role = "Employee, Chief Engineer",
                RoleID = "2,10",
                DivisionID = "37",
                Division = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictID = 20,
                District = "Rupnagar",
                EmailId = "ce",
                EmpID = "129",
                MobileNo = "231221234",
                SubDivision = "",
                SubDivisionID = 0
            };
            user_info7 = new user_info
            {
                OfficeWiseRoleID = new List<OfficeWiseRolesIds>
                    {
                        new OfficeWiseRolesIds { office_id = 37, role = 90 },
                        new OfficeWiseRolesIds { office_id = 37, role = 2 }
                    },
                OfficeWiseRoleName = new List<OfficeWiseRolesNames>
                    {
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "ADE/DWS" },
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "Employee" }
                    },
                Name = "ADE",
                EmployeeName = "N",
                Designation = "xyz",
                DesignationID = 8,
                Role = "Employee, ADE/DWS",
                RoleID = "2,90",
                DivisionID = "37",
                Division = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictID = 20,
                District = "Rupnagar",
                EmailId = "ade",
                EmpID = "134",
                MobileNo = "231221234",
                SubDivision = "",
                SubDivisionID = 0
            };
            user_info8 = new user_info
            {
                OfficeWiseRoleID = new List<OfficeWiseRolesIds>
                    {
                        new OfficeWiseRolesIds { office_id = 37, role = 35 },
                        new OfficeWiseRolesIds { office_id = 37, role = 2 }
                    },
                OfficeWiseRoleName = new List<OfficeWiseRolesNames>
                    {
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "Director Drainage" },
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "Employee" }
                    },
                Name = "Director Drainage",
                EmployeeName = "N",
                Designation = "xyz",
                DesignationID = 8,
                Role = "Employee, Director Drainage",
                RoleID = "2,35",
                DivisionID = "37",
                Division = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictID = 20,
                District = "Rupnagar",
                EmailId = "dd",
                EmpID = "130",
                MobileNo = "231221234",
                SubDivision = "",
                SubDivisionID = 0
            };
            user_info9 = new user_info
            {
                OfficeWiseRoleID = new List<OfficeWiseRolesIds>
                    {
                        new OfficeWiseRolesIds { office_id = 37, role = 6 },
                        new OfficeWiseRolesIds { office_id = 37, role = 2 }
                    },
                OfficeWiseRoleName = new List<OfficeWiseRolesNames>
                    {
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "Principal Secretary" },
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "Employee" }
                    },
                Name = "Principal Secretary",
                EmployeeName = "N",
                Designation = "xyz",
                DesignationID = 8,
                Role = "Employee, Principal Secretary",
                RoleID = "2,6",
                DivisionID = "37",
                Division = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictID = 20,
                District = "Rupnagar",
                EmailId = "ps",
                EmpID = "131",
                MobileNo = "231221234",
                SubDivision = "",
                SubDivisionID = 0
            };
            user_info10 = new user_info { OfficeWiseRoleID = new List<OfficeWiseRolesIds> { new OfficeWiseRolesIds { office_id = 63, role = 1 }} ,Name = "Administrator", EmployeeName = "N", Designation = "xyz", DesignationID = 1, Role = "Administrator", RoleID = 1.ToString(), DivisionID = 63.ToString(), Division = "test", DistrictID = 22, District = "Amritsar", EmailId = "admin", EmpID = "132", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info11 = new user_info
            {
                OfficeWiseRoleID = new List<OfficeWiseRolesIds>
                    {
                        new OfficeWiseRolesIds { office_id = 37, role = 114 },
                        new OfficeWiseRolesIds { office_id = 37, role = 2 }
                    },
                OfficeWiseRoleName = new List<OfficeWiseRolesNames>
                    {
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "Minister(WR)" },
                        new OfficeWiseRolesNames { office_name = "CHIEF ENGINEER DESIGN CHANDIGARH", role_name = "Employee" }
                    },
                Name = "Minister",
                EmployeeName = "N",
                Designation = "xyz",
                DesignationID = 8,
                Role = "Employee, Minister(WR)",
                RoleID = "2,114",
                DivisionID = "37",
                Division = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictID = 20,
                District = "Rupnagar",
                EmailId = "minister",
                EmpID = "133",
                MobileNo = "231221234",
                SubDivision = "",
                SubDivisionID = 0
            };
            //user_info11 = new user_info { OfficeWiseRoleID = new List<OfficeWiseRolesIds> { new OfficeWiseRolesIds { office_id=54,role=7}} ,Name = "ExecutiveEngineer", EmployeeName = "N", Designation = "EXECUTIVE ENGINEER", DesignationID = 8, Role = "Executive Engineer", RoleID = 7.ToString(), DivisionID = 54.ToString(), Division = "test", DistrictID = 22, District = "Amritsar", EmailId = "xen", EmpID = "15319", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            //user_info12 = new user_info { OfficeWiseRoleID = new List<OfficeWiseRolesIds> { new OfficeWiseRolesIds { office_id=33,role=7}} ,Name = "XEN Faridkot", EmployeeName = "N", Designation = "EXECUTIVE ENGINEER", DesignationID = 8, Role = "Executive Engineer", RoleID = 7.ToString(), DivisionID = 33.ToString(), Division = "test", DistrictID = 29, District = "Faridkot", EmailId = "xen2", EmpID = "15320", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            //user_info13 = new user_info { OfficeWiseRoleID = new List<OfficeWiseRolesIds> { new OfficeWiseRolesIds { office_id=34,role=7}} ,Name = "XEN Mohali", EmployeeName = "N", Designation = "EXECUTIVE ENGINEER", DesignationID = 8, Role = "Executive Engineer", RoleID = 7.ToString(), DivisionID = 34.ToString(), Division = "\"Executive Engineer SAS Nagar Drainage-cum-Mining & Geology Division, WRD Punjab\"", DistrictID = 19, District = "Mohali", EmailId = "xenmohali", EmpID = "15321", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            //user_info14 = new user_info { OfficeWiseRoleID = new List<OfficeWiseRolesIds> { new OfficeWiseRolesIds { office_id=34,role=60}} ,Name = "Junior Engineer Mohali", EmployeeName = "N", Designation = "xyz Mohali", DesignationID = 1, Role = "Junior Engineer", RoleID = "60", DivisionID = 34.ToString(), Division = "Mohali", DistrictID = 29, District = "Mohali", EmailId = "jemohali", EmpID = "1231", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            //user_info15 = new user_info { OfficeWiseRoleID = new List<OfficeWiseRolesIds> { new OfficeWiseRolesIds { office_id=34,role=67}} ,Name = "Sub Divisional Officer Mohali", EmployeeName = "N", Designation = "xyz Mohali", DesignationID = 1, Role = "Sub Divisional Officer", RoleID = 67.ToString(), DivisionID = 34.ToString(), Division = "test", DistrictID = 29, District = "Mohali", EmailId = "sdomohali", EmpID = "1242", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            //user_info16 = new user_info { OfficeWiseRoleID = new List<OfficeWiseRolesIds> { new OfficeWiseRolesIds { office_id=33,role=60}} ,Name = "Junior Engineer Faridkot", EmployeeName = "N", Designation = "xyz Faridkot", DesignationID = 1, Role = "Junior Engineer", RoleID = "60", DivisionID = 33.ToString(), Division = "Faridkot", DistrictID = 29, District = "Mohali", EmailId = "jefaridkot", EmpID = "1233", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            //user_info17 = new user_info { OfficeWiseRoleID = new List<OfficeWiseRolesIds> { new OfficeWiseRolesIds { office_id = 33, role = 67 }, new OfficeWiseRolesIds { office_id = 63, role = 60 }}, Name = "Sub Divisional Officer Faridkot", EmployeeName = "N", Designation = "xyz Faridkot", DesignationID = 1, Role = "Sub Divisional Officer", RoleID = 67.ToString(), DivisionID = 33.ToString(), Division = "Faridkot", DistrictID = 29, District = "Faridkot", EmailId = "sdofaridkot", EmpID = "1243", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };

            LoginResponseViewModel o1 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info1 };
            LoginResponseViewModel o2 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info2 };
            LoginResponseViewModel o3 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info3 };
            LoginResponseViewModel o4 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info4 };
            LoginResponseViewModel o5 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info5 };
            LoginResponseViewModel o6 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info6 };
            LoginResponseViewModel o7 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info7 };
            LoginResponseViewModel o8 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info8 };
            LoginResponseViewModel o9 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info9 };
            LoginResponseViewModel o10 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info10 };
            LoginResponseViewModel o11 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info11 };
            LoginResponseViewModel o12 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info12 };
            //LoginResponseViewModel o13 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info13 };
            //LoginResponseViewModel o14 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info14 };
            //LoginResponseViewModel o15 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info15 };
            //LoginResponseViewModel o16 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info16 };
            //LoginResponseViewModel o17 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info17 };
            users.Add(o1);
            users.Add(o2);
            users.Add(o3);
            users.Add(o4);
            users.Add(o5);
            users.Add(o6);
            users.Add(o7);
            users.Add(o8);
            users.Add(o9);
            users.Add(o10);
            users.Add(o11);
            users.Add(o12);
            //users.Add(o13);
            //users.Add(o14);
            //users.Add(o15);
            //users.Add(o16);
            //users.Add(o17);
            return users;

        }
    }
}
