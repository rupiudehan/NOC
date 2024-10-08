﻿

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Noc_App.Models;
using Noc_App.Models.interfaces;
using Noc_App.Models.ViewModel;
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
        //private readonly IRepository<VillageDetails> _villageRpo;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepo;
        private readonly IRepository<UserRoleDetails> _userRolesRepository;
        private readonly IRepository<ReportApplicationCountViewModel> _grantReportAppCountDetailsRepo;
        private readonly IRepository<ReportApplicationsViewModel> _grantReportTotalAppDetailsRepo;

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
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","DWS","ADE","DIRECTOR DRAINAGE","CIRCLE OFFICER","EXECUTIVE ENGINEER HQ"
                        });
                        break;
                case "PRINCIPAL SECRETARY":
                    list.Add(new object[]
                    {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","DWS","ADE","DIRECTOR DRAINAGE","CIRCLE OFFICER","EXECUTIVE ENGINEER HQ","CHIEF ENGINEER HQ"
                    });
                    break;
                default:
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","DWS","ADE","DIRECTOR DRAINAGE","CIRCLE OFFICER","EXECUTIVE ENGINEER HQ","CHIEF ENGINEER HQ","PRINCIPAL SECRETARY"
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