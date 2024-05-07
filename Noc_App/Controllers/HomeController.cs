

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Noc_App.Models;
using Noc_App.Models.interfaces;
using Noc_App.Models.ViewModel;
using System.Diagnostics;
using System.Xml.Linq;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
namespace Noc_App.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        //private readonly IEmployeeRepository _employeeRepository;
        //[Obsolete]
        //private readonly IHostingEnvironment _hostingEnvironment;

        [Obsolete]
        public HomeController(ILogger<HomeController> logger/*, IEmployeeRepository employeeRepository, IHostingEnvironment hostingEnvironment*/)
        {
            _logger = logger;
            //_employeeRepository = employeeRepository;
            //_hostingEnvironment = hostingEnvironment;
        }
         
        public IActionResult Index()
        {
            //return View(_employeeRepository.GetAllEmployees());
            return View();
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
    }
}