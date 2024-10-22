

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Noc_App.Models;
using Noc_App.Models.interfaces;
using Noc_App.Models.ViewModel;
using Noc_App.UtilityService;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Security.Claims;
using System.Xml.Linq;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
namespace Noc_App.Controllers
{
    public class HomeController :Controller
    {
        private readonly IRepository<DashboardPendencyAll> _pendencyDetailsRepo;
        private readonly IRepository<DashboardPendencyViewModel> _pendencyRepo;
        private readonly IRepository<DivisionDetails> _divisionRepo;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepo;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepo;
        private readonly IRepository<UserRoleDetails> _userRolesRepository;
        private readonly IRepository<ReportApplicationCountViewModel> _grantReportAppCountDetailsRepo;
        private readonly IRepository<ReportApplicationsViewModel> _grantReportTotalAppDetailsRepo;
        private readonly IRepository<GrantDetails> _repo;
        private readonly IRepository<PlanSanctionAuthorityMaster> _repoPlanSanctionAuthtoryMaster;
        private readonly IRepository<NocTypeDetails> _nocTypeRepo;
        private readonly IRepository<OwnerTypeDetails> _ownerTypeRepo;
        private readonly IRepository<GrantKhasraDetails> _khasraRepo;
        private readonly IRepository<ProjectTypeDetails> _projectTypeRepo;
        private readonly IRepository<NocPermissionTypeDetails> _nocPermissionTypeRepo;
        private readonly IRepository<SiteAreaUnitDetails> _siteUnitsRepo;
        private readonly IRepository<MasterPlanDetails> _masterPlanDetailsRepository;
        private readonly IRepository<GrantApprovalDetail> _repoApprovalDetail;
        private readonly IRepository<GrantApprovalMaster> _repoApprovalMaster;
        private readonly IRepository<GrantApprovalProcessDocumentsDetails> _repoApprovalDocument;
        private readonly IRepository<OwnerDetails> _grantOwnersRepo;
        private readonly IRepository<DrainWidthTypeDetails> _drainwidthRepository;
        private readonly IRepository<RecommendationDetail> _repoRecommendation;
        private readonly ICalculations _calculations;
        private readonly IRepository<GrantFileTransferDetails> _grantFileTransferRepository;
        private readonly IRepository<GrantPaymentDetails> _grantPaymentRepo;

        public IRepository<GrantUnprocessedAppDetails> _grantUnprocessedAppDetailsRepo { get; }

