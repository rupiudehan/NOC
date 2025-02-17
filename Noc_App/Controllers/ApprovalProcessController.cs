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
using System.Diagnostics.Eventing.Reader;

namespace Noc_App.Controllers
{
    public class ApprovalProcessController :Controller
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
        private readonly IRepository<CircleDivisionMapping> _circleDivRepository;
        private readonly IRepository<EstablishmentOfficeDetails> _establishOffRepository;
        private readonly IRepository<ProcessedApplicationsViewModel> _processedAppDetailsRepo;
        private readonly IRepository<TransferedApplicationsViewModel> _transferedAppDetailsRepo;
        private readonly IConfiguration _configuration;


        public ApprovalProcessController(IRepository<GrantDetails> repo, /*IRepository<VillageDetails> villageRepo,*/ IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<SubDivisionDetails> subDivisionRepo,
            IRepository<DivisionDetails> divisionRepo, IRepository<GrantPaymentDetails> repoPayment,IRepository<GrantApprovalDetail> repoApprovalDetail, IRepository<GrantApprovalMaster> repoApprovalMaster
            , IRepository<ProjectTypeDetails> projectTypeRepo, IRepository<NocPermissionTypeDetails> nocPermissionTypeRepo, IRepository<MasterPlanDetails> masterPlanDetailsRepository,
            IRepository<NocTypeDetails> nocTypeRepo, IRepository<OwnerTypeDetails> ownerTypeRepo, IRepository<GrantPaymentDetails> grantPaymentRepo, IRepository<OwnerDetails> grantOwnersRepo,
            IRepository<GrantKhasraDetails> khasraRepo, IRepository<SiteAreaUnitDetails> siteUnitsRepo, IWebHostEnvironment hostingEnvironment, IEmailService emailService
            , IRepository<GrantApprovalProcessDocumentsDetails> repoApprovalDocument,IRepository<GrantUnprocessedAppDetails> grantUnprocessedAppDetailsRepo
            , ICalculations calculations, IRepository<SiteUnitMaster> repoSiteUnitMaster, IRepository<RecommendationDetail> repoRecommendation
            , IRepository<UserRoleDetails> userRolesRepository, IRepository<GrantSectionsDetails> grantsectionRepository, IRepository<DrainWidthTypeDetails> drainwidthRepository
            , IRepository<GrantRejectionShortfallSection> grantrejectionRepository, IRepository<PlanSanctionAuthorityMaster> repoPlanSanctionAuthtoryMaster
            , IRepository<GrantFileTransferDetails> grantFileTransferRepository, IRepository<CircleDivisionMapping> circleDivRepository, IRepository<TransferedApplicationsViewModel> transferedAppDetailsRepo
            , IRepository<EstablishmentOfficeDetails> establishOffRepository, IRepository<ProcessedApplicationsViewModel> processedAppDetailsRepo, IConfiguration configuration)
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
            _circleDivRepository = circleDivRepository;
            _establishOffRepository = establishOffRepository;
            _processedAppDetailsRepo = processedAppDetailsRepo;
            _transferedAppDetailsRepo = transferedAppDetailsRepo;
            _configuration = configuration;
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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
                if (role == "PRINCIPAL SECRETARY" || role == "MINISTER" || role == "CHIEF ENGINEER DRAINAGE" || role== "EXECUTIVE ENGINEER DRAINAGE" || role== "DIRECTOR DRAINAGE" || role== "DWS" || role== "ADE")
                    divisionId = "0";
                model =await _grantUnprocessedAppDetailsRepo.ExecuteStoredProcedureAsync<GrantUnprocessedAppDetails>("getapplicationstoforward", "0", "0", "0", "0", divisionId, "'" +role+"'", "'" + userId + "'");
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        public async Task<ViewResult> TransferedApplications()
        {
            try
            {

                string userId = LoggedInUserID();
                string divisionId = LoggedInDivisionID();
                var roleName = LoggedInRoleName();
                string role = roleName;
                role = (await GetAppRoleName(role)).AppRoleName;
                if (role == "PRINCIPAL SECRETARY" || role == "CHIEF ENGINEER DRAINAGE" || role == "EXECUTIVE ENGINEER DRAINAGE" || role == "DIRECTOR DRAINAGE" || role == "DWS" || role == "ADE")
                    divisionId = "0";
                List<TransferedApplicationsViewModel> model = new List<TransferedApplicationsViewModel>();

                model = await _transferedAppDetailsRepo.ExecuteStoredProcedureAsync<TransferedApplicationsViewModel>("gettransferedapplications", "0", divisionId, "'" + userId + "'");

                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        public async Task<ViewResult> ProcessedApplications()
        {
            try
            {

                string userId = LoggedInUserID();
                string divisionId = LoggedInDivisionID();
                var roleName = LoggedInRoleName();
                string role = roleName;
                role = (await GetAppRoleName(role)).AppRoleName;
                if (role == "PRINCIPAL SECRETARY" || role == "CHIEF ENGINEER DRAINAGE" || role == "EXECUTIVE ENGINEER DRAINAGE" || role == "DIRECTOR DRAINAGE" || role == "DWS" || role == "ADE")
                    divisionId = "0";
                List<ProcessedApplicationsViewModel> model = new List<ProcessedApplicationsViewModel>();
               
                model = await _processedAppDetailsRepo.ExecuteStoredProcedureAsync<ProcessedApplicationsViewModel>("getprocessedapplications", "0",divisionId, "'" + userId + "'");
               
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }
        }
        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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
                    DivisionDetails grantdivision= (from d in _divisionRepo.GetAll()
                                                    join s in _subDivisionRepo.GetAll() on d.Id equals s.DivisionId
                                                    where s.Id==grant.SubDivisionId
                                                    select new DivisionDetails
                                                    {
                                                        Id=d.Id,
                                                        Name=d.Name
                                                    })
                                                    .FirstOrDefault();

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
                        FromLocationId=Convert.ToInt32(divisionId),
                        ToLocationId= grantdivision.Id,
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
        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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
            var users = (await GetOfficer("0", "EXECUTIVE ENGINEER", "0","0","0")).ToList();
            if (users != null && users.Count > 0)
                officerDetail = users.ToList();
                    //.Where(x => x.UserId != userId).ToList();
            var roleName = LoggedInRoleName();

            string role = roleName;
            List<DivisionDetails> divs = new List<DivisionDetails>();
            GrantFileTransferDetailCreate model = new GrantFileTransferDetailCreate
            {
                Name = grant.Name,
                ApplicantEmailID = grantdetail.Grant.ApplicantEmailID,
                ApplicationID = grantdetail.Grant.ApplicationID,
                ForwardToRole=role,
                Officers = officerDetail.Count > 0 ? new SelectList(officerDetail, "UserId", "UserName") : null,
                Divisions = new SelectList(divs, "Id", "Name"),
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
                officerDetail = (await GetOfficer("0", "EXECUTIVE ENGINEER", "0", "0", "0"));//.Where(x => x.UserId != userId).ToList();
                model.Officers = officerDetail.Count > 0 ? new SelectList(officerDetail, "UserId", "UserName", model.SelectedOfficerId) : null;
                List<OfficerResponseViewModel> officers = (await LoadOfficersAsync("EXECUTIVE ENGINEER", "0", "0", "0", "0")).FindAll(x => x.user_info.EmployeeId == model.SelectedOfficerId);
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
                var roleName = LoggedInRoleName();

                string role = roleName;
                List<DivisionDetails> UserRoleLocation = await GetOfficerLocations("0", role, model.SelectedOfficerId,model.ApplicationID);
                if (model.SelectedDivisionId == null || model.SelectedDivisionId == "" || model.SelectedDivisionId == "0")
                {
                    model.Divisions = new SelectList(UserRoleLocation, "Id", "Name");
                    ModelState.AddModelError("", $"Please select division");

                    return View(model);
                }
                else
                {

                    model.Divisions = new SelectList(UserRoleLocation, "Id", "Name", model.SelectedDivisionId);
                }
                GrantFileTransferDetails details = new GrantFileTransferDetails {
                    GrantId = grant.Id,
                    FromAuthorityId=userId,
                    ToAuthorityId=model.SelectedOfficerId,
                    Remarks=model.Remarks,
                    TransferedOn=DateTime.Now,
                    FromName=username,
                    FromDesignationName=designation,
                    ToName=userRole.Name,
                    FromLocationId= Convert.ToInt32(divisionId),
                    ToLocationId= Convert.ToInt32(model.SelectedDivisionId),
                    ToDesignationName =userRole.Designation
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

            List<DivisionDetails> divs = new List<DivisionDetails>();
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
            officerDetail = await GetOfficer("0", "JUNIOR ENGINEER", "0", "0", "0");//,SUB DIVISIONAL OFFICER

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
                Divisions = new SelectList(divs, "Id", "Name"),
                Officers = officerDetail.Count>0 ? new SelectList(officerDetail, "UserId", "UserName") : null,
                LocationDetails = "Division: " + grantdetail.division.Name + ", Sub-Division: " + grantdetail.subdiv.Name + ", Tehsil/Block: " + grantdetail.tehsil.Name + ", Village: " + grantdetail.Grant.VillageName + ", Pincode: " + grantdetail.Grant.PinCode,
            };
            return View(model);
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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
                //model.SelectedSubDivisionId = "0";
                GrantApprovalMaster master = (await _repoApprovalMaster.FindAsync(x => x.Code == "F")).FirstOrDefault();
                List<OfficerResponseViewModel> officers = (await LoadOfficersAsync("JUNIOR ENGINEER","0", "0", "0", "0")).FindAll(x=>x.user_info.EmployeeId==model.SelectedOfficerId);
                OfficerDetail userRole = (from u in _userRolesRepository.GetAll().AsEnumerable()
                                join officer in officers on u.Id.ToString() equals officer.user_info.RoleID
                                select new OfficerDetail
                                {
                                    EmployeeId= officer.user_info.EmployeeId,
                                    RoleName=u.AppRoleName,
                                    RoleId=u.Id.ToString(),
                                    UserName = officer.user_info.EmployeeName+"("+ officer.user_info.DeesignationName + ")"
                                }).FirstOrDefault();
                List<OfficerDetails> officerDetail = new List<OfficerDetails>();
                officerDetail = await GetOfficer("0", "JUNIOR ENGINEER", "0", "0", "0");//,SUB DIVISIONAL OFFICER
                model.Officers = officerDetail.Count > 0 ? new SelectList(officerDetail, "UserId", "UserName") : null;
                List<DivisionDetails> UserRoleLocation = await GetOfficerLocations(LoggedInDivisionID(), "JUNIOR ENGINEER", model.SelectedOfficerId,model.ApplicationID);
                if (model.SelectedDivisionId == null || model.SelectedDivisionId == "" || model.SelectedDivisionId == "0")
                {
                    model.Divisions = new SelectList(UserRoleLocation, "Id", "Name");
                    ModelState.AddModelError("", $"Please select division");

                    return View(model);
                }
                else
                {

                    model.Divisions = new SelectList(UserRoleLocation, "Id", "Name", model.SelectedDivisionId);
                }
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
                approvalDetail.ProcessedToName = userRole.UserName;
                approvalDetail.UpdatedOn = DateTime.Now;
                approvalDetail.FromLocationId = Convert.ToInt32(divisionId);
                approvalDetail.ToLocationId = Convert.ToInt32(model.SelectedDivisionId);
                
                await _repoApprovalDetail.UpdateAsync(approvalDetail);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [Obsolete]
        [HttpGet]
        public async Task<ViewResult> Forward(string Id)
        {
            try
            {
                string circleIdd = "0";
                string estabOfficeid = "0";
                GrantDetails grant = (await _repo.FindAsync(x => x.ApplicationID == Id)).FirstOrDefault();
                if (grant == null)
                {
                    ViewBag.ErrorMessage = $"Grant with Application Id = {Id} cannot be found";

                    return View("NotFound");
                }

                //int grantDivisionId = (from s in _subDivisionRepo.GetAll()
                //                      where s.Id == grant.SubDivisionId
                //                      select new {division=s.DivisionId}
                //                     ).FirstOrDefault().division;

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
                        else if (approval.ProcessedByRole == "SUB DIVISIONAL OFFICER")
                        {
                            forwardToRole = "DWS,CIRCLE OFFICER";

                            string circleId = (from c in _circleDivRepository.GetAll()
                                               join d in _divisionRepo.GetAll() on c.DivisionId equals d.Id
                                               where d.Id.ToString() == divisionId
                                               select new
                                               {
                                                   CircleId = c.CircleId
                                               }
                                               ).FirstOrDefault().ToString();
                            circleIdd = circleId;
                            divisionId = "0";
                        }
                        else
                        {
                            forwardToRole = "CIRCLE OFFICER";

                            string circleId = (from c in _circleDivRepository.GetAll()
                                               join d in _divisionRepo.GetAll() on c.DivisionId equals d.Id
                                               where d.Id.ToString() == divisionId
                                               select new
                                               {
                                                   CircleId = c.CircleId
                                               }
                                               ).FirstOrDefault().ToString();
                            circleIdd = circleId;
                            divisionId = "0";
                        }
                        break;
                    case "CIRCLE OFFICER":
                        forwardToRole = "EXECUTIVE ENGINEER DRAINAGE";
                        divisionId = "0";
                        break;
                    case "DWS":
                        if (approval.ProcessedByRole == "EXECUTIVE ENGINEER")
                        {
                            forwardToRole = "ADE";
                            divisionId = "0";
                        }
                        else
                        {
                            forwardToRole = "DIRECTOR DRAINAGE";
                            divisionId = "0";
                        }
                        break;
                    case "ADE":
                        forwardToRole = "DWS";
                        divisionId = "0";
                        break;
                    case "DIRECTOR DRAINAGE":
                        forwardToRole = "EXECUTIVE ENGINEER";
                        divisionId = "0";
                        break;
                    case "EXECUTIVE ENGINEER DRAINAGE":
                        forwardToRole = "CHIEF ENGINEER DRAINAGE";
                        divisionId = "0";
                        break;
                    case "CHIEF ENGINEER DRAINAGE":
                        forwardToRole = "PRINCIPAL SECRETARY";
                        divisionId = "0";
                        break;
                    case "PRINCIPAL SECRETARY":
                        forwardToRole = "MINISTER";
                        divisionId = "0";
                        break;
                    default:
                        forwardToRole = "JUNIOR ENGINEER"; break;
                }
                GrantApprovalDetail approvalOfficer = new GrantApprovalDetail();
                //if (forwardToRole == "EXECUTIVE ENGINEER")
                approvalOfficer = null;// (from a in _repoApprovalDetail.GetAll() where a.GrantID == grant.Id && a.ProcessedByRole==forwardToRole orderby a.ProcessedOn descending select a).FirstOrDefault();
                //UserRoleDetails userRoleDetails =  (await GetAppRoleName(forwardToRole));

                //officerDetail = await GetOfficer(divisionId, userRoleDetails.RoleName,"0"); 

                //officerDetail = forwardToRole== "JUNIOR ENGINEER"? await GetOfficer(divisionId, forwardToRole, "0"): await GetOfficer(divisionId, userRoleDetails.RoleName, "0");
                officerDetail = approvalOfficer == null ? (await GetOfficer(divisionId, forwardToRole, "0", circleIdd, estabOfficeid)).Distinct().ToList() :
                    (await GetOfficerLastForwardedBy(forwardToRole, approvalOfficer.ProcessedBy)) ;
                //}
                List<TautologyDetails> tautologyDetails = new List<TautologyDetails>
                {
                    new TautologyDetails{Text="No",Value="false"},
                    new TautologyDetails{Text="Yes",Value="true"}
                };
                List<RecommendationDetail> recommendations = new List<RecommendationDetail>();
                recommendations = _repoRecommendation.GetAll().Where(x=>x.Code!="NA").ToList();
                List<DivisionDetails> divs = new List<DivisionDetails>();
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
                                                          FromLocationId = Convert.ToInt32(LoggedInDivisionID()),
                                                          ToLocationId = approvalOfficer == null ? Convert.ToInt32(LoggedInDivisionID()) : approvalOfficer.FromLocationId,
                                                          Divisions = new SelectList(divs, "Id", "Name"),
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
        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        [Obsolete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Forward(ForwardApplicationViewModel model)
        {
            try
            {
                string circleIdd = "0";
                string estabOfficeid = "0";
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
                int grantid = 0;
                OfficerDetails officerDetail = new OfficerDetails();
                GrantApprovalDetail approval = (from a in _repoApprovalDetail.GetAll() where a.GrantID == grant.Id orderby a.Id descending select a).FirstOrDefault();
                switch (role)
                {
                    case "JUNIOR ENGINEER":
                        forwardToRole = "SUB DIVISIONAL OFFICER";
                        grantid = grant.Id;
                        break;
                    case "SUB DIVISIONAL OFFICER":
                        forwardToRole = "EXECUTIVE ENGINEER";
                        grantid = grant.Id;
                        break;
                    case "EXECUTIVE ENGINEER":
                        if (grant.IsForwarded == false)
                        {
                            forwardToRole = "JUNIOR ENGINEER";
                            grantid = grant.Id;
                        }
                        else if (approval.ProcessedByRole == "SUB DIVISIONAL OFFICER")
                        {
                            forwardToRole = "DWS,CIRCLE OFFICER";

                            string circleId = (from c in _circleDivRepository.GetAll()
                                               join d in _divisionRepo.GetAll() on c.DivisionId equals d.Id
                                               where d.Id.ToString() == divisionId
                                               select new
                                               {
                                                   CircleId = c.CircleId
                                               }
                                               ).FirstOrDefault().ToString();
                            circleIdd = circleId;
                            divisionId = "0";
                        }
                        else
                        {
                            forwardToRole = "CIRCLE OFFICER";

                            string circleId = (from c in _circleDivRepository.GetAll()
                                               join d in _divisionRepo.GetAll() on c.DivisionId equals d.Id
                                               where d.Id.ToString() == divisionId
                                               select new
                                               {
                                                   CircleId = c.CircleId
                                               }
                                               ).FirstOrDefault().ToString();
                            circleIdd = circleId;
                            divisionId = "0";
                        }
                        break;
                    case "CIRCLE OFFICER":
                        forwardToRole = "EXECUTIVE ENGINEER DRAINAGE";
                        divisionId = "0";
                        break;
                    case "DWS":
                        if (approval.ProcessedByRole == "EXECUTIVE ENGINEER")
                        {
                            forwardToRole = "ADE";
                            divisionId = "0";
                        }
                        else
                        {
                            forwardToRole = "DIRECTOR DRAINAGE";
                            divisionId = "0";
                        }
                        break;
                    case "ADE":
                        forwardToRole = "DWS";
                        divisionId = "0";
                        break;
                    case "DIRECTOR DRAINAGE":
                        forwardToRole = "EXECUTIVE ENGINEER";
                        divisionId = "0";
                        grantid = grant.Id;
                        break;
                    case "EXECUTIVE ENGINEER DRAINAGE":
                        forwardToRole = "CHIEF ENGINEER DRAINAGE";
                        divisionId = "0";
                        break;
                    case "CHIEF ENGINEER DRAINAGE":
                        forwardToRole = "PRINCIPAL SECRETARY";
                        divisionId = "0";
                        break;
                    case "PRINCIPAL SECRETARY":
                        forwardToRole = "MINISTER";
                        divisionId = "0";
                        break;
                    default:
                        forwardToRole = "JUNIOR ENGINEER"; break;
                }
                model.LoggedInRole = role;
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
                List<OfficerDetails> officerDetails = new List<OfficerDetails>();
                GrantApprovalDetail approvalOfficer = new GrantApprovalDetail();
                //if (forwardToRole == "EXECUTIVE ENGINEER")
                //approvalOfficer = (from a in _repoApprovalDetail.GetAll() where a.GrantID == grant.Id && a.ProcessedByRole == forwardToRole orderby a.ProcessedOn descending select a).FirstOrDefault();
                if (approvalOfficer == null)
                {
                    officerDetails = await GetOfficer(divisionId, forwardToRole, "0", circleIdd, estabOfficeid);
                }
                else {
                    officerDetails = (await GetOfficerLastForwardedBy(forwardToRole, approvalOfficer.ProcessedBy));
                }

                if (forwardToRole=="JUNIOR ENGINEER")
                {
                    subdiv = "0";
                    //officerDetail = (await GetOfficer(divisionId, forwardToRole, subdiv)).FindAll(x => x.UserId == model.SelectedOfficerId).FirstOrDefault();
                    officerDetail = (await GetOfficer(divisionId, forwardrole, subdiv, circleIdd, estabOfficeid)).FindAll(x => x.UserId == model.SelectedOfficerId).FirstOrDefault();
                }
                else
                {
                    //officerDetail = (await GetOfficer(divisionId, forwardToRole, subdiv)).FindAll(x => x.UserId == model.SelectedOfficerId).FirstOrDefault();
                    var offc = (await GetOfficer(divisionId, forwardrole, subdiv, circleIdd, estabOfficeid)).FindAll(x => x.UserId == model.SelectedOfficerId);
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
                string ErrorMessage = string.Empty;
                model.FromLocationId = Convert.ToInt32(LoggedInDivisionID());
                model.ToLocationId = approvalOfficer == null ? Convert.ToInt32(LoggedInDivisionID()) : approvalOfficer.FromLocationId;
                if (forwardedRole == "EXECUTIVE ENGINEER" || forwardedRole == "JUNIOR ENGINEER" || forwardedRole == "SUB DIVISIONAL OFFICER" || forwardedRole == "CIRCLE OFFICER" || forwardedRole == "DWS,CIRCLE OFFICER")
                {
                    List<DivisionDetails> UserRoleLocation = await GetOfficerLocations(LoggedInDivisionID(), forwardedRole, model.SelectedOfficerId,grant.ApplicationID);
                    if (model.SelectedDivisionId == null || model.SelectedDivisionId == "" || model.SelectedDivisionId == "0")
                    {
                        model.Divisions = new SelectList(UserRoleLocation, "Id", "Name");
                        ErrorMessage = $"Please select division";
                        ModelState.AddModelError("", ErrorMessage);

                        return View(model);
                    }
                    else
                    {

                        model.Divisions = new SelectList(UserRoleLocation, "Id", "Name",model.SelectedDivisionId);
                    }
                }
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
                    Remarks=model.Remarks,
                    FromLocationId= model.FromLocationId,
                    ToLocationId= Convert.ToInt32(model.SelectedDivisionId)
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,ADE,DIRECTOR DRAINAGE")]
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,ADE,DIRECTOR DRAINAGE")]
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

                DivisionDetails grantdivision = (from d in _divisionRepo.GetAll()
                                                 join s in _subDivisionRepo.GetAll() on d.Id equals s.DivisionId
                                                 where s.Id == grant.SubDivisionId
                                                 select new DivisionDetails
                                                 {
                                                     Id = d.Id,
                                                     Name = d.Name
                                                 })
                                                .FirstOrDefault();
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
                    FromLocationId=Convert.ToInt32(divisionId),
                    ToLocationId= grantdivision.Id,
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CHIEF ENGINEER DRAINAGE")]
        [HttpGet]
        public async Task<IActionResult> PartiallyApproveNOC(string id)
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
            PartiallyApproveViewModelCreate model = new PartiallyApproveViewModelCreate();
            string forwardToRole = "EXECUTIVE ENGINEER DRAINAGE";
            var roleName = LoggedInRoleName();

            string divisionId = LoggedInDivisionID();
            List<DivisionDetails> divs = new List<DivisionDetails>();
            List<OfficerDetails> officerDetail = new List<OfficerDetails>();
            officerDetail = (await GetOfficer(divisionId, forwardToRole, "0", "0", "0")).Distinct().ToList();
            if (grant.IsUnderMasterPlan)
            {
                model = (from g in _repo.GetAll()
                         join pay in _masterPlanDetailsRepository.GetAll() on g.MasterPlanId equals pay.Id
                         //join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                         join t in _tehsilBlockRepo.GetAll() on g.TehsilID equals t.Id
                         join sub in _subDivisionRepo.GetAll() on g.SubDivisionId equals sub.Id
                         join div in _divisionRepo.GetAll() on sub.DivisionId equals div.Id
                         where g.ApplicationID == id
                         select new PartiallyApproveViewModelCreate
                         {
                             Id = g.Id,
                             Name = g.Name,
                             ApplicationID = g.ApplicationID,
                             IsUnderMasterPlan = g.IsUnderMasterPlan,
                             ForwardToRole = forwardToRole,
                             LoggedInRole = roleName,
                             FromLocationId = Convert.ToInt32(LoggedInDivisionID()),
                             ToLocationId = Convert.ToInt32(LoggedInDivisionID()),
Divisions = new SelectList(divs, "Id", "Name"),
                             Officers = officerDetail != null ? officerDetail.Count() > 0 ? new SelectList(officerDetail, "UserId", "UserName") : null : null,
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
                         select new PartiallyApproveViewModelCreate
                         {
                             Id = g.Id,
                             Name = g.Name,
                             ApplicationID = g.ApplicationID,
                             IsUnderMasterPlan = g.IsUnderMasterPlan,
                             ForwardToRole = forwardToRole,
                             LoggedInRole = roleName,
                             FromLocationId = Convert.ToInt32(LoggedInDivisionID()),
                             ToLocationId = Convert.ToInt32(LoggedInDivisionID()),
                             Divisions = new SelectList(divs, "Id", "Name"),
                             Officers = officerDetail != null ? officerDetail.Count() > 0 ? new SelectList(officerDetail, "UserId", "UserName") : null : null,
                             LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + g.VillageName + ", Pincode: " + g.PinCode,
                         }).FirstOrDefault();
            }
            return View(model);
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CHIEF ENGINEER DRAINAGE")]
        [HttpPost]
        [Obsolete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartiallyApproveNOC(PartiallyApproveViewModelCreate model)
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

            string userId = LoggedInUserID();

            string divisionId = LoggedInDivisionID();

            string subdivisionId = LoggedInSubDivisionID();

            string username = LoggedInUserName();

            string designation = LoggedInDesignationName();

            // Retrieve roles associated with the user
            var roleName = LoggedInRoleName();

            string role = roleName;
            DivisionDetails grantdivision = (from d in _divisionRepo.GetAll()
                                             join s in _subDivisionRepo.GetAll() on d.Id equals s.DivisionId
                                             where s.Id == grant.SubDivisionId
                                             select new DivisionDetails
                                             {
                                                 Id = d.Id,
                                                 Name = d.Name
                                             })
                                               .FirstOrDefault();
            GrantApprovalMaster master = (await _repoApprovalMaster.FindAsync(x => x.Code == "A")).FirstOrDefault();
            int approvalLevel = (await _repoApprovalDetail.FindAsync(x => x.GrantID == grant.Id && x.ApprovalID == master.Id)).Count();
            var recommendations = _repoRecommendation.Find(x => x.Code == "A").FirstOrDefault();
            string forwardToRole = "EXECUTIVE ENGINEER DRAINAGE";

            List<OfficerDetails> officerDetail = new List<OfficerDetails>();
            officerDetail = (await GetOfficer("0", forwardToRole, "0", "0", divisionId));
            model.Officers = new SelectList(officerDetail, "UserId", "UserName", model.SelectedOfficerId);
            List<DivisionDetails> UserRoleLocation = await GetOfficerLocations(LoggedInDivisionID(), forwardToRole, model.SelectedOfficerId, grant.ApplicationID);
            model.Divisions = new SelectList(UserRoleLocation, "Id", "Name", model.SelectedDivisionId);
                if (model.SelectedDivisionId == null || model.SelectedDivisionId == "" || model.SelectedDivisionId == "0")
                {
                model.Divisions = new SelectList(UserRoleLocation, "Id", "Name");
                ModelState.AddModelError("", $"Please select division");

                    return View(model);
                }
                model.Divisions = new SelectList(UserRoleLocation, "Id", "Name", model.SelectedDivisionId);
               
                    GrantApprovalDetail approvalDetail = new GrantApprovalDetail
                    {
                        GrantID = grant.Id,
                        ApprovalID = master.Id,
                        ProcessedBy = userId,
                        ProcessedOn = DateTime.Now,
                        ProcessedByRole = role,
                        ProcessedByName = username + "(" + designation + ")",
                        ProcessLevel = approvalLevel + 1,
                        ProcessedToRole = forwardToRole,
                        ProcessedToUser = model.SelectedOfficerId,
                        RecommendationID = recommendations.Id,
                        FromLocationId = Convert.ToInt32(divisionId),
                        ToLocationId = Convert.ToInt32(model.SelectedDivisionId),
                        Remarks=model.Remarks
                    };

                    await _repoApprovalDetail.CreateAsync(approvalDetail);
                    //_repoApprovalDetail.
                    grant.IsForwarded = false;
                    grant.ProcessLevel = approvalDetail.ProcessLevel;
                    grant.IsPending = false;
                    grant.IsPartiallyApproved = true;
                    grant.PartiallyApprovedByRole = role;
                    grant.PartiallyApprovedBy = userId;
                    grant.PartiallyApprovedOn = DateTime.Now;
                    await _repo.UpdateAsync(grant);

                    var emailModel = new EmailModel(grant.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForIssue(grant.ApplicantName, grant.ApplicationID, "", grant.IsUnderMasterPlan));
                    _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");

                    return RedirectToAction("Index");
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,EXECUTIVE ENGINEER DRAINAGE,CHIEF ENGINEER DRAINAGE")]
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
            var approval = (await _repoApprovalDetail.FindAsync(x => x.GrantID == grant.Id)).OrderBy(x => x.Id).LastOrDefault();
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
                             IsPartiallyApproved=grant.IsPartiallyApproved,
                             PreviousRemarks= grant.IsPartiallyApproved?approval.Remarks:"",
                             PreviosAuthorityRole = grant.IsPartiallyApproved ? approval.ProcessedByRole : "",
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
                             IsPartiallyApproved = grant.IsPartiallyApproved,
                             PreviousRemarks = grant.IsPartiallyApproved ? approval.Remarks : "",
                             PreviosAuthorityRole= grant.IsPartiallyApproved ?approval.ProcessedByRole :"",
                             LocationDetails = "Division: " + div.Name + ", Sub-Division: " + sub.Name + ", Tehsil/Block: " + t.Name + ", Village: " + g.VillageName + ", Pincode: " + g.PinCode,
                         }).FirstOrDefault();
            }
            return View(model);
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,EXECUTIVE ENGINEER DRAINAGE,CHIEF ENGINEER DRAINAGE")]
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
                //if(model.IsUnderMasterPlan) ErrorMessage = $"Invalid NOC file type. Please upload a PDF file only";
                //else 
                    ErrorMessage = $"Invalid certificate file type. Please upload a PDF file only";
                    ModelState.AddModelError("", ErrorMessage);

                    return View(model);

                }
                else if (certificateValidation == 2)
                {
                //if(model.IsUnderMasterPlan)
                //    ErrorMessage = "Certificate field is required";
                //else
                    ErrorMessage = "Certificate field is required";
                ModelState.AddModelError("", ErrorMessage);
                    return View(model);
                }

                if (!AllowedFileSize(model.CertificateFile))
                {
                //    if(model.IsUnderMasterPlan)
                //    ErrorMessage = "NOC size exceeds the allowed limit of 4MB";
                //else
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
            DivisionDetails grantdivision = (from d in _divisionRepo.GetAll()
                                             join s in _subDivisionRepo.GetAll() on d.Id equals s.DivisionId
                                             where s.Id == grant.SubDivisionId
                                             select new DivisionDetails
                                             {
                                                 Id = d.Id,
                                                 Name = d.Name
                                             })
                                               .FirstOrDefault();
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
                RecommendationID=3,
                FromLocationId=Convert.ToInt32(divisionId),
                ToLocationId=grantdivision.Id
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE,Administrator")]
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
                                                                     RecommendedToName=ap.ProcessedToName,
                                                                     CreatedOn= string.Format("{0:dd/MM/yyyy HH:mm tt}", ap.ProcessedOn)
                                                                 }).ToList();
                List<GrantFileTransferDetailsViewModel> modelFileTransfer = (from ap in _grantFileTransferRepository.GetAll()
                                                                               where ap.GrantId == obj.Id 
                                                                               orderby ap.TransferedOn descending
                                                                               select new GrantFileTransferDetailsViewModel
                                                                               {
                                                                                   FromName=ap.FromName,
                                                                                   Remarks = ap.Remarks,
                                                                                   FromDesignationName=ap.FromDesignationName,
                                                                                   FromAuthorityId=ap.FromAuthorityId,
                                                                                   ToAuthorityId=ap.ToAuthorityId,
                                                                                   ToName=ap.ToName,
                                                                                   ToDesignationName=ap.ToDesignationName,
                                                                                   TransferedOn=string.Format("{0:dd/MM/yyyy HH:mm tt}", ap.TransferedOn)
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        public async Task<IActionResult> ViewProcessedApplication(string Id)
        {
            try
            {
                GrantDetails obj = (await _repo.FindAsync(x => x.ApplicationID.ToLower() == Id.ToLower())).FirstOrDefault();
                PlanSanctionAuthorityMaster planAuth = await _repoPlanSanctionAuthtoryMaster.GetByIdAsync(obj.PlanSanctionAuthorityId);
                //VillageDetails village = await _villageRpo.GetByIdAsync(obj.VillageID);
                TehsilBlockDetails tehsil = await _tehsilBlockRepo.GetByIdAsync(obj.TehsilID);
                SubDivisionDetails subDivision = await _subDivisionRepo.GetByIdAsync(obj.SubDivisionId);
                DivisionDetails division = await _divisionRepo.GetByIdAsync(subDivision.DivisionId);
                NocPermissionTypeDetails permission = await _nocPermissionTypeRepo.GetByIdAsync(obj.NocPermissionTypeID ?? 0);
                ProjectTypeDetails projecttype = await _projectTypeRepo.GetByIdAsync(obj.ProjectTypeId);
                NocTypeDetails noctype = await _nocTypeRepo.GetByIdAsync(obj.NocTypeId ?? 0);
                SiteAreaUnitDetails unit = await _siteUnitsRepo.GetByIdAsync(obj.SiteAreaUnitId ?? 0);
                MasterPlanDetails masterplan = (from np in _masterPlanDetailsRepository.GetAll() where obj.MasterPlanId == np.Id select np).FirstOrDefault();
                List<GrantKhasraDetails> khasras = (await _khasraRepo.FindAsync(x => x.GrantID == obj.Id)).ToList();
                List<OwnerDetails> owners = (await _grantOwnersRepo.FindAsync(x => x.GrantId == obj.Id)).ToList();
                List<GrantInspectionDocuments> documents = (from d in _repoApprovalDocument.GetAll()
                                                            join a in _repoApprovalDetail.GetAll() on d.GrantApprovalID equals a.Id
                                                            join dr in _drainwidthRepository.GetAll() on d.TypeOfWidth equals dr.Id
                                                            where a.GrantID == obj.Id
                                                            orderby d.Id descending
                                                            select new GrantInspectionDocuments
                                                            {
                                                                GisOrDwsFilePath = d.GISOrDWSReportPath,
                                                                CatchmentAreaFilePath = d.CatchmentAreaAndFlowPath,
                                                                CrossSectionOrCalculationFilePath = d.CrossSectionOrCalculationSheetReportPath,
                                                                DistanceFromCreekFilePath = d.DistanceFromCreekPath,
                                                                LSectionOfDrainFilePath = d.DrainLSectionPath,
                                                                IsKMLByApplicantValid = d.IsKMLByApplicantValid,
                                                                UploadedByRole = d.ProcessedByRole,
                                                                UploadedByName = d.ProcessedByName,
                                                                SiteConditionReportFilePath = d.SiteConditionReportPath,
                                                                IsDrainNotified = Convert.ToBoolean(d.IsDrainNotified),
                                                                DrainWidth = d.DrainWidth,
                                                                TypeOfWidthName = "Width " + dr.Name

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
                                                                                   RecommendedByName = ap.ProcessedByName,
                                                                                   RecommendedToName = ap.ProcessedToName,
                                                                                   CreatedOn = string.Format("{0:dd/MM/yyyy HH:mm tt}", ap.ProcessedOn)
                                                                               }).ToList();
                var rm = (from ap in _repoApprovalDetail.GetAll()
                          join recommend in _repoRecommendation.GetAll() on ap.RecommendationID equals recommend.Id
                          join ma in _repoApprovalMaster.GetAll() on ap.ApprovalID equals ma.Id
                          where ap.GrantID == obj.Id && (obj.IsRejected == true || obj.IsApproved == true) && recommend.Code == "NA"
                          orderby ap.ProcessedOn descending
                          select new GrantApprovalRecommendationDetail
                          {
                              Remarks = ap.Remarks
                          });
                string remarks =rm!=null && rm.Count()>0? rm.AsEnumerable().FirstOrDefault().Remarks==null?"": rm.AsEnumerable().LastOrDefault().Remarks:"";
                List<GrantFileTransferDetailsViewModel> modelFileTransfer = (from ap in _grantFileTransferRepository.GetAll()
                                                                    where ap.GrantId == obj.Id
                                                                    orderby ap.TransferedOn descending
                                                                    select new GrantFileTransferDetailsViewModel
                                                                    {
                                                                        FromName = ap.FromName,
                                                                        Remarks = ap.Remarks,
                                                                        FromDesignationName = ap.FromDesignationName,
                                                                        FromAuthorityId = ap.FromAuthorityId,
                                                                        ToAuthorityId = ap.ToAuthorityId,
                                                                        ToName = ap.ToName,
                                                                        ToDesignationName = ap.ToDesignationName,
                                                                        TransferedOn = string.Format("{0:dd/MM/yyyy HH:mm tt}", ap.TransferedOn)
                                                                    }).ToList();
                var payment = await _grantPaymentRepo.FindAsync(x => x.GrantID == obj.Id);
                if (payment == null || payment.Count() == 0)
                {
                    GrantDetailViewModel model = new GrantDetailViewModel
                    {
                        Id = obj.Id,
                        IsApproved=obj.IsApproved,
                        IsRejected=obj.IsRejected,
                        ApplicationID = obj.ApplicationID,
                        PaymentOrderId = "0",
                        ReceiptNo = "0",
                        Amount = "0",
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
                        UnderMasterPlan = obj.IsUnderMasterPlan ? "Yes" : "No",
                        Remarks = remarks
                    };
                    return View(model);
                }
                else
                {
                    GrantPaymentDetails objPyment = (payment).FirstOrDefault();
                    GrantDetailViewModel model = new GrantDetailViewModel
                    {
                        Id = obj.Id,
                        IsApproved = obj.IsApproved,
                        IsRejected = obj.IsRejected,
                        ApplicationID = obj.ApplicationID,
                        PaymentOrderId = objPyment.deptRefNo,
                        Amount = Math.Round(objPyment.TotalAmount ?? 0, 5).ToString(),
                        ReceiptNo = objPyment.PaymentOrderId,
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
                        UnderMasterPlan = obj.IsUnderMasterPlan ? "Yes" : "No",
                        Remarks=remarks
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
        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        public async Task<IActionResult> ViewTransferedApplication(string Id)
        {
            try
            {
                GrantDetails obj = (await _repo.FindAsync(x => x.ApplicationID.ToLower() == Id.ToLower())).FirstOrDefault();
                PlanSanctionAuthorityMaster planAuth = await _repoPlanSanctionAuthtoryMaster.GetByIdAsync(obj.PlanSanctionAuthorityId);
                //VillageDetails village = await _villageRpo.GetByIdAsync(obj.VillageID);
                TehsilBlockDetails tehsil = await _tehsilBlockRepo.GetByIdAsync(obj.TehsilID);
                SubDivisionDetails subDivision = await _subDivisionRepo.GetByIdAsync(obj.SubDivisionId);
                DivisionDetails division = await _divisionRepo.GetByIdAsync(subDivision.DivisionId);
                NocPermissionTypeDetails permission = await _nocPermissionTypeRepo.GetByIdAsync(obj.NocPermissionTypeID ?? 0);
                ProjectTypeDetails projecttype = await _projectTypeRepo.GetByIdAsync(obj.ProjectTypeId);
                NocTypeDetails noctype = await _nocTypeRepo.GetByIdAsync(obj.NocTypeId ?? 0);
                SiteAreaUnitDetails unit = await _siteUnitsRepo.GetByIdAsync(obj.SiteAreaUnitId ?? 0);
                MasterPlanDetails masterplan = (from np in _masterPlanDetailsRepository.GetAll() where obj.MasterPlanId == np.Id select np).FirstOrDefault();
                List<GrantKhasraDetails> khasras = (await _khasraRepo.FindAsync(x => x.GrantID == obj.Id)).ToList();
                List<OwnerDetails> owners = (await _grantOwnersRepo.FindAsync(x => x.GrantId == obj.Id)).ToList();
                List<GrantInspectionDocuments> documents = (from d in _repoApprovalDocument.GetAll()
                                                            join a in _repoApprovalDetail.GetAll() on d.GrantApprovalID equals a.Id
                                                            join dr in _drainwidthRepository.GetAll() on d.TypeOfWidth equals dr.Id
                                                            where a.GrantID == obj.Id
                                                            orderby d.Id descending
                                                            select new GrantInspectionDocuments
                                                            {
                                                                GisOrDwsFilePath = d.GISOrDWSReportPath,
                                                                CatchmentAreaFilePath = d.CatchmentAreaAndFlowPath,
                                                                CrossSectionOrCalculationFilePath = d.CrossSectionOrCalculationSheetReportPath,
                                                                DistanceFromCreekFilePath = d.DistanceFromCreekPath,
                                                                LSectionOfDrainFilePath = d.DrainLSectionPath,
                                                                IsKMLByApplicantValid = d.IsKMLByApplicantValid,
                                                                UploadedByRole = d.ProcessedByRole,
                                                                UploadedByName = d.ProcessedByName,
                                                                SiteConditionReportFilePath = d.SiteConditionReportPath,
                                                                IsDrainNotified = Convert.ToBoolean(d.IsDrainNotified),
                                                                DrainWidth = d.DrainWidth,
                                                                TypeOfWidthName = "Width " + dr.Name

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
                                                                                   RecommendedByName = ap.ProcessedByName,
                                                                                   RecommendedToName = ap.ProcessedToName,
                                                                                   CreatedOn = string.Format("{0:dd/MM/yyyy HH:mm tt}", ap.ProcessedOn)
                                                                               }).ToList();
                var rm = (from ap in _repoApprovalDetail.GetAll()
                          join recommend in _repoRecommendation.GetAll() on ap.RecommendationID equals recommend.Id
                          join ma in _repoApprovalMaster.GetAll() on ap.ApprovalID equals ma.Id
                          where ap.GrantID == obj.Id && (obj.IsRejected == true || obj.IsApproved == true) && recommend.Code == "NA"
                          orderby ap.ProcessedOn descending
                          select new GrantApprovalRecommendationDetail
                          {
                              Remarks = ap.Remarks
                          });
                string remarks = rm != null && rm.Count() > 0 ? rm.AsEnumerable().FirstOrDefault().Remarks == null ? "" : rm.AsEnumerable().LastOrDefault().Remarks : "";
                List<GrantFileTransferDetailsViewModel> modelFileTransfer = (from ap in _grantFileTransferRepository.GetAll()
                                                                             where ap.GrantId == obj.Id
                                                                             orderby ap.TransferedOn descending
                                                                             select new GrantFileTransferDetailsViewModel
                                                                             {
                                                                                 FromName = ap.FromName,
                                                                                 Remarks = ap.Remarks,
                                                                                 FromDesignationName = ap.FromDesignationName,
                                                                                 FromAuthorityId = ap.FromAuthorityId,
                                                                                 ToAuthorityId = ap.ToAuthorityId,
                                                                                 ToName = ap.ToName,
                                                                                 ToDesignationName = ap.ToDesignationName,
                                                                                 TransferedOn = string.Format("{0:dd/MM/yyyy HH:mm tt}", ap.TransferedOn)
                                                                             }).ToList();
                var payment = await _grantPaymentRepo.FindAsync(x => x.GrantID == obj.Id);
                if (payment == null || payment.Count() == 0)
                {
                    GrantDetailViewModel model = new GrantDetailViewModel
                    {
                        Id = obj.Id,
                        IsApproved = obj.IsApproved,
                        IsRejected = obj.IsRejected,
                        ApplicationID = obj.ApplicationID,
                        PaymentOrderId = "0",
                        ReceiptNo = "0",
                        Amount = "0",
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
                        UnderMasterPlan = obj.IsUnderMasterPlan ? "Yes" : "No",
                        Remarks = remarks
                    };
                    return View(model);
                }
                else
                {
                    GrantPaymentDetails objPyment = (payment).FirstOrDefault();
                    GrantDetailViewModel model = new GrantDetailViewModel
                    {
                        Id = obj.Id,
                        IsApproved = obj.IsApproved,
                        IsRejected = obj.IsRejected,
                        ApplicationID = obj.ApplicationID,
                        PaymentOrderId = objPyment.deptRefNo,
                        Amount = Math.Round(objPyment.TotalAmount ?? 0, 5).ToString(),
                        ReceiptNo = objPyment.PaymentOrderId,
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
                        UnderMasterPlan = obj.IsUnderMasterPlan ? "Yes" : "No",
                        Remarks = remarks
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [Obsolete]
        [HttpPost]
        public async Task<IActionResult> GetOfficers(int subdivisionId,string roleName)
        {
            string divisionId = "0";
            //string role = (await GetAppRoleName(roleName)).RoleName;
            List<OfficerDetails> officerDetail = await GetOfficer(divisionId, roleName, subdivisionId.ToString(),"0","0");
            return Json(new SelectList(officerDetail, "UserId", "UserName"));
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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
                                                                     Remarks = a.Remarks,
                                                                     CreatedOn = string.Format("{0:dd/MM/yyyy HH:mm tt}", a.ProcessedOn)
                                                                 }).ToList();



                return Json(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                //ModelState.AddModelError(string.Empty, ex.Message);
                return Json(null);
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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
                return Json(null);
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        private OfficerResponseViewModel1 FetchUser()
        {
            OfficerResponseViewModel1 user_info = new OfficerResponseViewModel1();
            List<officer_info> users = new List<officer_info>();
            officer_info user_info1 = new officer_info();
            officer_info user_info2 = new officer_info();
            officer_info user_info3 = new officer_info();
            officer_info user_info4 = new officer_info();
            officer_info user_info5 = new officer_info();
            officer_info user_info6 = new officer_info();
            officer_info user_info7 = new officer_info();
            officer_info user_info8 = new officer_info();
            officer_info user_info9 = new officer_info();
            officer_info user_info10 = new officer_info();
            officer_info user_info11 = new officer_info();
            officer_info user_info12 = new officer_info();
            //user_info1 =  new user_info { OfficeWiseRoleID = new List<OfficeWiseRolesIds> { new OfficeWiseRolesIds { office_id = 54, role = 10 }, new OfficeWiseRolesIds { office_id = 33, role = 60 }}, Name = "Junior Engineer",EmployeeName="N", Designation = "xyz", DesignationID = 1, Role = "Chief Engineer,Junior Engineer", RoleID = "10,60", DivisionID = 54.ToString(), Division = "Executive Engineer Shri Muktsar Sahib Drainage-cum-Mining &amp; Geology Division, WRD, Punjab", DistrictID = 22, District = "Mukatsar", EmailId = "juniorengineer", EmpID = "123", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };

            user_info1 = new officer_info
            {
                EmployeeName = "Junior Engineer",
                DeesignationName = "xyz",
                DesignationID = 8,
                RoleName = "Employee,Junior Engineer",
                RoleID = "2,60",
                DivisionID = "27",
                DivisionName = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab, Chief Engineer Bbmb Nangal, Bbmb Nangal",
                DistrictId = 20,
                DistrictName = "Rupnagar",
                email = "je",
                EmployeeId = "124",
                MobileNo = "231221234",
                SubdivisionName = "",
                SubdivisionId = 0
            };
            user_info2 = new officer_info
            {
                EmployeeName = "Executive Engineer",
                DeesignationName = "xyz",
                DesignationID = 8,
                RoleName = "Employee, DDO, Executive Engineer",
                RoleID = "3,2,7",
                DivisionID = "168,161,27",
                DivisionName = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab, Chief Engineer Bbmb Nangal, Bbmb Nangal",
                DistrictId = 20,
                DistrictName = "Rupnagar",
                email = "xen",
                EmployeeId = "123",
                MobileNo = "231221234",
                SubdivisionName = "",
                SubdivisionId = 0
            };
            user_info12 = new officer_info
            {
                EmployeeName = "Sub Divisional Officer",
                DeesignationName = "xyz",
                DesignationID = 8,
                RoleName = "Employee,  Sub Divisional Officer",
                RoleID = "2,67",
                DivisionID = "27",
                DivisionName = "Executive Engineer Sh Anandpur Sahib Drainage cum Mining and Geology Division WRD Punjab",
                DistrictId = 20,
                DistrictName = "Rupnagar",
                email = "sdo",
                EmployeeId = "125",
                MobileNo = "231221234",
                SubdivisionName = "",
                SubdivisionId = 0
            };
            user_info3 = new officer_info
            {
                EmployeeName = "Superintending Engineer",
                DeesignationName = "xyz",
                DesignationID = 8,
                RoleName = "Employee, Superintending Engineer",
                RoleID = "2,8",
                DivisionID = "209",
                DivisionName = "Superintending Engineer Ropar  Drainage-cum-Mining & Geology Circle, WRD Punjab",
                DistrictId = 20,
                DistrictName = "Rupnagar",
                email = "se",
                EmployeeId = "126",
                MobileNo = "231221234",
                SubdivisionName = "",
                SubdivisionId = 0
            };
            user_info4 = new officer_info
            {
                EmployeeName = "XEN/DWS",
                DeesignationName = "xyz",
                DesignationID = 8,
                RoleName = "Employee, XEN/DWS",
                RoleID = "2,83",
                DivisionID = "37",
                DivisionName = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictId = 20,
                DistrictName = "Rupnagar",
                email = "dws",
                EmployeeId = "127",
                MobileNo = "231221234",
                SubdivisionName = "",
                SubdivisionId = 0
            };
            user_info5 = new officer_info
            {
                EmployeeName = "XEN HO Drainage",
                DeesignationName = "xyz",
                DesignationID = 8,
                RoleName = "Employee, XEN HO Drainage",
                RoleID = "2,128",
                DivisionID = "37",
                DivisionName = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictId = 20,
                DistrictName = "Rupnagar",
                email = "xenho",
                EmployeeId = "128",
                MobileNo = "231221234",
                SubdivisionName = "",
                SubdivisionId = 0
            };
            user_info6 = new officer_info
            {
                EmployeeName = "Chief Engineer",
                DeesignationName = "xyz",
                DesignationID = 8,
                RoleName = "Employee, Chief Engineer",
                RoleID = "2,10",
                DivisionID = "37",
                DivisionName = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictId = 20,
                DistrictName = "Rupnagar",
                email = "ce",
                EmployeeId = "129",
                MobileNo = "231221234",
                SubdivisionName = "",
                SubdivisionId = 0
            };
            user_info7 = new officer_info
            {
                EmployeeName = "ADE",
                DeesignationName = "xyz",
                DesignationID = 8,
                RoleName = "Employee, ADE/DWS",
                RoleID = "2,90",
                DivisionID = "37",
                DivisionName = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictId = 20,
                DistrictName = "Rupnagar",
                email = "ade",
                EmployeeId = "134",
                MobileNo = "231221234",
                SubdivisionName = "",
                SubdivisionId = 0
            };
            user_info8 = new officer_info
            {
                EmployeeName = "Director Drainage",
                DeesignationName = "xyz",
                DesignationID = 8,
                RoleName = "Employee, Director Drainage",
                RoleID = "2,35",
                DivisionID = "37",
                DivisionName = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictId = 20,
                DistrictName = "Rupnagar",
                email = "dd",
                EmployeeId = "130",
                MobileNo = "231221234",
                SubdivisionName = "",
                SubdivisionId = 0
            };
            user_info9 = new officer_info
            {   
                EmployeeName = "Principal Secretary",
                DeesignationName = "xyz",
                DesignationID = 8,
                RoleName = "Employee, Principal Secretary",
                RoleID = "2,6",
                DivisionID = "37",
                DivisionName = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictId = 20,
                DistrictName = "Rupnagar",
                email = "ps",
                EmployeeId = "131",
                MobileNo = "231221234",
                SubdivisionName = "",
                SubdivisionId = 0
            };
            user_info10 = new officer_info { EmployeeName = "N", DeesignationName = "xyz", DesignationID = 1, RoleName = "Administrator", RoleID = 1.ToString(), DivisionID = 63.ToString(), DivisionName = "test", DistrictId = 22, DistrictName = "Amritsar", email = "admin", EmployeeId = "132", MobileNo = "231221234", SubdivisionName = "", SubdivisionId = 0 };
            user_info11 = new officer_info
            {               
                EmployeeName = "Minister",
                DeesignationName = "xyz",
                DesignationID = 8,
                RoleName = "Employee, Minister(WR)",
                RoleID = "2,114",
                DivisionID = "37",
                DivisionName = "CHIEF ENGINEER DESIGN CHANDIGARH",
                DistrictId = 20,
                DistrictName = "Rupnagar",
                email = "minister",
                EmployeeId = "133",
                MobileNo = "231221234",
                SubdivisionName = "",
                SubdivisionId = 0
            };
            users.Add(user_info1);
            users.Add(user_info2);
            users.Add(user_info3);
            users.Add(user_info4);
            users.Add(user_info5);
            users.Add(user_info6);
            users.Add(user_info7);
            users.Add(user_info8);
            users.Add(user_info9);
            users.Add(user_info10);
            users.Add(user_info11);
            users.Add(user_info12);
            user_info.user_info = users;
            user_info.Status = "200";
            return user_info;

        }
        //[Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        //[Obsolete]
        private async Task<LoginResponseViewModel> LoadUserDetailById(string userid)
        {
            //int istest = Convert.ToInt16(_configuration["Testing"].ToString());
            //if (istest == 0)
            //{
                string baseUrl = "https://wrdpbind.com/api/loginv7.php";
                string salt = "7QCHhk5cJ6OMGfEE";
                string checksum = "jOOwFCGMqKvJARC1tU6l8r77";
                string combinedPassword = userid + "|" + checksum;

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
                    ModelState.AddModelError(string.Empty, "An error has occurred.");
                    return null;
                }
                LoginResponseViewModel root = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginResponseViewModel>(resultContent);
                return root;
            //}
            //else
            //{
            //    var list = await FetchUserLogin();
            //    LoginResponseViewModel root = list.FirstOrDefault(x => x.user_info.EmpID == userid);
            //    return root;
            //}
        }
        private async Task<List<LoginResponseViewModel>> FetchUserLogin()
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
            user_info10 = new user_info { OfficeWiseRoleID = new List<OfficeWiseRolesIds> { new OfficeWiseRolesIds { office_id = 63, role = 1 } }, Name = "Administrator", EmployeeName = "N", Designation = "xyz", DesignationID = 1, Role = "Administrator", RoleID = 1.ToString(), DivisionID = 63.ToString(), Division = "test", DistrictID = 22, District = "Amritsar", EmailId = "admin", EmpID = "132", MobileNo = "231221234", SubDivision = "", SubDivisionID = 0 };
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
        private async Task<List<OfficerResponseViewModel>> LoadOfficersAsync(string officerRole, string subdivisionId, string divisionId, string circleid, string establishmentOfficeId)
        {
            try
            {
                //int istest = Convert.ToInt16(_configuration["Testing"].ToString());
                //if (istest == 0)
                //{
                    string baseUrl = "";
                    string salt = "";
                    string checksum = "";
                    string combinedPassword = "";
                    string circleId = circleid;

                    UserRoleDetails officer = (await GetAppRoleName(officerRole));
                    officerRole = officer.RoleName;
                    baseUrl = "https://wrdpbind.com/api/login6.php";
                    salt = "6WCHhk3cJ6OMGdRg";
                    checksum = "dTOwFCGMqKvJARC1tU6l4sv6";
                    combinedPassword = officerRole + "|" + divisionId + "|" + checksum;
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
                    client.DefaultRequestHeaders.Add("details", encryptedString);
                    var tokenResponse1 = await client.GetAsync(client.BaseAddress.ToString());
                    string resultContent = //"[" + 
                        tokenResponse1.Content.ReadAsStringAsync().Result;//.Replace("}{", "},{") + "]";
                    OfficerResponseViewModel1 list = Newtonsoft.Json.JsonConvert.DeserializeObject<OfficerResponseViewModel1>(resultContent);
                    if (list.Status == "400")
                    {
                        return null;
                    }
                    else
                    {
                        if (list != null)
                        {
                            if (list.Status == "200")
                            {
                                List<OfficerResponseViewModel> listOfficers = new List<OfficerResponseViewModel>();
                                List<officer_info> officers = new List<officer_info>();

                                officers = list.user_info.FindAll(x => x.RoleName.Contains(officerRole) && x.DivisionID != null & x.DivisionID != "");

                                foreach (officer_info user in officers)
                                {

                                    OfficerResponseViewModel obj = new OfficerResponseViewModel
                                    {
                                        msg = "success",
                                        Status = "200",
                                        user_info = new officer_info { UserName = user.UserName, EmployeeId = user.EmployeeId, EmployeeName = user.EmployeeName, email = user.email, DeesignationName = user.DeesignationName, DesignationID = user.DesignationID, DistrictId = user.DistrictId, DistrictName = user.DistrictName, DivisionID = user.DivisionID, DivisionName = user.DivisionName, MobileNo = user.MobileNo, RoleID = officer.Id.ToString(), RoleName = officerRole, SubdivisionId = user.SubdivisionId, SubdivisionName = user.SubdivisionName }
                                    };
                                    listOfficers.Add(obj);
                                }
                                return listOfficers;

                            }
                        }
                    }
                //}
                //else
                //{
                //    OfficerResponseViewModel1 list = FetchUser();
                //    if (list.Status == "400")
                //    {
                //        return null;
                //    }
                //    else
                //    {
                //        if (list != null)
                //        {
                //            if (list.Status == "200")
                //            {
                //                List<OfficerResponseViewModel> listOfficers = new List<OfficerResponseViewModel>();
                //                List<officer_info> officers = new List<officer_info>();
                //                officerRole = (await GetAppRoleName(officerRole)).RoleName;
                //                string roleid = (await GetRoleName(officerRole)).Id.ToString();
                //                officers = list.user_info.FindAll(x => x.RoleName.Contains(officerRole) && x.DivisionID != null & x.DivisionID != "");

                //                foreach (officer_info user in officers)
                //                {

                //                    OfficerResponseViewModel obj = new OfficerResponseViewModel
                //                    {
                //                        msg = "success",
                //                        Status = "200",
                //                        user_info = new officer_info { UserName = user.UserName, EmployeeId = user.EmployeeId, EmployeeName = user.EmployeeName, email = user.email, DeesignationName = user.DeesignationName, DesignationID = user.DesignationID, DistrictId = user.DistrictId, DistrictName = user.DistrictName, DivisionID = user.DivisionID, DivisionName = user.DivisionName, MobileNo = user.MobileNo, RoleID = roleid, RoleName = officerRole, SubdivisionId = user.SubdivisionId, SubdivisionName = user.SubdivisionName }
                //                    };
                //                    listOfficers.Add(obj);
                //                }
                //                return listOfficers;

                //            }
                //        }
                //    }
                //}
                return null;
            }

            catch (Exception ex)
            {
                return null;
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DD")]
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
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

                    var users = (await LoadOfficersAsync(roleName, "0", "0", "0", "0"));
                    List<OfficerResponseViewModel> o = new List<OfficerResponseViewModel>();
                    if (users!=null && users.Count>0)
                        o = users.FindAll(x => x.user_info.EmployeeId == lastForwardedByid);
                    
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
                                                                UserName = u.user_info.EmployeeName + "-" + u.user_info.UserName + "(" + u.user_info.DeesignationName + ")",
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [Obsolete]
        private async Task<List<OfficerDetails>> GetOfficer(string divisionId,string roleName,string subDivision,string circleid,string establishmentOfficeId)
        {
            try
            {
                var role = roleName.Split(',');
                List<OfficerResponseViewModel> officers = new List<OfficerResponseViewModel>();
                for (int i = 0;i< role.Count(); i++)
                {
                    List<OfficerResponseViewModel> officer = new List<OfficerResponseViewModel>();

                    if (role[i] == "CIRCLE OFFICER")
                    {
                        var o = (await LoadOfficersAsync(role[i], subDivision, divisionId, circleid, establishmentOfficeId));

                        string div = LoggedInDivisionID();
                        var userdetail = (from ofi in o.AsEnumerable()
                                          from divisId in ofi.user_info.DivisionID.Split(',').Select(id => id.Trim())
                                          join c in _circleDivRepository.GetAll().AsEnumerable() on divisId equals c.CircleId.ToString()
                                          join d in _divisionRepo.GetAll() on c.DivisionId equals d.Id
                                          where d.Id.ToString() == div
                                          select new officer_info
                                          {
                                              CurrentJoiningDate = ofi.user_info.CurrentJoiningDate,
                                              DateOfRetirement = ofi.user_info.DateOfRetirement,
                                              DesignationID = ofi.user_info.DesignationID,
                                              DeesignationName = ofi.user_info.DeesignationName,
                                              DistrictId = ofi.user_info.DistrictId,
                                              DistrictName = ofi.user_info.DistrictName,
                                              DivisionID = ofi.user_info.DivisionID,
                                              DivisionName = ofi.user_info.DivisionName,
                                              email = ofi.user_info.email,
                                              EmployeeId = ofi.user_info.EmployeeId,
                                              EmployeeName = ofi.user_info.EmployeeName,
                                              IntialJoiningDate = ofi.user_info.IntialJoiningDate,
                                              MobileNo = ofi.user_info.MobileNo,
                                              RoleID = ofi.user_info.RoleID,
                                              RoleName = ofi.user_info.RoleName,
                                              SubdivisionId = ofi.user_info.SubdivisionId,
                                              SubdivisionName = ofi.user_info.SubdivisionName,
                                              UserName = ofi.user_info.UserName,
                                              Status = ofi.user_info.Status

                                          }).GroupBy(x => new
                                          {
                                              x.EmployeeId,
                                              x.EmployeeName,
                                              x.DesignationID,
                                              x.DistrictId,
                                              x.DivisionID,
                                              x.SubdivisionId
                                          })
                                          .Select(g => g.First()).ToList();
                        List<OfficerResponseViewModel> officerDetails = new List<OfficerResponseViewModel>();
                        foreach (var item in userdetail)
                        {
                            OfficerResponseViewModel obj = new OfficerResponseViewModel
                            {
                                msg = "success",
                                Status = "200",
                                user_info = item
                            };
                            officerDetails.Add(obj);
                        }
                        officer = officerDetails == null ? null : officerDetails.ToList();
                        if (officer != null)
                            officers.AddRange(officer);

                        //officer = o == null ? null : o.ToList();
                        //if (officer != null)
                        //    officers.AddRange(officer);
                    }
                    else if (role[i] == "EXECUTIVE ENGINEER" || role[i] == "JUNIOR ENGINEER" || role[i] == "SUB DIVISIONAL OFFICER")
                    {
                        List<OfficerResponseViewModel> officerDetails = new List<OfficerResponseViewModel>();
                        var o = (await LoadOfficersAsync(role[i], subDivision, divisionId, circleid, establishmentOfficeId));
                        
                            var userdetail = (from ofi in o.AsEnumerable()
                                              from divisId in ofi.user_info.DivisionID.Split(',').Select(id => id.Trim())
                                              join ofice in _divisionRepo.GetAll().AsEnumerable() on divisId equals ofice.Id.ToString()
                                              orderby ofi.user_info.UserName
                                              select new officer_info
                                              {
                                                  CurrentJoiningDate = ofi.user_info.CurrentJoiningDate,
                                                  DateOfRetirement = ofi.user_info.DateOfRetirement,
                                                  DesignationID = ofi.user_info.DesignationID,
                                                  DeesignationName = ofi.user_info.DeesignationName,
                                                  DistrictId = ofi.user_info.DistrictId,
                                                  DistrictName = ofi.user_info.DistrictName,
                                                  DivisionID = ofi.user_info.DivisionID,
                                                  DivisionName = ofi.user_info.DivisionName,
                                                  email = ofi.user_info.email,
                                                  EmployeeId = ofi.user_info.EmployeeId,
                                                  EmployeeName = ofi.user_info.EmployeeName,
                                                  IntialJoiningDate = ofi.user_info.IntialJoiningDate,
                                                  MobileNo = ofi.user_info.MobileNo,
                                                  RoleID = ofi.user_info.RoleID,
                                                  RoleName = ofi.user_info.RoleName,
                                                  SubdivisionId = ofi.user_info.SubdivisionId,
                                                  SubdivisionName = ofi.user_info.SubdivisionName,
                                                  UserName = ofi.user_info.UserName,
                                                  Status = ofi.user_info.Status

                                              }).GroupBy(x => new
                                              {
                                                  x.EmployeeId,
                                                  x.EmployeeName,
                                                  x.DesignationID,
                                                  x.DistrictId,
                                                  x.DivisionID,
                                                  x.SubdivisionId
                                              })
                                              .Select(g => g.First()).ToList();
                            foreach (var item in userdetail)
                            {
                                OfficerResponseViewModel obj = new OfficerResponseViewModel
                                {
                                    msg = "success",
                                    Status = "200",
                                    user_info = item
                                };
                                officerDetails.Add(obj);
                            }
                            officer = officerDetails == null ? null : officerDetails.ToList();
                            if (officer != null)
                                officers.AddRange(officer);
                   
                        //officer = o == null ? null : o.ToList();
                        //if (officer != null)
                        //    officers.AddRange(officer);
                    }
                    
                    else
                    {
                        //List<EstablishmentOfficeDetails> est = (from ofice in _establishOffRepository.GetAll()
                        //                                        select new EstablishmentOfficeDetails { Id = ofice.Id, Name = ofice.Name }).ToList();
                        //foreach (var item in est)
                        //{
                        var o = (await LoadOfficersAsync(role[i], subDivision, divisionId, "0", "0"));
                        var userdetail = (from ofi in o.AsEnumerable()
                                          from divisId in ofi.user_info.DivisionID.Split(',').Select(id => id.Trim())
                                          join ofice in _establishOffRepository.GetAll().AsEnumerable() on divisId equals ofice.Id.ToString()
                                          //join ofice in _establishOffRepository.GetAll().AsEnumerable() on ofi.user_info.DivisionID equals ofice.Id.ToString()
                                          select new officer_info
                                          {
                                              CurrentJoiningDate = ofi.user_info.CurrentJoiningDate,
                                              DateOfRetirement = ofi.user_info.DateOfRetirement,
                                              DesignationID = ofi.user_info.DesignationID,
                                              DeesignationName = ofi.user_info.DeesignationName,
                                              DistrictId = ofi.user_info.DistrictId,
                                              DistrictName = ofi.user_info.DistrictName,
                                              DivisionID = ofi.user_info.DivisionID,
                                              DivisionName = ofi.user_info.DivisionName,
                                              email = ofi.user_info.email,
                                              EmployeeId = ofi.user_info.EmployeeId,
                                              EmployeeName = ofi.user_info.EmployeeName,
                                              IntialJoiningDate = ofi.user_info.IntialJoiningDate,
                                              MobileNo = ofi.user_info.MobileNo,
                                              RoleID = ofi.user_info.RoleID,
                                              RoleName = ofi.user_info.RoleName,
                                              SubdivisionId = ofi.user_info.SubdivisionId,
                                              SubdivisionName = ofi.user_info.SubdivisionName,
                                              UserName = ofi.user_info.UserName,
                                              Status = ofi.user_info.Status

                                          }).GroupBy(x => new
                                          {
                                              x.EmployeeId,
                                              x.EmployeeName,
                                              x.DesignationID,
                                              x.DistrictId,
                                              x.DivisionID,
                                              x.SubdivisionId
                                          })
                                          .Select(g => g.First()).ToList();
                        List<OfficerResponseViewModel> officerDetails = new List<OfficerResponseViewModel>();
                        foreach (var item in userdetail)
                        {
                            OfficerResponseViewModel obj = new OfficerResponseViewModel
                            {
                                msg = "success",
                                Status = "200",
                                user_info = item
                            };
                            officerDetails.Add(obj);
                        }
                        officer = officerDetails == null ? null : officerDetails.ToList();
                        if (officer != null)
                            officers.AddRange(officer);

                        //officer = o == null ? null : o.ToList();
                        //if (officer != null)
                        //    officers.AddRange(officer);
                        //}

                    }
                    
                    
                }
                
                if (officers!=null && officers.Count > 0)
                {
                    officers = officers.Distinct().ToList();
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,MINISTER,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER DRAINAGE,DWS,EXECUTIVE ENGINEER DRAINAGE,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE")]
        [Obsolete]
        private async Task<List<DivisionDetails>> GetOfficerLocations(string divisionId, string roleName, string userid, string appId="")
        {
            try
            {
                var role = roleName.Split(',');
                List<DivisionDetails> RoleDetail = new List<DivisionDetails>();

                var loggedInRoleName = LoggedInRoleName();

                if (loggedInRoleName == "DIRECTOR DRAINAGE")
                {
                    GrantApprovalDetail approval = (from a in _repoApprovalDetail.GetAll() join g in _repo.GetAll() on a.GrantID equals (g.Id) where g.ApplicationID == appId orderby a.Id ascending select a).FirstOrDefault();
                    divisionId = approval.FromLocationId.ToString();
                }

                for (int i = 0; i < role.Count(); i++)
                {
                    LoginResponseViewModel root = await LoadUserDetailById(userid);
                    if (root.Status == "200")
                    {
                        if (role[i] == "EXECUTIVE ENGINEER" || role[i] == "JUNIOR ENGINEER" || role[i] == "SUB DIVISIONAL OFFICER")
                        {
                            if (divisionId != "0")
                            {
                                var LocationRoleDetail = (from rr in root.user_info.OfficeWiseRoleID
                                                          join r in _userRolesRepository.GetAll().AsEnumerable() on rr.role equals r.Id
                                                          join loc in _divisionRepo.GetAll().AsEnumerable() on rr.office_id equals loc.Id
                                                          where r.AppRoleName == role[i] && loc.Id == Convert.ToInt32(divisionId)
                                                          select new
                                                          {
                                                              Roles = r,
                                                              Location = rr,
                                                              Division = loc
                                                          }
                                                          ).ToList();
                                if (LocationRoleDetail.Count > 0)
                                {

                                    RoleDetail = (from rr in LocationRoleDetail
                                                  select new DivisionDetails
                                                  {
                                                      Id = rr.Division.Id,
                                                      Name = rr.Division.Name,
                                                      CreatedBy = rr.Roles.AppRoleName
                                                  }
                                                                ).ToList();
                                }
                            }
                            else
                            {
                                var LocationRoleDetail = (from rr in root.user_info.OfficeWiseRoleID
                                                          join r in _userRolesRepository.GetAll().AsEnumerable() on rr.role equals r.Id
                                                          join loc in _divisionRepo.GetAll().AsEnumerable() on rr.office_id equals loc.Id
                                                          where r.AppRoleName == role[i]
                                                          select new
                                                          {
                                                              Roles = r,
                                                              Location = rr,
                                                              Division = loc
                                                          }
                                                           ).ToList();
                                if (LocationRoleDetail.Count > 0)
                                {

                                    RoleDetail = (from rr in LocationRoleDetail
                                                  select new DivisionDetails
                                                  {
                                                      Id = rr.Division.Id,
                                                      Name = rr.Division.Name,
                                                      CreatedBy = rr.Roles.AppRoleName
                                                  }
                                                                ).ToList();
                                }
                            }

                        }
                        else if(role[i] == "CIRCLE OFFICER")
                        {
                            var LocationRoleDetail = (from rr in root.user_info.OfficeWiseRoleID
                                                      join r in _userRolesRepository.GetAll().AsEnumerable() on rr.role equals r.Id
                                                      join c in _circleDivRepository.GetAll() on rr.office_id equals c.CircleId
                                                      join loc in _divisionRepo.GetAll() on c.DivisionId equals loc.Id
                                                      where r.AppRoleName == role[i] && loc.Id == Convert.ToInt32(divisionId)
                                                      select new
                                                      {
                                                          Roles = r,
                                                          Location = rr,
                                                          Division = loc
                                                      }
                                                          ).ToList();
                            if (LocationRoleDetail.Count > 0)
                            {

                                RoleDetail = (from rr in LocationRoleDetail
                                              select new DivisionDetails
                                              {
                                                  Id = rr.Division.Id,
                                                  Name = rr.Division.Name,
                                                  CreatedBy = rr.Roles.AppRoleName
                                              }
                                                                ).ToList();
                            }
                        }
                        else if (role[i] == "DWS" || role[i] == "EXECUTIVE ENGINEER DRAINAGE" || role[i] == "CHIEF ENGINEER DRAINAGE" || role[i] == "ADE" || role[i] == "DIRECTOR DRAINAGE" || role[i] == "PRINCIPAL SECRETARY" )
                        {
                            var LocationRoleDetail = (from rr in root.user_info.OfficeWiseRoleID
                                                      join r in _userRolesRepository.GetAll().AsEnumerable() on rr.role equals r.Id
                                                      join c in _establishOffRepository.GetAll() on rr.office_id equals c.Id
                                                      where r.AppRoleName == role[i] //&& c.Id == Convert.ToInt32(divisionId)
                                                      select new
                                                      {
                                                          Roles = r,
                                                          Location = rr,
                                                          Division = c
                                                      }
                                                          ).ToList();
                            if (LocationRoleDetail.Count > 0)
                            {

                                RoleDetail = (from rr in LocationRoleDetail
                                              select new DivisionDetails
                                              {
                                                  Id = rr.Division.Id,
                                                  Name = rr.Division.Name,
                                                  CreatedBy = rr.Roles.AppRoleName
                                              }
                                                                ).ToList();
                            }
                        }
                        else
                        {
                            var LocationRoleDetail = (from rr in root.user_info.OfficeWiseRoleID
                                                      join r in _userRolesRepository.GetAll().AsEnumerable() on rr.role equals r.Id
                                                      where r.AppRoleName == role[i]
                                                      select new
                                                      {
                                                          Roles = r,
                                                          Location = rr
                                                      }
                                                          ).ToList();
                            if (LocationRoleDetail.Count > 0)
                            {

                                RoleDetail = (from rr in LocationRoleDetail
                                              select new DivisionDetails
                                              {
                                                  Id = 0,
                                                  Name = "",
                                                  CreatedBy = rr.Roles.AppRoleName
                                              }
                                                                 ).ToList();
                            }
                        }


                    }
                }
                return RoleDetail;
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> GetOfficerLocation(string divisionId, string roleName, string userid, string appId)
        {
            var filtereddivisions = await GetOfficerLocations(divisionId,roleName,userid, appId);
            var result = filtereddivisions.Select(div => new
            {
                Id = div.Id,
                Name = div.Name,
                RoleName = div.CreatedBy // Sending the provided roleName in the JSON
            });
            return Json(result);
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
