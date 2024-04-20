using Microsoft.AspNetCore.Mvc;
using Noc_App.Models.interfaces;
using Noc_App.Models;
using System.Runtime.CompilerServices;
using Noc_App.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Noc_App.Controllers
{
    public class ApprovalProcess2Controller : Controller
    {
        private readonly IRepository<GrantDetails> _repo;
        private readonly IRepository<VillageDetails> _villageRpo;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepo;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepo;
        private readonly IRepository<DivisionDetails> _divisionRepo;
        private readonly IRepository<UserDivision> _userDivisionRepository;
        private readonly IRepository<UserSubdivision> _userSubDivisionRepository;
        private readonly IRepository<UserVillage> _userVillageRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<GrantPaymentDetails> _repoPayment;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRepository<GrantApprovalDetail> _repoApprovalDetail;
        private readonly IRepository<GrantApprovalMaster> _repoApprovalMaster;
        public ApprovalProcess2Controller(IRepository<GrantDetails> repo, IRepository<VillageDetails> villageRepo, IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<SubDivisionDetails> subDivisionRepo,
            IRepository<DivisionDetails> divisionRepo, IRepository<UserDivision> userDivisionRepository, UserManager<ApplicationUser> userManager, IRepository<GrantPaymentDetails> repoPayment, RoleManager<IdentityRole> roleManager
            , IRepository<UserVillage> userVillageRepository, IRepository<GrantApprovalDetail> repoApprovalDetail, IRepository<GrantApprovalMaster> repoApprovalMaster, IRepository<UserSubdivision> userSubDivisionRepository)
        {
            _repo = repo;
            _villageRpo = villageRepo;
            _tehsilBlockRepo = tehsilBlockRepo;
            _subDivisionRepo = subDivisionRepo;
            _divisionRepo = divisionRepo;
            _userDivisionRepository = userDivisionRepository;
            _userManager = userManager;
            _repoPayment = repoPayment;
            _roleManager = roleManager;
            _userVillageRepository = userVillageRepository;
            _repoApprovalDetail = repoApprovalDetail;
            _repoApprovalMaster= repoApprovalMaster;
            _userSubDivisionRepository = userSubDivisionRepository;
        }

        public async Task<ViewResult> Index()
        {
            try
            {

            
            var user = await _userManager.GetUserAsync(User);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user object
            var userDetail = await _userManager.FindByIdAsync(userId);

            // Retrieve roles associated with the user
            var role = (await _userManager.GetRolesAsync(userDetail)).FirstOrDefault();

                //var user2 = await _userManager.FindByNameAsync(user.UserName);
                IQueryable<UserDivision> userDiv = (await _userDivisionRepository.FindAsync(x => x.UserId == user.Id)).AsQueryable();
                IEnumerable<UserSubdivision> userSubdiv = (await _userSubDivisionRepository.FindAsync(x => x.UserId == user.Id));
                IEnumerable<UserVillage> userVillage = (await _userVillageRepository.FindAsync(x => x.UserId == user.Id));

            List<GrantUnprocessedAppDetails> model = new List<GrantUnprocessedAppDetails>();
                IEnumerable<GrantUnprocessedAppDetails> model2=null;
                if (userVillage.Count() > 0)
                {
                    model2 = (from g in _repo.GetAll()
                              join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                              join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                              join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                              join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals sub.Id
                              join div in _divisionRepo.GetAll() on sub.DivisionId equals div.Id
                              join uv in userVillage on v.Id equals uv.VillageId
                              where g.IsPending == true && g.IsForwarded == (userDiv.Count()>0 && g.ProcessLevel == 0 ? false : role == "EXECUTIVE ENGINEER" ? false : true)
                              orderby g.CreatedOn
                              select new GrantUnprocessedAppDetails
                              {
                                  Id = g.Id,
                                  Name = g.Name,
                                  ApplicantEmailID = g.ApplicantEmailID,
                                  ApplicantName = g.ApplicantName,
                                  ApplicationID = g.ApplicationID,
                                  DivisionName = div.Name,
                                  Hadbast = g.Hadbast,
                                  NocNumber = g.NocNumber,
                                  NocPermissionTypeID = g.NocPermissionTypeID,
                                  NocTypeId = g.NocTypeId,
                                  OtherProjectTypeDetail = g.OtherProjectTypeDetail,
                                  PlotNo = g.PlotNo,
                                  PreviousDate = g.PreviousDate,
                                  ProjectTypeId = g.ProjectTypeId,
                                  SiteAreaUnitId = g.SiteAreaUnitId,
                                  SubDivisionName = sub.Name,
                                  TehsilBlockName = t.Name,
                                  VillageID = g.VillageID,
                                  VillageName = v.Name,
                                  LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + v.Name + ", Pincode: " + v.PinCode,
                              });
                }
                else if (userSubdiv.Count() > 0)
                {
                    model2 = (from g in _repo.GetAll()
                              join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                              join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                              join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                              join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals sub.Id
                              join div in _divisionRepo.GetAll() on sub.DivisionId equals div.Id
                              join uv in userSubdiv on v.Id equals uv.SubdivisionId
                              where g.IsPending == true && g.IsForwarded == (userDiv.Count() > 0 && g.ProcessLevel == 0 ? false : role == "EXECUTIVE ENGINEER" ? false : true)
                              //&& (userDiv != null ? div.Id.Equals(userDiv.DivisionId) : userSubdiv != null ? sub.Id.Equals(userSubdiv.SubdivisionId) : userVillage != null ? v.Id.Equals(userVillage.VillageId) : false)
                              orderby g.CreatedOn
                              select new GrantUnprocessedAppDetails
                              {
                                  Id = g.Id,
                                  Name = g.Name,
                                  ApplicantEmailID = g.ApplicantEmailID,
                                  ApplicantName = g.ApplicantName,
                                  ApplicationID = g.ApplicationID,
                                  DivisionName = div.Name,
                                  Hadbast = g.Hadbast,
                                  NocNumber = g.NocNumber,
                                  NocPermissionTypeID = g.NocPermissionTypeID,
                                  NocTypeId = g.NocTypeId,
                                  OtherProjectTypeDetail = g.OtherProjectTypeDetail,
                                  PlotNo = g.PlotNo,
                                  PreviousDate = g.PreviousDate,
                                  ProjectTypeId = g.ProjectTypeId,
                                  SiteAreaUnitId = g.SiteAreaUnitId,
                                  SubDivisionName = sub.Name,
                                  TehsilBlockName = t.Name,
                                  VillageID = g.VillageID,
                                  VillageName = v.Name,
                                  LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + v.Name + ", Pincode: " + v.PinCode,
                              });
                }
                else if (userDiv.Count() > 0)
                {
                    model2 = (from g in _repo.GetAll()
                              join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                              join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                              join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                              join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals sub.Id
                              join div in _divisionRepo.GetAll().AsEnumerable() on sub.DivisionId equals div.Id
                              //join uv in userDiv on v.Id equals uv.DivisionId
                              where g.IsPending == true && g.IsForwarded == (userDiv.Count() > 0 && g.ProcessLevel == 0 ? false : role == "EXECUTIVE ENGINEER" ? false : true)
                              orderby g.CreatedOn
                              select new GrantUnprocessedAppDetails
                              {
                                  Id = g.Id,
                                  Name = g.Name,
                                  ApplicantEmailID = g.ApplicantEmailID,
                                  ApplicantName = g.ApplicantName,
                                  ApplicationID = g.ApplicationID,
                                  DivisionName = div.Name,
                                  Hadbast = g.Hadbast,
                                  NocNumber = g.NocNumber,
                                  NocPermissionTypeID = g.NocPermissionTypeID,
                                  NocTypeId = g.NocTypeId,
                                  OtherProjectTypeDetail = g.OtherProjectTypeDetail,
                                  PlotNo = g.PlotNo,
                                  PreviousDate = g.PreviousDate,
                                  ProjectTypeId = g.ProjectTypeId,
                                  SiteAreaUnitId = g.SiteAreaUnitId,
                                  SubDivisionName = sub.Name,
                                  TehsilBlockName = t.Name,
                                  VillageID = g.VillageID,
                                  VillageName = v.Name,
                                  LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + v.Name + ", Pincode: " + v.PinCode,
                              });
                }
                model = model = (from g in model2
                             join app in _repoApprovalDetail.GetAll() on g.Id equals app.GrantID into ad
                             from app1 in ad.DefaultIfEmpty()
                             where app1.ProcessedToRole!=null && app1.ProcessedToRole!=string.Empty? app1.ProcessedToRole == role:true
                             orderby app1.ProcessedOn
                             select new GrantUnprocessedAppDetails
                             {
                                 Id = g.Id,
                                 Name = g.Name,
                                 ApplicantEmailID = g.ApplicantEmailID,
                                 ApplicantName = g.ApplicantName,
                                 ApplicationID = g.ApplicationID,
                                 DivisionName = g.Name,
                                 Hadbast = g.Hadbast,
                                 NocNumber = g.NocNumber,
                                 NocPermissionTypeID = g.NocPermissionTypeID,
                                 NocTypeId = g.NocTypeId,
                                 OtherProjectTypeDetail = g.OtherProjectTypeDetail,
                                 PlotNo = g.PlotNo,
                                 PreviousDate = g.PreviousDate,
                                 ProjectTypeId = g.ProjectTypeId,
                                 SiteAreaUnitId = g.SiteAreaUnitId,
                                 SubDivisionName = g.Name,
                                 TehsilBlockName = g.Name,
                                 VillageID = g.VillageID,
                                 VillageName = g.Name,
                                 LocationDetails = g.LocationDetails,
                             }).ToList();
            return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [HttpGet]
        public async Task<ViewResult> Forward(string Id)
        {
            try
            {
                GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == Id)).FirstOrDefault();

                //var user = await _userManager.GetUserAsync(User);
                //UserDivision userDiv = (await _userDivisionRepository.FindAsync(x => x.UserId == user.Id)).ToList().FirstOrDefault();
                //var role = await _roleManager.FindByNameAsync("JE");

                // Get the users in the role
                var users = await _userVillageRepository.FindAsync(x => x.VillageId==grant.VillageID);
                var usersInRole = (await _userManager.GetUsersInRoleAsync("JUNIOR ENGINEER".ToUpper()));
                List<OfficerDetails> officerDetail = (from u in users
                                  join ur in usersInRole on u.UserId equals ur.Id
                                  select new OfficerDetails
                                  {
                                      UserId = u.UserId,
                                      UserName=ur.UserName
                                  }
                                  ).ToList();
                
                ForwardApplicationViewModel model = (from g in _repo.GetAll()
                                                      join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                                                      join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                                                      join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                                                      join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals sub.Id
                                                      join div in _divisionRepo.GetAll() on sub.DivisionId equals div.Id
                                                      where g.ApplicationID == Id
                                                      select new ForwardApplicationViewModel
                                                      {
                                                          Id = g.Id,
                                                          Name = g.Name,
                                                          ApplicantEmailID = g.ApplicantEmailID,
                                                          ApplicantName = g.ApplicantName,
                                                          ApplicationID = g.ApplicationID,
                                                          Officers = new SelectList(officerDetail, "UserId", "UserName"),
                                                          LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + v.Name + ", Pincode: " + v.PinCode,
                                                      }).FirstOrDefault();
                //ForwardApplicationViewModel model2 = new ForwardApplicationViewModel
                //{
                //    Id = Id,
                //    ApplicantEmailID=grant.ApplicantEmailID,
                //    ApplicantName=grant.ApplicantName,
                //    Name=grant.Name,
                //    Officers = new SelectList(usersInRole, "UserId", "UserName")
                //};
                //DivisionDetails obj = await _repo.GetByIdAsync(Id);
                //DivisionViewModelEdit model = new DivisionViewModelEdit
                //{
                //    Id = obj.Id,
                //    Name = obj.Name
                //};
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Forward(ForwardApplicationViewModel model)
        {
            if (string.IsNullOrEmpty(model.ApplicationID))
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {model.ApplicationID} cannot be found";
                return View("NotFound");
            }

            GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == model.ApplicationID)).FirstOrDefault();

            if (grant == null)
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {model.ApplicationID} cannot be found";

                return View("NotFound");
            }
            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user object
            var user = await _userManager.FindByIdAsync(userId);

            // Retrieve roles associated with the user
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            GrantApprovalMaster master = (await _repoApprovalMaster.FindAsync(x => x.Code == "F")).FirstOrDefault();
            var forwardedUser = await _userManager.FindByIdAsync(model.SelectedOfficerId);
            var forwardedRole = (await _userManager.GetRolesAsync(forwardedUser)).FirstOrDefault();
            int approvalLevel = (await _repoApprovalDetail.FindAsync(x=>x.GrantID==grant.Id && x.ApprovalID==master.Id)).Count();
            GrantApprovalDetail approvalDetail = new GrantApprovalDetail
            {
                GrantID = grant.Id,
                ApprovalID = master.Id,
                ProcessedBy = User.Identity.Name,
                ProcessedOn = DateTime.Now,
                ProcessedByRole = role,
                ProcessLevel = approvalLevel+1,
                ProcessedToRole = forwardedRole,
                ProcessedToUser = forwardedUser.Email
            };

            await _repoApprovalDetail.CreateAsync(approvalDetail);

            grant.IsForwarded = true;
            grant.ProcessLevel = approvalDetail.ProcessLevel;
            grant.UpdatedOn = DateTime.Now;
            await _repo.UpdateAsync(grant);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";
                return View("NotFound");
            }

            GrantDetails grant = (await _repo.FindAsync(x=>x.ApplicationID==id)).FirstOrDefault();

            if (grant == null)
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";

                return View("NotFound");
            }
            // Get the current user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user object
            var user = await _userManager.FindByIdAsync(userId);

            // Retrieve roles associated with the user
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            GrantApprovalMaster master = (await _repoApprovalMaster.FindAsync(x => x.Code == "R")).FirstOrDefault();
            int rejectionLevel = (await _repoApprovalDetail.FindAsync(x => x.GrantID == grant.Id && x.ApprovalID == master.Id)).Count();
            GrantApprovalDetail approvalDetail = new GrantApprovalDetail
            {
                GrantID=grant.Id,
                ApprovalID=master.Id,
                ProcessedBy=User.Identity.Name,
                ProcessedOn=DateTime.Now,
                ProcessedByRole=role,
                ProcessLevel= rejectionLevel+1,
                ProcessedToRole="",
                ProcessedToUser=""
            };

            await _repoApprovalDetail.CreateAsync(approvalDetail);
            //_repoApprovalDetail.
            grant.IsRejected = true;
            grant.ProcessLevel = approvalDetail.ProcessLevel;
            grant.IsPending = false;
            grant.UpdatedOn = DateTime.Now;
            await _repo.UpdateAsync(grant);

            return RedirectToAction("Index"); 
        }
    }
}
