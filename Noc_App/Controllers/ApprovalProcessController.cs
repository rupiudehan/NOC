using Microsoft.AspNetCore.Mvc;
using Noc_App.Models.interfaces;
using Noc_App.Models;
using System.Runtime.CompilerServices;
using Noc_App.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Rotativa.AspNetCore;
using Noc_App.UtilityService;
using Noc_App.Helpers;

namespace Noc_App.Controllers
{
    public class ApprovalProcessController : Controller
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
        private readonly IRepository<GrantApprovalProcessDocumentsDetails> _repoApprovalDocument;
        private readonly IRepository<ProjectTypeDetails> _projectTypeRepo;
        private readonly IRepository<NocPermissionTypeDetails> _nocPermissionTypeRepo;
        private readonly IRepository<NocTypeDetails> _nocTypeRepo;
        private readonly IRepository<OwnerTypeDetails> _ownerTypeRepo;
        private readonly IRepository<GrantKhasraDetails> _khasraRepo;
        private readonly IRepository<SiteAreaUnitDetails> _siteUnitsRepo;
        private readonly IRepository<GrantPaymentDetails> _grantPaymentRepo;
        private readonly IRepository<OwnerDetails> _grantOwnersRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmailService _emailService;
        public ApprovalProcessController(IRepository<GrantDetails> repo, IRepository<VillageDetails> villageRepo, IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<SubDivisionDetails> subDivisionRepo,
            IRepository<DivisionDetails> divisionRepo, IRepository<UserDivision> userDivisionRepository, UserManager<ApplicationUser> userManager, IRepository<GrantPaymentDetails> repoPayment, RoleManager<IdentityRole> roleManager
            , IRepository<UserVillage> userVillageRepository, IRepository<GrantApprovalDetail> repoApprovalDetail, IRepository<GrantApprovalMaster> repoApprovalMaster, IRepository<UserSubdivision> userSubDivisionRepository
            , IRepository<ProjectTypeDetails> projectTypeRepo, IRepository<NocPermissionTypeDetails> nocPermissionTypeRepo,
            IRepository<NocTypeDetails> nocTypeRepo, IRepository<OwnerTypeDetails> ownerTypeRepo, IRepository<GrantPaymentDetails> grantPaymentRepo, IRepository<OwnerDetails> grantOwnersRepo,
            IRepository<GrantKhasraDetails> khasraRepo, IRepository<SiteAreaUnitDetails> siteUnitsRepo, IWebHostEnvironment hostingEnvironment, IEmailService emailService
            , IRepository<GrantApprovalProcessDocumentsDetails> repoApprovalDocument)
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
            _repoApprovalMaster = repoApprovalMaster;
            _userSubDivisionRepository = userSubDivisionRepository;
            _hostingEnvironment = hostingEnvironment;
            _projectTypeRepo = projectTypeRepo;
            _nocPermissionTypeRepo = nocPermissionTypeRepo;
            _nocTypeRepo = nocTypeRepo;
            _ownerTypeRepo = ownerTypeRepo;
            _khasraRepo = khasraRepo;
            _siteUnitsRepo = siteUnitsRepo;
            _grantPaymentRepo = grantPaymentRepo;
            _grantOwnersRepo = grantOwnersRepo;
            _emailService = emailService;
            _repoApprovalDocument = repoApprovalDocument;
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
                List<UserDivision> userDiv = (await _userDivisionRepository.FindAsync(x => x.UserId == user.Id)).ToList();
                List<UserSubdivision> userSubdiv = (await _userSubDivisionRepository.FindAsync(x => x.UserId == user.Id)).ToList();
                List<UserVillage> userVillage = (await _userVillageRepository.FindAsync(x => x.UserId == user.Id)).ToList();