        //private readonly IEmployeeRepository _employeeRepository;
        //[Obsolete]
        //private readonly IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public HomeController(ILogger<HomeController> logger, IRepository<DashboardPendencyAll> pendencyDetailsRepo
            , IRepository<DivisionDetails> divisionRepo, IRepository<DashboardPendencyViewModel> pendencyRepo, IRepository<SubDivisionDetails> subDivisionRepo
            /*, IRepository<VillageDetails> villageRepo*/, IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<UserRoleDetails> userRolesRepository
            , IRepository<ReportApplicationCountViewModel> grantReportAppCountDetailsRepo, IRepository<GrantUnprocessedAppDetails> grantUnprocessedAppDetailsRepo
            , IRepository<ReportApplicationsViewModel> grantReportTotalAppDetailsRepo
            ,IRepository<GrantDetails> repo
            ,IRepository<PlanSanctionAuthorityMaster> repoPlanSanctionAuthtoryMaster
            ,IRepository<NocTypeDetails> nocTypeRepo
            ,IRepository<OwnerTypeDetails> ownerTypeRepo
            ,IRepository<GrantKhasraDetails> khasraRepo
            ,IRepository<ProjectTypeDetails> projectTypeRepo
            ,IRepository<NocPermissionTypeDetails> nocPermissionTypeRepo
            ,IRepository<SiteAreaUnitDetails> siteUnitsRepo
            ,IRepository<MasterPlanDetails> masterPlanDetailsRepository
            ,IRepository<GrantApprovalDetail> repoApprovalDetail
            ,IRepository<GrantApprovalMaster> repoApprovalMaster
            ,IRepository<GrantApprovalProcessDocumentsDetails> repoApprovalDocument
            ,IRepository<OwnerDetails> grantOwnersRepo
            ,IRepository<DrainWidthTypeDetails> drainwidthRepository
            ,IRepository<RecommendationDetail> repoRecommendation
            ,ICalculations calculations
            ,IRepository<GrantFileTransferDetails> grantFileTransferRepository
            ,IRepository<GrantPaymentDetails> grantPaymentRepo
            )
        {            
            _pendencyDetailsRepo = pendencyDetailsRepo;
            _divisionRepo = divisionRepo;
            _subDivisionRepo = subDivisionRepo;
            _pendencyRepo=pendencyRepo;
            //_villageRpo = villageRepo;
            _tehsilBlockRepo=tehsilBlockRepo;
            _userRolesRepository=userRolesRepository;
            _grantReportAppCountDetailsRepo = grantReportAppCountDetailsRepo;
            _grantUnprocessedAppDetailsRepo = grantUnprocessedAppDetailsRepo;
            _grantReportTotalAppDetailsRepo = grantReportTotalAppDetailsRepo;
            _repo = repo;
            _repoPlanSanctionAuthtoryMaster = repoPlanSanctionAuthtoryMaster;
            _nocTypeRepo = nocTypeRepo;
            _ownerTypeRepo = ownerTypeRepo;
            _khasraRepo = khasraRepo;
            _projectTypeRepo=projectTypeRepo;
            _nocPermissionTypeRepo = nocPermissionTypeRepo;
            _siteUnitsRepo = siteUnitsRepo;
            _masterPlanDetailsRepository= masterPlanDetailsRepository;
            _repoApprovalDetail= repoApprovalDetail;
            _repoApprovalMaster = repoApprovalMaster;
            _repoApprovalDocument = repoApprovalDocument;
            _grantOwnersRepo=grantOwnersRepo;

            _drainwidthRepository = drainwidthRepository;
            _repoRecommendation = repoRecommendation;
            _calculations = calculations;
            _grantFileTransferRepository = grantFileTransferRepository;
            _grantPaymentRepo = grantPaymentRepo;
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
        public IActionResult Index()
        {
            try
            {
                List<DivisionDetails> divisions = new List<DivisionDetails>();
                //List<SubDivisionDetails> subdivisions = new List<SubDivisionDetails>();
                string userId = LoggedInUserID();

                string divisionId = LoggedInDivisionID();

                //string subdivisionId = LoggedInSubDivisionID();

                // Retrieve roles associated with the user
                var roleName = LoggedInRoleName();
                string div = "0";

                string role = roleName;// (await GetRoleName(roleName)).AppRoleName;
                                       //var user = await _userManager.GetUserAsync(User);
                                       //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                                       //// Retrieve the user object
                                       //var userDetail = await _userManager.FindByIdAsync(userId);

                //// Retrieve roles associated with the user
                //var role = (await _userManager.GetRolesAsync(userDetail)).FirstOrDefault().ToUpper();

                //if (role.ToUpper() == "ADMINISTRATOR") return View();
                //else
                //{
                if (role == "PRINCIPAL SECRETARY" || role == "EXECUTIVE ENGINEER HQ" || role == "CHIEF ENGINEER HQ" || role == "DWS" || role == "ADE" || role == "DIRECTOR DRAINAGE" || role.ToUpper() == "ADMINISTRATOR")
                {
                    divisions = _divisionRepo.GetAll().OrderBy(x => x.Name).ToList();
                }
                else if (role == "EXECUTIVE ENGINEER" || role == "CIRCLE OFFICER")
                {
                    divisions = (from d in _divisionRepo.GetAll()
                                 where d.Id == Convert.ToInt32(divisionId)
                                 select new DivisionDetails
                                 {
                                     Id = d.Id,
                                     Name = d.Name
                                 }).ToList();
                    div = divisionId;
                    //subdivisions = (from  div in _divisionRepo.GetAll()
                    //                join d in _subDivisionRepo.GetAll() on div.Id equals (d.DivisionId)
                    //                where div.Id == Convert.ToInt32(divisionId)
                    //                select new SubDivisionDetails
                    //                {
                    //                    Id = d.Id,
                    //                    Name = d.Name
                    //                }
                    //                       ).Distinct().ToList();
                }
                //else 
                //{
                //    subdivisions = (from d in _subDivisionRepo.GetAll()
                //                    where d.Id == Convert.ToInt32(subdivisionId)
                //                    select new SubDivisionDetails
                //                    {
                //                        Id = d.Id,
                //                        Name = d.Name
                //                    }
                //                           ).Distinct().OrderBy(x => x.Name).ToList();
                //    divisions = (from sub in _subDivisionRepo.GetAll()
                //                 join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                //                 where d.Id == Convert.ToInt32(subdivisionId)
                //                 select new DivisionDetails
                //                 {
                //                     Id = d.Id,
                //                     Name = d.Name
                //                 }
                //                            ).Distinct().OrderBy(x => x.Name).ToList();
                //}
                ReportApplicationCountViewModel modelreport = new ReportApplicationCountViewModel();
                if (role == "EXECUTIVE ENGINEER" || role == "CIRCLE OFFICER")
                {
                    modelreport = _grantReportAppCountDetailsRepo.ExecuteStoredProcedure<ReportApplicationCountViewModel>("reportapplicationscountXEN", "0", "0", "0", div).FirstOrDefault();
                }
                else if (role == "PRINCIPAL SECRETARY" || role == "EXECUTIVE ENGINEER HQ" || role == "CHIEF ENGINEER HQ" || role == "DWS" || role == "ADE" || role == "DIRECTOR DRAINAGE" || role.ToUpper() == "ADMINISTRATOR")
                {
                    modelreport = _grantReportAppCountDetailsRepo.ExecuteStoredProcedure<ReportApplicationCountViewModel>("reportapplicationscount", "0", "0", "0", div).FirstOrDefault();
                }
                    //subdivisions = divisions != null && divisions.Count()>0 ? (await _subDivisionRepo.FindAsync(x => x.DivisionId == divisions.FirstOrDefault().Id)).ToList() : null;
                    DashboardDropdownViewModelView model = new DashboardDropdownViewModelView
                    {
                        Divisions = new SelectList(divisions, "Id", "Name",54),
                        //SubDivisions = new SelectList(subdivisions, "Id", "Name"),
                        RoleName = role.ToUpper(),
                        hdnDivisionId = divisions.Count()>0? 63/*divisions.FirstOrDefault().Id*/:0,
                        hdnSubDivisionId = 0,//subdivisions.Count() > 0 ? 114/*subdivisions.FirstOrDefault().Id*/ : 0,
                        TotalCount = modelreport.TotalCount,
                        ApprovedCount= modelreport.ApprovedCount,
                        RejectedCount= modelreport.RejectedCount,
                        Pending= modelreport.TotalCount- (modelreport.ApprovedCount+ modelreport.RejectedCount),
                        LoggedInRole= role,
                        SelectedDivisionId=63,
                        SelectedSubDivisionId=117
                    };
                    //return View(_employeeRepository.GetAllEmployees());
                    return View(model);
                //}
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
        public IActionResult ApplicationStatus()
        {
            try
            {
                List<DivisionDetails> divisions = new List<DivisionDetails>();
                List<SubDivisionDetails> subdivisions = new List<SubDivisionDetails>();
                var roleName = LoggedInRoleName();
                divisions = _divisionRepo.GetAll().OrderBy(x => x.Name).ToList();
                ApplicationStatusReportViewModel model = new ApplicationStatusReportViewModel
                {
                    Divisions = new SelectList(divisions, "Id", "Name",54),
                    hdnDivisionId = divisions.Count() > 0 ? 54/*divisions.FirstOrDefault().Id*/ : 0,
                    SelectedDivisionId= divisions.Count() > 0 ? 54/*divisions.FirstOrDefault().Id*/ : 0,
                    LoggedInRole = roleName.ToUpper()
                };
                return View(model);
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        public IActionResult GetSubDivisions(int divisionId)
        {
            var subDivision = _subDivisionRepo.GetAll();
            var filteredSubdivisions = subDivision.Where(c => c.DivisionId == divisionId).ToList();
            return Json(new SelectList(filteredSubdivisions, "Id", "Name"));
        }


        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        public async Task<IActionResult> GetRoleLevel(string divisiondetailId, string subdivisiondetailId, string role)
        {
            int divisionId = divisiondetailId != null ? Convert.ToInt32(divisiondetailId) : 0;
            int subdivisionId = 0;//subdivisiondetailId != null ? Convert.ToInt32(subdivisiondetailId) : 0;
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user object
            //var userDetail = await _userManager.FindByIdAsync(userId);

            // Retrieve roles associated with the user
            string userId = LoggedInUserID();

            string divisionsId = role== "EXECUTIVE ENGINEER" || role=="CIRCLE OFFICER" ? LoggedInDivisionID():divisiondetailId;

            string subdivisionsId = LoggedInSubDivisionID();

            // Retrieve roles associated with the user
            var roleName = LoggedInRoleName();

            role = role == null ? roleName : role;
            //if (role.ToUpper() == "ADMINISTRATOR")
            //{
            //    return Json(null);
            //}
            //else
            //{
                DivisionDetails divisions = new DivisionDetails();
            //if (divisionId == 0)
            {
                if (role == "PRINCIPAL SECRETARY" || role == "EXECUTIVE ENGINEER HQ" || role == "CHIEF ENGINEER HQ" || role == "DWS" || role == "ADE" || role == "DIRECTOR DRAINAGE" || role.ToUpper() == "ADMINISTRATOR")
                {
                    if (divisionId != 0)
                    {
                        divisions = (from d in _divisionRepo.GetAll()
                                     where d.Id == Convert.ToInt32(divisionsId)
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }).FirstOrDefault();
                    }
                    else
                    {
                        divisions = _divisionRepo.GetAll().FirstOrDefault();
                    }
                }
                else
                //if (role != "SUB DIVISIONAL OFFICER" && role != "JUNIOR ENGINEER")
                {
                    if (divisionId != 0)
                    {
                        divisions = (from d in _divisionRepo.GetAll()
                                     where d.Id == Convert.ToInt32(divisionsId)
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }).FirstOrDefault();
                    }
                }
                //else
                //{
                //if (subdivisionsId != "0")
                //{
                //    divisions = (from sub in _subDivisionRepo.GetAll()
                //                 join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                //                 where d.Id == Convert.ToInt32(subdivisionsId)
                //                 select new DivisionDetails
                //                 {
                //                     Id = d.Id,
                //                     Name = d.Name
                //                 }
                //                            ).Distinct().FirstOrDefault();
                //}
                //}
                divisionId = divisions.Id;
            }
                //if (role.ToUpper() == "EXECUTIVE ENGINEER")
                //{
                //    if (subdivisionId == 0)
                //    {
                //        var subs = (await _subDivisionRepo.FindAsync(x => x.DivisionId == divisionId));
                //        subdivisionId = subs.Count()>0? subs.FirstOrDefault().Id:0;
                //    }
                //}
                //else 
                //{
                //    if (subdivisionId == 0)
                //    {
                //        var subs = (from d in _subDivisionRepo.GetAll()
                //                        where d.DivisionId == Convert.ToInt32(divisionId)
                //                        select new SubDivisionDetails
                //                        {
                //                            Id = d.Id,
                //                            Name = d.Name
                //                        }
                //                              ).Distinct().ToList();
                //        subdivisionId = subs.Count() > 0 ? subs.FirstOrDefault().Id : 0;
                //    }
                //}
                List<object> list = new List<object>();

                switch (role)
                {
                    case "JUNIOR ENGINEER":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER"
                        });
                        break;
                    case "SUB DIVISIONAL OFFICER":
                        list.Add(new object[]
                        {
                        "Division","SUB DIVISIONAL OFFICER"
                        });
                        break;
                    case "EXECUTIVE ENGINEER":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER"
                        });
                        break;
                    case "DWS":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER"/*,"CIRCLE OFFICER"*/
                        });
                        break;
                    case "ADE":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","DWS"
                        });
                        break;
                    case "DIRECTOR DRAINAGE":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","DWS","ADE"
                        });
                        break;
                    case "CIRCLE OFFICER":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","DWS","ADE","DIRECTOR DRAINAGE"
                        });
                        break;
                    case "EXECUTIVE ENGINEER HQ":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","DWS","ADE","DIRECTOR DRAINAGE","CIRCLE OFFICER"
                        });
                        break;
                    case "CHIEF ENGINEER HQ":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","DWS","ADE","DIRECTOR DRAINAGE","CIRCLE OFFICER","EXECUTIVE ENGINEER DRAINAGE"
                        });
                        break;
                case "PRINCIPAL SECRETARY":
                    list.Add(new object[]
                    {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","DWS","ADE","DIRECTOR DRAINAGE","CIRCLE OFFICER","EXECUTIVE ENGINEER DRAINAGE","CHIEF ENGINEER DRAINAGE"
                    });
                    break;
                default:
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","DWS","ADE","DIRECTOR DRAINAGE","CIRCLE OFFICER","EXECUTIVE ENGINEER DRAINAGE","CHIEF ENGINEER DRAINAGE","PRINCIPAL SECRETARY"
                        }); break;
                }

                List<DashboardPendencyAll> model = (await _pendencyDetailsRepo.ExecuteStoredProcedureAsync<DashboardPendencyAll>("getpendencytoforward1", "'" + divisionId + "'", "'" + subdivisionId + "'", "'" + role + "'")).ToList();
                foreach (DashboardPendencyAll item in model)
                {
                    switch (role)
                    {
                        case "JUNIOR ENGINEER":
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER)
                               });
                            break;
                        case "SUB DIVISIONAL OFFICER":
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER)
                               });
                            break;
                        case "EXECUTIVE ENGINEER":
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER)
                               });
                            break;
                        case "DWS":
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                               });
                            break;
                        case "ADE":
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                                ,Convert.ToInt32(item.dws)
                               });
                            break;
                        case "DIRECTOR DRAINAGE":
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                                ,Convert.ToInt32(item.dws),Convert.ToInt32(item.ADE)
                               });
                            break;
                        case "CIRCLE OFFICER":
                            list.Add(new object[]
                                {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                                ,Convert.ToInt32(item.dws),Convert.ToInt32(item.ADE),Convert.ToInt32(item.DIRECTOR_DRAINAGE)
                                });
                            break;
                        case "EXECUTIVE ENGINEER HQ":
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                                ,Convert.ToInt32(item.dws),Convert.ToInt32(item.ADE),Convert.ToInt32(item.DIRECTOR_DRAINAGE),Convert.ToInt32(item.CIRCLE_OFFICER)
                               });
                            break;
                        case "CHIEF ENGINEER HQ":
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                                ,Convert.ToInt32(item.dws),Convert.ToInt32(item.ADE),Convert.ToInt32(item.DIRECTOR_DRAINAGE),Convert.ToInt32(item.CIRCLE_OFFICER),Convert.ToInt32(item.EXECUTIVE_ENGINEER_HQ)
                               });
                            break;
                    case "PRINCIPAL SECRETARY":
                        list.Add(new object[]
                        {
                        item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                        ,Convert.ToInt32(item.dws),Convert.ToInt32(item.ADE),Convert.ToInt32(item.DIRECTOR_DRAINAGE),Convert.ToInt32(item.CIRCLE_OFFICER),Convert.ToInt32(item.EXECUTIVE_ENGINEER_HQ), Convert.ToInt32(item.CHIEF_ENGINEER_HQ)
                        }); break;
                        break;
                    default:
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                                ,Convert.ToInt32(item.dws),Convert.ToInt32(item.ADE),Convert.ToInt32(item.DIRECTOR_DRAINAGE),Convert.ToInt32(item.CIRCLE_OFFICER),
                                   Convert.ToInt32(item.EXECUTIVE_ENGINEER_HQ), Convert.ToInt32(item.CHIEF_ENGINEER_HQ), Convert.ToInt32(item.PRINCIPAL_SECRETARY)
                               }); break;
                    }
                }
                return Json(list);
            //}
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
        private async Task<UserRoleDetails> GetAppRoleName(string rolename)
        {
            try
            {
                return (await _userRolesRepository.FindAsync(x => x.AppRoleName == rolename)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        public async Task<IActionResult> GetRoleLevelPendencyReport(string divisiondetailId, string subdivisiondetailId, string role)
        {
            try
            {
                string divisionsId = LoggedInDivisionID();

                string subdivisionsId = LoggedInSubDivisionID();

                // Retrieve roles associated with the user
                var roleName = LoggedInRoleName();
                divisionsId = roleName == "EXECUTIVE ENGINEER" || roleName == "CIRCLE OFFICER" ? LoggedInDivisionID() : divisiondetailId;
                roleName = (await GetAppRoleName(roleName)).RoleName;
                //roleName = (await GetRoleName(roleName)).AppRoleName;
                if (role == "EXECUTIVE ENGINEER DRAINAGE")
                    role = "EXECUTIVE ENGINEER HQ";
                if (role == "CHIEF ENGINEER DRAINAGE")
                    role = "CHIEF ENGINEER HQ";
                int divisionId = divisiondetailId != null ? Convert.ToInt32(divisiondetailId) : 0;
                int subdivisionId = 0;// subdivisiondetailId != null ? Convert.ToInt32(subdivisiondetailId) : 0;
                //if (roleName != null && roleName.ToUpper() == "ADMINISTRATOR")
                //{
                //    return Json(null);
                //}
                //else
                //{
                    DivisionDetails divisions = new DivisionDetails();
                //if (divisionId == 0)
                {
                    if (role == "PRINCIPAL SECRETARY" || role == "EXECUTIVE ENGINEER HQ" || role == "CHIEF ENGINEER HQ" || role == "DWS" || role == "ADE" || role == "DIRECTOR DRAINAGE")
                    {
                        if (divisionId != 0)
                        {
                            divisions = (from d in _divisionRepo.GetAll()
                                         where d.Id == Convert.ToInt32(divisionsId)
                                         select new DivisionDetails
                                         {
                                             Id = d.Id,
                                             Name = d.Name
                                         }).FirstOrDefault();
                        }
                        else
                        {
                            divisions = _divisionRepo.GetAll().FirstOrDefault();
                        }
                    }
                    else
                    //if (roleName != "SUB DIVISIONAL OFFICER" && roleName != "JUNIOR ENGINEER")
                    {
                        divisions = (from d in _divisionRepo.GetAll()
                                     where d.Id == Convert.ToInt32(divisionsId)
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }).FirstOrDefault();
                    }
                    //else 
                    //{
                    //    divisions = (from sub in _subDivisionRepo.GetAll()
                    //                 join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                    //                 where d.Id == Convert.ToInt32(subdivisionsId)
                    //                 select new DivisionDetails
                    //                 {
                    //                     Id = d.Id,
                    //                     Name = d.Name
                    //                 }
                    //                        ).Distinct().FirstOrDefault();
                    //}
                    divisionId = divisions.Id;
                }
                    //if (roleName.ToUpper() == "EXECUTIVE ENGINEER")
                    //{
                    //    if (subdivisionId == 0)
                    //    {
                    //        var subs = (_subDivisionRepo.Find(x => x.DivisionId == divisionId));
                    //        subdivisionId = subs.Count() > 0 ? subs.FirstOrDefault().Id : 0;
                    //    }
                    //}
                    //else { 
                    //    if (subdivisionId == 0)
                    //    {
                    //        var subs = (from d in _subDivisionRepo.GetAll()
                    //                    where d.DivisionId == Convert.ToInt32(divisionId)
                    //                    select new SubDivisionDetails
                    //                    {
                    //                        Id = d.Id,
                    //                        Name = d.Name
                    //                    }
                    //                          ).Distinct().ToList();
                    //        subdivisionId = subs.Count() > 0 ? subs.FirstOrDefault().Id : 0;
                    //    }
                    //}

                    
                    List<DashboardPendencyViewModel> model = _pendencyRepo.ExecuteStoredProcedure<DashboardPendencyViewModel>("getpendencytoforwardReport", "'" + divisionId + "'", "'" + subdivisionId + "'", "'" + role + "'", "'" + roleName.ToUpper() + "'").ToList();
                    
                    return Json(model);
                //}
            }
            catch (Exception ex)
            {

                return View();
            }
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult GetApplicationStatusReport(string divisiondetailId)
        {
            try
            {
                string divisionsId = LoggedInDivisionID();
                int divisionId = divisiondetailId != null ? Convert.ToInt32(divisiondetailId) : 0;
                    
                    List<GrantUnprocessedAppDetails> model = _grantUnprocessedAppDetailsRepo.ExecuteStoredProcedure<GrantUnprocessedAppDetails>("getapplicationsstatus", 0, 0, 0, 0, divisionId).ToList();

                    return Json(model);
               
            }
            catch (Exception ex)
            {

                return Json(null);
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        public async Task<IActionResult> GetTotalApplicationReport(string divisiondetailId)
        {
            try
            {
                var roleName = LoggedInRoleName();
                string div = "0";

                string divisionsId = LoggedInDivisionID();
                int divisionId = divisiondetailId != null ? Convert.ToInt32(divisiondetailId) : 0;
                List<ReportApplicationsViewModel> model = new List<ReportApplicationsViewModel>();

                if (roleName == "EXECUTIVE ENGINEER" || roleName == "CIRCLE OFFICER")
                {
                    divisionId = Convert.ToInt32(divisionsId);
                    model = (await _grantReportTotalAppDetailsRepo.ExecuteStoredProcedureAsync<ReportApplicationsViewModel>("gettotalapplicationsXEN", divisionId)).ToList();
                }
                else if (roleName == "PRINCIPAL SECRETARY" || roleName == "EXECUTIVE ENGINEER HQ" || roleName == "CHIEF ENGINEER HQ" || roleName == "DWS" || roleName == "ADE" || roleName == "DIRECTOR DRAINAGE" || roleName.ToUpper() == "ADMINISTRATOR")
                {
                    model = (await _grantReportTotalAppDetailsRepo.ExecuteStoredProcedureAsync<ReportApplicationsViewModel>("gettotalapplications", divisionId)).ToList();
                }
                    


                return Json(model);

            }
            catch (Exception ex)
            {

                return Json(null);
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        public async Task<IActionResult> GetApprovedApplicationReport(string divisiondetailId)
        {
            try
            {
                var roleName = LoggedInRoleName();
                string divisionsId = LoggedInDivisionID();
                int divisionId = divisiondetailId != null ? Convert.ToInt32(divisiondetailId) : 0;
                List<ReportApplicationsViewModel> model = new List<ReportApplicationsViewModel>();

                if (roleName == "EXECUTIVE ENGINEER" || roleName == "CIRCLE OFFICER")
                {
                    divisionId = Convert.ToInt32(divisionsId);
                    model = (await _grantReportTotalAppDetailsRepo.ExecuteStoredProcedureAsync<ReportApplicationsViewModel>("gettotalapprovedapplicationsXEN", divisionId)).ToList();
                }
                else if (roleName == "PRINCIPAL SECRETARY" || roleName == "EXECUTIVE ENGINEER HQ" || roleName == "CHIEF ENGINEER HQ" || roleName == "DWS" || roleName == "ADE" || roleName == "DIRECTOR DRAINAGE" || roleName.ToUpper() == "ADMINISTRATOR")
                {
                    model = (await _grantReportTotalAppDetailsRepo.ExecuteStoredProcedureAsync<ReportApplicationsViewModel>("gettotalapprovedapplications", divisionId)).ToList();
                }

                

                return Json(model);

            }
            catch (Exception ex)
            {

                return Json(null);
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        public async Task<IActionResult> GetRejectedApplicationReport(string divisiondetailId)
        {
            try
            {
                var roleName = LoggedInRoleName();
                string divisionsId = LoggedInDivisionID();
                int divisionId = divisiondetailId != null ? Convert.ToInt32(divisiondetailId) : 0;
                List<ReportApplicationsViewModel> model = new List<ReportApplicationsViewModel>();

                if (roleName == "EXECUTIVE ENGINEER" || roleName == "CIRCLE OFFICER")
                {
                    divisionId = Convert.ToInt32(divisionsId);
                    model = (await _grantReportTotalAppDetailsRepo.ExecuteStoredProcedureAsync<ReportApplicationsViewModel>("gettotalrejectedapplicationsXEN", divisionId)).ToList();
                }
                else if (roleName == "PRINCIPAL SECRETARY" || roleName == "EXECUTIVE ENGINEER HQ" || roleName == "CHIEF ENGINEER HQ" || roleName == "DWS" || roleName == "ADE" || roleName == "DIRECTOR DRAINAGE" || roleName.ToUpper() == "ADMINISTRATOR")
                {
                    model = (await _grantReportTotalAppDetailsRepo.ExecuteStoredProcedureAsync<ReportApplicationsViewModel>("gettotalrejectedapplications", divisionId)).ToList();
                }

                

                return Json(model);

            }
            catch (Exception ex)
            {

                return Json(null);
            }
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
        [HttpPost]
        public async Task<IActionResult> GetPendingApplicationReport(string divisiondetailId)
        {
            try
            {
                var roleName = LoggedInRoleName();
                string divisionsId = LoggedInDivisionID();
                int divisionId = divisiondetailId != null ? Convert.ToInt32(divisiondetailId) : 0;
                List<ReportApplicationsViewModel> model = new List<ReportApplicationsViewModel>();

                if (roleName == "EXECUTIVE ENGINEER" || roleName == "CIRCLE OFFICER")
                {
                    divisionId = Convert.ToInt32(divisionsId);
                    model = (await _grantReportTotalAppDetailsRepo.ExecuteStoredProcedureAsync<ReportApplicationsViewModel>("gettotalpendingapplicationsXEN", divisionId)).ToList();
                }
                else if (roleName == "PRINCIPAL SECRETARY" || roleName == "EXECUTIVE ENGINEER HQ" || roleName == "CHIEF ENGINEER HQ" || roleName == "DWS" || roleName == "ADE" || roleName == "DIRECTOR DRAINAGE" || roleName.ToUpper() == "ADMINISTRATOR")
                {
                    model = (await _grantReportTotalAppDetailsRepo.ExecuteStoredProcedureAsync<ReportApplicationsViewModel>("gettotalpendingapplications", divisionId)).ToList();
                }

                

                return Json(model);

            }
            catch (Exception ex)
            {

                return Json(null);
            }
        }


        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ,ADE,DIRECTOR DRAINAGE")]
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

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,DWS,EXECUTIVE ENGINEER HQ,JUNIOR ENGINEER,SUB DIVISIONAL OFFICER,ADE,DIRECTOR DRAINAGE,Administrator")]
        public async Task<IActionResult> ViewApplication(string Id)
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
}