using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Noc_App.Helpers;
using Noc_App.Models;
using Noc_App.Models.interfaces;
using Noc_App.Models.ViewModel;
using Noc_App.UtilityService;
using System.Data;
using System.Linq;

namespace Noc_App.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<UserDivision> _userDivisionRepository;
        private readonly IRepository<UserSubdivision> _userSubDivisionRepository;
        private readonly IRepository<UserTehsil> _userTehsilRepository;
        private readonly IRepository<UserVillage> _userVillageRepository;
        private readonly IRepository<VillageDetails> _villageRepository;
        private readonly IEmailService _emailService;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepository;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepository;
        private readonly IRepository<DivisionDetails> _divisionRepository;
        private readonly GoogleCaptchaService _googleCaptchaService;

        //private readonly IRepository<DrainDetails> _drainRepo;

        public AccountController(GoogleCaptchaService googleCaptchaService, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, IRepository<UserDivision> userDivisionRepository, IRepository<UserSubdivision> userSubDivisionRepository, IRepository<UserTehsil> userTehsilRepository, IRepository<UserVillage> userVillageRepository, RoleManager<IdentityRole> roleManager, IRepository<DivisionDetails> divisionRepository, IRepository<SubDivisionDetails> subDivisionRepository, IRepository<TehsilBlockDetails> tehsilBlockRepository, IRepository<VillageDetails> villageRepository, IEmailService emailService/*, IRepository<DrainDetails> drainRepo*/)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _divisionRepository = divisionRepository;
            _subDivisionRepository = subDivisionRepository;
            _tehsilBlockRepository = tehsilBlockRepository;
            _villageRepository = villageRepository;
            _emailService = emailService;
            _roleManager = roleManager;
            _userDivisionRepository = userDivisionRepository;
            _userSubDivisionRepository = userSubDivisionRepository;
            _userTehsilRepository=userTehsilRepository;
            _userVillageRepository = userVillageRepository;
            //_drainRepo = drainRepo;
            //_grantRepo = grantRepo;
            _googleCaptchaService = googleCaptchaService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(ModelState.IsValid)
            {
                if (!await _roleManager.RoleExistsAsync("Administrator"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Administrator"));
                }
                var user = new ApplicationUser { UserName=registerViewModel.Email,Email=registerViewModel.Email };
                user.EmailConfirmed = true;
                var result = await userManager.CreateAsync(user,registerViewModel.Password);
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Administrator");
                    //userManager.AddToRoleAsync(user, "User").Wait();
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index","home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("login", "account");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
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
                    var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return Redirect(returnUrl);
                        else return RedirectToAction("index", "home");
                    }


                    ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

                }
            }
            catch (Exception ex)
            {

            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterEmployee()
        {
            var role = (from r in _roleManager.Roles.AsEnumerable()
                        where r.Name!= "Administrator"
                        select new
                       {
                           Name=r.Name
                       }).ToList();
            var viewModel = new RegisterEmployeeViewModel
            {
                //Village = new SelectList(Enumerable.Empty<VillageDetails>(), "Id", "Name"),
                //TehsilBlock = new SelectList(Enumerable.Empty<TehsilBlockDetails>(), "Id", "Name"),
                //SubDivision = new SelectList(Enumerable.Empty<SubDivisionDetails>(), "Id", "Name"),
                //Divisions1 =  new SelectList(_divisionRepository.GetAll(), "Id", "Name"),
                Divisions = _divisionRepository.GetAll(),// new SelectList(_divisionRepository.GetAll(), "Id", "Name"),
                Roles = new SelectList(role, "Name", "Name"),
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployee(RegisterEmployeeViewModel registerViewModel)
        {
            var divisions = _divisionRepository.GetAll();
            var selectedDivisionIds = registerViewModel.SelectedDivisionId;
            var selectedSubdivisionIds = registerViewModel.SelectedSubDivisionId;
            var selectedTehsilIds = registerViewModel.SelectedTehsilBlockId;
            var selectedVillageIds = registerViewModel.SelectedVillageId;
            //var filteredSubdivisions = _subDivisionRepository.GetAll().Where(c => registerViewModel.SelectedDivisionId.Contains(c.DivisionId) ).ToList();
            //var filteredtehsilBlock = _tehsilBlockRepository.GetAll().Where(c => c.SubDivisionId == registerViewModel.SelectedSubDivisionId).ToList();
            //var fileteedvillage = _villageRepository.GetAll().Where(c => c.TehsilBlockId == registerViewModel.SelectedTehsilBlockId).ToList();
            var role = (from r in _roleManager.Roles.AsEnumerable()
                        where r.Name != "Administrator"
                        select new
                        {
                            Name = r.Name
                        }).ToList();
            
            var filteredSubdivisions = _subDivisionRepository.GetAll().Where(c => selectedDivisionIds.Contains(c.DivisionId)).ToList();
            var filteredTehsil = _tehsilBlockRepository.GetAll().Where(c => selectedSubdivisionIds.Contains(c.SubDivisionId)).ToList();
            var filteredVillages = _villageRepository.GetAll().Where(c => selectedTehsilIds.Contains(c.TehsilBlockId)).ToList();
            var viewModel = new RegisterEmployeeViewModel
            {
                Village = filteredVillages,
                TehsilBlock = filteredTehsil,
                SubDivision = filteredSubdivisions,
                Divisions = divisions,// new SelectList(divisions, "Id", "Name"),
                Roles = new SelectList(role, "Name", "Name"),
            };

            if (((registerViewModel.SelectedVillageId != null && registerViewModel.SelectedVillageId.Count() > 0) || (registerViewModel.SelectedTehsilBlockId!=null && registerViewModel.SelectedTehsilBlockId.Count()>0)  || registerViewModel.SelectedSubDivisionId!=null && (registerViewModel.SelectedSubDivisionId.Count()>0) || registerViewModel.SelectedDivisionId.Count() >0) && registerViewModel.Email!=null && registerViewModel.SelectedRole!=null)
            {
                var user = new ApplicationUser { 
                    UserName = registerViewModel.Email, 
                    Email = registerViewModel.Email
                    //VillageId = registerViewModel.SelectedVillageId,
                    //TehsilBlockId = registerViewModel.SelectedTehsilBlockId,
                    //SubDivisionId = registerViewModel.SelectedSubDivisionId,
                    //DivisionId = registerViewModel.SelectedDivisionId
                };
                user.EmailConfirmed = true;
                string password = "Abc@123";
                var result = await userManager.CreateAsync(user, password);

                
                if (result.Succeeded)
                {
                    //List<LocationUserMapping> list = new List<LocationUserMapping>();
                    if (registerViewModel.SelectedVillageId!=null && registerViewModel.SelectedVillageId.Count > 0)
                    {
                        List<UserVillage> list = new List<UserVillage>();
                        foreach (var item in registerViewModel.SelectedVillageId)
                        {
                            var village = await _villageRepository.GetByIdAsync(item);
                            if (village != null)
                            {
                                list.Add(new UserVillage { User = user, Village = village });
                                
                            }
                        }
                        user.UserVillages = list;
                    }
                    else if (registerViewModel.SelectedTehsilBlockId!=null && registerViewModel.SelectedTehsilBlockId.Count > 0)
                    {
                        List<UserTehsil> list = new List<UserTehsil>();
                        foreach (var tehsilid in registerViewModel.SelectedTehsilBlockId)
                        {
                            var tehsil = await _tehsilBlockRepository.GetByIdAsync(tehsilid);
                            if (tehsil != null)
                            {
                                list.Add(new UserTehsil { User = user, Tehsil = tehsil });
                            }
                        }
                        user.UserTehsils=list;
                    }
                    else if (registerViewModel.SelectedSubDivisionId!=null && registerViewModel.SelectedSubDivisionId.Count > 0)
                    {
                        List<UserSubdivision> list = new List<UserSubdivision>();
                        foreach (var subdivisionID in registerViewModel.SelectedSubDivisionId)
                        {
                            var subdivision = await _subDivisionRepository.GetByIdAsync(subdivisionID);
                            if (subdivision != null)
                            {
                                list.Add(new UserSubdivision { User = user, Subdivision = subdivision });
                            }
                        }
                        user.UserSubdivisions = list;
                    }

                    else if (registerViewModel.SelectedDivisionId!=null && registerViewModel.SelectedDivisionId.Count > 0)
                    {
                        List<UserDivision> list = new List<UserDivision>();
                        foreach (var divisionID in registerViewModel.SelectedDivisionId)
                        {
                            var division = await _divisionRepository.GetByIdAsync(divisionID);
                            if (division != null)
                            {
                                list.Add(new UserDivision { User = user, Division = division });
                            }
                        }
                        user.UserDivisions = list;
                    }
                    
                    // Add user to the specified role
                    await this.userManager.AddToRoleAsync(user, registerViewModel.SelectedRole);
                    // Update user's navigation properties

                    //registerViewModel.Divisions = _tehsilBlockRepository.GetAll().Where(c => subdivisionIds.Contains(c.SubDivisionId)).ToList();
                    //new SelectList(_divisionRepo.GetAll(), "Id", "Name");


                    //Rupinder Start

                    //user.Village = village;
                    //user.TehsilBlock = tehsilBlock;
                    //user.SubDivision = subDivision;
                    //user.Division = division;

                    //RupinderEnd

                    // Save changes
                    await this.userManager.UpdateAsync(user);

                    var emailModel = new EmailModel(user.Email, "Login Credentials", EmailBody.EmailStringBodyForInformation(user.Email, password));
                    _emailService.SendEmail(emailModel, "Punjab Irrigation Department");

                    //userManager.AddToRoleAsync(user, "User").Wait();
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("List", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(viewModel);
            }
            else
            {
                ModelState.AddModelError("", $"EmailID and atleast on field is required out of division/subdivision/tehsil/block/village");
            }
            return View(viewModel);
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailExists(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailUnique(string email,string id)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user != null && user.Id!=id)
            {
                return Json($"Email {email} is already in use");
            }
            else
            {
                return Json(true);
            }
        }

        public IActionResult GetDivisions()
        {
            var divisions = _divisionRepository.GetAll()
                .Select(d => new DivisionViewModelEdit
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();
            return Json(divisions);
        }

        [HttpPost]
        public IActionResult GetSubDivisions(/*int divisionId*/ List<int> divisionIds)
        {
            var subDivision = _subDivisionRepository.GetAll();
            var filteredSubdivisions = subDivision.Where(c => divisionIds.Contains(c.DivisionId)).ToList();
            return Json(new SelectList(filteredSubdivisions, "Id", "Name"));
        }

        [HttpPost]
        public IActionResult GetTehsilBlocks(/*int subDivisionId*/ List<int> subdivisionIds)
        {
            var tehsilBlock = _tehsilBlockRepository.GetAll().Where(c => subdivisionIds.Contains(c.SubDivisionId)).ToList();
            return Json(new SelectList(tehsilBlock, "Id", "Name"));
        }

        [HttpPost]
        public IActionResult GetVillagess(/*int tehsilBlockId*/ List<int> tehsilIds)
        {
            var village = _villageRepository.GetAll().Where(c => tehsilIds.Contains(c.TehsilBlockId)).ToList();
            return Json(new SelectList(village, "Id", "Name"));
        }

        [HttpGet]
        public async Task<ViewResult> List()
        {
            try
            {
                var list = this.userManager.Users.Include(x=>x.UserDivisions).Include(x => x.UserSubdivisions).Include(x => x.UserTehsils).Include(x => x.UserVillages).ToList();
                var usersWithRoles = new List<UserWithRolesViewModel>();
                foreach (var user in list)
                {

                    var roles =  userManager.GetRolesAsync(user).Result;
                    user.UserDivisions = (await _userDivisionRepository.FindAsync(x => x.UserId == user.Id)).ToList();
                    user.UserSubdivisions = (await _userSubDivisionRepository.FindAsync(x => x.UserId == user.Id)).ToList();
                    user.UserTehsils = (await _userTehsilRepository.FindAsync(x => x.UserId == user.Id)).ToList();
                    user.UserVillages = (await _userVillageRepository.FindAsync(x => x.UserId == user.Id)).ToList();

                    foreach (var location in user.UserDivisions)
                    {
                        location.Division = await _divisionRepository.GetByIdAsync(location.DivisionId);

                    }

                    foreach (var location in user.UserSubdivisions)
                    {
                        location.Subdivision = await _subDivisionRepository.GetByIdAsync(location.SubdivisionId);

                    }

                    foreach (var location in user.UserTehsils)
                    {
                        location.Tehsil = await _tehsilBlockRepository.GetByIdAsync(location.TehsilId);
                    }
                    foreach (var location in user.UserVillages)
                    {
                        location.Village = await _villageRepository.GetByIdAsync(location.VillageId);
                    }
                    //user.LocatioinUserMapping=_locationUserRepository.GetAll().Where(x=>x.UserID==user.Id).ToList();

                    //foreach (var location in user.LocatioinUserMapping)
                    //{

                    //    location.Division = await _divisionRepository.GetByIdAsync(location.DivisionID ?? 0);
                    //    if (location.SubDivisionID != 0)
                    //        location.SubDivision = await _subDivisionRepository.GetByIdAsync(location.SubDivisionID ?? 0);
                    //    if (location.TehsilBlockID != 0)
                    //        location.TehsilBlock = await _tehsilBlockRepository.GetByIdAsync(location.TehsilBlockID ?? 0);
                    //    if (location.VillageID != 0)
                    //        location.Village = await _villageRepository.GetByIdAsync(location.VillageID ?? 0);
                    //}
                    
                    //if (user.Divisions.Count()>0)
                    //{

                    //    var subdivisionids = user.LocatioinUserMapping.Select(x => x.SubDivisionID).ToList();
                    //    user.SubDivisions = (await _subDivisionRepository.FindAsync(x => subdivisionids.Contains(x.Id))).ToList();
                    //    if (user.SubDivisions.Count>0)
                    //    {
                    //        var tehsilids = user.LocatioinUserMapping.Select(x => x.TehsilBlockID).ToList();
                    //        user.TehsilBlocks = (await _tehsilBlockRepository.FindAsync(x => tehsilids.Contains(x.Id))).ToList();
                    //        if (user.TehsilBlocks.Count()>0)
                    //        {
                    //            var villageids = user.LocatioinUserMapping.Select(x => x.VillageID).ToList();
                    //            user.Villages = (await _villageRepository.FindAsync(x => villageids.Contains(x.Id))).ToList();
                    //        }
                    //    }
                    //}

                    //Rupinder End

                    var userWithRoles = new UserWithRolesViewModel
                    {
                        User = user,
                        Roles = new List<string>(roles)
                    };

                    usersWithRoles.Add(userWithRoles);
                }
                //_repo.GetAll().Include(x => x.TehsilBlock).ThenInclude(x => x.SubDivision).ThenInclude(x => x.Division);

                return View(usersWithRoles);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        [HttpGet]
        public async Task<ViewResult> Edit(string Id)
        {
            try
            {
                var user = this.userManager.Users.FirstOrDefault(x => x.Id == Id);
                var roles = userManager.GetRolesAsync(user).Result;

                //Rupinder Start
                //user.LocatioinUserMapping = _locationUserRepository.GetAll().Where(x => x.UserID == user.Id).ToList();
                user.UserVillages = (await _userVillageRepository.FindAsync(x=>x.UserId==user.Id)).ToList();
                user.UserTehsils = (await _userTehsilRepository.FindAsync(x => x.UserId == user.Id)).ToList();
                user.UserSubdivisions = (await _userSubDivisionRepository.FindAsync(x => x.UserId == user.Id)).ToList();
                user.UserDivisions = (await _userDivisionRepository.FindAsync(x => x.UserId == user.Id)).ToList();
                
                List<int> DivisionIds = new List<int>();
                List<int> SubDivisionIds = new List<int>();
                List<int> TehsilIds = new List<int>();
                List<int> VillageIds = new List<int>();

                foreach (var location in user.UserDivisions)
                {
                    DivisionIds.Add(location.DivisionId);
                    location.Division = await _divisionRepository.GetByIdAsync(location.DivisionId);
                }

                foreach (var location in user.UserSubdivisions)
                {
                        SubDivisionIds.Add(location.SubdivisionId);
                        location.Subdivision = await _subDivisionRepository.GetByIdAsync(location.SubdivisionId);
                    
                }

                foreach (var location in user.UserTehsils)
                {
                        TehsilIds.Add(location.TehsilId);
                        location.Tehsil = await _tehsilBlockRepository.GetByIdAsync(location.TehsilId);
                }
                foreach (var location in user.UserVillages)
                {
                        VillageIds.Add(location.VillageId);
                        location.Village = await _villageRepository.GetByIdAsync(location.VillageId);
                }

                var userRoles = await userManager.GetRolesAsync(user);

                var availableRoles = _roleManager.Roles.Where(x=>x.Name!= "Administrator").Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = userRoles.Contains(r.Name)
                });
                var village = (await _villageRepository.FindAsync(x => TehsilIds.Contains(x.TehsilBlockId))).ToList();
                var tehsilBlock = (await _tehsilBlockRepository.FindAsync(x => SubDivisionIds.Contains(x.SubDivisionId))).ToList();
                var subDivision = (await _subDivisionRepository.FindAsync(x=> DivisionIds.Contains(x.DivisionId))).ToList();
                var division = _divisionRepository.GetAll();

                RegisterEmployeeViewModelEdit model = new RegisterEmployeeViewModelEdit
                {
                    Id = Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    SelectedRole=roles.FirstOrDefault(),
                    Roles = availableRoles,
                    SelectedVillageId = VillageIds,
                    SelectedTehsilBlockId = TehsilIds,
                    SelectedSubDivisionId = SubDivisionIds,
                    SelectedDivisionId = DivisionIds,
                    Divisions= division,
                    SubDivision=subDivision,
                    TehsilBlock = tehsilBlock,
                    Village = village,
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }

        }
        [HttpPost]
        public async Task<IActionResult> Edit(RegisterEmployeeViewModelEdit model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";
                    return View("NotFound");
                }

                var existingUser = await userManager.FindByEmailAsync(user.Email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    // Duplicate email found
                    ModelState.AddModelError("Email", "Email is already in use");
                    return View(user);
                }
                await _userVillageRepository.DeleteNonPrimaryAsync(x => x.UserId == user.Id);
                if (model.SelectedVillageId != null && model.SelectedVillageId.Count > 0)
                {
                    List<UserVillage> list = new List<UserVillage>();
                    foreach (var item in model.SelectedVillageId)
                    {
                        var village = await _villageRepository.GetByIdAsync(item);
                        if (village != null)
                        {
                            //await _userVillageRepository.DeleteNonPrimaryAsync(x => x.UserId == user.Id && x.VillageId ==item);
                            var data = new UserVillage { User = user, Village = village };
                            list.Add(data);
                        }
                    }
                    user.UserVillages=list;
                }
                await _userTehsilRepository.DeleteNonPrimaryAsync(x => x.UserId == user.Id);
                if (model.SelectedTehsilBlockId != null && model.SelectedTehsilBlockId.Count > 0)
                {
                    List<UserTehsil> list = new List<UserTehsil>();
                    foreach (var tehsilid in model.SelectedTehsilBlockId)
                    {
                        var tehsil = await _tehsilBlockRepository.GetByIdAsync(tehsilid);
                        if (tehsil != null)
                        {
                            //await _userTehsilRepository.DeleteNonPrimaryAsync(x => x.UserId == user.Id && x.TehsilId== tehsilid);
                            var data = new UserTehsil { User = user, Tehsil = tehsil };
                            list.Add(data);
                            
                        }
                    }
                    user.UserTehsils=list;
                }
                await _userSubDivisionRepository.DeleteNonPrimaryAsync(x => x.UserId == user.Id );
                if (model.SelectedSubDivisionId != null && model.SelectedSubDivisionId.Count > 0)
                {
                    List<UserSubdivision> list = new List<UserSubdivision>();
                    foreach (var subdivisionID in model.SelectedSubDivisionId)
                    {
                        var subdivision = await _subDivisionRepository.GetByIdAsync(subdivisionID);
                        if (subdivision != null)
                        {
                            //await _userSubDivisionRepository.DeleteNonPrimaryAsync(x => x.UserId == user.Id && x.SubdivisionId== subdivisionID);
                            var data = new UserSubdivision { User = user, Subdivision = subdivision };
                            list.Add(data);
                        }
                    }
                    user.UserSubdivisions = list;
                }
                await _userDivisionRepository.DeleteNonPrimaryAsync(x => x.UserId == user.Id);
                if (model.SelectedDivisionId != null && model.SelectedDivisionId.Count > 0)
                {
                    List<UserDivision> list = new List<UserDivision>();
                    foreach (var divisionID in model.SelectedDivisionId)
                    {
                        var division = await _divisionRepository.GetByIdAsync(divisionID);
                        if (division != null)
                        {
                            var data = new UserDivision { User = user, Division = division };
                            list.Add(data);
                            
                        }
                    }
                    user.UserDivisions = list;  
                }


                var userRoles = await userManager.GetRolesAsync(user);

                var resultRemove = await userManager.RemoveFromRolesAsync(user, userRoles);

                if (!resultRemove.Succeeded)
                {
                    // Handle errors if removing roles fails
                    foreach (var error in resultRemove.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                var resultAdd = await userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!resultAdd.Succeeded)
                {
                    // Handle errors if adding roles fails
                    foreach (var error in resultAdd.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
                user.UserName = model.Email;
                user.Email = model.Email;
                //user.VillageId = model.SelectedVillageId;
                //user.TehsilBlockId = model.SelectedTehsilBlockId;
                //user.SubDivisionId = model.SelectedSubDivisionId;
                //user.DivisionId = model.SelectedDivisionId;


                var result = await userManager.UpdateAsync(user);
                // Retrieve associated regions from the repository
                //var village = model.SelectedVillageId==null?null:await _villageRepository.GetByIdAsync(model.SelectedVillageId??0);
                //var tehsilBlock = model.SelectedTehsilBlockId==null?null:await _tehsilBlockRepository.GetByIdAsync(model.SelectedTehsilBlockId ?? 0);
                //var subDivision = model.SelectedSubDivisionId==null?null:await _subDivisionRepository.GetByIdAsync(model.SelectedSubDivisionId ?? 0);
                //var division = await _divisionRepository.GetByIdAsync(model.SelectedDivisionId);


                if (result.Succeeded)
                {
                    // Update user's navigation properties

                    //Rupinder Start
                    //user.Village = village;
                    //user.TehsilBlock = tehsilBlock;
                    //user.SubDivision = subDivision;
                    //user.Division = division;
                    //Rupinder End

                    // Save changes
                    //await this.userManager.UpdateAsync(user);

                    //userManager.AddToRoleAsync(user, "User").Wait();
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("List", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }


            var village = await _villageRepository.FindAsync(x => x.CreatedBy == user.Id || x.UpdatedBy == user.Id);
            var tehsil = await _tehsilBlockRepository.FindAsync(x => x.CreatedBy == user.Id || x.UpdatedBy == user.Id);
            var subdivision = await _subDivisionRepository.FindAsync(x => x.CreatedBy == user.Id || x.UpdatedBy == user.Id);
            var division = await _divisionRepository.FindAsync(x => x.CreatedBy == user.Id || x.UpdatedBy == user.Id);
            //var drain = await _drainRepo.FindAsync(x => x.CreatedBy == user.Id || x.UpdatedBy == user.Id);
            if (village.Count()>0 || tehsil.Count() > 0 || subdivision.Count() > 0 || division.Count() > 0/* ||drain.Count()<=0*/)
            {
                ModelState.AddModelError("e", $"User {user.UserName} is already in use");
                return RedirectToAction("List");
            }

            await _userVillageRepository.DeleteNonPrimaryAsync(x => x.UserId == user.Id);
            await _userTehsilRepository.DeleteNonPrimaryAsync(x => x.UserId == user.Id);
            await _userSubDivisionRepository.DeleteNonPrimaryAsync(x => x.UserId == user.Id);
            await _userDivisionRepository.DeleteNonPrimaryAsync(x => x.UserId == user.Id);

            // Delete associated user roles
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var roleName in userRoles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    await userManager.RemoveFromRoleAsync(user, roleName);
                }
            }

            // Delete the user
            await userManager.DeleteAsync(user);

            return RedirectToAction("List"); // Redirect to wherever you want after deletion
        }
        //public async Task<IActionResult> Delete(int id)
        //{
        //    var obj = await _repo.GetByIdAsync(id);
        //    obj.TehsilBlock = await _tehsilBlockRepo.GetByIdAsync(obj.TehsilBlockId);
        //    obj.TehsilBlock.SubDivision = await _subDivisionRepo.GetByIdAsync(obj.TehsilBlock.SubDivisionId);
        //    obj.TehsilBlock.SubDivision.Division = await _divisionRepo.GetByIdAsync(obj.TehsilBlock.SubDivision.DivisionId);
        //    if (obj == null)
        //    {
        //        ViewBag.ErrorMessage = $"Village with Id = {obj.Id} cannot be found";
        //        return View("NotFound");
        //    }

        //    return View(obj);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var obj = await _repo.GetByIdAsync(id);
        //    if (obj == null)
        //    {
        //        ViewBag.ErrorMessage = $"Village with Id = {obj.Id} cannot be found";
        //        return View("NotFound");
        //    }

        //    await _repo.DeleteAsync(id);

        //    return RedirectToAction("List", "Village");
        //}
    }
}
