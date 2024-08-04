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
        private readonly GoogleCaptchaService _googleCaptchaService;

        //private readonly IRepository<DrainDetails> _drainRepo;

        public AccountController(GoogleCaptchaService googleCaptchaService, IRepository<DivisionDetails> divisionRepository, 
            IRepository<SubDivisionDetails> subDivisionRepository, IRepository<TehsilBlockDetails> tehsilBlockRepository, /*IRepository<VillageDetails> villageRepository, */
            IEmailService emailService, IRepository<UserRoleDetails> userRolesRepository)
        {
            _divisionRepository = divisionRepository;
            _subDivisionRepository = subDivisionRepository;
            _tehsilBlockRepository = tehsilBlockRepository;
            //_villageRepository = villageRepository;
            _userRolesRepository = userRolesRepository;
            _googleCaptchaService = googleCaptchaService;
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach (var cookies in storedCookies)
            {
                Response.Cookies.Delete(cookies);
            }
            return RedirectToAction("login", "account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl)
        {
            ViewData["returnedUrl"] = ReturnUrl;
            return View();
        }

        [HttpPost]
        [Obsolete]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string ReturnUrl)
        {
            LoginRoleViewModel login = new LoginRoleViewModel();
            login.Email = model.Email; login.Password = model.Password; login.Token = model.Token;
            try
            {
                var googlereCaptchaResponse = _googleCaptchaService.VerifyreCaptcha(model.Token);
                if (!googlereCaptchaResponse.Result.success && googlereCaptchaResponse.Result.score <= 0.5)
                {
                    login.Errors = "You are not human. Please the refresh page.";
                    ModelState.AddModelError(string.Empty, login.Errors);
                    return Json(login);
                }
                if (ModelState.IsValid)
                {
                    if ((model.Email == "admin"))
                    //if (model.Email != "ExecutiveEngineer" && (model.Email == "xen" || model.Email == "jefaridkot" || model.Email == "sdofaridkot" || model.Email == "jemohali" || model.Email == "sdomohali" || model.Email == "xen2" || model.Email == "xenmohali" || model.Email == "juniorengineer" || model.Email == "sdo" || model.Email == "co" || model.Email == "dws" || model.Email == "eehq" || model.Email == "cehq" || model.Email == "ps" || model.Email == "ade" || model.Email == "dd" || model.Email == "admin"))
                    {
                        LoginResponseViewModel root = FetchUser().Find(x => x.user_info.EmailId == model.Email && model.Password == "123");
                        if (root != null)
                        {
                            List<UserRoleDetails> RoleDetail = (from r in _userRolesRepository.GetAll().AsEnumerable()
                                                                where root.user_info.Role.ToString().Contains(r.RoleName.ToString())
                                                                select new UserRoleDetails
                                                                {
                                                                    AppRoleName = r.RoleName,
                                                                    Id = r.Id,
                                                                    RoleLevel = r.RoleLevel,
                                                                    RoleName = r.RoleName
                                                                }
                                                                ).ToList();

                            login.Roles = RoleDetail;
                            login.Designation = root.user_info.Designation;
                            login.DivisionID = root.user_info.DivisionID.ToString();
                            login.DistrictID = root.user_info.DistrictID.ToString();
                            login.Email = root.user_info.EmailId;
                            login.EmpID = root.user_info.EmpID;
                            login.Success = "1";
                            login.Name = root.user_info.Name;
                            return Json(login);
                            //UserRoleDetails RoleDetail = (await _userRolesRepository.FindAsync(x => x.Id.ToString() == root.user_info.RoleID)).FirstOrDefault();
                            //if (RoleDetail != null)
                            //{
                            //    string role = RoleDetail.AppRoleName;
                            //    List<Claim> claims = new List<Claim>();
                            //    claims.Add(new Claim(ClaimTypes.NameIdentifier, model.Email));
                            //    claims.Add(new Claim(ClaimTypes.Name, model.Email));
                            //    claims.Add(new Claim(ClaimTypes.Role, role));
                            //    //claims.Add(new Claim(ClaimTypes.Role, "Dev"));
                            //    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            //    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                            //    await HttpContext.SignInAsync(principal);
                            //    HttpContext.Session.SetString("Username", root.user_info.Name);
                            //    HttpContext.Session.SetString("Designation", root.user_info.Designation);
                            //    HttpContext.Session.SetString("Userid", root.user_info.EmpID);
                            //    HttpContext.Session.SetString("Districtid", root.user_info.DistrictID.ToString());
                            //    HttpContext.Session.SetString("Divisionid", root.user_info.DivisionID.ToString());
                            //    HttpContext.Session.SetString("RoleId", root.user_info.RoleID.ToString());
                            //    HttpContext.Session.SetString("SubDivisionid", root.user_info.SubDivisionID.ToString());
                            //    HttpContext.Session.SetString("Rolename", role);
                            //    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl)) return Redirect(ReturnUrl);
                            //    else
                            //    {
                            //        if (role.ToUpper() != "JUNIOR ENGINEER" && role.ToUpper() != "SUB DIVISIONAL OFFICER")
                            //            return RedirectToAction("Index", "Home");
                            //        else
                            //            return RedirectToAction("Index", "ApprovalProcess");
                            //    }
                            //}
                        }
                    }
                    else
                    {
                        string baseUrl = "https://wrdpbind.com/api/user_login.php";
                        string salt = "8QCHhk3cJ6OMGfEW";
                        string checksum = "zUOwFCGMqKvJARC1tU6l4r24";
                        string combinedPassword = model.Email + "|" + model.Password + "|" + checksum;

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
                            login.Errors = "An error has occurred.";
                            ModelState.AddModelError(string.Empty, login.Errors);
                            return Json(login);
                        }
                        LoginResponseViewModel root = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseViewModel>(resultContent);

                        if (root.Status == "200")
                        {

                            List<UserRoleDetails> RoleDetail = (from r in _userRolesRepository.GetAll().AsEnumerable()
                                                                where root.user_info.Role.ToString().Contains(r.RoleName.ToString())
                                                                select new UserRoleDetails
                                                                {
                                                                    AppRoleName = r.RoleName,
                                                                    Id = r.Id,
                                                                    RoleLevel = r.RoleLevel,
                                                                    RoleName = r.RoleName
                                                                }
                                                                ).ToList();

                            login.Roles = RoleDetail;
                            login.Designation = root.user_info.Designation;
                            login.DivisionID = root.user_info.DivisionID.ToString();
                            login.DistrictID = root.user_info.DistrictID.ToString();
                            login.Email = root.user_info.EmailId;
                            login.EmpID = root.user_info.EmpID;
                            login.Success = "1";
                            login.Name = root.user_info.Name;
                            return Json(login);
                        }
                    }

                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Token has expired now. Please refresh the page.");
            }
            login.Errors = "Invalid Login Attempt";
            return Json(login);
        }

        //[HttpGet]
        //[Obsolete]
        //[AllowAnonymous]
        //public async Task<IActionResult> RedirecToLoginRole(LoginRoleViewModel model)
        //{
        //    try
        //    {
        //        if (model != null)
        //        { 

        //        }
        //        else
        //        {
        //            ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
        //        }
        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
        //    }
        //    return View(model);
        //}

        [HttpPost]
        [Obsolete]
        [AllowAnonymous]
        public async Task<IActionResult> RedirecToLoginRole(LoginRoleViewModel model, string ReturnUrl)
        {
            try
            {
                if (model != null)
                {
                    string role = "";
                    UserRoleDetails RoleDetail = (await _userRolesRepository.FindAsync(x => model.RoleID == x.Id.ToString())).FirstOrDefault();
                    if (RoleDetail != null)
                    {
                        role = RoleDetail.AppRoleName;
                    }
                    List<Claim> claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, model.Email));
                    claims.Add(new Claim(ClaimTypes.Name, model.Email));
                    claims.Add(new Claim(ClaimTypes.Role, role));
                    //claims.Add(new Claim(ClaimTypes.Role, "Dev"));
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(principal);
                    HttpContext.Session.SetString("Username", model.Name);
                    HttpContext.Session.SetString("Designation", model.Designation);
                    HttpContext.Session.SetString("Userid", model.EmpID);
                    HttpContext.Session.SetString("Districtid", model.DistrictID.ToString());
                    HttpContext.Session.SetString("Divisionid", model.DivisionID.ToString());
                    HttpContext.Session.SetString("RoleId", model.RoleID.ToString());
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
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);

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
            user_info1 = new user_info { Name = "Junior Engineer", Designation = "xyz", DesignationID = 1, Role = "Chief Engineer,Junior Engineer", RoleID = "10,60", DivisionID = 54, Division = "Executive Engineer Shri Muktsar Sahib Drainage-cum-Mining &amp; Geology Division, WRD, Punjab", DistrictID = 22, District = "Mukatsar", EmailId = "juniorengineer", EmpID = "123", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info2 = new user_info { Name = "Sub Divisional Officer", Designation = "xyz", DesignationID = 1, Role = "Sub Divisional Officer", RoleID = 67.ToString(), DivisionID = 54, Division = "Executive Engineer Shri Muktsar Sahib Drainage-cum-Mining &amp; Geology Division, WRD, Punjab", DistrictID = 22, District = "Mukatsar", EmailId = "sdo", EmpID = "124", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info3 = new user_info { Name = "Superintending Engineer", Designation = "xyz", DesignationID = 1, Role = "Superintending Engineer", RoleID = 8.ToString(), DivisionID = 54, Division = "Executive Engineer Shri Muktsar Sahib Drainage-cum-Mining &amp; Geology Division, WRD, Punjab", DistrictID = 22, District = "Mukatsar", EmailId = "co", EmpID = "125", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info4 = new user_info { Name = "XEN/DWS", Designation = "xyz", DesignationID = 1, Role = "XEN/DWS", RoleID = 83.ToString(), DivisionID = 54, Division = "test", DistrictID = 22, District = "Amritsar", EmailId = "dws", EmpID = "126", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info5 = new user_info { Name = "XEN HO Drainage", Designation = "xyz", DesignationID = 1, Role = "XEN HO Drainage", RoleID = 128.ToString(), DivisionID = 54, Division = "test", DistrictID = 22, District = "Amritsar", EmailId = "eehq", EmpID = "122", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info6 = new user_info { Name = "Chief Engineer", Designation = "xyz", DesignationID = 1, Role = "Chief Engineer", RoleID = 10.ToString(), DivisionID = 54, Division = "test", DistrictID = 22, District = "Amritsar", EmailId = "cehq", EmpID = "128", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info7 = new user_info { Name = "Principal Secretary", Designation = "xyz", DesignationID = 1, Role = "Principal Secretary", RoleID = 6.ToString(), DivisionID = 54, Division = "test", DistrictID = 22, District = "Amritsar", EmailId = "ps", EmpID = "129", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info8 = new user_info { Name = "ADE", Designation = "xyz", DesignationID = 1, Role = "ADE/DWS", RoleID = 90.ToString(), DivisionID = 54, Division = "test", DistrictID = 22, District = "Amritsar", EmailId = "ade", EmpID = "130", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info9 = new user_info { Name = "Director Drainage", Designation = "xyz", DesignationID = 1, Role = "Director Drainage", RoleID = 35.ToString(), DivisionID = 54, Division = "test", DistrictID = 22, District = "Amritsar", EmailId = "dd", EmpID = "131", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info10 = new user_info { Name = "Administrator", Designation = "xyz", DesignationID = 1, Role = "Administrator", RoleID = 1.ToString(), DivisionID = 63, Division = "test", DistrictID = 22, District = "Amritsar", EmailId = "admin", EmpID = "132", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info11 = new user_info { Name = "ExecutiveEngineer", Designation = "EXECUTIVE ENGINEER", DesignationID = 8, Role = "Executive Engineer", RoleID = 7.ToString(), DivisionID = 54, Division = "test", DistrictID = 22, District = "Amritsar", EmailId = "xen", EmpID = "15319", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info12 = new user_info { Name = "XEN Faridkot", Designation = "EXECUTIVE ENGINEER", DesignationID = 8, Role = "Executive Engineer", RoleID = 7.ToString(), DivisionID = 33, Division = "test", DistrictID = 29, District = "Faridkot", EmailId = "xen2", EmpID = "15320", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info13 = new user_info { Name = "XEN Mohali", Designation = "EXECUTIVE ENGINEER", DesignationID = 8, Role = "Executive Engineer", RoleID = 7.ToString(), DivisionID = 34, Division = "\"Executive Engineer SAS Nagar Drainage-cum-Mining & Geology Division, WRD Punjab\"", DistrictID = 19, District = "Mohali", EmailId = "xenmohali", EmpID = "15321", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info14 = new user_info { Name = "Junior Engineer Mohali", Designation = "xyz Mohali", DesignationID = 1, Role = "Junior Engineer", RoleID = "60", DivisionID = 34, Division = "Mohali", DistrictID = 29, District = "Mohali", EmailId = "jemohali", EmpID = "1231", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info15 = new user_info { Name = "Sub Divisional Officer Mohali", Designation = "xyz Mohali", DesignationID = 1, Role = "Sub Divisional Officer", RoleID = 67.ToString(), DivisionID = 34, Division = "test", DistrictID = 29, District = "Mohali", EmailId = "sdomohali", EmpID = "1242", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info16 = new user_info { Name = "Junior Engineer Faridkot", Designation = "xyz Faridkot", DesignationID = 1, Role = "Junior Engineer", RoleID = "60", DivisionID = 33, Division = "Faridkot", DistrictID = 29, District = "Mohali", EmailId = "jefaridkot", EmpID = "1233", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
            user_info17 = new user_info { Name = "Sub Divisional Officer Faridkot", Designation = "xyz Faridkot", DesignationID = 1, Role = "Sub Divisional Officer", RoleID = 67.ToString(), DivisionID = 33, Division = "Faridkot", DistrictID = 29, District = "Faridkot", EmailId = "sdofaridkot", EmpID = "1243", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };

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
            LoginResponseViewModel o13 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info13 };
            LoginResponseViewModel o14 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info14 };
            LoginResponseViewModel o15 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info15 };
            LoginResponseViewModel o16 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info16 };
            LoginResponseViewModel o17 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info17 };
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
            users.Add(o13);
            users.Add(o14);
            users.Add(o15);
            users.Add(o16);
            users.Add(o17);
            return users;

        }
    }
}
