using Microsoft.AspNetCore.Mvc;
using Noc_App.Models.interfaces;
using Noc_App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Noc_App.Models.ViewModel;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;

namespace Noc_App.Controllers
{
    public class GrantController : Controller
    {
        private readonly IRepository<GrantDetails> _repo;
        private readonly IRepository<VillageDetails> _villageRpo;
        private readonly IRepository<TehsilBlockDetails> _tehsilBlockRepo;
        private readonly IRepository<SubDivisionDetails> _subDivisionRepo;
        private readonly IRepository<DivisionDetails> _divisionRepo;
        private readonly IRepository<ProjectTypeDetails> _projectTypeRepo;
        private readonly IRepository<NocPermissionTypeDetails> _nocPermissionTypeRepo;
        private readonly IRepository<NocTypeDetails> _nocTypeRepo;
        private readonly IRepository<OwnerTypeDetails> _ownerTypeRepo;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        [Obsolete]
        public GrantController(IRepository<GrantDetails> repo,IRepository<VillageDetails> villageRepo, IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<SubDivisionDetails> subDivisionRepo, 
            IRepository<DivisionDetails> divisionRepo, IRepository<ProjectTypeDetails> projectTypeRepo, IRepository<NocPermissionTypeDetails> nocPermissionTypeRepo, 
            IRepository<NocTypeDetails> nocTypeRepo,IRepository<OwnerTypeDetails> ownerTypeRepo, IHostingEnvironment hostingEnvironment)
        {
            _villageRpo = villageRepo;
            _tehsilBlockRepo = tehsilBlockRepo;
            _subDivisionRepo = subDivisionRepo;
            _divisionRepo = divisionRepo;
            _projectTypeRepo= projectTypeRepo;
            _nocPermissionTypeRepo= nocPermissionTypeRepo;
            _nocTypeRepo= nocTypeRepo;
            _ownerTypeRepo= ownerTypeRepo;
            _hostingEnvironment = hostingEnvironment;
            _repo = repo;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var divisions = _divisionRepo.GetAll();
            var projectType = _projectTypeRepo.GetAll();
            var nocPermission = _nocPermissionTypeRepo.GetAll();
            var nocType = _nocTypeRepo.GetAll();
            var ownerType = _ownerTypeRepo.GetAll();
            var viewModel = new GrantViewModelCreate
            {
                Divisions = new SelectList(divisions, "Id", "Name"),
                ProjectType = new SelectList(projectType, "Id", "Name"),
                NocPermissionType = new SelectList(nocPermission, "Id", "Name"),
                NocType = new SelectList(nocType, "Id", "Name"),
                OwnerType = new SelectList(ownerType, "Id", "Name"),
                Village = new SelectList(Enumerable.Empty<VillageDetails>(), "Id", "Name"),
                TehsilBlock = new SelectList(Enumerable.Empty<TehsilBlockDetails>(), "Id", "Name"),
                SubDivision = new SelectList(Enumerable.Empty<SubDivisionDetails>(), "Id", "Name"),
                Owners = new List<OwnerViewModelCreate> { new OwnerViewModelCreate { Name = "", Address = "",MobileNo="",Email="" } },
                IsOtherTypeSelected=false,
                IsConfirmed=false,
                IsExtension=false
            };

            return View(viewModel);
        }