                List<GrantUnprocessedAppDetails> model = new List<GrantUnprocessedAppDetails>();
                if (role == "EXECUTIVE ENGINEER")
                {
                    model = (from g in _repo.GetAll()
                             join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                             join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                             join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                             join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals sub.Id
                             join div in _divisionRepo.GetAll().AsEnumerable() on sub.DivisionId equals div.Id
                             where g.IsPending == true && g.IsForwarded == (g.ProcessLevel == 0 ? false : true)
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
                                 DivisionId = div.Id,
                                 SubDivisionId = sub.Id,
                                 VillageName = v.Name,
                                 IsForwarded=g.IsForwarded,
                                 LoggedInRole=role,
                                 LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + v.Name + ", Pincode: " + v.PinCode,
                             }).ToList();
                }
                else if (role == "JUNIOR ENGINEER")
                {
                    model = (from g in _repo.GetAll()
                             join kh in _khasraRepo.GetAll() on g.Id equals (kh.GrantID)
                             join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                             join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                             join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                             join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals sub.Id
                             join div in _divisionRepo.GetAll().AsEnumerable() on sub.DivisionId equals div.Id
                             where g.IsPending == true && g.IsForwarded == true && g.ProcessLevel==1
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
                                 DivisionId = div.Id,
                                 SubDivisionId = sub.Id,
                                 VillageName = v.Name,
                                 IsForwarded = g.IsForwarded,
                                 LoggedInRole = role,
                                 LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + v.Name + ", Pincode: " + v.PinCode,
                             }).ToList();
                }
                else if (role == "SUB DIVISIONAL OFFICER")
                {
                    model = (from g in _repo.GetAll()
                             join kh in _khasraRepo.GetAll() on g.Id equals (kh.GrantID)
                             join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                             join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                             join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                             join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals sub.Id
                             join div in _divisionRepo.GetAll().AsEnumerable() on sub.DivisionId equals div.Id
                             where g.IsPending == true && g.IsForwarded == true && g.ProcessLevel==2
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
                                 DivisionId = div.Id,
                                 SubDivisionId = sub.Id,
                                 VillageName = v.Name,
                                 IsForwarded = g.IsForwarded,
                                 LoggedInRole = role,
                                 LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + v.Name + ", Pincode: " + v.PinCode,
                             }).ToList();
                }
                    if (userVillage.Count() > 0)
                {

                    model = (from m in model
                             join uv in userVillage.AsEnumerable() on m.VillageID equals (uv.VillageId)
                             select new GrantUnprocessedAppDetails
                             {
                                 Id = m.Id,
                                 Name = m.Name,
                                 ApplicantEmailID = m.ApplicantEmailID,
                                 ApplicantName = m.ApplicantName,
                                 ApplicationID = m.ApplicationID,
                                 DivisionName = m.Name,
                                 Hadbast = m.Hadbast,
                                 NocNumber = m.NocNumber,
                                 NocPermissionTypeID = m.NocPermissionTypeID,
                                 NocTypeId = m.NocTypeId,
                                 OtherProjectTypeDetail = m.OtherProjectTypeDetail,
                                 PlotNo = m.PlotNo,
                                 PreviousDate = m.PreviousDate,
                                 ProjectTypeId = m.ProjectTypeId,
                                 SiteAreaUnitId = m.SiteAreaUnitId,
                                 SubDivisionName = m.Name,
                                 TehsilBlockName = m.Name,
                                 VillageID = m.VillageID,
                                 DivisionId = m.Id,
                                 SubDivisionId = m.Id,
                                 VillageName = m.Name,
                                 LoggedInRole=role,
                                 LocationDetails = m.LocationDetails
                             }).ToList();
                }
                else if (userSubdiv.Count() > 0)
                {

                    model = (from m in model
                             join uv in userSubdiv.AsEnumerable() on m.DivisionId equals (uv.SubdivisionId)
                             select new GrantUnprocessedAppDetails
                             {
                                 Id = m.Id,
                                 Name = m.Name,
                                 ApplicantEmailID = m.ApplicantEmailID,
                                 ApplicantName = m.ApplicantName,
                                 ApplicationID = m.ApplicationID,
                                 DivisionName = m.Name,
                                 Hadbast = m.Hadbast,
                                 NocNumber = m.NocNumber,
                                 NocPermissionTypeID = m.NocPermissionTypeID,
                                 NocTypeId = m.NocTypeId,
                                 OtherProjectTypeDetail = m.OtherProjectTypeDetail,
                                 PlotNo = m.PlotNo,
                                 PreviousDate = m.PreviousDate,
                                 ProjectTypeId = m.ProjectTypeId,
                                 SiteAreaUnitId = m.SiteAreaUnitId,
                                 SubDivisionName = m.Name,
                                 TehsilBlockName = m.Name,
                                 VillageID = m.VillageID,
                                 DivisionId = m.Id,
                                 SubDivisionId = m.Id,
                                 VillageName = m.Name,
                                 LoggedInRole = role,
                                 LocationDetails = m.LocationDetails,

                             }).ToList();
                }
                else if (userDiv.Count() > 0)
                {
                    
                    model = (from m in model
                             join uv in userDiv.AsEnumerable() on m.DivisionId equals (uv.DivisionId)
                             select new GrantUnprocessedAppDetails
                             {
                                 Id = m.Id,
                                 Name = m.Name,
                                 ApplicantEmailID = m.ApplicantEmailID,
                                 ApplicantName = m.ApplicantName,
                                 ApplicationID = m.ApplicationID,
                                 DivisionName = m.Name,
                                 Hadbast = m.Hadbast,
                                 NocNumber = m.NocNumber,
                                 NocPermissionTypeID = m.NocPermissionTypeID,
                                 NocTypeId = m.NocTypeId,
                                 OtherProjectTypeDetail = m.OtherProjectTypeDetail,
                                 PlotNo = m.PlotNo,
                                 PreviousDate = m.PreviousDate,
                                 ProjectTypeId = m.ProjectTypeId,
                                 SiteAreaUnitId = m.SiteAreaUnitId,
                                 SubDivisionName = m.Name,
                                 TehsilBlockName = m.Name,
                                 VillageID = m.VillageID,
                                 DivisionId = m.Id,
                                 SubDivisionId = m.Id,
                                 VillageName = m.Name,
                                 LoggedInRole = role,
                                 LocationDetails = m.LocationDetails
                             }).ToList();
                }
                var model1 = (from g in model
                                 join app in _repoApprovalDetail.GetAll() on g.Id equals app.GrantID into ad
                                 from app1 in ad.DefaultIfEmpty()
                                  join appDoc in _repoApprovalDocument.GetAll() on app1.Id equals appDoc.GrantApprovalID into adDoc
                                  from appDoc1 in adDoc.DefaultIfEmpty()
                              where app1 != null ? app1.ProcessedToRole == role : true
                                
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
                                     DivisionId = g.Id,
                                     SubDivisionId = g.Id,
                                     VillageName = g.Name,
                                     LocationDetails = g.LocationDetails,
                                     LoggedInRole=role,
                                     GrantApprovalId=appDoc1 != null ? appDoc1.GrantApprovalID : 0
                                 }).Distinct().ToList().Distinct(new GrantUnprocessedAppDetailsComparer()); ;
                return View(model1);
            }
           catch(Exception ex){
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
                if (grant == null)
                {
                    ViewBag.ErrorMessage = $"Grant with Application Id = {Id} cannot be found";

                    return View("NotFound");
                }
                var total = Math.Round(((from kh in _khasraRepo.GetAll()
                             join u in _siteUnitsRepo.GetAll().AsEnumerable() on kh.UnitId equals u.Id
                             into grouped
                             where kh.GrantID == grant.Id
                             select new
                             {
                                 TotalArea =
                                 grouped.FirstOrDefault().Name.ToUpper() == "KANAL/MARLA/SARSAI" ?
                                 //Math.Round((
                                 (kh.KanalOrBiswa / 8) + (kh.MarlaOrBiswansi / 160) + (kh.SarsaiOrBigha / 1440)
                                 //), 4)
                                 /*Math.Round(grouped.Sum(d => Math.Round(((d.KanalOrBiswa / 8) + (d.MarlaOrBiswansi / 160) + (d.SarsaiOrBigha / 1440)), 4)))*/ //:
                                 ://Math.Round((
                                 (kh.KanalOrBiswa * 0.0125) + (kh.MarlaOrBiswansi * 0.000625) + (kh.SarsaiOrBigha * 0.25)
                                 //),4)
                                 /*Math.Round(grouped.Sum(d => Math.Round(((d.KanalOrBiswa * 0.0125) + (d.MarlaOrBiswansi * 0.000625) + (d.SarsaiOrBigha * 0.25)))), 4) }).Sum(x => x.TotalArea)*/
                             }).Sum(d=>d.TotalArea)),4);
                var subdivision = (from g in (await _repo.FindAsync(x => x.ApplicationID == Id))
                           join v in _villageRpo.GetAll() on  g.VillageID equals(v.Id)
                           join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals (t.Id)
                           select new {
                            SubDivisionId=t.SubDivisionId
                         }
                         ).ToList().FirstOrDefault();

                //var user = await _userManager.GetUserAsync(User);
                //UserDivision userDiv = (await _userDivisionRepository.FindAsync(x => x.UserId == user.Id)).ToList().FirstOrDefault();
                //var role = await _roleManager.FindByNameAsync("JE");
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Retrieve the user object
                var userDetail = await _userManager.FindByIdAsync(userId);

                // Retrieve roles associated with the user
                var role = (await _userManager.GetRolesAsync(userDetail)).FirstOrDefault();
                // Get the users in the role
                List<UserLocationDetails> users = (role == "JUNIOR ENGINEER"
                    ? (await _userSubDivisionRepository.FindAsync(x => x.SubdivisionId == subdivision.SubDivisionId)).Select(x=>new UserLocationDetails { UserId=x.UserId,SubDivisionId=x.SubdivisionId,DivisionId=0,TehsilBlockId=0,VillageId=0}).ToList()
                    : (await _userVillageRepository.FindAsync(x => x.VillageId==grant.VillageID)).Select(x => new UserLocationDetails { UserId = x.UserId, SubDivisionId = 0, DivisionId = 0, TehsilBlockId = 0, VillageId = x.VillageId }).ToList());
                string forwardToRole = role == "JUNIOR ENGINEER" ? "SUB DIVISIONAL OFFICER" : "JUNIOR ENGINEER".ToUpper();
                var usersInRole = (await _userManager.GetUsersInRoleAsync(forwardToRole));
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
                                                          TotalArea = total,
                                                          ApplicantEmailID = g.ApplicantEmailID,
                                                          ApplicantName = g.ApplicantName,
                                                          ApplicationID = g.ApplicationID,
                                                          ForwardToRole= forwardToRole,
                                                          LoggedInRole = role,
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
        [Obsolete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Forward(ForwardApplicationViewModel model)
        {
            try
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

                var total = Math.Round(((from kh in _khasraRepo.GetAll()
                                         join u in _siteUnitsRepo.GetAll().AsEnumerable() on kh.UnitId equals u.Id
                                         into grouped
                                         where kh.GrantID == grant.Id
                                         select new
                                         {
                                             TotalArea =
                                             grouped.FirstOrDefault().Name.ToUpper() == "KANAL/MARLA/SARSAI" ?
                                             (kh.KanalOrBiswa / 8) + (kh.MarlaOrBiswansi / 160) + (kh.SarsaiOrBigha / 1440)
                                             :(kh.KanalOrBiswa * 0.0125) + (kh.MarlaOrBiswansi * 0.000625) + (kh.SarsaiOrBigha * 0.25)
                                         }).Sum(d => d.TotalArea)), 4);
                // Get the current user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Retrieve the user object
                var user = await _userManager.FindByIdAsync(userId);

                // Retrieve roles associated with the user
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                GrantApprovalMaster master = (await _repoApprovalMaster.FindAsync(x => x.Code == "F")).FirstOrDefault();
                var forwardedUser = await _userManager.FindByIdAsync(model.SelectedOfficerId);
                var forwardedRole = (await _userManager.GetRolesAsync(forwardedUser)).FirstOrDefault();
                int approvalLevel = (await _repoApprovalDetail.FindAsync(x => x.GrantID == grant.Id && x.ApprovalID == master.Id)).Count();
                GrantApprovalDetail approvalDetail = new GrantApprovalDetail
                {
                    GrantID = grant.Id,
                    ApprovalID = master.Id,
                    ProcessedBy = User.Identity.Name,
                    ProcessedOn = DateTime.Now,
                    ProcessedByRole = role,
                    ProcessLevel = approvalLevel + 1,
                    ProcessedToRole = forwardedRole,
                    ProcessedToUser = forwardedUser.Email
                };

                if (total<=2)
                {
                    if(model.SiteConditionReportFile!=null && model.CatchmentAreaFile!=null && model.DistanceFromCreekFile!=null && model.GisOrDwsFile!=null && model.KmlFile!=null && model.CrossSectionOrCalculationFile!=null && model.LSectionOfDrainFile != null)
                    {
                        string ErrorMessage = string.Empty;
                        int siteConditionValidation = AllowedCheckExtensions(model.SiteConditionReportFile);
                        int CatchmentAreaValidation = AllowedCheckExtensions(model.CatchmentAreaFile);
                        int DistanceFromCreekFileValidation = AllowedCheckExtensions(model.DistanceFromCreekFile);
                        int GisOrDwsFileValidation = AllowedCheckExtensions(model.GisOrDwsFile);
                        int KmlFileValidation = AllowedCheckExtensions(model.KmlFile);
                        int CrossSectionOrCalculationFileValidation = AllowedCheckExtensions(model.CrossSectionOrCalculationFile);
                        int LSectionOfDrainFileValidation = AllowedCheckExtensions(model.LSectionOfDrainFile);
                        if (siteConditionValidation == 0)
                        {
                            ErrorMessage = $"Invalid site condition report file type. Please upload a PDF file only";
                            ModelState.AddModelError("", ErrorMessage);

                            return View(model);

                        }
                        else if (siteConditionValidation == 2)
                        {
                            ErrorMessage = "Site condition report field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        if (CatchmentAreaValidation == 0)
                        {
                            ErrorMessage = $"Invalid catchment area file type. Please upload a PDF file only";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);

                        }
                        else if (CatchmentAreaValidation == 2)
                        {
                            ErrorMessage = "Catchment area field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        if (DistanceFromCreekFileValidation == 0)
                        {
                            ErrorMessage = $"Invalid distance from creek file type. Please upload a PDF file only";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);

                        }
                        else if (DistanceFromCreekFileValidation == 2)
                        {
                            ErrorMessage = "Distance from creek file field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }

                        if (LSectionOfDrainFileValidation == 0)
                        {
                            ErrorMessage = $"Invalid GIS/DWS file type. Please upload a PDF file only";
                            ModelState.AddModelError("", ErrorMessage);

                            return View(model);

                        }
                        else if (GisOrDwsFileValidation == 2)
                        {
                            ErrorMessage = "GIS/DWS File field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }

                        if (KmlFileValidation == 0)
                        {
                            ErrorMessage = $"Invalid KML file type. Please upload a PDF file only";
                            ModelState.AddModelError("", ErrorMessage);

                            return View(model);

                        }
                        else if (KmlFileValidation == 2)
                        {
                            ErrorMessage = "KML File field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        if (CrossSectionOrCalculationFileValidation == 0)
                        {
                            ErrorMessage = $"Invalid Cross-Section/Calculation file type. Please upload a PDF file only";
                            ModelState.AddModelError("", ErrorMessage);

                            return View(model);

                        }
                        else if (CrossSectionOrCalculationFileValidation == 2)
                        {
                            ErrorMessage = "Cross-Section/Calculation file field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        if (LSectionOfDrainFileValidation == 0)
                        {
                            ErrorMessage = $"Invalid L-Section of drain file type. Please upload a PDF file only";
                            ModelState.AddModelError("", ErrorMessage);

                            return View(model);

                        }
                        else if (LSectionOfDrainFileValidation == 2)
                        {
                            ErrorMessage = "L-Section of drain file field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }

                        if (!AllowedFileSize(model.SiteConditionReportFile))
                        {
                            ErrorMessage = "Site condition report size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        if (!AllowedFileSize(model.CatchmentAreaFile))
                        {
                            ErrorMessage = "Catchment area file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        if (!AllowedFileSize(model.DistanceFromCreekFile))
                        {
                            ErrorMessage = "Distance from creek file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }

                        if (!AllowedFileSize(model.GisOrDwsFile))
                        {
                            ErrorMessage = "GIS/DWS file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }

                        if (!AllowedFileSize(model.KmlFile))
                        {
                            ErrorMessage = "KML file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }

                        if (!AllowedFileSize(model.CrossSectionOrCalculationFile))
                        {
                            ErrorMessage = "Cross-Section/Calculation file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }

                        if (!AllowedFileSize(model.LSectionOfDrainFile))
                        {
                            ErrorMessage = "L-Section of drain file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        string uniqueSiteConditionFileName = ProcessUploadedFile(model.SiteConditionReportFile, "SiteCondition");
                        string uniqueCatchmentAreaFileName = ProcessUploadedFile(model.CatchmentAreaFile, "CatchmentArea");
                        string uniqueDistanceFromCreekFileName = ProcessUploadedFile(model.DistanceFromCreekFile, "DistanceFromCreek");
                        string uniqueGisOrDwsFileName = ProcessUploadedFile(model.GisOrDwsFile, "GisOrDws");
                        string uniqueCrossSectionOrCalculationFileName = ProcessUploadedFile(model.CrossSectionOrCalculationFile, "CrossSectionOrCalculation");
                        string uniqueLSectionOfDrainFileName = ProcessUploadedFile(model.LSectionOfDrainFile, "LSectionOfDrain");
                        string uniqueKmlFileName = ProcessUploadedFile(model.KmlFile, "kmlReport");

                        await _repoApprovalDetail.CreateAsync(approvalDetail);

                        GrantApprovalProcessDocumentsDetails approvalObj = new GrantApprovalProcessDocumentsDetails
                        {
                            SiteConditionReportPath = uniqueSiteConditionFileName,
                            CatchmentAreaAndFlowPath = uniqueCatchmentAreaFileName,
                            CrossSectionOrCalculationSheetReportPath = uniqueCrossSectionOrCalculationFileName,
                            DistanceFromCreekPath = uniqueDistanceFromCreekFileName,
                            DrainLSectionPath = uniqueLSectionOfDrainFileName,
                            GISOrDWSReportPath= uniqueGisOrDwsFileName,
                            KmlFileVerificationReportPath = uniqueKmlFileName
                        };

                        approvalDetail.GrantApprovalProcessDocuments=approvalObj;
                        await _repoApprovalDetail.UpdateAsync(approvalDetail);

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "All documents are required to be uploaded");
                        return View(model);
                    }
                }
                else
                {
                    await _repoApprovalDetail.CreateAsync(approvalDetail);
                }

                grant.IsForwarded = true;
                grant.ProcessLevel = approvalDetail.ProcessLevel;
                grant.UpdatedOn = DateTime.Now;
                await _repo.UpdateAsync(grant);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty,ex.Message);
                return View(model);
            }
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

            var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForRejection(grant.ApplicantName, grant.ApplicationID));
            _emailService.SendEmail(emailModel, "Punjab Irrigation Department");

            return RedirectToAction("Index"); 
        }

        public IActionResult RedirectToExternalSite(string siteAddress)
        {
            return Redirect(siteAddress);
        }

        public IActionResult Download(string fileName)
        {
            // Replace "path_to_your_file" with the actual path to your file
            string relativeFilePath = "../wwwroot/Documents/"+ fileName;
            string filePath= Path.Combine(_hostingEnvironment.WebRootPath, relativeFilePath);
            //var fileName = "your_file_name.extension"; // Specify the file name

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            // Determine the MIME type
            var mimeType = "application/octet-stream"; // Default MIME type
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (provider.TryGetContentType(fileName, out var resolvedContentType))
            {
                mimeType = resolvedContentType;
            }

            // Return the file
            return PhysicalFile(filePath, mimeType, fileName);
        }

        public async Task<IActionResult> ViewApplication(string Id)
        {
            try
            {
                GrantDetails obj = (await _repo.FindAsync(x => x.ApplicationID.ToLower() == Id.ToLower())).FirstOrDefault();
                VillageDetails village = await _villageRpo.GetByIdAsync(obj.VillageID);
                TehsilBlockDetails tehsil = await _tehsilBlockRepo.GetByIdAsync(village.TehsilBlockId);
                SubDivisionDetails subDivision = await _subDivisionRepo.GetByIdAsync(tehsil.SubDivisionId);
                DivisionDetails division = await _divisionRepo.GetByIdAsync(subDivision.DivisionId);
                NocPermissionTypeDetails permission = await _nocPermissionTypeRepo.GetByIdAsync(obj.NocPermissionTypeID);
                NocTypeDetails noctype = await _nocTypeRepo.GetByIdAsync(obj.NocTypeId);
                SiteAreaUnitDetails unit = await _siteUnitsRepo.GetByIdAsync(obj.SiteAreaUnitId);
                List<GrantKhasraDetails> khasras = (await _khasraRepo.FindAsync(x => x.GrantID == obj.Id)).ToList();
                List<OwnerDetails> owners = (await _grantOwnersRepo.FindAsync(x => x.GrantId == obj.Id)).ToList();
                List<OwnerTypeDetails> ownertype = new List<OwnerTypeDetails>();
                foreach (OwnerDetails item in owners)
                {
                    OwnerTypeDetails type = await _ownerTypeRepo.GetByIdAsync(item.OwnerTypeId);
                    item.OwnerType = type;
                }
                double totalArea = 0;
                foreach (GrantKhasraDetails item in khasras)
                {
                    double marla = 0, kanal = 0, sarsai = 0, biswansi = 0, biswa = 0, bigha = 0;
                    if (unit.Name == @"Marla/Kanal/Sarsai")
                    {
                        marla = item.MarlaOrBiswansi / 160;
                        kanal = item.KanalOrBiswa / 8;
                        sarsai = item.SarsaiOrBigha / 1440;
                    }
                    else
                    {
                        biswansi = item.MarlaOrBiswansi * 0.000625;
                        biswa = item.KanalOrBiswa * 0.0125;
                        bigha = item.SarsaiOrBigha * 0.25;
                    }
                    totalArea = Math.Round(totalArea + marla + kanal + sarsai + biswansi + biswa + bigha, 4);
                }
                var payment = await _grantPaymentRepo.FindAsync(x => x.GrantID == obj.Id);
                if (payment == null || payment.Count() == 0)
                {
                    GrantDetailViewModel model = new GrantDetailViewModel
                    {
                        Id = obj.Id,
                        ApplicationID = obj.ApplicationID,
                        PaymentOrderId = "0",
                        Name = obj.Name,
                        VillageName = village.Name,
                        TehsilBlockName = tehsil.Name,
                        SubDivisionName = subDivision.Name,
                        DivisionName = division.Name,
                        AddressProofPhotoPath = obj.AddressProofPhotoPath,
                        ApplicantEmailID = obj.ApplicantEmailID,
                        ApplicantName = obj.ApplicantName,
                        AuthorizationLetterPhotoPath = obj.AuthorizationLetterPhotoPath,
                        Hadbast = obj.Hadbast,
                        IDProofPhotoPath = obj.IDProofPhotoPath,
                        KMLFilePath = obj.KMLFilePath,
                        KmlLinkName = obj.KMLLinkName,
                        NocNumber = obj.NocNumber,
                        NocPermissionTypeName = permission.Name,
                        NocTypeName = noctype.Name,
                        OtherProjectTypeDetail = obj.OtherProjectTypeDetail,
                        Pincode = village.PinCode.ToString(),
                        PlotNo = obj.PlotNo,
                        PreviousDate = string.Format("{0:dd/MM/yyyy}", obj.PreviousDate),
                        ProjectTypeName = obj.Name,
                        SiteAreaUnitName = unit.Name,
                        TotalArea = totalArea.ToString(),
                        TotalAreaSqFeet = (totalArea * 43560).ToString(),
                        TotalAreaSqMetre = (totalArea * 4046.86).ToString(),
                        Owners = owners,
                        Khasras = khasras,
                        LocationDetail= "Hadbast: " + obj.Hadbast + ", Plot No: " + obj.PlotNo + ", Division: " + division.Name + ", Sub-Division: " + subDivision.Name + ", Tehsil/Block: " + tehsil.Name + ", Village: " + village.Name + ", Pincode: " + village.PinCode
                    };
                    return View(model);
                }
                else
                {
                    GrantPaymentDetails objPyment = (payment).FirstOrDefault();
                    GrantDetailViewModel model = new GrantDetailViewModel
                    {
                        Id = obj.Id,
                        ApplicationID = obj.ApplicationID,
                        PaymentOrderId = objPyment.PaymentOrderId,
                        Name = obj.Name,
                        VillageName = village.Name,
                        TehsilBlockName = tehsil.Name,
                        SubDivisionName = subDivision.Name,
                        DivisionName = division.Name,
                        AddressProofPhotoPath = obj.AddressProofPhotoPath,
                        ApplicantEmailID = obj.ApplicantEmailID,
                        ApplicantName = obj.ApplicantName,
                        AuthorizationLetterPhotoPath = obj.AuthorizationLetterPhotoPath,
                        Hadbast = obj.Hadbast,
                        IDProofPhotoPath = obj.IDProofPhotoPath,
                        KMLFilePath = obj.KMLFilePath,
                        KmlLinkName = obj.KMLLinkName,
                        NocNumber = obj.NocNumber,
                        NocPermissionTypeName = permission.Name,
                        NocTypeName = noctype.Name,
                        OtherProjectTypeDetail = obj.OtherProjectTypeDetail,
                        Pincode = village.PinCode.ToString(),
                        PlotNo = obj.PlotNo,
                        PreviousDate = string.Format("{0:dd/MM/yyyy}", obj.PreviousDate),
                        ProjectTypeName = obj.Name,
                        SiteAreaUnitName = unit.Name,
                        TotalArea = totalArea.ToString(),
                        TotalAreaSqFeet = (totalArea * 43560).ToString(),
                        TotalAreaSqMetre = (totalArea * 4046.86).ToString(),
                        Owners = owners,
                        Khasras = khasras,
                        LocationDetail = "Hadbast: " + obj.Hadbast + ", Plot No: " + obj.PlotNo + ", Division: " + division.Name + ", Sub-Division: " + subDivision.Name + ", Tehsil/Block: " + tehsil.Name + ", Village: " + village.Name + ", Pincode: " + village.PinCode
                    };
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        private bool AllowedFileSize(IFormFile file)
        {
            var maxSize = 4 * 1024 * 1024;
            if (file.Length > maxSize) // 4MB limit
            {
                return false;
            }
            return true;
        }
        private int AllowedCheckExtensions(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var allowedExtensions = new[] { ".pdf" };
                var fileExtension = Path.GetExtension(file.FileName);

                if (!allowedExtensions.Contains(fileExtension.ToLower()))
                {
                    return 0;
                }
                return 1;
            }
            else return 2;
        }

        [Obsolete]
        private string ProcessUploadedFile(IFormFile file, string prefixName)
        {
            string uniqueFileName = null;
            if (file != null && file.Length > 0)
            {

                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");
                uniqueFileName = prefixName + "_" + Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }

    public class GrantUnprocessedAppDetailsComparer : IEqualityComparer<GrantUnprocessedAppDetails>
    {
        public bool Equals(GrantUnprocessedAppDetails x, GrantUnprocessedAppDetails y)
        {
            // Compare based on Id property
            return x.Id == y.Id;
        }

        public int GetHashCode(GrantUnprocessedAppDetails obj)
        {
            // Get hash code of Id property
            return obj.Id.GetHashCode();
        }
    }
}
