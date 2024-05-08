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
using System.Threading.Tasks.Dataflow;
using System.Data;

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
        private readonly IRepository<GrantUnprocessedAppDetails> _grantUnprocessedAppDetailsRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IEmailService _emailService;
        private readonly ICalculations _calculations;
        private readonly IRepository<SiteUnitMaster> _repoSiteUnitMaster;

        public ApprovalProcessController(IRepository<GrantDetails> repo, IRepository<VillageDetails> villageRepo, IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<SubDivisionDetails> subDivisionRepo,
            IRepository<DivisionDetails> divisionRepo, IRepository<UserDivision> userDivisionRepository, UserManager<ApplicationUser> userManager, IRepository<GrantPaymentDetails> repoPayment, RoleManager<IdentityRole> roleManager
            , IRepository<UserVillage> userVillageRepository, IRepository<GrantApprovalDetail> repoApprovalDetail, IRepository<GrantApprovalMaster> repoApprovalMaster, IRepository<UserSubdivision> userSubDivisionRepository
            , IRepository<ProjectTypeDetails> projectTypeRepo, IRepository<NocPermissionTypeDetails> nocPermissionTypeRepo,
            IRepository<NocTypeDetails> nocTypeRepo, IRepository<OwnerTypeDetails> ownerTypeRepo, IRepository<GrantPaymentDetails> grantPaymentRepo, IRepository<OwnerDetails> grantOwnersRepo,
            IRepository<GrantKhasraDetails> khasraRepo, IRepository<SiteAreaUnitDetails> siteUnitsRepo, IWebHostEnvironment hostingEnvironment, IEmailService emailService
            , IRepository<GrantApprovalProcessDocumentsDetails> repoApprovalDocument,IRepository<GrantUnprocessedAppDetails> grantUnprocessedAppDetailsRepo
            , ICalculations calculations, IRepository<SiteUnitMaster> repoSiteUnitMaster)
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
            _grantUnprocessedAppDetailsRepo = grantUnprocessedAppDetailsRepo;
            _calculations = calculations;
            _repoSiteUnitMaster = repoSiteUnitMaster;

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
                model=await _grantUnprocessedAppDetailsRepo.ExecuteStoredProcedureAsync<GrantUnprocessedAppDetails>("getapplicationstoforward", "0", "0", "0", "0", "0", "'" +role+"'", "'" + userId + "'");
                
                return View(model);
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
                
                double total = 0;
                var divisionDetail = (from g in (await _repo.FindAsync(x => x.ApplicationID == Id))
                                   join v in _villageRpo.GetAll() on g.VillageID equals (v.Id)
                                   join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals (t.Id)
                                join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals (sub.Id)
                                select new
                                   {
                                       DivisionId = sub.DivisionId,
                                       SubDivisionId = t.SubDivisionId
                                }
                         ).ToList().FirstOrDefault();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Retrieve the user object
                var userDetail = await _userManager.FindByIdAsync(userId);

                // Retrieve roles associated with the user
                var role = (await _userManager.GetRolesAsync(userDetail)).FirstOrDefault();
                
                // Get the users in the role
                List<UserLocationDetails> users = new List<UserLocationDetails>();
                //List<UserLocationDetails> users = (role == "JUNIOR ENGINEER"
                //    ? (await _userSubDivisionRepository.FindAsync(x => x.SubdivisionId == divisionDetail.SubDivisionId)).Select(x=>new UserLocationDetails { UserId=x.UserId,SubDivisionId=x.SubdivisionId,DivisionId=0,TehsilBlockId=0,VillageId=0}).ToList()
                //    : role == "SUB DIVISIONAL OFFICER" ? 
                //    (await _userDivisionRepository.FindAsync(x => x.DivisionId == divisionDetail.DivisionId)).Select(x => new UserLocationDetails { UserId = x.UserId, SubDivisionId = 0, DivisionId = x.DivisionId, TehsilBlockId = 0, VillageId = 0 }).ToList()
                //    : (await _userVillageRepository.FindAsync(x => x.VillageId == grant.VillageID)).Select(x => new UserLocationDetails { UserId = x.UserId, SubDivisionId = 0, DivisionId = 0, TehsilBlockId = 0, VillageId = x.VillageId }).ToList());
                List<SubDivisionDetails> subdivisions = new List<SubDivisionDetails>();
                
                
                string forwardToRole = "JUNIOR ENGINEER";
                
                //string forwardToRole = role == "JUNIOR ENGINEER" ? "SUB DIVISIONAL OFFICER" : role == "SUB DIVISIONAL OFFICER" ? "EXECUTIVE ENGINEER" : "JUNIOR ENGINEER".ToUpper();
                
                List<OfficerDetails> officerDetail = new List<OfficerDetails>();
                
                if (role == "EXECUTIVE ENGINEER" && grant.IsForwarded == false)
                {
                    var units = await _repoSiteUnitMaster.FindAsync(x => x.SiteAreaUnitId == grant.SiteAreaUnitId);
                    SiteUnitMaster k = units.Where(x => x.UnitCode.ToUpper() == "K").FirstOrDefault();
                    SiteUnitMaster m = units.Where(x => x.UnitCode.ToUpper() == "M").FirstOrDefault();
                    SiteUnitMaster s = units.Where(x => x.UnitCode.ToUpper() == "S").FirstOrDefault();
                    total = Math.Round(((from kh in _khasraRepo.GetAll()
                                         where kh.GrantID == grant.Id
                                         select new
                                         {
                                             TotalArea = ((kh.KanalOrBigha*k.UnitValue*k.Timesof)/k.DivideBy)+ ((kh.MarlaOrBiswa * m.UnitValue * m.Timesof) / m.DivideBy)+ ((kh.SarsaiOrBiswansi * s.UnitValue * s.Timesof) / s.DivideBy)
                                            
                                         }).Sum(d => d.TotalArea)), 4);
                    subdivisions = (from u in _userDivisionRepository.GetAll()
                                    join sub in _subDivisionRepo.GetAll() on u.DivisionId equals (sub.DivisionId)
                                    where u.UserId == userId
                                    select new SubDivisionDetails
                                    {
                                        Id = sub.Id,
                                        Name = sub.Name
                                    }
                                    ).ToList();
                }
                else
                {
                    switch (role)
                    {
                        case "JUNIOR ENGINEER":
                            users = (await _userSubDivisionRepository.FindAsync(x => x.SubdivisionId == divisionDetail.SubDivisionId)).Select(x => new UserLocationDetails { UserId = x.UserId, SubDivisionId = x.SubdivisionId, DivisionId = 0, TehsilBlockId = 0, VillageId = 0 }).ToList();
                            break;
                        case "SUB DIVISIONAL OFFICER":
                            users = (await _userDivisionRepository.FindAsync(x => x.DivisionId == divisionDetail.DivisionId)).Select(x => new UserLocationDetails { UserId = x.UserId, SubDivisionId = 0, DivisionId = x.DivisionId, TehsilBlockId = 0, VillageId = 0 }).ToList();
                            break;
                        case "EXECUTIVE ENGINEER":
                            users = grant.IsForwarded == false
                                ? (await _userVillageRepository.FindAsync(x => x.VillageId == grant.VillageID)).Select(x => new UserLocationDetails { UserId = x.UserId, SubDivisionId = 0, DivisionId = 0, TehsilBlockId = 0, VillageId = x.VillageId }).ToList()
                                : (await _userDivisionRepository.FindAsync(x => x.DivisionId == divisionDetail.DivisionId)).Select(x => new UserLocationDetails { UserId = x.UserId, SubDivisionId = 0, DivisionId = x.DivisionId, TehsilBlockId = 0, VillageId = 0 }).ToList();
                            break;
                        default:
                            users = (await _userDivisionRepository.FindAsync(x => x.DivisionId == divisionDetail.DivisionId)).Select(x => new UserLocationDetails { UserId = x.UserId, SubDivisionId = 0, DivisionId = x.DivisionId, TehsilBlockId = 0, VillageId = 0 }).ToList();
                            break;
                    }
                    switch (role)
                    {
                        case "JUNIOR ENGINEER":
                            forwardToRole = "SUB DIVISIONAL OFFICER";
                            break;
                        case "SUB DIVISIONAL OFFICER":
                            forwardToRole = "EXECUTIVE ENGINEER";
                            break;
                        case "EXECUTIVE ENGINEER":
                            forwardToRole = grant.IsForwarded == false ? "JUNIOR ENGINEER" : "CIRCLE OFFICER";
                            break;
                        case "CIRCLE OFFICER":
                            forwardToRole = "DWS";
                            break;
                        case "DWS":
                            forwardToRole = "EXECUTIVE ENGINEER HQ";
                            break;
                        case "EXECUTIVE ENGINEER HQ":
                            forwardToRole = "CHIEF ENGINEER HQ";
                            break;
                        case "CHIEF ENGINEER HQ":
                            forwardToRole = "PRINCIPAL SECRETARY";
                            break;
                        default:
                            forwardToRole = "JUNIOR ENGINEER"; break;
                    }
                    var usersInRole = (await _userManager.GetUsersInRoleAsync(forwardToRole));
                    officerDetail = (from u in users
                                     join ur in usersInRole on u.UserId equals ur.Id
                                     select new OfficerDetails
                                     {
                                         UserId = u.UserId,
                                         UserName = ur.UserName
                                     }
                                  ).ToList();
                }

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
                                                          IsForwarded=g.IsForwarded,
                                                          Name = g.Name,
                                                          TotalArea = total,
                                                          ApplicantEmailID = g.ApplicantEmailID,
                                                          ApplicantName = g.ApplicantName,
                                                          ApplicationID = g.ApplicationID,
                                                          ForwardToRole= forwardToRole,
                                                          LoggedInRole = role,
                                                          SubDivisions= subdivisions!=null? new SelectList(subdivisions, "Id", "Name") : null,
                                                          Officers = officerDetail!=null? new SelectList(officerDetail, "UserId", "UserName"):null,
                                                          LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + v.Name + ", Pincode: " + v.PinCode,
                                                      }).FirstOrDefault();
                
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
                var units = await _repoSiteUnitMaster.FindAsync(x => x.SiteAreaUnitId == grant.SiteAreaUnitId);
                SiteUnitMaster k = units.Where(x => x.UnitCode.ToUpper() == "K").FirstOrDefault();
                SiteUnitMaster m = units.Where(x => x.UnitCode.ToUpper() == "M").FirstOrDefault();
                SiteUnitMaster s = units.Where(x => x.UnitCode.ToUpper() == "S").FirstOrDefault();
                
                var total = Math.Round(((from kh in _khasraRepo.GetAll()
                                         where kh.GrantID == grant.Id
                                         select new
                                         {
                                             TotalArea = ((kh.KanalOrBigha * k.UnitValue * k.Timesof) / k.DivideBy) + ((kh.MarlaOrBiswa * m.UnitValue * m.Timesof) / m.DivideBy) + ((kh.SarsaiOrBiswansi * s.UnitValue * s.Timesof) / s.DivideBy)

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

                if (total<=2 && role=="JUNIOR ENGINEER")
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
                            KmlFileVerificationReportPath = uniqueKmlFileName,
                            ProcessedBy = User.Identity.Name,
                            ProcessedOn = DateTime.Now,
                            ProcessedByRole = role
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

        [HttpGet]
        public ViewResult EditApprovalDocuments(string grantId,long docId)
        {
            try
            {
                if (grantId == null || grantId==string.Empty)
                {
                    ViewBag.ErrorMessage = $"Id = {grantId} cannot be found";

                    return View("NotFound");
                }
                ApprovalDocumentsViewModelEdit model = new ApprovalDocumentsViewModelEdit
                {
                    GrantApprovalDocId= docId
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
        [Obsolete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditApprovalDocuments(ApprovalDocumentsViewModelEdit model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    GrantApprovalProcessDocumentsDetails obj=await _repoApprovalDocument.GetBylongIdAsync(model.GrantApprovalDocId);
                    if (obj == null)
                    {
                        ViewBag.ErrorMessage = $"Document with Id = {obj.Id} cannot be found";
                        return View("NotFound");
                    }
                    else
                    {
                        //if (model.SiteConditionReportFile != null && model.CatchmentAreaFile != null && model.DistanceFromCreekFile != null && model.GisOrDwsFile != null && model.KmlFile != null && model.CrossSectionOrCalculationFile != null && model.LSectionOfDrainFile != null)
                        //{
                        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                        // Retrieve the user object
                        var user = await _userManager.FindByIdAsync(userId);

                        // Retrieve roles associated with the user
                        var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
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


                        obj.SiteConditionReportPath = uniqueSiteConditionFileName;
                        obj.CatchmentAreaAndFlowPath = uniqueCatchmentAreaFileName;
                        obj.CrossSectionOrCalculationSheetReportPath = uniqueCrossSectionOrCalculationFileName;
                        obj.DistanceFromCreekPath = uniqueDistanceFromCreekFileName;
                        obj.DrainLSectionPath = uniqueLSectionOfDrainFileName;
                        obj.GISOrDWSReportPath = uniqueGisOrDwsFileName;
                        obj.KmlFileVerificationReportPath = uniqueKmlFileName;
                        //}
                        //else
                        //{
                        //    ModelState.AddModelError(string.Empty, "All documents are required to be uploaded");
                        //    return View(model);
                        //}
                        obj.UpdatedOn = DateTime.Now;
                        obj.UpdatedBy = user.UserName;
                        obj.UpdatedByRole = role;
                        await _repoApprovalDocument.UpdateAsync(obj);
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Reject(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";
                return View("NotFound");
            }
            GrantApprovalDetailReject model = new GrantApprovalDetailReject
            {
                id=id
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(GrantApprovalDetailReject model)
       {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.id))
                {
                    ViewBag.ErrorMessage = $"Grant with Application Id = {model.id} cannot be found";
                    return View("NotFound");
                }

                GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == model.id)).FirstOrDefault();

                if (grant == null)
                {
                    ViewBag.ErrorMessage = $"Grant with Application Id = {model.id} cannot be found";

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
                    GrantID = grant.Id,
                    ApprovalID = master.Id,
                    ProcessedBy = User.Identity.Name,
                    ProcessedOn = DateTime.Now,
                    ProcessedByRole = role,
                    ProcessLevel = rejectionLevel + 1,
                    ProcessedToRole = "",
                    ProcessedToUser = "",
                    Remarks = model.Remarks
                };

                await _repoApprovalDetail.CreateAsync(approvalDetail);
                //_repoApprovalDetail.
                grant.IsRejected = true;
                grant.ProcessLevel = approvalDetail.ProcessLevel;
                grant.IsPending = false;
                grant.UpdatedOn = DateTime.Now;
                await _repo.UpdateAsync(grant);

                var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForRejection(grant.ApplicantName, grant.ApplicationID, model.Remarks));
                _emailService.SendEmail(emailModel, "Punjab Irrigation Department");

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> IssueNOC(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";
                return View("NotFound");
            }

            GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == id)).FirstOrDefault();

            if (grant == null)
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";

                return View("NotFound");
            }
            IssueNocViewModelCreate model = (from g in _repo.GetAll()
                                             join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                                             join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                                             join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                                             join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals sub.Id
                                             join div in _divisionRepo.GetAll() on sub.DivisionId equals div.Id
                                             where g.ApplicationID == id
                                             select new IssueNocViewModelCreate
                                             {
                                                 Id = g.Id,
                                                 Name = g.Name,
                                                 ApplicationID = g.ApplicationID,
                                                 LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + v.Name + ", Pincode: " + v.PinCode,
                                             }).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        [Obsolete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IssueNOC(IssueNocViewModelCreate model)
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

                string ErrorMessage = string.Empty;
                int certificateValidation = AllowedCheckExtensions(model.CertificateFile);
                if (certificateValidation == 0)
                {
                    ErrorMessage = $"Invalid certificate file type. Please upload a PDF file only";
                    ModelState.AddModelError("", ErrorMessage);

                    return View(model);

                }
                else if (certificateValidation == 2)
                {
                    ErrorMessage = "Certificate field is required";
                    ModelState.AddModelError("", ErrorMessage);
                    return View(model);
                }

                if (!AllowedFileSize(model.CertificateFile))
                {
                    ErrorMessage = "Certificate size exceeds the allowed limit of 4MB";
                    ModelState.AddModelError("", ErrorMessage);
                    return View(model);
                }
                string uniqueCertificateFileName = ProcessUploadedFile(model.CertificateFile, "noc");


                //GrantApprovalProcessDocumentsDetails approvalObj = new GrantApprovalProcessDocumentsDetails
                //{
                //    SiteConditionReportPath = uniqueSiteConditionFileName,
                //    CatchmentAreaAndFlowPath = uniqueCatchmentAreaFileName,
                //    CrossSectionOrCalculationSheetReportPath = uniqueCrossSectionOrCalculationFileName,
                //    DistanceFromCreekPath = uniqueDistanceFromCreekFileName,
                //    DrainLSectionPath = uniqueLSectionOfDrainFileName,
                //    GISOrDWSReportPath = uniqueGisOrDwsFileName,
                //    KmlFileVerificationReportPath = uniqueKmlFileName,
                //    ProcessedBy = User.Identity.Name,
                //    ProcessedOn = DateTime.Now,
                //    ProcessedByRole = role
                //};
                // Get the current user's ID
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user object
            var user = await _userManager.FindByIdAsync(userId);

            // Retrieve roles associated with the user
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            GrantApprovalMaster master = (await _repoApprovalMaster.FindAsync(x => x.Code == "A")).FirstOrDefault();
            int approvalLevel = (await _repoApprovalDetail.FindAsync(x => x.GrantID == grant.Id && x.ApprovalID == master.Id)).Count();
            GrantApprovalDetail approvalDetail = new GrantApprovalDetail
            {
                GrantID = grant.Id,
                ApprovalID = master.Id,
                ProcessedBy = User.Identity.Name,
                ProcessedOn = DateTime.Now,
                ProcessedByRole = role,
                ProcessLevel = approvalLevel + 1,
                ProcessedToRole = "",
                ProcessedToUser = ""
            };

            await _repoApprovalDetail.CreateAsync(approvalDetail);
            //_repoApprovalDetail.
            grant.IsApproved = true;
            grant.IsForwarded = false;
            grant.ProcessLevel = approvalDetail.ProcessLevel;
            grant.IsPending = false;
            grant.CertificateFilePath = uniqueCertificateFileName;
            grant.UploadedByRole=role;
            grant.UploadedBy = User.Identity.Name;
            grant.UploadedOn = DateTime.Now;
            await _repo.UpdateAsync(grant);

            //var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForRejection(grant.ApplicantName, grant.ApplicationID));
            //_emailService.SendEmail(emailModel, "Punjab Irrigation Department");

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
                    SiteUnitsViewModel unitDetails = new SiteUnitsViewModel
                    {
                        KanalOrBigha = item.KanalOrBigha,
                        MarlaOrBiswa = item.MarlaOrBiswa,
                        SarsaiOrBiswansi = item.SarsaiOrBiswansi,
                        SiteUnitId = unit.Id
                    };
                    var units = await _calculations.CalculateUnits(unitDetails);
                    totalArea = Math.Round(totalArea + units.KanalOrBigha + units.MarlaOrBiswa + units.SarsaiOrBiswansi, 4);
                   
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

        [HttpPost]
        public async Task<IActionResult> GetOfficers(int subdivisionId)
        {
            List<UserLocationDetails> users = new List<UserLocationDetails>();
            users = (from te in _tehsilBlockRepo.GetAll() 
                                   join v in _villageRpo.GetAll() on te.Id equals(v.TehsilBlockId)
                                   join uv in _userVillageRepository.GetAll() on v.Id equals(uv.VillageId)
                                   where te.SubDivisionId==subdivisionId
                                   select new UserLocationDetails
                                   {
                                       UserId = uv.UserId,
                                       SubDivisionId = 0,
                                       DivisionId = 0,
                                       TehsilBlockId = 0,
                                       //VillageId = uv.VillageId
                                   }
                                   ).Distinct().ToList();

            string forwardToRole = "JUNIOR ENGINEER";
            var usersInRole = (await _userManager.GetUsersInRoleAsync(forwardToRole));
            List<OfficerDetails> officerDetail = (from u in users
                             join ur in usersInRole on u.UserId equals ur.Id
                             select new OfficerDetails
                             {
                                 UserId = u.UserId,
                                 UserName = ur.UserName
                             }
                          ).ToList();
            return Json(new SelectList(officerDetail, "UserId", "UserName"));
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
