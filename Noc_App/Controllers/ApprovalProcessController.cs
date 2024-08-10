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
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Noc_App.Controllers
{
    public class ApprovalProcessController : Controller
    {
        private readonly IRepository<GrantDetails> _repo;
        //private readonly IRepository<VillageDetails> _villageRpo;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepo;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepo;
        private readonly IRepository<DivisionDetails> _divisionRepo;
        //private readonly IRepository<UserDivision> _userDivisionRepository;
        //private readonly IRepository<UserSubdivision> _userSubDivisionRepository;
        //private readonly IRepository<UserVillage> _userVillageRepository;
        private readonly IRepository<GrantPaymentDetails> _repoPayment;
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
        private readonly IRepository<RecommendationDetail> _repoRecommendation;
        private readonly IRepository<UserRoleDetails> _userRolesRepository;
        private readonly IRepository<GrantSectionsDetails> _grantsectionRepository;
        private readonly IRepository<GrantRejectionShortfallSection> _grantrejectionRepository;
        private readonly IRepository<PlanSanctionAuthorityMaster> _repoPlanSanctionAuthtoryMaster;
        private readonly IRepository<DrainWidthTypeDetails> _drainwidthRepository;
        private readonly IRepository<GrantFileTransferDetails> _grantFileTransferRepository;
        private readonly IRepository<MasterPlanDetails> _masterPlanDetailsRepository;

        public ApprovalProcessController(IRepository<GrantDetails> repo, /*IRepository<VillageDetails> villageRepo,*/ IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<SubDivisionDetails> subDivisionRepo,
            IRepository<DivisionDetails> divisionRepo, IRepository<GrantPaymentDetails> repoPayment,IRepository<GrantApprovalDetail> repoApprovalDetail, IRepository<GrantApprovalMaster> repoApprovalMaster
            , IRepository<ProjectTypeDetails> projectTypeRepo, IRepository<NocPermissionTypeDetails> nocPermissionTypeRepo, IRepository<MasterPlanDetails> masterPlanDetailsRepository,
            IRepository<NocTypeDetails> nocTypeRepo, IRepository<OwnerTypeDetails> ownerTypeRepo, IRepository<GrantPaymentDetails> grantPaymentRepo, IRepository<OwnerDetails> grantOwnersRepo,
            IRepository<GrantKhasraDetails> khasraRepo, IRepository<SiteAreaUnitDetails> siteUnitsRepo, IWebHostEnvironment hostingEnvironment, IEmailService emailService
            , IRepository<GrantApprovalProcessDocumentsDetails> repoApprovalDocument,IRepository<GrantUnprocessedAppDetails> grantUnprocessedAppDetailsRepo
            , ICalculations calculations, IRepository<SiteUnitMaster> repoSiteUnitMaster, IRepository<RecommendationDetail> repoRecommendation
            , IRepository<UserRoleDetails> userRolesRepository, IRepository<GrantSectionsDetails> grantsectionRepository, IRepository<DrainWidthTypeDetails> drainwidthRepository
            , IRepository<GrantRejectionShortfallSection> grantrejectionRepository, IRepository<PlanSanctionAuthorityMaster> repoPlanSanctionAuthtoryMaster, IRepository<GrantFileTransferDetails> grantFileTransferRepository)
        {
            _repo = repo;
            //_villageRpo = villageRepo;
            _tehsilBlockRepo = tehsilBlockRepo;
            _subDivisionRepo = subDivisionRepo;
            _divisionRepo = divisionRepo;
            _repoPayment = repoPayment;
            _repoApprovalDetail = repoApprovalDetail;
            _repoApprovalMaster = repoApprovalMaster;
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
            _repoRecommendation = repoRecommendation;
            _userRolesRepository = userRolesRepository;
            _grantsectionRepository = grantsectionRepository;
            _drainwidthRepository = drainwidthRepository;
            _grantrejectionRepository = grantrejectionRepository;
            _repoPlanSanctionAuthtoryMaster = repoPlanSanctionAuthtoryMaster;
            _grantFileTransferRepository= grantFileTransferRepository;
            _masterPlanDetailsRepository=masterPlanDetailsRepository;
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        public async Task<ViewResult> Index()
        {
            try
            {

                string userId=LoggedInUserID();

                    string divisionId = LoggedInDivisionID();
                
                // Retrieve roles associated with the user
                var roleName = LoggedInRoleName();
                string role = roleName;
                role = (await GetAppRoleName(role)).AppRoleName;
                List<List<GrantUnprocessedAppDetails>> modelView = new List<List<GrantUnprocessedAppDetails>>();
                List<GrantUnprocessedAppDetails> model = new List<GrantUnprocessedAppDetails>();
                model=await _grantUnprocessedAppDetailsRepo.ExecuteStoredProcedureAsync<GrantUnprocessedAppDetails>("getapplicationstoforward", "0", "0", "0", "0", divisionId, "'" +role+"'", "'" + userId + "'");
                var modelForwarded = await _grantUnprocessedAppDetailsRepo.ExecuteStoredProcedureAsync<GrantUnprocessedAppDetails>("getapplicationsforwarded", "0", "0", "0", "0", divisionId, "'" + role + "'", "'" + userId + "'");
                var modelRejected = await _grantUnprocessedAppDetailsRepo.ExecuteStoredProcedureAsync<GrantUnprocessedAppDetails>("getapplicationsrejected", "0", "0", "0", "0", divisionId, "'" + role + "'", "'" + userId + "'");
                var modelIssued = await _grantUnprocessedAppDetailsRepo.ExecuteStoredProcedureAsync<GrantUnprocessedAppDetails>("getapplicationsissued", "0", "0", "0", "0", divisionId, "'" + role + "'", "'" + userId + "'");
                //var modelShortfall = await _grantUnprocessedAppDetailsRepo.ExecuteStoredProcedureAsync<GrantUnprocessedAppDetails>("getapplicationsexpiredshortfall", "0", "0", "0", "0", "0", "'" + role + "'", "'" + userId + "'");

                modelView.Add(model);
                modelView.Add(modelForwarded);
                modelView.Add(modelRejected);
                modelView.Add(modelIssued);
                //modelView.Add(modelShortfall);

                return View(modelView);
            }
           catch(Exception ex){
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [HttpGet]
        public IActionResult ShortFall(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";
                return View("NotFound");
            }
            List<GrantSectionsDetails> sections = _grantsectionRepository.GetAll().Where(x=>x.SectionCode!="KH").OrderBy(x=>x.SectionName).ToList();
            GrantApprovalDetailShortfall model = new GrantApprovalDetailShortfall
            {
                id = id,
                GrantSectionsDetailsList=sections
                
            };
            return View(model);
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShortFall(GrantApprovalDetailShortfall model)
        {
            try
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

                    string userId = LoggedInUserID();

                    string divisionId = LoggedInDivisionID();

                    string subdivisionId = LoggedInSubDivisionID();

                    string username = LoggedInUserName();

                    string designation = LoggedInDesignationName();

                    // Retrieve roles associated with the user
                    var roleName = LoggedInRoleName();

                    string role = roleName;
                    GrantApprovalMaster master = (await _repoApprovalMaster.FindAsync(x => x.Code == "SF")).FirstOrDefault();
                    int shortfallLevel = (await _repoApprovalDetail.FindAsync(x => x.GrantID == grant.Id && x.ApprovalID == master.Id)).Count();
                    var selectedSectionIds = model.SelectedGrantSectionIDs;
                    List<GrantRejectionShortfallSection> grantSections = new List<GrantRejectionShortfallSection>();


                    GrantApprovalDetail approvalDetail = new GrantApprovalDetail
                    {
                        GrantID = grant.Id,
                        ApprovalID = master.Id,
                        ProcessedBy = userId,
                        ProcessedOn = DateTime.Now,
                        ProcessedByRole = role,
                        ProcessedByName = username + "(" + designation + ")",
                        ProcessLevel = shortfallLevel + 1,
                        ProcessedToRole = "Applicant",
                        ProcessedToUser = "0",
                        ProcessedToName = "Applicant",
                        Remarks = model.Remarks,
                        RecommendationID = 3
                    };

                    await _repoApprovalDetail.CreateAsync(approvalDetail);
                    foreach (var item in selectedSectionIds)
                    {
                        grantSections.Add(new GrantRejectionShortfallSection { SectionId = item, GrantApprovalId = approvalDetail.Id, IsCompleted = 0, CreatedOn = DateTime.Now });
                    }

                    approvalDetail.GrantRejectionShortfallSection = grantSections;
                    await _repoApprovalDetail.UpdateAsync(approvalDetail);

                    grant.IsForwarded = false;
                    grant.IsShortFall = true;
                    grant.ShortFallLevel = approvalDetail.ProcessLevel;
                    grant.IsShortFallCompleted = false;
                    grant.ShortFallReportedOn = DateTime.Now;
                    grant.ShortFallReportedByRole = role;
                    grant.ShortFallReportedById = userId;
                    grant.ShortFallReportedByName = username + "(" + designation + ")";
                    await _repo.UpdateAsync(grant);

                    string domain = HttpContext.Request.Host.Value;
                    string scheme = HttpContext.Request.Scheme;
                    string link = scheme + "://" + domain + Url.Action("Modify", "Grant", new { id = grant.ApplicationID });
                    var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForShortfall(grant.ApplicantName, grant.ApplicationID, model.Remarks, link));
                    _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Something went wrong");
            }
            return View(model);
        }
        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [HttpGet]
        [Obsolete]
        public async Task<IActionResult> TransferFile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";
                return View("NotFound");
            }
            var grantdetail = (from g in _repo.GetAll()
                               //join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                               //join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                               join t in _tehsilBlockRepo.GetAll() on g.TehsilID equals t.Id
                               join sub in _subDivisionRepo.GetAll() on g.SubDivisionId equals sub.Id
                               join div in _divisionRepo.GetAll() on sub.DivisionId equals div.Id
                               
                               where g.ApplicationID == id
                               select new
                               {
                                   Grant = g,
                                   subdiv = sub,
                                   division = div,
                                   tehsil = t
                               }).FirstOrDefault();
            GrantDetails grant = grantdetail.Grant;
            if (grant == null)
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";

                return View("NotFound");
            }
            string userId = LoggedInUserID();

            string divisionId = LoggedInDivisionID();
            List<OfficerDetails> officerDetail = new List<OfficerDetails>();
            officerDetail = (await GetOfficer("0", "EXECUTIVE ENGINEER", "0")).Where(x=>x.UserId!=userId).ToList();

            GrantFileTransferDetailCreate model = new GrantFileTransferDetailCreate
            {
                Name = grant.Name,
                ApplicantEmailID = grantdetail.Grant.ApplicantEmailID,
                ApplicationID = grantdetail.Grant.ApplicationID,
                Officers = officerDetail.Count > 0 ? new SelectList(officerDetail, "UserId", "UserName") : null,
                LocationDetails = "Division: " + grantdetail.division.Name + ", Sub-Division: " + grantdetail.subdiv.Name + ", Tehsil/Block: " + grantdetail.tehsil.Name + ", Village: " + grantdetail.Grant.VillageName + ", Pincode: " + grantdetail.Grant.PinCode,
            };
            return View(model);
        }

        [HttpPost]
        [Obsolete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TransferFile(GrantFileTransferDetailCreate model)
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

                string userId = LoggedInUserID();

                string divisionId = LoggedInDivisionID();
                List<OfficerDetails> officerDetail = new List<OfficerDetails>();
                officerDetail = (await GetOfficer("0", "EXECUTIVE ENGINEER", "0")).Where(x => x.UserId != userId).ToList();
                model.Officers = officerDetail.Count > 0 ? new SelectList(officerDetail, "UserId", "UserName", model.SelectedOfficerId) : null;
                List<OfficerResponseViewModel> officers = (await LoadOfficersAsync("EXECUTIVE ENGINEER", "0", "0")).FindAll(x => x.user_info.EmployeeId == model.SelectedOfficerId);
                OfficerDetail userRole = (from u in _userRolesRepository.GetAll().AsEnumerable()
                                          join officer in officers on u.Id.ToString() equals officer.user_info.RoleID
                                          select new OfficerDetail
                                          {
                                              EmployeeId = officer.user_info.EmployeeId,
                                              RoleName = u.AppRoleName,
                                              RoleId = u.Id.ToString(),
                                              UserName = officer.user_info.EmployeeName + "(" + officer.user_info.DeesignationName + ")",
                                              Designation=officer.user_info.DeesignationName,
                                              Name=officer.user_info.EmployeeName
                                          }).FirstOrDefault();

                
                string username = LoggedInUserName();
                string designation=LoggedInDesignationName();
                GrantFileTransferDetails details = new GrantFileTransferDetails {
                    GrantId = grant.Id,
                    FromAuthorityId=userId,
                    ToAuthorityId=model.SelectedOfficerId,
                    Remarks=model.Remarks,
                    TransferedOn=DateTime.Now,
                    FromName=username,
                    FromDesignationName=designation,
                    ToName=userRole.Name,
                    ToDesignationName=userRole.Designation
                };


                await _grantFileTransferRepository.CreateAsync(details);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [Obsolete]
        public async Task<IActionResult> Transfer(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";
                return View("NotFound");
            }
           
            var grantdetail = (from g in _repo.GetAll()
                               join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                               //join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                               //join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                               join t in _tehsilBlockRepo.GetAll() on g.TehsilID equals t.Id
                               join sub in _subDivisionRepo.GetAll() on g.SubDivisionId equals sub.Id
                               join div in _divisionRepo.GetAll() on sub.DivisionId equals div.Id
                               where a.ProcessedToRole.ToUpper() == "JUNIOR ENGINEER" && g.ApplicationID == id
                               select new
                               {
                                   Grant = g,
                                   subdiv = sub,
                                   division = div,
                                   //village =v,
                                   tehsil=t,
                                   Approval = a
                               }).FirstOrDefault();
           
            GrantDetails grant = grantdetail.Grant;
            if (grant == null)
            {
                ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";

                return View("NotFound");
            }
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //// Retrieve the user object
            //var userDetail = await _userManager.FindByIdAsync(userId);

            string userId = LoggedInUserID();

            string divisionId = LoggedInDivisionID();

            string subdivisionId = LoggedInSubDivisionID();

            // Retrieve roles associated with the user
            var role = LoggedInRoleName();

            //List<SubDivisionDetails> subdivisions = new List<SubDivisionDetails>();
            //subdivisions = (from sub in _subDivisionRepo.GetAll()
            //                where sub.DivisionId == Convert.ToInt32(divisionId)
            //                select new SubDivisionDetails
            //                {
            //                    Id = sub.Id,
            //                    Name = sub.Name
            //                }
            //                        ).ToList();

            List<OfficerDetails> officerDetail = new List<OfficerDetails>();
            officerDetail = await GetOfficer("0", "JUNIOR ENGINEER", "0");//,SUB DIVISIONAL OFFICER

            GrantApprovalDetailTransferCreate model = new GrantApprovalDetailTransferCreate
            {
                id = id,
                //SubDivisions = subdivisions != null ? new SelectList(subdivisions, "Id", "Name") : null,
                 Name = grantdetail.Grant.Name,
                ApplicantEmailID = grantdetail.Grant.ApplicantEmailID,
                ApplicantName = grantdetail.Grant.ApplicantName,
                ApplicationID = grantdetail.Grant.ApplicationID,
                CurrentOfficer=grantdetail.Approval.ProcessedToUser,
                ApprovalId=grantdetail.Approval.Id,
                ForwardToRole="JUNIOR ENGINEER",
                Officers = officerDetail.Count>0 ? new SelectList(officerDetail, "UserId", "UserName") : null,
                LocationDetails = "Division: " + grantdetail.division.Name + ", Sub-Division: " + grantdetail.subdiv.Name + ", Tehsil/Block: " + grantdetail.tehsil.Name + ", Village: " + grantdetail.Grant.VillageName + ", Pincode: " + grantdetail.Grant.PinCode,
            };
            return View(model);
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        [Obsolete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(GrantApprovalDetailTransferCreate model)
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

                // Get the current user's ID
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                //string userId = HttpContext.Session.GetString("Userid");
                string userId = LoggedInUserID();

                string divisionId = LoggedInDivisionID();

                //string subdivisionId = LoggedInSubDivisionID();

                // Retrieve roles associated with the user
                var role = LoggedInRoleName();
                GrantApprovalMaster master = (await _repoApprovalMaster.FindAsync(x => x.Code == "F")).FirstOrDefault();
                List<OfficerResponseViewModel> officers = (await LoadOfficersAsync("JUNIOR ENGINEER",model.SelectedSubDivisionId, "0")).FindAll(x=>x.user_info.EmployeeId==model.SelectedOfficerId);
                OfficerDetail userRole = (from u in _userRolesRepository.GetAll().AsEnumerable()
                                join officer in officers on u.Id.ToString() equals officer.user_info.RoleID
                                select new OfficerDetail
                                {
                                    EmployeeId= officer.user_info.EmployeeId,
                                    RoleName=u.AppRoleName,
                                    RoleId=u.Id.ToString(),
                                    UserName = officer.user_info.EmployeeName+"("+ officer.user_info.DeesignationName + ")"
                                }).FirstOrDefault();
                //var forwardedUser = await _userManager.FindByIdAsync(model.SelectedOfficerId);
                //var forwardedRole = (await _userManager.GetRolesAsync(forwardedUser)).FirstOrDefault();
                //int approvalLevel = (await _repoApprovalDetail.FindAsync(x => x.GrantID == grant.Id && x.ApprovalID == master.Id)).Count();

                //List<GrantApprovalProcessDocumentsDetails> docDetail = (from d in _repoApprovalDocument.GetAll()
                //                                            join a in _repoApprovalDetail.GetAll() on d.GrantApprovalID equals a.Id
                //                                            join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                //                                            where a.GrantID== grant.Id && m.Code=="F" && model.ApprovalId==a.Id
                //                                            select new GrantApprovalProcessDocumentsDetails
                //                                            {
                //                                                CatchmentAreaAndFlowPath=d.CatchmentAreaAndFlowPath,
                //                                                CrossSectionOrCalculationSheetReportPath=d.CrossSectionOrCalculationSheetReportPath,
                //                                                DistanceFromCreekPath=d.DistanceFromCreekPath,
                //                                                DrainLSectionPath=d.DrainLSectionPath,
                //                                                GISOrDWSReportPath=d.GISOrDWSReportPath,
                //                                                GrantApproval=d.GrantApproval,
                //                                                GrantApprovalID=d.GrantApprovalID,
                //                                                Id=d.Id,
                //                                                KmlFileVerificationReportPath=d.KmlFileVerificationReportPath,
                //                                                ProcessedBy= userRole.EmployeeId,
                //                                                ProcessedByRole= userRole.RoleName,
                //                                                SiteConditionReportPath=d.SiteConditionReportPath,
                //                                                ProcessedOn=d.ProcessedOn
                //                                            }
                //                                            ).ToList();
                //GrantApprovalProcessDocumentsDetails doc = new GrantApprovalProcessDocumentsDetails();
                //if (docDetail.Count > 0)
                //{
                //    doc = docDetail.FirstOrDefault();
                //    if (doc != null)
                //        await _repoApprovalDocument.UpdateAsync(doc);
                //}
                GrantApprovalDetail approvalDetail=(await _repoApprovalDetail.FindAsync(x => x.GrantID == grant.Id && x.Id == model.ApprovalId)).FirstOrDefault();

                approvalDetail.ProcessedBy = userId;
                approvalDetail.ProcessedByRole = role;
                approvalDetail.ProcessedToRole = userRole.RoleName;
                approvalDetail.ProcessedToUser = userRole.EmployeeId;
                approvalDetail.ProcessedByName = userRole.UserName;
                approvalDetail.UpdatedOn = DateTime.Now;
                
                await _repoApprovalDetail.UpdateAsync(approvalDetail);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [Obsolete]
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

                GrantApprovalDetail approval=(from a in _repoApprovalDetail.GetAll() where a.GrantID==grant.Id orderby a.Id descending select a).FirstOrDefault();
                
                double total = 0;
                var divisionDetail = (from g in (await _repo.FindAsync(x => x.ApplicationID == Id))
                                        //join v in _villageRpo.GetAll() on g.VillageID equals (v.Id)
                                        //join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals (t.Id)
                                        join sub in _subDivisionRepo.GetAll() on g.SubDivisionId equals (sub.Id)
                                        select new
                                        {
                                            DivisionId = sub.DivisionId,
                                            SubDivisionId = g.SubDivisionId
                                        }
                         ).ToList().FirstOrDefault();
                var typeofwidth = (await _drainwidthRepository.FindAsync(x => x.Code == "C")).FirstOrDefault();
                
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Retrieve the user object
                // var userDetail = await _userManager.FindByIdAsync(userId);

                string userId = LoggedInUserID();

                string divisionId = LoggedInDivisionID();

                string subdivisionId = LoggedInSubDivisionID();

                // Retrieve roles associated with the user
                var roleName = LoggedInRoleName();

                string role = (await GetAppRoleName(roleName)).RoleName;
                // Get the users in the role
                //List<SubDivisionDetails> subdivisions = new List<SubDivisionDetails>();
                
                
                string forwardToRole = "JUNIOR ENGINEER";
                
                List<OfficerDetails> officerDetail = new List<OfficerDetails>();
                if (!grant.IsUnderMasterPlan)
                {
                    var units = await _repoSiteUnitMaster.FindAsync(x => x.SiteAreaUnitId == grant.SiteAreaUnitId);
                    SiteUnitMaster k = units.Where(x => x.UnitCode.ToUpper() == "K").FirstOrDefault();
                    SiteUnitMaster m = units.Where(x => x.UnitCode.ToUpper() == "M").FirstOrDefault();
                    SiteUnitMaster s = units.Where(x => x.UnitCode.ToUpper() == "S").FirstOrDefault();
                    total = Math.Round(((from kh in _khasraRepo.GetAll()
                                         where kh.GrantID == grant.Id
                                         select new
                                         {
                                             TotalArea = ((kh.KanalOrBigha * k.UnitValue * k.Timesof) / k.DivideBy) + ((kh.MarlaOrBiswa * m.UnitValue * m.Timesof) / m.DivideBy) + ((kh.SarsaiOrBiswansi * s.UnitValue * s.Timesof) / s.DivideBy)

                                         }).Sum(d => d.TotalArea)), 5);
                }
                //if (roleName == "EXECUTIVE ENGINEER" && grant.IsForwarded == false)
                //{
                //    subdivisions = (from sub in _subDivisionRepo.GetAll()  
                //                    where sub.DivisionId== Convert.ToInt32(divisionId)
                //                    select new SubDivisionDetails
                //                    {
                //                        Id = sub.Id,
                //                        Name = sub.Name
                //                    }
                //                    ).ToList();
                //}
                //else
                //{
                    switch (roleName)
                    {
                        case "JUNIOR ENGINEER":
                            forwardToRole = "SUB DIVISIONAL OFFICER";
                            break;
                        case "SUB DIVISIONAL OFFICER":
                            forwardToRole = "EXECUTIVE ENGINEER";
                            break;
                        case "EXECUTIVE ENGINEER":
                            if (grant.IsForwarded == false)
                                forwardToRole = "JUNIOR ENGINEER";
                            else if (approval.ProcessedByRole== "SUB DIVISIONAL OFFICER")
                                forwardToRole = "DWS,CIRCLE OFFICER";
                            else forwardToRole = "CIRCLE OFFICER";
                            break;
                        case "CIRCLE OFFICER":
                            forwardToRole = "EXECUTIVE ENGINEER HQ";
                            break;
                        case "DWS":
                            if (approval.ProcessedByRole == "EXECUTIVE ENGINEER")
                                forwardToRole = "ADE";
                            else forwardToRole = "DIRECTOR DRAINAGE";
                            break;
                        case "ADE":
                            forwardToRole = "DWS";
                            break;
                        case "DIRECTOR DRAINAGE":
                            forwardToRole = "EXECUTIVE ENGINEER";
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
                GrantApprovalDetail approvalOfficer = new GrantApprovalDetail();
                //if (forwardToRole == "EXECUTIVE ENGINEER")
                    approvalOfficer = (from a in _repoApprovalDetail.GetAll() where a.GrantID == grant.Id && a.ProcessedByRole==forwardToRole orderby a.ProcessedOn descending select a).FirstOrDefault();
                //UserRoleDetails userRoleDetails =  (await GetAppRoleName(forwardToRole));

                //officerDetail = await GetOfficer(divisionId, userRoleDetails.RoleName,"0"); 

                //officerDetail = forwardToRole== "JUNIOR ENGINEER"? await GetOfficer(divisionId, forwardToRole, "0"): await GetOfficer(divisionId, userRoleDetails.RoleName, "0");
                officerDetail = approvalOfficer == null ? await GetOfficer(divisionId, forwardToRole, "0") :
                    (await GetOfficerLastForwardedBy(forwardToRole, approvalOfficer.ProcessedBy)) ;
                //}
                List<TautologyDetails> tautologyDetails = new List<TautologyDetails>
                {
                    new TautologyDetails{Text="No",Value="false"},
                    new TautologyDetails{Text="Yes",Value="true"}
                };
                List<RecommendationDetail> recommendations = new List<RecommendationDetail>();
                recommendations = _repoRecommendation.GetAll().Where(x=>x.Code!="NA").ToList();
                ForwardApplicationViewModel model = (from g in _repo.GetAll()
                                                      //join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                                                      //join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                                                      join t in _tehsilBlockRepo.GetAll() on g.TehsilID equals t.Id
                                                      join sub in _subDivisionRepo.GetAll() on g.SubDivisionId equals sub.Id
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
                                                          LoggedInRole = roleName,
                                                          Remarks="",
                                                          IsDrainNotified=false,
                                                          TypeOfWidth= typeofwidth.Id,
                                                          IsUnderMasterPlan=g.IsUnderMasterPlan,
                                                          Recommendations = recommendations!=null && recommendations.Count()>0? new SelectList(recommendations, "Id", "Name"):null,
                                                          ConfirmUnderMasterPlan = new SelectList(tautologyDetails, "Value", "Text"),
                                                          Officers = officerDetail!=null?officerDetail.Count()>0? new SelectList(officerDetail, "UserId", "UserName"):null:null,
                                                          LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + g.VillageName + ", Pincode: " + g.PinCode,
                                                      }).FirstOrDefault();
                
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }

        }
        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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
                if (!grant.IsUnderMasterPlan)
                {
                    var units = await _repoSiteUnitMaster.FindAsync(x => x.SiteAreaUnitId == grant.SiteAreaUnitId);
                    SiteUnitMaster k = units.Where(x => x.UnitCode.ToUpper() == "K").FirstOrDefault();
                    SiteUnitMaster m = units.Where(x => x.UnitCode.ToUpper() == "M").FirstOrDefault();
                    SiteUnitMaster s = units.Where(x => x.UnitCode.ToUpper() == "S").FirstOrDefault();
                }
                
                //var total = Math.Round(((from kh in _khasraRepo.GetAll()
                //                         where kh.GrantID == grant.Id
                //                         select new
                //                         {
                //                             TotalArea = ((kh.KanalOrBigha * k.UnitValue * k.Timesof) / k.DivideBy) + ((kh.MarlaOrBiswa * m.UnitValue * m.Timesof) / m.DivideBy) + ((kh.SarsaiOrBiswansi * s.UnitValue * s.Timesof) / s.DivideBy)

                //                         }).Sum(d => d.TotalArea)), 4);
                // Get the current user's ID
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Retrieve the user object
                //var user = await _userManager.FindByIdAsync(userId);

                string userId = LoggedInUserID();

                string username = LoggedInUserName();

                string designation = LoggedInDesignationName();

                string divisionId = LoggedInDivisionID();

                string subdivisionId = LoggedInSubDivisionID();

                // Retrieve roles associated with the user
                var roleName = LoggedInRoleName();

                string role = roleName;


                if (role != "EXECUTIVE ENGINEER" && model.SelectedRecommendationId == 0)
                {
                    ModelState.AddModelError("", $"The Recommendation field is required");
                    return View(model);
                }

                GrantApprovalMaster master = (await _repoApprovalMaster.FindAsync(x => x.Code == "F")).FirstOrDefault();

                //UserRoleDetails userRoleDetails = (await GetAppRoleName(forwardToRole));
                string forwardToRole = "";
                string subdiv = "0";
                OfficerDetails officerDetail = new OfficerDetails();
                GrantApprovalDetail approval = (from a in _repoApprovalDetail.GetAll() where a.GrantID == grant.Id orderby a.Id descending select a).FirstOrDefault();
                switch (role)
                {
                    case "JUNIOR ENGINEER":
                        forwardToRole = "SUB DIVISIONAL OFFICER";
                        break;
                    case "SUB DIVISIONAL OFFICER":
                        forwardToRole = "EXECUTIVE ENGINEER";
                        break;
                    case "EXECUTIVE ENGINEER":
                        if (grant.IsForwarded == false)
                            forwardToRole = "JUNIOR ENGINEER";
                        else if (approval.ProcessedByRole == "SUB DIVISIONAL OFFICER")
                            forwardToRole = "DWS,CIRCLE OFFICER";
                        else forwardToRole = "CIRCLE OFFICER";
                        break;
                    case "CIRCLE OFFICER":
                        forwardToRole = "EXECUTIVE ENGINEER HQ";
                        break;
                    case "DWS":
                        if (approval.ProcessedByRole == "EXECUTIVE ENGINEER")
                            forwardToRole = "ADE";
                        else forwardToRole = "DIRECTOR DRAINAGE";
                        break;
                    case "ADE":
                        forwardToRole = "DWS";
                        break;
                    case "DIRECTOR DRAINAGE":
                        forwardToRole = "EXECUTIVE ENGINEER";
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
                model.LoggedInRole = role;
                //switch (role)
                //{
                //    case "JUNIOR ENGINEER":
                //        forwardToRole = "SUB DIVISIONAL OFFICER";
                //        break;
                //    case "SUB DIVISIONAL OFFICER":
                //        forwardToRole = "EXECUTIVE ENGINEER";
                //        break;
                //    case "EXECUTIVE ENGINEER":
                //        forwardToRole = grant.IsForwarded == false ? "JUNIOR ENGINEER" : "CIRCLE OFFICER";
                //        break;
                //    case "CIRCLE OFFICER":
                //        forwardToRole = "DWS";
                //        break;
                //    case "DWS":
                //        forwardToRole = "EXECUTIVE ENGINEER HQ";
                //        break;
                //    case "EXECUTIVE ENGINEER HQ":
                //        forwardToRole = "CHIEF ENGINEER HQ";
                //        break;
                //    case "CHIEF ENGINEER HQ":
                //        forwardToRole = "PRINCIPAL SECRETARY";
                //        break;
                //    default:
                //        forwardToRole = "JUNIOR ENGINEER"; break;
                //}
                var roles = forwardToRole.Split(',');
                string forwardrole = "";
                if (roles.Count() > 1)
                {
                    for (int i = 0; i < roles.Count(); i++)
                    {
                        if (forwardrole != "")
                            forwardrole += "," + roles[i];// ((await GetAppRoleName(roles[i])).RoleName);
                        else forwardrole += roles[i];// (await GetAppRoleName(roles[i])).RoleName;
                    }

                }
                else forwardrole = forwardToRole;// (await GetAppRoleName(forwardToRole)).RoleName;

                List<OfficerDetails> officerDetails = await GetOfficer(divisionId, forwardToRole, "0");
                if (forwardToRole=="JUNIOR ENGINEER")
                {
                    subdiv = model.SelectedSubDivisionId;
                    //officerDetail = (await GetOfficer(divisionId, forwardToRole, subdiv)).FindAll(x => x.UserId == model.SelectedOfficerId).FirstOrDefault();
                    officerDetail = (await GetOfficer(divisionId, forwardrole, subdiv)).FindAll(x => x.UserId == model.SelectedOfficerId).FirstOrDefault();
                }
                else
                {
                    //officerDetail = (await GetOfficer(divisionId, forwardToRole, subdiv)).FindAll(x => x.UserId == model.SelectedOfficerId).FirstOrDefault();
                    var offc = (await GetOfficer(divisionId, forwardrole, subdiv)).FindAll(x => x.UserId == model.SelectedOfficerId);
                    officerDetail = offc.Find(x=>x.UserId==model.SelectedOfficerId);
                }
                string forwardedUser = officerDetail.UserId;
                string forwardedRole = officerDetail.RoleName;
                int approvalLevel = (await _repoApprovalDetail.FindAsync(x => x.GrantID == grant.Id && x.ApprovalID == master.Id)).Count();


                string code = model.IsDrainNotified ? "N" : "C";
                var typeofwidth = (await _drainwidthRepository.FindAsync(x => x.Code == code)).FirstOrDefault();
                model.Id = grant.Id;
                model.Name = grant.Name;
                model.TotalArea = 0.0;
                model.ApplicantEmailID = grant.ApplicantEmailID;
                model.ApplicantName = grant.ApplicantName;
                model.ApplicationID = grant.ApplicationID;
                model.ForwardToRole = forwardToRole;
                model.LoggedInRole = role;
                model.Remarks = model.Remarks;
                model.IsDrainNotified = false;
                model.IsUnderMasterPlan = grant.IsUnderMasterPlan;
                model.IsForwarded = grant.IsForwarded;
                model.TypeOfWidth = typeofwidth.Id;
                model.Officers = new SelectList(officerDetails, "UserId", "UserName");
                GrantApprovalDetail approvalDetail = new GrantApprovalDetail
                {
                    GrantID = grant.Id,
                    ApprovalID = master.Id,
                    ProcessedBy =userId,
                    ProcessedOn = DateTime.Now,
                    ProcessedByRole = role,
                    ProcessLevel = approvalLevel + 1,
                    ProcessedByName=username+"("+designation+")",
                    ProcessedToRole = forwardedRole,
                    ProcessedToUser = forwardedUser,
                    ProcessedToName=officerDetail.UserName,                    
                    RecommendationID = role == "EXECUTIVE ENGINEER" && model.SelectedRecommendationId==0?3: model.SelectedRecommendationId,
                    Remarks=model.Remarks
                };
                List<RecommendationDetail> recommendations = new List<RecommendationDetail>();
                recommendations = _repoRecommendation.GetAll().Where(x => x.Code != "NA").ToList();
                model.Recommendations = recommendations != null && recommendations.Count() > 0 ? new SelectList(recommendations, "Id", "Name") : null;
                List<TautologyDetails> tautologyDetails = new List<TautologyDetails>
                {
                    new TautologyDetails{Text="No",Value="false"},
                    new TautologyDetails{Text="Yes",Value="true"}
                };
                model.ConfirmUnderMasterPlan = new SelectList(tautologyDetails, "Value", "Text", model.SelectedIsUnderMasterPlanId);

                if (role=="JUNIOR ENGINEER")
                {
                    if (!grant.IsUnderMasterPlan) {
                        if (model.SiteConditionReportFile != null && model.CatchmentAreaFile != null && model.DistanceFromCreekFile != null && model.GisOrDwsFile != null && model.CrossSectionOrCalculationFile != null && model.LSectionOfDrainFile != null)
                        {
                            string ErrorMessage = string.Empty;

                            if (model.DrainWidth <= 0)
                            {
                                ErrorMessage = $"Invalid width. Please enter value greater than 0";
                                ModelState.AddModelError("", ErrorMessage);

                                return View(model);

                            }
                            int siteConditionValidation = AllowedCheckExtensions(model.SiteConditionReportFile);
                            int CatchmentAreaValidation = AllowedCheckExtensions(model.CatchmentAreaFile);
                            int DistanceFromCreekFileValidation = AllowedCheckExtensions(model.DistanceFromCreekFile);
                            int GisOrDwsFileValidation = AllowedCheckExtensions(model.GisOrDwsFile);
                            //int KmlFileValidation = AllowedCheckExtensions(model.KmlFile);
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

                            //if (KmlFileValidation == 0)
                            //{
                            //    ErrorMessage = $"Invalid KML file type. Please upload a PDF file only";
                            //    ModelState.AddModelError("", ErrorMessage);

                            //    return View(model);

                            //}
                            //else if (KmlFileValidation == 2)
                            //{
                            //    ErrorMessage = "KML File field is required";
                            //    ModelState.AddModelError("", ErrorMessage);
                            //    return View(model);
                            //}
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

                            //if (!AllowedFileSize(model.KmlFile))
                            //{
                            //    ErrorMessage = "KML file size exceeds the allowed limit of 4MB";
                            //    ModelState.AddModelError("", ErrorMessage);
                            //    return View(model);
                            //}

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
                            string uniqueSiteConditionFileName = ProcessUploadedFileWithoutSave(model.SiteConditionReportFile, "SiteCondition");
                            string uniqueCatchmentAreaFileName = ProcessUploadedFileWithoutSave(model.CatchmentAreaFile, "CatchmentArea");
                            string uniqueDistanceFromCreekFileName = ProcessUploadedFileWithoutSave(model.DistanceFromCreekFile, "DistanceFromCreek");
                            string uniqueGisOrDwsFileName = ProcessUploadedFileWithoutSave(model.GisOrDwsFile, "GisOrDws");
                            string uniqueCrossSectionOrCalculationFileName = ProcessUploadedFileWithoutSave(model.CrossSectionOrCalculationFile, "CrossSectionOrCalculation");
                            string uniqueLSectionOfDrainFileName = ProcessUploadedFileWithoutSave(model.LSectionOfDrainFile, "LSectionOfDrain");
                            //string uniqueKmlFileName = ProcessUploadedFile(model.KmlFile, "kmlReport");

                            await _repoApprovalDetail.CreateAsync(approvalDetail);

                            GrantApprovalProcessDocumentsDetails approvalObj = new GrantApprovalProcessDocumentsDetails
                            {
                                SiteConditionReportPath = uniqueSiteConditionFileName,
                                CatchmentAreaAndFlowPath = uniqueCatchmentAreaFileName,
                                CrossSectionOrCalculationSheetReportPath = uniqueCrossSectionOrCalculationFileName,
                                DistanceFromCreekPath = uniqueDistanceFromCreekFileName,
                                DrainLSectionPath = uniqueLSectionOfDrainFileName,
                                GISOrDWSReportPath = uniqueGisOrDwsFileName,
                                IsKMLByApplicantValid = model.IsKMLByApplicantValid,
                                GrantApprovalID = approvalDetail.Id,
                                ProcessedBy = userId,
                                ProcessedOn = DateTime.Now,
                                ProcessedByRole = role,
                                IsDrainNotified = Convert.ToInt16(model.IsDrainNotified),
                                TypeOfWidth = typeofwidth.Id,
                                DrainWidth = model.DrainWidth,
                                ProcessedByName = LoggedInUserName() + "(" + LoggedInDesignationName() + ")"
                            };
                            List<GrantApprovalProcessDocumentsDetails> approvalDocList = new List<GrantApprovalProcessDocumentsDetails>();
                            approvalDocList.Add(approvalObj);
                            approvalDetail.GrantApprovalProcessDocuments = approvalDocList;
                            await _repoApprovalDetail.UpdateAsync(approvalDetail);
                            var SiteConditionReportPath = fileUploadeSave(model.SiteConditionReportFile, uniqueSiteConditionFileName);
                            var CatchmentAreaFile = fileUploadeSave(model.CatchmentAreaFile, uniqueCatchmentAreaFileName);
                            var CrossSectionOrCalculationFile = fileUploadeSave(model.CrossSectionOrCalculationFile, uniqueCrossSectionOrCalculationFileName);
                            var DistanceFromCreekFile = fileUploadeSave(model.DistanceFromCreekFile, uniqueDistanceFromCreekFileName);
                            var LSectionOfDrainFile = fileUploadeSave(model.LSectionOfDrainFile, uniqueLSectionOfDrainFileName);
                            var GisOrDwsFile = fileUploadeSave(model.GisOrDwsFile, uniqueGisOrDwsFileName);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "All documents are required to be uploaded");
                            return View(model);
                        }


                    }
                    else
                    {
                        if (model.SelectedIsUnderMasterPlanId==null || model.SelectedIsUnderMasterPlanId =="")
                        {
                            ModelState.AddModelError("", $"Is Site Under Master Plan field is required");
                            return View(model);
                        }

                        await _repoApprovalDetail.CreateAsync(approvalDetail);
                        GrantApprovalProcessDocumentsDetails approvalObj = new GrantApprovalProcessDocumentsDetails
                        {

                            IsUnderMasterPlan = model.SelectedIsUnderMasterPlanId=="false"?false:true,
                            GrantApprovalID = approvalDetail.Id,
                            ProcessedBy = userId,
                            ProcessedOn = DateTime.Now,
                            ProcessedByRole = role,
                            ProcessedByName = LoggedInUserName() + "(" + LoggedInDesignationName() + ")"
                        };
                        List<GrantApprovalProcessDocumentsDetails> approvalDocList = new List<GrantApprovalProcessDocumentsDetails>();
                        approvalDocList.Add(approvalObj);
                        approvalDetail.GrantApprovalProcessDocuments = approvalDocList;
                        await _repoApprovalDetail.UpdateAsync(approvalDetail);
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [HttpGet]
        public ViewResult EditApprovalDocuments(string grantId,long docId,long app)
        {
            try
            {
                if (grantId == null || grantId==string.Empty)
                {
                    ViewBag.ErrorMessage = $"An error occured";

                    return View("NotFound");
                }
                ApprovalDocumentsViewModelEdit model = (from g in _repo.GetAll()
                              join app2 in _repoApprovalDetail.GetAll() on g.Id equals app2.GrantID
                              join doc in _repoApprovalDocument.GetAll() on app2.Id equals doc.GrantApprovalID
                              where g.ApplicationID == grantId && app2.Id== app && (app2.ProcessedToUser==doc.ProcessedBy)
                              select new ApprovalDocumentsViewModelEdit
                              {
                                  CatchmentAreaFilePath=doc.CatchmentAreaAndFlowPath,
                                  CrossSectionOrCalculationFilePath=doc.CrossSectionOrCalculationSheetReportPath,
                                  DistanceFromCreekFilePath=doc.DistanceFromCreekPath,
                                  GisOrDwsFilePath=doc.GISOrDWSReportPath,
                                  IsKMLByApplicantValid=doc.IsKMLByApplicantValid,
                                  LSectionOfDrainFilePath=doc.DrainLSectionPath,
                                  SiteConditionReportFilePath=doc.SiteConditionReportPath,
                                  GrantApprovalDocId=docId,
                                  GrantApprovalId= app,
                                  IsDrainNotified = Convert.ToBoolean(doc.IsDrainNotified),
                                  TypeOfWidth = doc.TypeOfWidth??0,
                                  DrainWidth = doc.DrainWidth
                              }
                              ).FirstOrDefault();
                if (model == null)
                {
                    var d = (from g in _repo.GetAll()
                             join app2 in _repoApprovalDetail.GetAll() on g.Id equals app2.GrantID
                             join doc in _repoApprovalDocument.GetAll() on app2.Id equals doc.GrantApprovalID
                             where g.ApplicationID == grantId && (app2.ProcessedToUser == doc.ProcessedBy || app2.ProcessedBy == doc.ProcessedBy)
                             orderby doc.Id descending
                             select new ApprovalDocumentsViewModelEdit
                             {
                                 CatchmentAreaFilePath = doc.CatchmentAreaAndFlowPath,
                                 CrossSectionOrCalculationFilePath = doc.CrossSectionOrCalculationSheetReportPath,
                                 DistanceFromCreekFilePath = doc.DistanceFromCreekPath,
                                 GisOrDwsFilePath = doc.GISOrDWSReportPath,
                                 IsKMLByApplicantValid = doc.IsKMLByApplicantValid,
                                 LSectionOfDrainFilePath = doc.DrainLSectionPath,
                                 SiteConditionReportFilePath = doc.SiteConditionReportPath,
                                 GrantApprovalDocId = 0,//docId,
                                 GrantApprovalId = app,
                                 IsDrainNotified=false,
                                 TypeOfWidth=doc.TypeOfWidth??0,
                                 DrainWidth=0
                             }
                                      );
                    model = d.FirstOrDefault();
                }
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }

        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        [Obsolete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditApprovalDocuments(ApprovalDocumentsViewModelEdit model)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    string uniqueSiteConditionFileName = "";
                    string uniqueCatchmentAreaFileName = "";
                    string uniqueDistanceFromCreekFileName = "";
                    string uniqueGisOrDwsFileName = "";
                    string uniqueCrossSectionOrCalculationFileName = "";
                    string uniqueLSectionOfDrainFileName = "";

                    string userId = LoggedInUserID();

                    string divisionId = LoggedInDivisionID();

                    string subdivisionId = LoggedInSubDivisionID();

                    // Retrieve roles associated with the user
                    var roleName = LoggedInRoleName();

                    string role = roleName;
                    string ErrorMessage = string.Empty;
                    GrantApprovalProcessDocumentsDetails obj = new GrantApprovalProcessDocumentsDetails();
                    if (model.GrantApprovalDocId != 0)
                    {
                        obj = await _repoApprovalDocument.GetBylongIdAsync(model.GrantApprovalDocId);
                        if (obj == null)
                        {
                            ViewBag.ErrorMessage = $"Document with Id = {obj.Id} cannot be found";
                            return View("NotFound");
                        }

                        uniqueSiteConditionFileName = obj.SiteConditionReportPath;
                        uniqueCatchmentAreaFileName = obj.CatchmentAreaAndFlowPath;
                        uniqueDistanceFromCreekFileName = obj.DistanceFromCreekPath;
                        uniqueGisOrDwsFileName = obj.GISOrDWSReportPath;
                        uniqueCrossSectionOrCalculationFileName = obj.CrossSectionOrCalculationSheetReportPath;
                        uniqueLSectionOfDrainFileName = obj.DrainLSectionPath;
                    }
                    //if(CheckFiles(model.SiteConditionReportFile)==0 && CheckFiles(model.CatchmentAreaFile) == 0 && CheckFiles(model.DistanceFromCreekFile) == 0 &&
                    //    CheckFiles(model.GisOrDwsFile) == 0 && CheckFiles(model.CrossSectionOrCalculationFile) == 0 && CheckFiles(model.LSectionOfDrainFile) == 0)                   
                    //{
                    //    ErrorMessage = $"Atleast one file is required to upload";
                    //    ModelState.AddModelError("", ErrorMessage);

                    //    return View(model);
                    //}
                    if (CheckFiles(model.SiteConditionReportFile) != 0)
                    {
                        int siteConditionValidation = AllowedCheckExtensions(model.SiteConditionReportFile);
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
                        if (!AllowedFileSize(model.SiteConditionReportFile))
                        {
                            ErrorMessage = "Site condition report size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        uniqueSiteConditionFileName = ProcessUploadedFile(model.SiteConditionReportFile, "SiteCondition");
                    }
                    if (CheckFiles(model.CatchmentAreaFile) != 0)
                    {
                        int CatchmentAreaValidation = AllowedCheckExtensions(model.CatchmentAreaFile);
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
                        if (!AllowedFileSize(model.CatchmentAreaFile))
                        {
                            ErrorMessage = "Catchment area file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        uniqueCatchmentAreaFileName = ProcessUploadedFile(model.CatchmentAreaFile, "CatchmentArea");
                    }
                    if (CheckFiles(model.DistanceFromCreekFile) != 0)
                    {
                        int DistanceFromCreekFileValidation = AllowedCheckExtensions(model.DistanceFromCreekFile);
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
                        if (!AllowedFileSize(model.DistanceFromCreekFile))
                        {
                            ErrorMessage = "Distance from creek file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        uniqueDistanceFromCreekFileName = ProcessUploadedFile(model.DistanceFromCreekFile, "DistanceFromCreek");
                    }
                    if (CheckFiles(model.GisOrDwsFile) != 0)
                    {
                        int GisOrDwsFileValidation = AllowedCheckExtensions(model.GisOrDwsFile);
                        if (GisOrDwsFileValidation == 0)
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
                        if (!AllowedFileSize(model.GisOrDwsFile))
                        {
                            ErrorMessage = "GIS/DWS file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        uniqueGisOrDwsFileName = ProcessUploadedFile(model.GisOrDwsFile, "GisOrDws");

                    }
                    if (CheckFiles(model.CrossSectionOrCalculationFile) != 0)
                    {
                        int CrossSectionOrCalculationFileValidation = AllowedCheckExtensions(model.CrossSectionOrCalculationFile);
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
                        if (!AllowedFileSize(model.CrossSectionOrCalculationFile))
                        {
                            ErrorMessage = "Cross-Section/Calculation file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        uniqueCrossSectionOrCalculationFileName = ProcessUploadedFile(model.CrossSectionOrCalculationFile, "CrossSectionOrCalculation");
                    }
                    if (CheckFiles(model.LSectionOfDrainFile) != 0)
                    {
                        int LSectionOfDrainFileValidation = AllowedCheckExtensions(model.LSectionOfDrainFile);

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

                        if (!AllowedFileSize(model.LSectionOfDrainFile))
                        {
                            ErrorMessage = "L-Section of drain file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(model);
                        }
                        uniqueLSectionOfDrainFileName = ProcessUploadedFile(model.LSectionOfDrainFile, "LSectionOfDrain");
                    }
                    string code = model.IsDrainNotified ? "N" : "C";
                    var typeofwidth = (_drainwidthRepository.Find(x => x.Code == code)).FirstOrDefault();

                    obj.SiteConditionReportPath = uniqueSiteConditionFileName;
                    obj.CatchmentAreaAndFlowPath = uniqueCatchmentAreaFileName;
                    obj.CrossSectionOrCalculationSheetReportPath = uniqueCrossSectionOrCalculationFileName;
                    obj.DistanceFromCreekPath = uniqueDistanceFromCreekFileName;
                    obj.DrainLSectionPath = uniqueLSectionOfDrainFileName;
                    obj.GISOrDWSReportPath = uniqueGisOrDwsFileName;
                    obj.IsKMLByApplicantValid = model.IsKMLByApplicantValid;
                    obj.GrantApprovalID = model.GrantApprovalId;
                    obj.IsDrainNotified = Convert.ToInt16(model.IsDrainNotified);
                    obj.TypeOfWidth= typeofwidth.Id;
                    if(model.DrainWidth>0)
                    obj.DrainWidth = model.DrainWidth;
                        
                    if (model.GrantApprovalDocId != 0)
                    { 
                        obj.UpdatedOn = DateTime.Now;
                        obj.UpdatedBy = userId;
                        obj.UpdatedByRole = role;
                        obj.UpdatedByName = LoggedInUserName() + "(" + LoggedInDesignationName() + ")";
                        await _repoApprovalDocument.UpdateAsync(obj);
                    }
                    else
                    {
                        //var SiteConditionReportPath = fileUploadeSave(model.SiteConditionReportFile, uniqueSiteConditionFileName);
                        //var CatchmentAreaFile = fileUploadeSave(model.CatchmentAreaFile, uniqueCatchmentAreaFileName);
                        //var CrossSectionOrCalculationFile = fileUploadeSave(model.CrossSectionOrCalculationFile, uniqueCrossSectionOrCalculationFileName);
                        //var DistanceFromCreekFile = fileUploadeSave(model.DistanceFromCreekFile, uniqueDistanceFromCreekFileName);
                        //var LSectionOfDrainFile = fileUploadeSave(model.LSectionOfDrainFile, uniqueLSectionOfDrainFileName);
                        //var GisOrDwsFile = fileUploadeSave(model.GisOrDwsFile, uniqueGisOrDwsFileName);
                        obj.ProcessedOn = DateTime.Now;
                        obj.ProcessedBy = userId;
                        obj.ProcessedByRole = role;
                        obj.ProcessedByName = LoggedInUserName() + "(" + LoggedInDesignationName() + ")";
                        await _repoApprovalDocument.CreateAsync(obj);
                    }
                    return RedirectToAction("Index");
                //}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(model);
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
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
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                // Retrieve the user object
                //var user = await _userManager.FindByIdAsync(userId);

                string userId = LoggedInUserID();

                string divisionId = LoggedInDivisionID();

                string subdivisionId = LoggedInSubDivisionID();

                string username = LoggedInUserName();

                string designation = LoggedInDesignationName();

                // Retrieve roles associated with the user
                var roleName = LoggedInRoleName();

                string role = roleName;
                GrantApprovalMaster master = (await _repoApprovalMaster.FindAsync(x => x.Code == "R")).FirstOrDefault();
                int rejectionLevel = (await _repoApprovalDetail.FindAsync(x => x.GrantID == grant.Id && x.ApprovalID == master.Id)).Count();
                GrantApprovalDetail approvalDetail = new GrantApprovalDetail
                {
                    GrantID = grant.Id,
                    ApprovalID = master.Id,
                    ProcessedBy = userId,
                    ProcessedOn = DateTime.Now,
                    ProcessedByRole = role,
                    ProcessedByName = username + "(" + designation + ")",
                    ProcessLevel = rejectionLevel + 1,
                    ProcessedToRole = "",
                    ProcessedToUser = "",
                    ProcessedToName = "",
                    Remarks = model.Remarks,
                    RecommendationID=3
                };

                await _repoApprovalDetail.CreateAsync(approvalDetail);
                //_repoApprovalDetail.
                grant.IsRejected = true;
                grant.ProcessLevel = approvalDetail.ProcessLevel;
                grant.IsPending = false;
                grant.UpdatedOn = DateTime.Now;
                await _repo.UpdateAsync(grant);

                var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForRejection(grant.ApplicantName, grant.ApplicationID, model.Remarks));
                _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");

                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CHIEF ENGINEER HQ")]
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
            IssueNocViewModelCreate model = new IssueNocViewModelCreate();
            if (grant.IsUnderMasterPlan)
            {
                model = (from g in _repo.GetAll()
                         join pay in _masterPlanDetailsRepository.GetAll() on g.MasterPlanId equals pay.Id
                         //join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                         join t in _tehsilBlockRepo.GetAll() on g.TehsilID equals t.Id
                         join sub in _subDivisionRepo.GetAll() on g.SubDivisionId equals sub.Id
                         join div in _divisionRepo.GetAll() on sub.DivisionId equals div.Id
                         where g.ApplicationID == id
                         select new IssueNocViewModelCreate
                         {
                             Id = g.Id,
                             Name = g.Name,
                             ApplicationID = g.ApplicationID,
                             IsUnderMasterPlan = g.IsUnderMasterPlan,
                             LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + g.VillageName + ", Pincode: " + g.PinCode,
                         }).FirstOrDefault();
            }
            else
            {
                model = (from g in _repo.GetAll()
                         join pay in _repoPayment.GetAll() on g.Id equals pay.GrantID
                         //join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                         join t in _tehsilBlockRepo.GetAll() on g.TehsilID equals t.Id
                         join sub in _subDivisionRepo.GetAll() on g.SubDivisionId equals sub.Id
                         join div in _divisionRepo.GetAll() on sub.DivisionId equals div.Id
                         where g.ApplicationID == id
                         select new IssueNocViewModelCreate
                         {
                             Id = g.Id,
                             Name = g.Name,
                             ApplicationID = g.ApplicationID,
                             IsUnderMasterPlan = g.IsUnderMasterPlan,
                             LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + g.VillageName + ", Pincode: " + g.PinCode,
                         }).FirstOrDefault();
            }
            return View(model);
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CHIEF ENGINEER HQ")]
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
                if(model.IsUnderMasterPlan) ErrorMessage = $"Invalid exemption letter file type. Please upload a PDF file only";
                else ErrorMessage = $"Invalid certificate file type. Please upload a PDF file only";
                    ModelState.AddModelError("", ErrorMessage);

                    return View(model);

                }
                else if (certificateValidation == 2)
                {
                if(model.IsUnderMasterPlan)
                    ErrorMessage = "Exemption Letter field is required";
                else
                    ErrorMessage = "Certificate field is required";
                ModelState.AddModelError("", ErrorMessage);
                    return View(model);
                }

                if (!AllowedFileSize(model.CertificateFile))
                {
                    if(model.IsUnderMasterPlan)
                    ErrorMessage = "Exemption Letter size exceeds the allowed limit of 4MB";
                else
                    ErrorMessage = "Certificate size exceeds the allowed limit of 4MB";
                ModelState.AddModelError("", ErrorMessage);
                    return View(model);
                }
                string uniqueCertificateFileName = ProcessUploadedFile(model.CertificateFile, "noc");

            // Get the current user's ID
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user object
            // var user = await _userManager.FindByIdAsync(userId);

            string userId = LoggedInUserID();

            string divisionId = LoggedInDivisionID();

            string subdivisionId = LoggedInSubDivisionID();

            string username = LoggedInUserName();

            string designation = LoggedInDesignationName();

            // Retrieve roles associated with the user
            var roleName = LoggedInRoleName();

            string role = roleName;
            GrantApprovalMaster master = (await _repoApprovalMaster.FindAsync(x => x.Code == "A")).FirstOrDefault();
            int approvalLevel = (await _repoApprovalDetail.FindAsync(x => x.GrantID == grant.Id && x.ApprovalID == master.Id)).Count();
            GrantApprovalDetail approvalDetail = new GrantApprovalDetail
            {
                GrantID = grant.Id,
                ApprovalID = master.Id,
                ProcessedBy = userId,
                ProcessedOn = DateTime.Now,
                ProcessedByRole = role,
                ProcessedByName = username + "(" + designation + ")",
                ProcessLevel = approvalLevel + 1,
                ProcessedToRole = "",
                ProcessedToUser = "",
                RecommendationID=3
            };

            await _repoApprovalDetail.CreateAsync(approvalDetail);
            //_repoApprovalDetail.
            grant.IsApproved = true;
            grant.IsForwarded = false;
            grant.ProcessLevel = approvalDetail.ProcessLevel;
            grant.IsPending = false;
            grant.CertificateFilePath = uniqueCertificateFileName;
            grant.UploadedByRole=role;
            grant.UploadedBy = userId;
            grant.UploadedOn = DateTime.Now;
            await _repo.UpdateAsync(grant);

            var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForIssue(grant.ApplicantName, grant.ApplicationID,"",grant.IsUnderMasterPlan));
            _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");

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

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        public async Task<IActionResult> ViewApplication(string Id)
        {
            try
            {
                GrantDetails obj = (await _repo.FindAsync(x => x.ApplicationID.ToLower() == Id.ToLower())).FirstOrDefault();
                PlanSanctionAuthorityMaster planAuth = await  _repoPlanSanctionAuthtoryMaster.GetByIdAsync(obj.PlanSanctionAuthorityId);
                //VillageDetails village = await _villageRpo.GetByIdAsync(obj.VillageID);
                TehsilBlockDetails tehsil = await _tehsilBlockRepo.GetByIdAsync(obj.TehsilID);
                SubDivisionDetails subDivision = await _subDivisionRepo.GetByIdAsync(obj.SubDivisionId);
                DivisionDetails division = await _divisionRepo.GetByIdAsync(subDivision.DivisionId);
                NocPermissionTypeDetails permission = await _nocPermissionTypeRepo.GetByIdAsync(obj.NocPermissionTypeID??0);
                ProjectTypeDetails projecttype = await _projectTypeRepo.GetByIdAsync(obj.ProjectTypeId);
                NocTypeDetails noctype = await _nocTypeRepo.GetByIdAsync(obj.NocTypeId??0);
                SiteAreaUnitDetails unit = await _siteUnitsRepo.GetByIdAsync(obj.SiteAreaUnitId??0);
                MasterPlanDetails masterplan = (from np in _masterPlanDetailsRepository.GetAll() where obj.MasterPlanId == np.Id select np).FirstOrDefault();
                List<GrantKhasraDetails> khasras = (await _khasraRepo.FindAsync(x => x.GrantID == obj.Id)).ToList();
                List<OwnerDetails> owners = (await _grantOwnersRepo.FindAsync(x => x.GrantId == obj.Id)).ToList();
                List<GrantInspectionDocuments> documents = (from d in _repoApprovalDocument.GetAll()
                                join a in _repoApprovalDetail.GetAll() on d.GrantApprovalID equals a.Id
                                join dr in _drainwidthRepository.GetAll() on d.TypeOfWidth equals dr.Id
                                where a.GrantID==obj.Id orderby d.Id descending
                                select new GrantInspectionDocuments
                                { 
                                    GisOrDwsFilePath=d.GISOrDWSReportPath,
                                    CatchmentAreaFilePath=d.CatchmentAreaAndFlowPath,
                                    CrossSectionOrCalculationFilePath=d.CrossSectionOrCalculationSheetReportPath,
                                    DistanceFromCreekFilePath=d.DistanceFromCreekPath,
                                    LSectionOfDrainFilePath=d.DrainLSectionPath,
                                    IsKMLByApplicantValid=d.IsKMLByApplicantValid,
                                    UploadedByRole = d.ProcessedByRole,
                                    UploadedByName=d.ProcessedByName,
                                    SiteConditionReportFilePath=d.SiteConditionReportPath,
                                    IsDrainNotified= Convert.ToBoolean(d.IsDrainNotified),
                                    DrainWidth=d.DrainWidth,
                                    TypeOfWidthName= "Width "+dr.Name

                                }).ToList();
                    //(await _grantOwnersRepo.FindAsync(x => x.GrantId == obj.Id)).ToList();
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
                    totalArea = Math.Round(totalArea + units.KanalOrBigha + units.MarlaOrBiswa + units.SarsaiOrBiswansi, 5);
                   
                }
                List<GrantApprovalRecommendationDetail> modelRecommendation = (from ap in _repoApprovalDetail.GetAll()
                                                                 join recommend in _repoRecommendation.GetAll() on ap.RecommendationID equals recommend.Id
                                                                 where ap.GrantID == obj.Id && obj.IsPending == true && recommend.Code != "NA"
                                                                 orderby ap.ProcessedOn descending
                                                                 select new GrantApprovalRecommendationDetail
                                                                 {
                                                                     ApplicationId = obj.ApplicationID,
                                                                     Recommended = recommend.Name,
                                                                     RecommendedBy = ap.ProcessedByRole,
                                                                     RecommendedTo = ap.ProcessedToRole,
                                                                     Remarks = ap.Remarks,
                                                                     RecommendedByName=ap.ProcessedByName,
                                                                     RecommendedToName=ap.ProcessedToName
                                                                 }).ToList();
                List<GrantFileTransferDetails> modelFileTransfer = (from ap in _grantFileTransferRepository.GetAll()
                                                                               where ap.GrantId == obj.Id 
                                                                               orderby ap.TransferedOn descending
                                                                               select new GrantFileTransferDetails
                                                                               {
                                                                                   FromName=ap.FromName,
                                                                                   Remarks = ap.Remarks,
                                                                                   FromDesignationName=ap.FromDesignationName,
                                                                                   FromAuthorityId=ap.FromAuthorityId,
                                                                                   ToAuthorityId=ap.ToAuthorityId,
                                                                                   ToName=ap.ToName,
                                                                                   ToDesignationName=ap.ToDesignationName,
                                                                                   TransferedOn=ap.TransferedOn
                                                                               }).ToList();
                var payment = await _grantPaymentRepo.FindAsync(x => x.GrantID == obj.Id);
                if (payment == null || payment.Count() == 0)
                {
                    GrantDetailViewModel model = new GrantDetailViewModel
                    {
                        Id = obj.Id,
                        ApplicationID = obj.ApplicationID,
                        PaymentOrderId = "0",
                        ReceiptNo="0",
                        Amount="0",
                        Name = obj.Name,
                        VillageName = obj.VillageName,
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
                        NocPermissionTypeName = obj.IsUnderMasterPlan ? "" : permission.Name,
                        NocTypeName = obj.IsUnderMasterPlan ? "" : noctype.Name,
                        OtherProjectTypeDetail = obj.OtherProjectTypeDetail,
                        Pincode = obj.PinCode.ToString(),
                        PlotNo = obj.PlotNo,
                        PreviousDate = obj.IsUnderMasterPlan ? "" : string.Format("{0:dd/MM/yyyy}", obj.PreviousDate),
                        ProjectTypeName = projecttype.Name,
                        SiteAreaUnitName = obj.IsUnderMasterPlan ? "" : unit.Name,
                        TotalArea = totalArea.ToString(),
                        TotalAreaSqFeet = Math.Round((totalArea * 43560),5).ToString(),
                        TotalAreaSqMetre = Math.Round((totalArea * 4046.86),5).ToString(),
                        Owners = owners,
                        Khasras = khasras,
                        PlanSanctionAuthorityName= planAuth.Name,
                        FaradFilePath=obj.FaradFilePath,
                        LayoutPlanFilePath=obj.LayoutPlanFilePath,
                        GrantInspectionDocumentsDetail = documents,
                        GrantApprovalRecommendationDetails=modelRecommendation,
                        GrantFileTransferDetail= modelFileTransfer,
                        LocationDetail = "Hadbast: " + obj.Hadbast + ", Plot No: " + obj.PlotNo + ", Division: " + division.Name + ", Sub-Division: " + subDivision.Name + ", Tehsil/Block: " + tehsil.Name + ", Village: " + obj.VillageName + ", Pincode: " + obj.PinCode,
                        IsUnderMasterPlan = obj.IsUnderMasterPlan,
                        MasterPlanName = obj.IsUnderMasterPlan ? masterplan.Name : "",
                        UnderMasterPlan = obj.IsUnderMasterPlan ? "Yes" : "No"
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
                        PaymentOrderId = objPyment.deptRefNo,
                        Amount=Math.Round(objPyment.TotalAmount??0,5).ToString(),
                        ReceiptNo=objPyment.PaymentOrderId,
                        Name = obj.Name,
                        VillageName = obj.VillageName,
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
                        NocPermissionTypeName = obj.IsUnderMasterPlan ? "" : permission.Name,
                        NocTypeName = obj.IsUnderMasterPlan ? "" : noctype.Name,
                        OtherProjectTypeDetail = obj.OtherProjectTypeDetail,
                        Pincode = obj.PinCode.ToString(),
                        PlotNo = obj.PlotNo,
                        PreviousDate = obj.IsUnderMasterPlan ? "" : string.Format("{0:dd/MM/yyyy}", obj.PreviousDate),
                        ProjectTypeName = projecttype.Name,
                        SiteAreaUnitName = obj.IsUnderMasterPlan ? "" : unit.Name,
                        TotalArea = totalArea.ToString(),
                        TotalAreaSqFeet = Math.Round((totalArea * 43560), 5).ToString(),
                        TotalAreaSqMetre = Math.Round((totalArea * 4046.86), 5).ToString(),
                        Owners = owners,
                        Khasras = khasras,
                        PlanSanctionAuthorityName = planAuth.Name,
                        FaradFilePath = obj.FaradFilePath,
                        LayoutPlanFilePath = obj.LayoutPlanFilePath,
                        GrantInspectionDocumentsDetail = documents,
                        GrantApprovalRecommendationDetails = modelRecommendation,
                        GrantFileTransferDetail = modelFileTransfer,
                        LocationDetail = "Hadbast: " + obj.Hadbast + ", Plot No: " + obj.PlotNo + ", Division: " + division.Name + ", Sub-Division: " + subDivision.Name + ", Tehsil/Block: " + tehsil.Name + ", Village: " + obj.VillageName + ", Pincode: " + obj.PinCode,
                        IsUnderMasterPlan = obj.IsUnderMasterPlan,
                        MasterPlanName = obj.IsUnderMasterPlan ? masterplan.Name : "",
                        UnderMasterPlan = obj.IsUnderMasterPlan ? "Yes" : "No"
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

        private int CheckFiles(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                
                return 1;
            }
            else return 0;
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

                string extension = System.IO.Path.GetExtension(file.FileName);
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");
                uniqueFileName = prefixName + "_" + Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
        [Obsolete]
        private string ProcessUploadedFileWithoutSave(IFormFile file, string prefixName)
        {
            string uniqueFileName = null;
            if (file != null && file.Length > 0)
            {
                string extension = System.IO.Path.GetExtension(file.FileName);
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");
                uniqueFileName = prefixName + "_" + Guid.NewGuid().ToString() + extension;
            }

            return uniqueFileName;
        }

        [Obsolete]
        private bool fileUploadeSave(IFormFile file, string uniqueFileName)
        {
            try
            {
                if (file != null && file.Length > 0)
                {

                    string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");
                    string filePath = Path.Combine(uploadFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [Obsolete]
        [HttpPost]
        public async Task<IActionResult> GetOfficers(int subdivisionId,string roleName)
        {
            string divisionId = "0";
            //string role = (await GetAppRoleName(roleName)).RoleName;
            List<OfficerDetails> officerDetail = await GetOfficer(divisionId, roleName, subdivisionId.ToString());
            return Json(new SelectList(officerDetail, "UserId", "UserName"));
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        public IActionResult GetRecommendationDetail(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";
                    return View("NotFound");
                }

                List<GrantApprovalRecommendationDetail> model = (from g in _repo.GetAll()
                                                                 join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                                                                 join recommend in _repoRecommendation.GetAll() on a.RecommendationID equals recommend.Id
                                                                 where g.ApplicationID == id && g.IsPending == true && recommend.Code != "NA"
                                                                 orderby a.ProcessedOn descending
                                                                 select new GrantApprovalRecommendationDetail
                                                                 {
                                                                     ApplicationId = g.ApplicationID,
                                                                     Recommended = recommend.Name,
                                                                     RecommendedBy = a.ProcessedByRole,
                                                                     RecommendedTo = a.ProcessedToRole,
                                                                     RecommendedByName = a.ProcessedByName,
                                                                     RecommendedToName = a.ProcessedToName,
                                                                     Remarks = a.Remarks
                                                                 }).ToList();



                return Json(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                //ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        public IActionResult GetRecommendationDetailForOtherThanNA(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    ViewBag.ErrorMessage = $"Grant with Application Id = {id} cannot be found";
                    return View("NotFound");
                }

                List<GrantApprovalRecommendationDetail> model = (from g in _repo.GetAll()
                                                                 join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                                                                 join recommend in _repoRecommendation.GetAll() on a.RecommendationID equals recommend.Id
                                                                 where g.ApplicationID == id && g.IsPending == true 
                                                                 orderby a.ProcessedOn descending
                                                                 select new GrantApprovalRecommendationDetail
                                                                 {
                                                                     ApplicationId = g.ApplicationID,
                                                                     Recommended = recommend.Name,
                                                                     RecommendedBy = a.ProcessedByRole,
                                                                     RecommendedTo = a.ProcessedToRole,
                                                                     RecommendedByName=a.ProcessedByName,
                                                                     RecommendedToName=a.ProcessedToName,
                                                                     Remarks = a.Remarks
                                                                 }).ToList();



                return Json(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                //ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [Obsolete]
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
            user_info user_info11 = new user_info();
            user_info user_info9 = new user_info();
            user_info user_info10 = new user_info();
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
            LoginResponseViewModel o11 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info11 };
            LoginResponseViewModel o9 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info9 };
            LoginResponseViewModel o10 = new LoginResponseViewModel { msg = "success", Status = "200", user_info = user_info10 };
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
            users.Add(o11);
            users.Add(o9);
            users.Add(o10);
            users.Add(o12);
            users.Add(o13);
            users.Add(o14);
            users.Add(o15);
            users.Add(o16);
            users.Add(o17);
            return users;

        }
        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [Obsolete]
        private async Task<List<OfficerResponseViewModel>> LoadOfficersAsync(string officerRole, string subdivisionId, string divisionId)
        {
            try
            {
                string baseUrl = "";
                string salt = "";
                string checksum = "";
                string combinedPassword = "";
                //officerRole = "Employee";
                List<LoginResponseViewModel> users = FetchUser();
                //if (subdivisionId != "0")
                //{
                //    subdivisionId = "114";
                //    baseUrl = "https://wrdpbind.com/api/login5.php";
                //    salt = "4RCHhk3cJ6OMGdEf";
                //    checksum = "eUOwFCGMqKvJARC1tU6l4s34";
                //    combinedPassword = officerRole + "|" + subdivisionId + "|" + checksum;
                //}
                //else
                //{
                //    divisionId = "114";
                //    baseUrl = "https://wrdpbind.com/api/login6.php";
                //    salt = "6WCHhk3cJ6OMGdRg";
                //    checksum = "dTOwFCGMqKvJARC1tU6l4sv6";
                //    combinedPassword = officerRole + "|" + divisionId + "|" + checksum;
                //}


                //string plainText = combinedPassword;

                //var keyBytes = new byte[16];
                //var ivBytes = new byte[16];

                //string key = salt;
                //var keySalt = Encoding.UTF8.GetBytes(key);
                //var pdb = new Rfc2898DeriveBytes(keySalt, keySalt, 1000);

                //Array.Copy(pdb.GetBytes(16), keyBytes, 16);
                //Array.Copy(pdb.GetBytes(16), ivBytes, 16);
                //string encryptedString = NCC_encryptHelper(plainText, key, key);

                //HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = false };
                //HttpClient client = new HttpClient(handler);
                //client.BaseAddress = new Uri(baseUrl);
                //client.DefaultRequestHeaders.Accept.Clear();
                //client.DefaultRequestHeaders.Add("details", encryptedString);
                //var tokenResponse1 = await client.GetAsync(client.BaseAddress.ToString());
                //string resultContent = "["+tokenResponse1.Content.ReadAsStringAsync().Result.Replace("}{", "},{")+"]";
                //List<OfficerResponseViewModel> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OfficerResponseViewModel>>(resultContent);
                List<OfficerResponseViewModel> list = new List<OfficerResponseViewModel>();
                //officerRole = (await GetRoleName(officerRole)).RoleName;
                UserRoleDetails officer = (await GetAppRoleName(officerRole));
                officerRole = officer.RoleName;
                List<LoginResponseViewModel> officers = new List<LoginResponseViewModel>();

                officers = divisionId == "0" ? users.FindAll(x => x.user_info.Role.Contains(officerRole)) : users.FindAll(x => x.user_info.Role.Contains(officerRole) && x.user_info.DivisionID.ToString() == divisionId);
                //user_info user = officers.user_info;
                //user.Role = officerRole;
                //user.RoleID = officer.Id.ToString();
                foreach (LoginResponseViewModel user in officers)
                {

                    OfficerResponseViewModel obj = new OfficerResponseViewModel
                    {
                        msg = "success",
                        Status = "200",
                        user_info = new officer_info { EmployeeId = user.user_info.EmpID, EmployeeName = user.user_info.Name, email = user.user_info.EmailId, DeesignationName = user.user_info.Designation, DesignationID = user.user_info.DesignationID, DistrictId = user.user_info.DistrictID, DistrictName = user.user_info.District, DivisionID = user.user_info.DivisionID, DivisionName = user.user_info.Division, MobileNo = user.user_info.MobileNo, RoleID = officer.Id.ToString(), RoleName = officerRole, SubdivisionId = user.user_info.SubDivisionID, SubdivisionName = user.user_info.SubDivision }
                    };
                    list.Add(obj);
                }
                if (list.Count > 0)
                {
                    if (list.FirstOrDefault().Status == "200")
                    {
                        //    //OfficerResponseViewModel list3 = Newtonsoft.Json.JsonConvert.DeserializeObject<OfficerResponseViewModel>(resultContent);
                        //    //List<OfficerResponseViewModel> list = new List<OfficerResponseViewModel>();
                        //    //list.Add(list3);

                        return list;

                    }
                }
                return null;
            }

            catch (Exception ex)
            {
                return null;
            }
        }
        //private async Task<List<OfficerResponseViewModel>> LoadOfficersAsync(string officerRole, string subdivisionId, string divisionId)
        //{
        //    try
        //    {
        //        string baseUrl = "";
        //        string salt = "";
        //        string checksum = "";
        //        string combinedPassword = "";
        //        UserRoleDetails officer = (await GetAppRoleName(officerRole));
        //        officerRole = officer.RoleName;
        //        baseUrl = "https://wrdpbind.com/api/login6.php";
        //        salt = "6WCHhk3cJ6OMGdRg";
        //        checksum = "dTOwFCGMqKvJARC1tU6l4sv6";
        //        combinedPassword = officerRole + "|" + divisionId + "|" + checksum;
        //        string plainText = combinedPassword;

        //        var keyBytes = new byte[16];
        //        var ivBytes = new byte[16];

        //        string key = salt;
        //        var keySalt = Encoding.UTF8.GetBytes(key);
        //        var pdb = new Rfc2898DeriveBytes(keySalt, keySalt, 1000);

        //        Array.Copy(pdb.GetBytes(16), keyBytes, 16);
        //        Array.Copy(pdb.GetBytes(16), ivBytes, 16);
        //        string encryptedString = NCC_encryptHelper(plainText, key, key);

        //        HttpClientHandler handler = new HttpClientHandler() { UseDefaultCredentials = false };
        //        HttpClient client = new HttpClient(handler);
        //        client.BaseAddress = new Uri(baseUrl);
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Add("details", encryptedString);
        //        var tokenResponse1 = await client.GetAsync(client.BaseAddress.ToString());
        //        string resultContent = "[" + tokenResponse1.Content.ReadAsStringAsync().Result.Replace("}{", "},{") + "]";
        //        List<OfficerResponseViewModel> list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<OfficerResponseViewModel>>(resultContent);
        //        if (list.FirstOrDefault().Status == "400")
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            if (list.Count > 0)
        //            {
        //                if (list.FirstOrDefault().Status == "200")
        //                {
        //                    List<OfficerResponseViewModel> listOfficers = new List<OfficerResponseViewModel>();
        //                    List<OfficerResponseViewModel> officers = new List<OfficerResponseViewModel>();
        //                    officers = divisionId == "0" ? list.FindAll(x => x.user_info.RoleName.Contains(officerRole)) : list.FindAll(x => x.user_info.RoleName.Contains(officerRole) && x.user_info.DivisionID.ToString() == divisionId);
        //                    foreach (OfficerResponseViewModel user in officers)
        //                    {

        //                        OfficerResponseViewModel obj = new OfficerResponseViewModel
        //                        {
        //                            msg = "success",
        //                            Status = "200",
        //                            user_info = new officer_info { UserName=user.user_info.UserName,EmployeeId = user.user_info.EmployeeId, EmployeeName = user.user_info.EmployeeName, email = user.user_info.email, DeesignationName = user.user_info.DeesignationName, DesignationID = user.user_info.DesignationID, DistrictId = user.user_info.DistrictId, DistrictName = user.user_info.DistrictName, DivisionID = user.user_info.DivisionID, DivisionName = user.user_info.DivisionName, MobileNo = user.user_info.MobileNo, RoleID = officer.Id.ToString(), RoleName = officerRole, SubdivisionId = user.user_info.SubdivisionId, SubdivisionName = user.user_info.SubdivisionName }
        //                        };
        //                        listOfficers.Add(obj);
        //                    }
        //                    return listOfficers;

        //                }
        //            }
        //        }
        //        return null;
        //    }

        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DD")]
        private async Task<UserRoleDetails> GetAppRoleName(string rolename)
        {
            try
            {
                var desig = (await _userRolesRepository.FindAsync(x => x.AppRoleName == rolename));
                return desig.FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        private async Task<UserRoleDetails> GetRoleName(string rolename)
        {
            try
            {
                return (await _userRolesRepository.FindAsync(x => x.RoleName == rolename)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [Obsolete]
        private async Task<List<OfficerDetails>> GetOfficerLastForwardedBy(string roleName, string lastForwardedByid)
        {
            try
            {
                var role = roleName.Split(',');
                List<OfficerResponseViewModel> officers = new List<OfficerResponseViewModel>();
                for (int i = 0; i < role.Count(); i++)
                {
                    List<OfficerResponseViewModel> officer = new List<OfficerResponseViewModel>();
                    var o = (await LoadOfficersAsync(roleName, "0", "0")).FindAll(x => x.user_info.EmployeeId == lastForwardedByid);
                    
                    officer = o == null ? null : o.ToList();
                    if (officer != null)
                        officers.AddRange(officer);

                }

                if (officers != null && officers.Count > 0)
                {
                    var userRole = (from u in _userRolesRepository.GetAll().AsEnumerable()
                                    join officer in officers on u.Id.ToString() equals officer.user_info.RoleID
                                    select new OfficerDetail
                                    {
                                        EmployeeId = officer.user_info.EmployeeId,
                                        RoleName = u.AppRoleName,
                                        RoleId = u.Id.ToString(),
                                        Name = officer.user_info.EmployeeName,
                                        Designation = officer.user_info.DeesignationName
                                    });
                    //var usersInRole = (await _userManager.GetUsersInRoleAsync(forwardToRole));
                    List<OfficerDetails> officerDetails = ((from u in officers.AsEnumerable()
                                                            join o in userRole on u.user_info.EmployeeId equals o.EmployeeId
                                                            select new OfficerDetails
                                                            {
                                                                UserId = u.user_info.EmployeeId,
                                                                UserName = u.user_info.EmployeeName + "(" + u.user_info.DeesignationName + ")",
                                                                RoleId = u.user_info.RoleID.ToString(),
                                                                RoleName = o.RoleName,
                                                                Name = o.Name,
                                                                Designation = o.Designation
                                                            }
                                                          ).ToList());
                    return officerDetails;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [Obsolete]
        private async Task<List<OfficerDetails>> GetOfficer(string divisionId,string roleName,string subDivision)
        {
            try
            {
                var role = roleName.Split(',');
                List<OfficerResponseViewModel> officers = new List<OfficerResponseViewModel>();
                for (int i = 0;i< role.Count(); i++)
                {
                    List<OfficerResponseViewModel> officer = new List<OfficerResponseViewModel>();
                    var o = (await LoadOfficersAsync(role[i], subDivision, divisionId));
                    officer = o==null?null:o.ToList();
                    if(officer!=null)
                    officers.AddRange(officer);
                    
                }
                
                if (officers!=null && officers.Count > 0)
                {
                    var userRole = (from u in _userRolesRepository.GetAll().AsEnumerable()
                                              join officer in officers on u.Id.ToString() equals officer.user_info.RoleID
                                              select new OfficerDetail
                                              {
                                                  EmployeeId = officer.user_info.EmployeeId,
                                                  RoleName = u.AppRoleName,
                                                  RoleId = u.Id.ToString(),
                                                  Name=officer.user_info.EmployeeName,
                                                  Designation=officer.user_info.DeesignationName
                                              });
                    //var usersInRole = (await _userManager.GetUsersInRoleAsync(forwardToRole));
                    List<OfficerDetails> officerDetails = ((from u in officers.AsEnumerable()
                                                            join o in userRole on u.user_info.EmployeeId equals o.EmployeeId
                                                            select new OfficerDetails
                                                            {
                                                                UserId = u.user_info.EmployeeId,
                                                                UserName = u.user_info.EmployeeName+"-"+u.user_info.UserName+"("+u.user_info.DeesignationName+")",
                                                                RoleId = u.user_info.RoleID.ToString(),
                                                                RoleName = o.RoleName,
                                                                Name=o.Name,
                                                                Designation=o.Designation
                                                            }
                                                          ).ToList());
                    return officerDetails;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
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

        private string LoggedInUserID()
        {
            string userId = HttpContext.Session.GetString("Userid");
            return userId;
        }
        private string LoggedInDivisionID()
        {
            string userId = HttpContext.Session.GetString("Divisionid");
            return userId;
        }
        private string LoggedInSubDivisionID()
        {
            string userId = HttpContext.Session.GetString("SubDivisionid");
            return userId;
        }
        private string LoggedInRoleID()
        {
            string userId = HttpContext.Session.GetString("RoleId");
            return userId;
        }
        private string LoggedInUserName()
        {
            string userId = HttpContext.Session.GetString("Username");
            return userId;
        }
        private string LoggedInDesignationName()
        {
            string userId = HttpContext.Session.GetString("Designation");
            return userId;
        }
        private string LoggedInDistrict()
        {
            string userId = HttpContext.Session.GetString("Districtid");
            return userId;
        }
        private string LoggedInRoleName()
        {
            string userId = HttpContext.Session.GetString("Rolename");
            return userId;
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
