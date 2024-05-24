

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
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<DashboardPendencyAll> _pendencyDetailsRepo;
        private readonly IRepository<DashboardPendencyViewModel> _pendencyRepo;
        private readonly IRepository<UserDivision> _userDivisionRepository;
        private readonly IRepository<UserSubdivision> _userSubDivisionRepository;
        private readonly IRepository<DivisionDetails> _divisionRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepo;
        private readonly IRepository<UserVillage> _userVillageRepository;
        private readonly IRepository<VillageDetails> _villageRpo;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepo;
        //private readonly IEmployeeRepository _employeeRepository;
        //[Obsolete]
        //private readonly IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public HomeController(ILogger<HomeController> logger, IRepository<DashboardPendencyAll> pendencyDetailsRepo, IRepository<UserDivision> userDivisionRepository
            , IRepository<DivisionDetails> divisionRepo, IRepository<DashboardPendencyViewModel> pendencyRepo, IRepository<SubDivisionDetails> subDivisionRepo, 
            IRepository<UserSubdivision> userSubDivisionRepository, UserManager<ApplicationUser> userManager, IRepository<UserVillage> userVillageRepository
            , IRepository<VillageDetails> villageRepo, IRepository<TehsilBlockDetails> tehsilBlockRepo/*, IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment*/)
        {
            _logger = logger;
            _pendencyDetailsRepo = pendencyDetailsRepo;
            _userDivisionRepository = userDivisionRepository;
            _divisionRepo = divisionRepo;
            _userManager = userManager;
            _subDivisionRepo = subDivisionRepo;
            _pendencyRepo=pendencyRepo;
            _userSubDivisionRepository=userSubDivisionRepository;
            _userVillageRepository=userVillageRepository;
            _villageRpo = villageRepo;
            _tehsilBlockRepo=tehsilBlockRepo;
            //_employeeRepository = employeeRepository;
            //_hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                List<DivisionDetails> divisions = new List<DivisionDetails>();
                List<SubDivisionDetails> subdivisions = new List<SubDivisionDetails>();
                var user = await _userManager.GetUserAsync(User);
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                // Retrieve the user object
                var userDetail = await _userManager.FindByIdAsync(userId);

                // Retrieve roles associated with the user
                var role = (await _userManager.GetRolesAsync(userDetail)).FirstOrDefault().ToUpper();

                if (role.ToUpper() == "ADMINISTRATOR") return View();
                else
                {
                    if (role == "PRINCIPAL SECRETARY" || role == "EXECUTIVE ENGINEER HQ" || role == "CHIEF ENGINEER HQ" || role == "DWS")
                    {
                        divisions = _divisionRepo.GetAll().ToList();
                    }
                    else if (role != "SUB DIVISIONAL OFFICER" && role != "JUNIOR ENGINEER")
                    {
                        divisions = (from u in _userDivisionRepository.GetAll()
                                     join d in _divisionRepo.GetAll() on u.DivisionId equals (d.Id)
                                     where u.UserId == userId
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }
                                                ).ToList();
                        subdivisions = (from u in _userDivisionRepository.GetAll()
                                        join div in _divisionRepo.GetAll() on u.DivisionId equals (div.Id)
                                        join d in _subDivisionRepo.GetAll() on div.Id equals (d.DivisionId)
                                        where u.UserId == userId
                                        select new SubDivisionDetails
                                        {
                                            Id = d.Id,
                                            Name = d.Name
                                        }
                                               ).Distinct().ToList();
                    }
                    else if (role == "SUB DIVISIONAL OFFICER")
                    {
                        subdivisions = (from u in _userSubDivisionRepository.GetAll()
                                        join d in _subDivisionRepo.GetAll() on u.SubdivisionId equals (d.Id)
                                        where u.UserId == userId
                                        select new SubDivisionDetails
                                        {
                                            Id = d.Id,
                                            Name = d.Name
                                        }
                                               ).Distinct().ToList();
                        divisions = (from u in _userSubDivisionRepository.GetAll()
                                     join sub in _subDivisionRepo.GetAll() on u.SubdivisionId equals (sub.Id)
                                     join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                                     where u.UserId == userId
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }
                                                ).Distinct().ToList();
                    }
                    else
                    {
                        subdivisions = (from u in _userVillageRepository.GetAll()
                                        join v in _villageRpo.GetAll() on u.VillageId equals (v.Id)
                                        join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals (t.Id)
                                        join d in _subDivisionRepo.GetAll() on t.SubDivisionId equals (d.Id)
                                        where u.UserId == userId
                                        select new SubDivisionDetails
                                        {
                                            Id = d.Id,
                                            Name = d.Name
                                        }
                                                ).Distinct().ToList();
                        divisions = (from u in _userVillageRepository.GetAll()
                                     join v in _villageRpo.GetAll() on u.VillageId equals (v.Id)
                                     join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals (t.Id)
                                     join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals (sub.Id)
                                     join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                                     where u.UserId == userId
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

        //public ViewResult Details(int id)
        //{
        //    return View( _employeeRepository.GetEmployee(id));
        //    // return View();
        //}
        //[HttpGet]
        //public ViewResult Create()
        //{
        //    return View();
        //}
        //[HttpPost]
        //[Obsolete]
        //public IActionResult Create(EmployeeViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string uniqueFileName = ProcessUploadedFile(model);
        //        Employee newEmployee = new Employee { 
        //            Department= model.Department,
        //            Email=model.Email,
        //            Name=model.Name,
        //            PhotoPath = uniqueFileName
        //        };
        //            _employeeRepository.Add(newEmployee);
        //        return RedirectToAction("details", new { id = newEmployee.Id });
        //    }
        //    return View();
        //}

        //[HttpGet]
        //public ViewResult Edit(int Id)
        //{
        //    Employee employee = _employeeRepository.GetEmployee(Id);
        //    EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel { 
        //        Id= employee.Id,
        //        Name=employee.Name,
        //        Email=employee.Email,
        //        Department=employee.Department,
        //        ExistingPhotoPath=employee.PhotoPath
        //    };
        //    return View(employeeEditViewModel);
        //}

        //[HttpPost]
        //[Obsolete]
        //public IActionResult Edit(EmployeeEditViewModel employeeEditViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Employee employee = _employeeRepository.GetEmployee(employeeEditViewModel.Id);
        //        employee.Department = employeeEditViewModel.Department;
        //        employee.Email = employeeEditViewModel.Email;
        //        employee.Name = employeeEditViewModel.Name;
        //        if (employeeEditViewModel.Photo != null)
        //        {
        //            if (employeeEditViewModel.ExistingPhotoPath != null)
        //            {
        //                string filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", employeeEditViewModel.ExistingPhotoPath);
        //                System.IO.File.Delete(filePath);
        //            }
        //            employee.PhotoPath = ProcessUploadedFile(employeeEditViewModel);
        //        }

        //        _employeeRepository.Update(employee);
        //        return RedirectToAction("index");
        //    }
        //    return View(employeeEditViewModel);
        //}

        //[Obsolete]
        //private string ProcessUploadedFile(EmployeeViewModel employeeEditViewModel)
        //{
        //    string uniqueFileName = null;
        //    if (employeeEditViewModel.Photo != null)
        //    {
        //        string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
        //        uniqueFileName = Guid.NewGuid().ToString() + "_" + employeeEditViewModel.Photo.FileName;
        //        string filePath = Path.Combine(uploadFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            employeeEditViewModel.Photo.CopyTo(fileStream);
        //        }
        //    }

        //    return uniqueFileName;
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult GetSubDivisions(int divisionId)
        {
            var subDivision = _subDivisionRepo.GetAll();
            var filteredSubdivisions = subDivision.Where(c => c.DivisionId == divisionId).ToList();
            return Json(new SelectList(filteredSubdivisions, "Id", "Name"));
        }

        [HttpPost]
        public async Task<IActionResult> GetRoleLevel(string divisiondetailId, string subdivisiondetailId, string role)
        {
            int divisionId = divisiondetailId != null ? Convert.ToInt32(divisiondetailId) : 0;
            int subdivisionId = subdivisiondetailId != null ? Convert.ToInt32(subdivisiondetailId) : 0;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Retrieve the user object
            var userDetail = await _userManager.FindByIdAsync(userId);

            // Retrieve roles associated with the user

             role = role==null?(await _userManager.GetRolesAsync(userDetail)).FirstOrDefault(): role;
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
                        divisions = (from u in _userDivisionRepository.GetAll()
                                     join d in _divisionRepo.GetAll() on u.DivisionId equals (d.Id)
                                     where u.UserId == userId
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }
                                            ).FirstOrDefault();
                    }
                    else if (role == "SUB DIVISIONAL OFFICER")
                    {
                        divisions = (from u in _userSubDivisionRepository.GetAll()
                                     join sub in _subDivisionRepo.GetAll() on u.SubdivisionId equals (sub.Id)
                                     join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                                     where u.UserId == userId
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }
                                                ).FirstOrDefault();
                    }
                    else
                    {
                        divisions = (from u in _userVillageRepository.GetAll()
                                     join v in _villageRpo.GetAll() on u.VillageId equals (v.Id)
                                     join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals (t.Id)
                                     join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals (sub.Id)
                                     join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                                     where u.UserId == userId
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }
                                                ).FirstOrDefault();
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
                else if (role.ToUpper() == "SUB DIVISIONAL OFFICER")
                {
                    if (subdivisionId == 0)
                    {
                        var subs = (from u in _userSubDivisionRepository.GetAll()
                                    join d in _subDivisionRepo.GetAll() on u.SubdivisionId equals (d.Id)
                                    where u.UserId == userId && d.DivisionId == divisionId
                                    select new SubDivisionDetails
                                    {
                                        Id = d.Id,
                                        Name = d.Name
                                    }
                                            ).ToList();
                        subdivisionId = subs.Count() > 0 ? subs.FirstOrDefault().Id : 0;
                    }
                }
                else
                {
                    if (subdivisionId == 0)
                    {
                        var subs = (from u in _userVillageRepository.GetAll()
                                    join v in _villageRpo.GetAll() on u.VillageId equals (v.Id)
                                    join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals (t.Id)
                                    join d in _subDivisionRepo.GetAll() on t.SubDivisionId equals (d.Id)
                                    where u.UserId == userId && d.DivisionId == divisionId
                                    select new SubDivisionDetails
                                    {
                                        Id = d.Id,
                                        Name = d.Name
                                    }
                                            ).ToList();
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

        [HttpPost]
        public IActionResult GetRoleLevelPendencyReport(string divisiondetailId, string subdivisiondetailId, string role)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //// Retrieve the user object
            //var userDetail = await _userManager.FindByIdAsync(userId);

            //// Retrieve roles associated with the user
            //var role = (await _userManager.GetRolesAsync(userDetail)).FirstOrDefault();

            int divisionId = divisiondetailId != null ? Convert.ToInt32(divisiondetailId) : 0;
            int subdivisionId = subdivisiondetailId != null ? Convert.ToInt32(subdivisiondetailId) : 0;
            if (role != null && role.ToUpper() == "ADMINISTRATOR")
            {
                return Json(null);
            }
            else
            {
                DivisionDetails divisions = new DivisionDetails();
                if (divisionId == 0)
                {
                    if (role == "PRINCIPAL SECRETARY" || role == "EXECUTIVE ENGINEER HQ" || role == "CHIEF ENGINEER HQ" || role == "DWS")
                    {
                        divisions = _divisionRepo.GetAll().FirstOrDefault();
                    }
                    else if (role != "SUB DIVISIONAL OFFICER" && role != "JUNIOR ENGINEER")
                    {
                        divisions = (from u in _userDivisionRepository.GetAll()
                                     join d in _divisionRepo.GetAll() on u.DivisionId equals (d.Id)
                                     where u.UserId == userId
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }
                                            ).FirstOrDefault();
                    }
                    else if (role == "SUB DIVISIONAL OFFICER")
                    {
                        divisions = (from u in _userSubDivisionRepository.GetAll()
                                     join sub in _subDivisionRepo.GetAll() on u.SubdivisionId equals (sub.Id)
                                     join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                                     where u.UserId == userId
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }
                                                ).FirstOrDefault();
                    }
                    else
                    {
                        divisions = (from u in _userVillageRepository.GetAll()
                                     join v in _villageRpo.GetAll() on u.VillageId equals (v.Id)
                                     join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals (t.Id)
                                     join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals (sub.Id)
                                     join d in _divisionRepo.GetAll() on sub.DivisionId equals (d.Id)
                                     where u.UserId == userId
                                     select new DivisionDetails
                                     {
                                         Id = d.Id,
                                         Name = d.Name
                                     }
                                                ).FirstOrDefault();
                    }
                    divisionId = divisions.Id;
                }
                if (role.ToUpper() == "EXECUTIVE ENGINEER")
                {
                    if (subdivisionId == 0)
                    {
                        var subs = (_subDivisionRepo.Find(x => x.DivisionId == divisionId));
                        subdivisionId = subs.Count() > 0 ? subs.FirstOrDefault().Id : 0;
                    }
                }
                else if (role.ToUpper() == "SUB DIVISIONAL OFFICER")
                {
                    if (subdivisionId == 0)
                    {
                        var subs = (from u in _userSubDivisionRepository.GetAll()
                                    join d in _subDivisionRepo.GetAll() on u.SubdivisionId equals (d.Id)
                                    where u.UserId == userId && d.DivisionId == divisionId
                                    select new SubDivisionDetails
                                    {
                                        Id = d.Id,
                                        Name = d.Name
                                    }
                                            ).ToList();
                        subdivisionId = subs.Count() > 0 ? subs.FirstOrDefault().Id : 0;
                    }
                }
                else
                {
                    if (subdivisionId == 0)
                    {
                        var subs = (from u in _userVillageRepository.GetAll()
                                    join v in _villageRpo.GetAll() on u.VillageId equals (v.Id)
                                    join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals (t.Id)
                                    join d in _subDivisionRepo.GetAll() on t.SubDivisionId equals (d.Id)
                                    where u.UserId == userId && d.DivisionId == divisionId
                                    select new SubDivisionDetails
                                    {
                                        Id = d.Id,
                                        Name = d.Name
                                    }
                                            ).ToList();
                        subdivisionId = subs.Count() > 0 ? subs.FirstOrDefault().Id : 0;
                    }
                }


                List<DashboardPendencyViewModel> model = _pendencyRepo.ExecuteStoredProcedure<DashboardPendencyViewModel>("getpendencytoforwardReport", "'" + divisionId + "'", "'" + subdivisionId + "'", "'" + role + "'").ToList();

                return Json(model);
            }
        }
        
    }
}