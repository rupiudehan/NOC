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

namespace Noc_App.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IRepository<VillageDetails> _villageRepository;
        private readonly IEmailService _emailService;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepository;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepository;
        private readonly IRepository<DivisionDetails> _divisionRepository;
        private readonly IRepository<UserRoleDetails> _userRolesRepository;
        private readonly GoogleCaptchaService _googleCaptchaService;

        //private readonly IRepository<DrainDetails> _drainRepo;

        public AccountController(GoogleCaptchaService googleCaptchaService, IRepository<DivisionDetails> divisionRepository, 
            IRepository<SubDivisionDetails> subDivisionRepository, IRepository<TehsilBlockDetails> tehsilBlockRepository, IRepository<VillageDetails> villageRepository, 
            IEmailService emailService, IRepository<UserRoleDetails> userRolesRepository)
        {
            _divisionRepository = divisionRepository;
            _subDivisionRepository = subDivisionRepository;
            _tehsilBlockRepository = tehsilBlockRepository;
            _villageRepository = villageRepository;
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
            try
            {
                var googlereCaptchaResponse = _googleCaptchaService.VerifyreCaptcha(model.Token);
                if (!googlereCaptchaResponse.Result.success && googlereCaptchaResponse.Result.score <= 0.5)
                {
                    ModelState.AddModelError(string.Empty, "You are not human");
                    return View(model);
                }
                if (ModelState.IsValid)
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
                    LoginResponseViewModel root = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseViewModel>(resultContent);

                    if (root.Status == "200")
                    {
                        UserRoleDetails RoleDetail = (await _userRolesRepository.FindAsync(x => x.Id == root.user_info.RoleID)).FirstOrDefault();
                        if (RoleDetail != null)
                        {
                            string role = RoleDetail.AppRoleName;
                            List<Claim> claims = new List<Claim>();
                            claims.Add(new Claim(ClaimTypes.NameIdentifier, model.Email));
                            claims.Add(new Claim(ClaimTypes.Name, model.Email));
                            claims.Add(new Claim(ClaimTypes.Role, role));
                            //claims.Add(new Claim(ClaimTypes.Role, "Dev"));
                            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                            await HttpContext.SignInAsync(principal);
                            HttpContext.Session.SetString("Username", root.user_info.Name);
                            HttpContext.Session.SetString("Designation", root.user_info.Designation);
                            HttpContext.Session.SetString("Userid", root.user_info.EmpID);
                            HttpContext.Session.SetString("Districtid", root.user_info.DistrictID.ToString());
                            HttpContext.Session.SetString("Divisionid", root.user_info.DivisionID.ToString());
                            HttpContext.Session.SetString("RoleId", root.user_info.RoleID.ToString());
                            HttpContext.Session.SetString("SubDivisionid", root.user_info.SubDivisionID.ToString());
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
                    }

                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Invalid Token");
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
    }
}