        [HttpPost]
        [Obsolete]
        public async Task<IActionResult> Create(GrantViewModelCreate model)
        {
            bool isValid = true;
            var divisions = _divisionRepo.GetAll();
            var projectType = _projectTypeRepo.GetAll();
            var nocPermission = _nocPermissionTypeRepo.GetAll();
            var nocType = _nocTypeRepo.GetAll();
            var ownerType = _ownerTypeRepo.GetAll();
            var subDivision = _subDivisionRepo.GetAll();
            var filteredSubdivisions = subDivision.Where(c => c.DivisionId == model.SelectedDivisionId).ToList();
            
            
            var viewModel = new GrantViewModelCreate
            {
                Village = new SelectList(Enumerable.Empty<VillageDetails>(), "Id", "Name"),
                TehsilBlock = new SelectList(Enumerable.Empty<TehsilBlockDetails>(), "Id", "Name"),
                SubDivision = new SelectList(filteredSubdivisions, "Id", "Name"),// new SelectList(Enumerable.Empty<SubDivisionDetails>(), "Id", "Name"),
                Divisions = new SelectList(divisions, "Id", "Name"),
                ProjectType = new SelectList(projectType, "Id", "Name"),
                NocPermissionType = new SelectList(nocPermission, "Id", "Name"),
                NocType = new SelectList(nocType, "Id", "Name"),
                OwnerType = new SelectList(ownerType, "Id", "Name"),
                Owners = model.Owners
            };
            string uniqueIDProofFileName = ProcessUploadedFile(model.IDProofPhoto, "IDProof");
            string uniqueAddressProofFileName = ProcessUploadedFile(model.AddressProofPhoto, "Address");
            string uniqueAuthLetterFileName = ProcessUploadedFile(model.AuthorizationLetterPhoto, "AuthLetter");
            if (model.SiteAreaOrSizeInInches <= 0 && model.SiteAreaOrSizeInFeet<=0 && model.IDProofPhoto!=null && model.AddressProofPhoto!=null && model.AuthorizationLetterPhoto!=null
                && model.SelectedVillageID<=0 && model.SelectedProjectTypeId <= 0 && model.SelectedNocPermissionTypeID <= 0 && model.Latitude <= 0 && model.Longitute <= 0 && model.ApplicantName!=null
                && model.ApplicantEmailID!=null && model.SelectedNocTypeId<=0 && model.IsConfirmed
                )
            {
                if(model.IsExtension)
                {
                    if (model.NocNumber == null) isValid = false;
                    if (model.PreviousDate.ToString()== "1/1/0001 12:00:00 AM") { isValid = false; }
                    if (!isValid)
                    {
                        ModelState.AddModelError("", $"NOC Number and Date are required to fill");

                        return View(viewModel);
                    }
                }
                if (model.Khasra == null && model.Hadbast==null && model.PlotNo==null)
                {
                    isValid = false;

                    ModelState.AddModelError("", $"Atleast one field is required to fill out of Khasra/Hadbast/Plot No.");

                    return View(viewModel);
                }
                if (model.IsOtherTypeSelected)
                {
                    if (model.OtherProjectTypeDetail == null)
                    {
                        isValid = false;
                        ModelState.AddModelError("", $"Other Detail is required to fill");

                        return View(viewModel);
                    }
                }
                if (isValid)
                {

                    string inputString = string.Empty;
                    var grant = _repo.GetAll();
                    if (grant != null && grant.Count() > 0)
                    {
                        int grantId = Convert.ToInt32((from g in grant
                                                       select new { id = g.Id }
                                    ).Max());
                        var specificGrant = await _repo.GetByIdAsync(grantId);
                        int extractedNumber = Convert.ToInt32(ExtractNumber(specificGrant.ApplicationID)) + 1;
                        inputString = "GNT" + extractedNumber.ToString();
                    }
                    else inputString = "GNT1";

                    model.ApplicationID = inputString;

                    GrantDetails obj = new GrantDetails
                    {
                        Name = model.Name,
                        SiteAreaOrSizeInFeet = model.SiteAreaOrSizeInFeet,
                        SiteAreaOrSizeInInches = model.SiteAreaOrSizeInInches,
                        IDProofPhotoPath = uniqueIDProofFileName,
                        AddressProofPhotoPath = uniqueAddressProofFileName,
                        AuthorizationLetterPhotoPath = uniqueAuthLetterFileName,
                        VillageID = model.SelectedVillageID,
                        ProjectTypeId = model.SelectedProjectTypeId,
                        Khasra = model.Khasra,
                        Hadbast = model.Hadbast,
                        PlotNo = model.PlotNo,
                        Latitude = model.Latitude,
                        Longitute = model.Longitute,
                        ApplicantName = model.ApplicantName,
                        ApplicantEmailID = model.ApplicantEmailID,
                        NocPermissionTypeID = model.SelectedNocPermissionTypeID,
                        NocTypeId = model.SelectedNocTypeId,
                        IsExtension = model.IsExtension,
                        NocNumber = model.NocNumber,
                        PreviousDate = model.PreviousDate,
                        IsConfirmed = model.IsConfirmed,
                        ApplicationID = model.ApplicationID,
                        CreatedOn = DateTime.Now
                    };
                    await _repo.CreateAsync(obj);
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    ModelState.AddModelError("", $"All fields are required");

                    return View(viewModel);
                }
            }
            else
            {               

                ModelState.AddModelError("", $"All fields are required");

                return View(viewModel);
            }
            

        }
        


        [Obsolete]
        private string ProcessUploadedFile(IFormFile file,string prefixName)
        {
            string uniqueFileName = null;
            if (file != null)
            {
                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");
                uniqueFileName = prefixName+"_"+Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailExists(string applicantEmailID)
        {
            var user = await _repo.GetAll().AnyAsync(x=>x.ApplicantEmailID == applicantEmailID);

            if (!user)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {applicantEmailID} is already in use");
            }
        }

        private string ExtractNumber(string inputString)
        {
            // Use LINQ to filter out non-numeric characters and create a new string.
            string numericPart = new string(inputString.Where(c => Char.IsDigit(c)).ToArray());

            return numericPart;
        }

        [HttpPost]
        public IActionResult GetSubDivisions(int divisionId)
        {
            var subDivision = _subDivisionRepo.GetAll();
            var filteredSubdivisions = subDivision.Where(c => c.DivisionId == divisionId).ToList();
            return Json(new SelectList(filteredSubdivisions, "Id", "Name"));
        }

        [HttpPost]
        public IActionResult GetTehsilBlocks(int subDivisionId)
        {
            var tehsilBlock = _tehsilBlockRepo.GetAll().Where(c => c.SubDivisionId == subDivisionId).ToList();
            return Json(new SelectList(tehsilBlock, "Id", "Name"));
        }

        [HttpPost]
        public IActionResult GetVillagess(int tehsilBlockId)
        {
            var village = _villageRpo.GetAll().Where(c => c.TehsilBlockId == tehsilBlockId).ToList();
            return Json(new SelectList(village, "Id", "Name"));
        }


        [HttpPost]
        public async Task<IActionResult> GetVillageDetail(int villageId)
        {
            var village = await _villageRpo.GetByIdAsync(villageId);
            return Json(village);
        }
    }
}
