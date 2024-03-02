using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Noc_App.Helpers;
using Noc_App.Models;
using Noc_App.Models.interfaces;
using Noc_App.Models.ViewModel;
using Noc_App.UtilityService;
using System.Data;

namespace Noc_App.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<VillageDetails> _villageRepository;
        private readonly IEmailService _emailService;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepository;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepository;
        private readonly IRepository<DivisionDetails> _divisionRepository;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IRepository<DivisionDetails> divisionRepository, IRepository<SubDivisionDetails> subDivisionRepository, IRepository<TehsilBlockDetails> tehsilBlockRepository, IRepository<VillageDetails> villageRepository, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _divisionRepository = divisionRepository;
            _subDivisionRepository = subDivisionRepository;
            _tehsilBlockRepository = tehsilBlockRepository;
            _villageRepository = villageRepository;
            _emailService = emailService;
            _roleManager = roleManager;
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
                var user = new ApplicationUser { UserName=registerViewModel.Email,Email=registerViewModel.Email };
                user.EmailConfirmed = true;
                var result = await userManager.CreateAsync(user,registerViewModel.Password);
                if(result.Succeeded)
                {
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
                Village = new SelectList(Enumerable.Empty<VillageDetails>(), "Id", "Name"),
                TehsilBlock = new SelectList(Enumerable.Empty<TehsilBlockDetails>(), "Id", "Name"),
                SubDivision = new SelectList(Enumerable.Empty<SubDivisionDetails>(), "Id", "Name"),
                Divisions = new SelectList(_divisionRepository.GetAll(), "Id", "Name"),
                Roles = new SelectList(role, "Name", "Name"),
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterEmployee(RegisterEmployeeViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = registerViewModel.Email, 
                    Email = registerViewModel.Email,
                    VillageId = registerViewModel.SelectedVillageId,
                    TehsilBlockId = registerViewModel.SelectedTehsilBlockId,
                    SubDivisionId = registerViewModel.SelectedSubDivisionId,
                    DivisionId = registerViewModel.SelectedDivisionId
                };
                user.EmailConfirmed = true;
                string password = "Abc@123";
                var result = await userManager.CreateAsync(user, password);
                // Retrieve associated regions from the repository
                var village = await _villageRepository.GetByIdAsync(registerViewModel.SelectedVillageId);
                var tehsilBlock = await _tehsilBlockRepository.GetByIdAsync(registerViewModel.SelectedTehsilBlockId);
                var subDivision = await _subDivisionRepository.GetByIdAsync(registerViewModel.SelectedSubDivisionId);
                var division = await _divisionRepository.GetByIdAsync(registerViewModel.SelectedDivisionId);

                
                if (result.Succeeded)
                {
                    // Add user to the specified role
                    await this.userManager.AddToRoleAsync(user, registerViewModel.SelectedRole);
                    // Update user's navigation properties
                    user.Village = village;
                    user.TehsilBlock = tehsilBlock;
                    user.SubDivision = subDivision;
                    user.Division = division;

                    // Save changes
                    await this.userManager.UpdateAsync(user);

                    var emailModel = new EmailModel(user.Email, "Login Credentials", EmailBody.EmailStringBodyForInformation(user.Email, password));
                    _emailService.SendEmail(emailModel);

                    //userManager.AddToRoleAsync(user, "User").Wait();
                    //await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("List", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(registerViewModel);
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

        [HttpPost]
        public IActionResult GetSubDivisions(int divisionId)
        {
            var subDivision = _subDivisionRepository.GetAll();
            var filteredSubdivisions = subDivision.Where(c => c.DivisionId == divisionId).ToList();
            return Json(new SelectList(filteredSubdivisions, "Id", "Name"));
        }

        [HttpPost]
        public IActionResult GetTehsilBlocks(int subDivisionId)
        {
            var tehsilBlock = _tehsilBlockRepository.GetAll().Where(c => c.SubDivisionId == subDivisionId).ToList();
            return Json(new SelectList(tehsilBlock, "Id", "Name"));
        }

        [HttpPost]
        public IActionResult GetVillagess(int tehsilBlockId)
        {
            var village = _villageRepository.GetAll().Where(c => c.TehsilBlockId == tehsilBlockId).ToList();
            return Json(new SelectList(village, "Id", "Name"));
        }

        [HttpGet]
        public async Task<ViewResult> List()
        {
            try
            {
                var list = this.userManager.Users.ToList();
                var usersWithRoles = new List<UserWithRolesViewModel>();
                foreach (var user in list)
                {

                    var roles = userManager.GetRolesAsync(user).Result;
                    user.Village = await _villageRepository.GetByIdAsync(user.VillageId ?? 0);
                    if(user.Village!=null)
                        user.TehsilBlock = await _tehsilBlockRepository.GetByIdAsync(user.Village.TehsilBlockId!=null? user.Village.TehsilBlockId : 0);
                    if (user.TehsilBlock != null)
                    {
                        user.SubDivision = await _subDivisionRepository.GetByIdAsync(user.TehsilBlock.SubDivisionId != null ? user.TehsilBlock.SubDivisionId : 0);
                        if (user.TehsilBlock.SubDivision != null)
                            user.Division = await _divisionRepository.GetByIdAsync(user.TehsilBlock.SubDivision.DivisionId != null ? user.TehsilBlock.SubDivision.DivisionId : 0);
                    }
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
                
                user.Village = await _villageRepository.GetByIdAsync(user.VillageId??0);
                user.TehsilBlock = await _tehsilBlockRepository.GetByIdAsync(user.Village.TehsilBlockId);
                user.TehsilBlock.SubDivision = await _subDivisionRepository.GetByIdAsync(user.TehsilBlock.SubDivisionId);
                user.TehsilBlock.SubDivision.Division = await _divisionRepository.GetByIdAsync(user.TehsilBlock.SubDivision.DivisionId);
                var userRoles = await userManager.GetRolesAsync(user);

                var availableRoles = _roleManager.Roles.Where(x=>x.Name!= "Administrator").Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name,
                    Selected = userRoles.Contains(r.Name)
                });
                var village = _villageRepository.GetAll().Where(x => x.TehsilBlockId == user.Village.TehsilBlockId);
                var tehsilBlock = _tehsilBlockRepository.GetAll().Where(x => x.SubDivisionId == user.TehsilBlock.SubDivisionId);
                var subDivision = _subDivisionRepository.GetAll().Where(x => x.DivisionId == user.TehsilBlock.SubDivision.DivisionId);
                var division = _divisionRepository.GetAll();

                RegisterEmployeeViewModelEdit model = new RegisterEmployeeViewModelEdit
                {
                    Id = Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    SelectedRole=roles.FirstOrDefault(),
                    Roles = availableRoles,
                    SelectedVillageId = user.Village.Id,
                    Village = new SelectList(village, "Id", "VillageName", user.VillageId),
                    SelectedTehsilBlockId = user.TehsilBlock.Id,
                    TehsilBlock = new SelectList(tehsilBlock, "Id", "Name", user.TehsilBlockId),
                    SelectedSubDivisionId = user.TehsilBlock.SubDivisionId,
                    SubDivision = new SelectList(subDivision, "Id", "Name", user.TehsilBlock.SubDivisionId),
                    SelectedDivisionId = user.TehsilBlock.SubDivision.DivisionId,
                    Divisions = new SelectList(division, "Id", "Name", user.TehsilBlock.SubDivision.DivisionId)
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
                user.VillageId = model.SelectedVillageId;
                user.TehsilBlockId = model.SelectedTehsilBlockId;
                user.SubDivisionId = model.SelectedSubDivisionId;
                user.DivisionId = model.SelectedDivisionId;


                var result = await userManager.UpdateAsync(user);
                // Retrieve associated regions from the repository
                var village = await _villageRepository.GetByIdAsync(model.SelectedVillageId);
                var tehsilBlock = await _tehsilBlockRepository.GetByIdAsync(model.SelectedTehsilBlockId);
                var subDivision = await _subDivisionRepository.GetByIdAsync(model.SelectedSubDivisionId);
                var division = await _divisionRepository.GetByIdAsync(model.SelectedDivisionId);


                if (result.Succeeded)
                {
                    // Update user's navigation properties
                    user.Village = village;
                    user.TehsilBlock = tehsilBlock;
                    user.SubDivision = subDivision;
                    user.Division = division;

                    // Save changes
                    await this.userManager.UpdateAsync(user);

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
