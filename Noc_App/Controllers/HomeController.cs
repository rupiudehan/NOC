

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
    public class HomeController : Controller
    {
        private readonly IRepository<DashboardPendencyAll> _pendencyDetailsRepo;
        private readonly IRepository<DashboardPendencyViewModel> _pendencyRepo;
        private readonly IRepository<DivisionDetails> _divisionRepo;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepo;
        private readonly IRepository<VillageDetails> _villageRpo;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepo;
        private readonly IRepository<UserRoleDetails> _userRolesRepository;
        //private readonly IEmployeeRepository _employeeRepository;
        //[Obsolete]
        //private readonly IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public HomeController(ILogger<HomeController> logger, IRepository<DashboardPendencyAll> pendencyDetailsRepo
            , IRepository<DivisionDetails> divisionRepo, IRepository<DashboardPendencyViewModel> pendencyRepo, IRepository<SubDivisionDetails> subDivisionRepo
            , IRepository<VillageDetails> villageRepo, IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<UserRoleDetails> userRolesRepository
            )
        {            
            _pendencyDetailsRepo = pendencyDetailsRepo;
            _divisionRepo = divisionRepo;
            _subDivisionRepo = subDivisionRepo;
            _pendencyRepo=pendencyRepo;
            _villageRpo = villageRepo;
            _tehsilBlockRepo=tehsilBlockRepo;
            _userRolesRepository=userRolesRepository;
        }

        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ")]
        public IActionResult Index()
        {
            try
            {
                List<DivisionDetails> divisions = new List<DivisionDetails>();
                List<SubDivisionDetails> subdivisions = new List<SubDivisionDetails>();
                string userId = LoggedInUserID();

                string divisionId = LoggedInDivisionID();

                string subdivisionId = LoggedInSubDivisionID();

                // Retrieve roles associated with the user
                var roleName = LoggedInRoleName();

                string role = roleName;// (await GetRoleName(roleName)).AppRoleName;
                //var user = await _userManager.GetUserAsync(User);
                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                //// Retrieve the user object
                //var userDetail = await _userManager.FindByIdAsync(userId);

                //// Retrieve roles associated with the user
                //var role = (await _userManager.GetRolesAsync(userDetail)).FirstOrDefault().ToUpper();

                if (role.ToUpper() == "ADMINISTRATOR") return View();
                else
                {
                    if (role == "PRINCIPAL SECRETARY" || role == "EXECUTIVE ENGINEER HQ" || role == "CHIEF ENGINEER HQ" || role == "DWS")
                    {
                        divisions = _divisionRepo.GetAll().ToList();
                    }
                    else if (role != "SUB DIVISIONAL OFFICER" && role != "JUNIOR ENGINEER")
                    {
                        divisions = (from d in _divisionRepo.GetAll() 
                                     where d.Id == Convert.ToInt32(divisionId)
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }).ToList();
                        subdivisions = (from  div in _divisionRepo.GetAll()
                                        join d in _subDivisionRepo.GetAll() on div.Id equals (d.DivisionId)
                                        where div.Id == Convert.ToInt32(divisionId)
                                        select new SubDivisionDetails
                                        {
                                            Id = d.Id,
                                            Name = d.Name
                                        }
                                               ).Distinct().ToList();
                    }
                    else 
                    {
                        subdivisions = (from d in _subDivisionRepo.GetAll()
                                        where d.Id == Convert.ToInt32(subdivisionId)
                                        select new SubDivisionDetails
                                        {
                                            Id = d.Id,
                                            Name = d.Name
                                        }
                                               ).Distinct().ToList();
                        divisions = (from sub in _subDivisionRepo.GetAll()
                                     join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                                     where d.Id == Convert.ToInt32(subdivisionId)
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }
                                                ).Distinct().ToList();
                    }
                    //subdivisions = divisions != null && divisions.Count()>0 ? (await _subDivisionRepo.FindAsync(x => x.DivisionId == divisions.FirstOrDefault().Id)).ToList() : null;
                    DashboardDropdownViewModelView model = new DashboardDropdownViewModelView
                    {
                        Divisions = new SelectList(divisions, "Id", "Name"),
                        SubDivisions = new SelectList(subdivisions, "Id", "Name"),
                        RoleName = role.ToUpper(),
                        hdnDivisionId = divisions.Count()>0? divisions.FirstOrDefault().Id:0,
                        hdnSubDivisionId = subdivisions.Count() > 0 ? subdivisions.FirstOrDefault().Id : 0
                    };
                    //return View(_employeeRepository.GetAllEmployees());
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View();
            }
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


        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ")]
        [HttpPost]
        public IActionResult GetSubDivisions(int divisionId)
        {
            var subDivision = _subDivisionRepo.GetAll();
            var filteredSubdivisions = subDivision.Where(c => c.DivisionId == divisionId).ToList();
            return Json(new SelectList(filteredSubdivisions, "Id", "Name"));
        }


        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ")]
        [HttpPost]
        public async Task<IActionResult> GetRoleLevel(string divisiondetailId, string subdivisiondetailId, string role)
        {
            int divisionId = divisiondetailId != null ? Convert.ToInt32(divisiondetailId) : 0;
            int subdivisionId = subdivisiondetailId != null ? Convert.ToInt32(subdivisiondetailId) : 0;
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user object
            //var userDetail = await _userManager.FindByIdAsync(userId);

            // Retrieve roles associated with the user
            string userId = LoggedInUserID();

            string divisionsId = LoggedInDivisionID();

            string subdivisionsId = LoggedInSubDivisionID();

            // Retrieve roles associated with the user
            var roleName = LoggedInRoleName();

            role = role == null ? roleName : role;
            if (role.ToUpper() == "ADMINISTRATOR")
            {
                return Json(null);
            }
            else
            {
                DivisionDetails divisions = new DivisionDetails();
                if (divisionId == 0)
                {
                    if (role.ToUpper() == "PRINCIPAL SECRETARY" || role.ToUpper() == "EXECUTIVE ENGINEER HQ" || role.ToUpper() == "CHIEF ENGINEER HQ" || role.ToUpper() == "DWS")
                    {
                        divisions = _divisionRepo.GetAll().FirstOrDefault();
                    }
                    else if (role != "SUB DIVISIONAL OFFICER" && role != "JUNIOR ENGINEER")
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
                        divisions = (from sub in _subDivisionRepo.GetAll()
                                     join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                                     where d.Id == Convert.ToInt32(subdivisionsId)
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }
                                                ).Distinct().FirstOrDefault();
                    }
                    divisionId = divisions.Id;
                }
                if (role.ToUpper() == "EXECUTIVE ENGINEER")
                {
                    if (subdivisionId == 0)
                    {
                        var subs = (await _subDivisionRepo.FindAsync(x => x.DivisionId == divisionId));
                        subdivisionId = subs.Count()>0? subs.FirstOrDefault().Id:0;
                    }
                }
                else 
                {
                    if (subdivisionId == 0)
                    {
                        var subs = (from d in _subDivisionRepo.GetAll()
                                        where d.Id == Convert.ToInt32(subdivisionsId)
                                        select new SubDivisionDetails
                                        {
                                            Id = d.Id,
                                            Name = d.Name
                                        }
                                              ).Distinct().ToList();
                        subdivisionId = subs.Count() > 0 ? subs.FirstOrDefault().Id : 0;
                    }
                }
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
                    case "CIRCLE OFFICER":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER"
                        });
                        break;
                    case "DWS":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","CIRCLE OFFICER"
                        });
                        break;
                    case "EXECUTIVE ENGINEER HQ":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","CIRCLE OFFICER","DWS"
                        });
                        break;
                    case "CHIEF ENGINEER HQ":
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","CIRCLE OFFICER","DWS","EXECUTIVE ENGINEER HQ"
                        });
                        break;
                    default:
                        list.Add(new object[]
                        {
                        "Division","JUNIOR ENGINEER","SUB DIVISIONAL OFFICER","EXECUTIVE ENGINEER","CIRCLE OFFICER","DWS","EXECUTIVE ENGINEER HQ","CHIEF ENGINEER HQ"
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
                        case "CIRCLE OFFICER":
                            list.Add(new object[]
                                {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)

                                });
                            break;
                        case "DWS":
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                                ,Convert.ToInt32(item.CIRCLE_OFFICER)
                               });
                            break;
                        case "EXECUTIVE ENGINEER HQ":
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                                ,Convert.ToInt32(item.CIRCLE_OFFICER), Convert.ToInt32(item.dws)
                               });
                            break;
                        case "CHIEF ENGINEER HQ":
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                                ,Convert.ToInt32(item.CIRCLE_OFFICER), Convert.ToInt32(item.dws),Convert.ToInt32(item.EXECUTIVE_ENGINEER_HQ)
                               });
                            break;
                        default:
                            list.Add(new object[]
                               {
                                item.Division,Convert.ToInt32(item.JUNIOR_ENGINEER),Convert.ToInt32(item.SUB_DIVISIONAL_OFFICER), Convert.ToInt32(item.EXECUTIVE_ENGINEER)
                                ,Convert.ToInt32(item.CIRCLE_OFFICER), Convert.ToInt32(item.dws),Convert.ToInt32(item.EXECUTIVE_ENGINEER_HQ), Convert.ToInt32(item.CHIEF_ENGINEER_HQ)
                               }); break;
                    }
                }
                return Json(list);
            }
        }


        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ")]
        [HttpPost]
        public async Task<IActionResult> GetRoleLevelPendencyReport(string divisiondetailId, string subdivisiondetailId, string role)
        {
            try
            {
                string divisionsId = LoggedInDivisionID();

                string subdivisionsId = LoggedInSubDivisionID();

                // Retrieve roles associated with the user
                var roleName = LoggedInRoleName();

                roleName = (await GetRoleName(roleName)).AppRoleName;

                int divisionId = divisiondetailId != null ? Convert.ToInt32(divisiondetailId) : 0;
                int subdivisionId = subdivisiondetailId != null ? Convert.ToInt32(subdivisiondetailId) : 0;
                if (roleName != null && roleName.ToUpper() == "ADMINISTRATOR")
                {
                    return Json(null);
                }
                else
                {
                    DivisionDetails divisions = new DivisionDetails();
                    if (divisionId == 0)
                    {
                        if (roleName == "PRINCIPAL SECRETARY" || roleName == "EXECUTIVE ENGINEER HQ" || roleName == "CHIEF ENGINEER HQ" || roleName == "DWS")
                        {
                            divisions = _divisionRepo.GetAll().FirstOrDefault();
                        }
                        else if (roleName != "SUB DIVISIONAL OFFICER" && roleName != "JUNIOR ENGINEER")
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
                            divisions = (from sub in _subDivisionRepo.GetAll()
                                         join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                                         where d.Id == Convert.ToInt32(subdivisionsId)
                                         select new DivisionDetails
                                         {
                                             Id = d.Id,
                                             Name = d.Name
                                         }
                                                ).Distinct().FirstOrDefault();
                        }
                        divisionId = divisions.Id;
                    }
                    if (roleName.ToUpper() == "EXECUTIVE ENGINEER")
                    {
                        if (subdivisionId == 0)
                        {
                            var subs = (_subDivisionRepo.Find(x => x.DivisionId == divisionId));
                            subdivisionId = subs.Count() > 0 ? subs.FirstOrDefault().Id : 0;
                        }
                    }
                    else { 
                        if (subdivisionId == 0)
                        {
                            var subs = (from d in _subDivisionRepo.GetAll()
                                        where d.Id == Convert.ToInt32(subdivisionsId)
                                        select new SubDivisionDetails
                                        {
                                            Id = d.Id,
                                            Name = d.Name
                                        }
                                              ).Distinct().ToList();
                            subdivisionId = subs.Count() > 0 ? subs.FirstOrDefault().Id : 0;
                        }
                    }

                    
                    List<DashboardPendencyViewModel> model = _pendencyRepo.ExecuteStoredProcedure<DashboardPendencyViewModel>("getpendencytoforwardReport", "'" + divisionId + "'", "'" + subdivisionId + "'", "'" + role + "'").ToList();
                    
                    return Json(model);
                }
            }
            catch (Exception ex)
            {

                return View();
            }
        }


        [Authorize(Roles = "PRINCIPAL SECRETARY,EXECUTIVE ENGINEER,CIRCLE OFFICER,CHIEF ENGINEER HQ,Administrator,DWS,EXECUTIVE ENGINEER HQ")]
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