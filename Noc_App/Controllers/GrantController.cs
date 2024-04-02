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
using Noc_App.Helpers;
using Noc_App.UtilityService;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        private readonly IRepository<GrantKhasraDetails> _khasraRepo;
        private readonly IRepository<SiteAreaUnitDetails> _siteUnitsRepo;
        private readonly IEmailService _emailService;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        [Obsolete]
        public GrantController(IRepository<GrantDetails> repo,IRepository<VillageDetails> villageRepo, IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<SubDivisionDetails> subDivisionRepo, 
            IRepository<DivisionDetails> divisionRepo, IRepository<ProjectTypeDetails> projectTypeRepo, IRepository<NocPermissionTypeDetails> nocPermissionTypeRepo, 
            IRepository<NocTypeDetails> nocTypeRepo,IRepository<OwnerTypeDetails> ownerTypeRepo, IHostingEnvironment hostingEnvironment,
            IRepository<GrantKhasraDetails> khasraRepo, IRepository<SiteAreaUnitDetails> siteUnitsRepo, IEmailService emailService)
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
            _emailService = emailService;
            _khasraRepo = khasraRepo;
            _siteUnitsRepo = siteUnitsRepo;
        }
        [AllowAnonymous]
        public async Task<ViewResult> Index(int Id)
        {
            try
            {
                GrantDetails obj = await _repo.GetByIdAsync(Id);
                GrantViewModel model = new GrantViewModel
                {
                    Id = obj.Id,
                    ApplicationID = obj.ApplicationID
                };
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        [AllowAnonymous]
        public ViewResult PayNow()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Create()
        {
            var divisions = _divisionRepo.GetAll();
            var projectType = _projectTypeRepo.GetAll();
            var nocPermission = _nocPermissionTypeRepo.GetAll();
            var nocType = _nocTypeRepo.GetAll();
            var ownerType = _ownerTypeRepo.GetAll();
            var siteUnits = _siteUnitsRepo.GetAll();
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
                SiteAreaUnit = new SelectList(siteUnits, "Id", "Name"),
                GrantKhasras = new List<GrantKhasraViewModelCreate> { new GrantKhasraViewModelCreate { KanalOrBiswa = 0, KhasraNo = "", MarlaOrBiswansi = 0, SarsaiOrBigha = 0/*,SelectedUnitId= siteUnits.OrderBy(x=>x.Name).Select(x=>x.Id).FirstOrDefault()*/ } },
                Owners = new List<OwnerViewModelCreate> { new OwnerViewModelCreate { Name = "", Address = "",MobileNo="",Email="" } },
                IsOtherTypeSelected=0,
                IsPaymentDone = false,
                IsConfirmed =false,
                IsExtension=0
            };

            return View(viewModel);
        }

        [HttpPost]
        [Obsolete]
        [AllowAnonymous]
        public async Task<IActionResult> Create(GrantViewModelCreate model)
        {
            if (model != null)
            {
                bool isValid = true;
                var divisions = _divisionRepo.GetAll();
                var projectType = _projectTypeRepo.GetAll();
                var nocPermission = _nocPermissionTypeRepo.GetAll();
                var nocType = _nocTypeRepo.GetAll();
                var ownerType = _ownerTypeRepo.GetAll();
                var siteUnits = _siteUnitsRepo.GetAll();
                var filteredSubdivisions = _subDivisionRepo.GetAll().Where(c => c.DivisionId == model.SelectedDivisionId).ToList();
                var filteredtehsilBlock = _tehsilBlockRepo.GetAll().Where(c => c.SubDivisionId == model.SelectedSubDivisionId).ToList();
                var fileteedvillage = _villageRpo.GetAll().Where(c => c.TehsilBlockId == model.SelectedTehsilBlockId).ToList();
                var selectedvillage = await _villageRpo.GetByIdAsync(model.SelectedVillageID ?? 0);
                string ErrorMessage = string.Empty;
                if (filteredSubdivisions.Count > 0 && filteredtehsilBlock.Count > 0 && fileteedvillage.Count > 0 && selectedvillage != null)
                {
                    var viewModel = new GrantViewModelCreate
                    {
                        Village = new SelectList(fileteedvillage, "Id", "Name"),
                        TehsilBlock = new SelectList(filteredtehsilBlock, "Id", "Name"),
                        SubDivision = new SelectList(filteredSubdivisions, "Id", "Name"),// new SelectList(Enumerable.Empty<SubDivisionDetails>(), "Id", "Name"),
                        Divisions = new SelectList(divisions, "Id", "Name"),
                        ProjectType = new SelectList(projectType, "Id", "Name"),
                        NocPermissionType = new SelectList(nocPermission, "Id", "Name"),
                        NocType = new SelectList(nocType, "Id", "Name"),
                        OwnerType = new SelectList(ownerType, "Id", "Name"),
                        SiteAreaUnit = new SelectList(siteUnits, "Id", "Name"),
                        GrantKhasras=model.GrantKhasras,
                        Owners = model.Owners,
                        Pincode = selectedvillage.PinCode.ToString(),
                        //SiteAreaOrSizeInFeet = model.SiteAreaOrSizeInFeet,
                        //SiteAreaOrSizeInInches = model.SiteAreaOrSizeInInches,
                        //Latitude = model.Latitude,
                        //Longitute = model.Longitute
                    };
                    if (model.SelectedSiteAreaUnitId > 0 && model.KMLFile != null && model.KMLLinkName != null && model.KMLLinkName != "" && model.IDProofPhoto != null && model.AddressProofPhoto != null && model.AuthorizationLetterPhoto != null
                        && model.SelectedVillageID > 0 && model.SelectedProjectTypeId > 0 && model.SelectedNocPermissionTypeID > 0 /*&& model.Latitude > 0 && model.Longitute > 0*/ && model.ApplicantName != null
                        && model.ApplicantEmailID != null && model.SelectedNocTypeId > 0 && model.IsConfirmed
                        )
                    {
                        int IdProofValidation = AllowedCheckExtensions(model.IDProofPhoto, "proof");
                        int AddressProofValidation = AllowedCheckExtensions(model.AddressProofPhoto, "proof");
                        int AuthorizationValidation = AllowedCheckExtensions(model.AuthorizationLetterPhoto, "proof");
                        int kmlFileValidation = AllowedCheckExtensions(model.KMLFile, "kml");
                        if (IdProofValidation == 0)
                        {
                            ErrorMessage = $"Invalid ID proof file type. Please upload a JPG, PNG, or PDF file";
                            ModelState.AddModelError("", ErrorMessage);

                            return View(viewModel);

                        }
                        else if (IdProofValidation == 2)
                        {
                            ErrorMessage = "ID proof field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(viewModel);
                        }
                        if (AddressProofValidation == 0)
                        {
                            ErrorMessage = $"Invalid address proof file type. Please upload a JPG, PNG, or PDF file";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(viewModel);

                        }
                        else if (AddressProofValidation == 2)
                        {
                            ErrorMessage = "Address proof field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(viewModel);
                        }
                        if (AuthorizationValidation == 0)
                        {
                            ErrorMessage = $"Invalid authorization letter file type. Please upload a JPG, PNG, or PDF file";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(viewModel);

                        }
                        else if (AuthorizationValidation == 2)
                        {
                            ErrorMessage = "Authorization letter field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(viewModel);
                        }

                        if (kmlFileValidation == 0)
                        {
                            ErrorMessage = $"Invalid KML file type. Please upload a PDF file only";
                            ModelState.AddModelError("", ErrorMessage);

                            return View(viewModel);

                        }
                        else if (kmlFileValidation == 2)
                        {
                            ErrorMessage = "KML File field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(viewModel);
                        }

                        if (!AllowedFileSize(model.IDProofPhoto))
                        {
                            ErrorMessage = "ID proof file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(viewModel);
                        }
                        if (!AllowedFileSize(model.AddressProofPhoto))
                        {
                            ErrorMessage = "Address proof file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(viewModel);
                        }
                        if (!AllowedFileSize(model.AuthorizationLetterPhoto))
                        {
                            ErrorMessage = "Authorization letter file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(viewModel);
                        }

                        if (!AllowedFileSize(model.KMLFile))
                        {
                            ErrorMessage = "KML file size exceeds the allowed limit of 4MB";
                            ModelState.AddModelError("", ErrorMessage);
                            return View(viewModel);
                        }

                        string uniqueIDProofFileName = ProcessUploadedFile(model.IDProofPhoto, "IDProof");
                        string uniqueAddressProofFileName = ProcessUploadedFile(model.AddressProofPhoto, "Address");
                        string uniqueAuthLetterFileName = ProcessUploadedFile(model.AuthorizationLetterPhoto, "AuthLetter");
                        string uniqueKmlFileName = ProcessUploadedFile(model.KMLFile, "kml");
                        if (model.IsExtension == 1)
                        {
                            if (model.NocNumber == null) isValid = false;
                            if (model.PreviousDate == null) { isValid = false; }
                            if (!isValid)
                            {
                                ModelState.AddModelError("", $"NOC Number and Date both are required to fill");

                                return View(viewModel);
                            }
                        }
                        if (/*model.Khasra == null &&*/ model.Hadbast == null && model.PlotNo == null)
                        {
                            isValid = false;

                            ModelState.AddModelError("", $"Atleast one field is required to fill out of Hadbast/Plot No.");

                            return View(viewModel);
                        }
                        if (model.IsOtherTypeSelected == 1)
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
                                            ).AsEnumerable().Max(x => x.id));
                                var specificGrant = await _repo.GetByIdAsync(grantId);
                                int extractedNumber = Convert.ToInt32(ExtractNumber(specificGrant.ApplicationID)) + 1;
                                inputString = "GNTNOC" + extractedNumber.ToString();
                            }
                            else inputString = "GNTNOC1";

                            model.ApplicationID = inputString;

                            GrantDetails obj = new GrantDetails
                            {
                                Name = model.Name,
                                SiteAreaUnitId = model.SelectedSiteAreaUnitId,
                                //SiteAreaOrSizeInFeet = model.SiteAreaOrSizeInFeet,
                                //SiteAreaOrSizeInInches = model.SiteAreaOrSizeInInches,
                                IDProofPhotoPath = uniqueIDProofFileName,
                                AddressProofPhotoPath = uniqueAddressProofFileName,
                                AuthorizationLetterPhotoPath = uniqueAuthLetterFileName,
                                VillageID = model.SelectedVillageID ?? 0,
                                ProjectTypeId = model.SelectedProjectTypeId ?? 0,
                                //Khasra = model.Khasra,
                                Hadbast = model.Hadbast,
                                PlotNo = model.PlotNo,
                                //Latitude = model.Latitude,
                                //Longitute = model.Longitute,
                                ApplicantName = model.ApplicantName,
                                ApplicantEmailID = model.ApplicantEmailID,
                                NocPermissionTypeID = model.SelectedNocPermissionTypeID ?? 0,
                                NocTypeId = model.SelectedNocTypeId ?? 0,
                                IsExtension = Convert.ToBoolean(model.IsExtension),
                                NocNumber = model.NocNumber,
                                PreviousDate = model.PreviousDate,
                                IsConfirmed = model.IsConfirmed,
                                ApplicationID = model.ApplicationID,
                                OtherProjectTypeDetail=model.OtherProjectTypeDetail,                                
                                CreatedOn = DateTime.Now
                            };
                            await _repo.CreateAsync(obj);
                            var emailModel = new EmailModel(model.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessage(model.ApplicantName, model.ApplicationID));
                            _emailService.SendEmail(emailModel, "Punjab Irrigation Department");
                            double TotalPayment = 0;
                            if (Convert.ToDouble(model.TotalArea) <= 0.25)
                            {
                                TotalPayment = 250;
                            }
                            else 
                            if (Convert.ToDouble(model.TotalArea) > 0.25 && Convert.ToDouble(model.TotalArea) <= 0.50)
                            {
                                TotalPayment = 500;
                            }
                            else if (Convert.ToDouble(model.TotalArea) > 0.50 && Convert.ToDouble(model.TotalArea) <= 1)
                            {
                                TotalPayment = 1000;
                            }
                            else if (Convert.ToDouble(model.TotalArea) > 1)
                            {
                                double area=Convert.ToDouble(model.TotalArea) - 1;
                                TotalPayment = 1000;
                                int count = 0;
                                do
                                {
                                    count++;
                                    area = area - 1;
                                } while (area> 0);
                                TotalPayment = TotalPayment + (count * 250);
                            }
                            if (TotalPayment > 0)
                            {
                                TempData["TotalAreaAmount"] = TotalPayment.ToString();
                                TempData["GrantID"] = obj.Id.ToString();
                                return RedirectToAction("Index", "Checkout");
                            }
                            else {
                                return RedirectToAction("Index", "Grant", new { Id = obj.Id });
                            }

                        }
                        else
                        {

                            ModelState.AddModelError("", $"All fields are required");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", $"All fields are required");
                    }

                    return View(viewModel);
                }
                else
                {
                    ModelState.AddModelError("", $"All fields are required");
                }


                return View(model);
            }
            else
            {
                ModelState.AddModelError("", $"Please check file size");
                return View(model);
            }
        }

        //private bool AllowedFileSize(IFormFile file)
        //{
        //    var maxSize = 4 * 1024 * 1024;
        //    if (file.Length > maxSize) // 4MB limit
        //    {
        //        return false;                
        //    }
        //    return true;
        //}
        //private int AllowedCheckExtensions(IFormFile file)
        //{
        //    if (file != null && file.Length > 0)
        //    {
        //        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
        //        var fileExtension = Path.GetExtension(file.FileName);

        //        if (!allowedExtensions.Contains(fileExtension.ToLower()))
        //        {
        //            return 0;
        //        }
        //        return 1;
        //    }
        //    else return 2;
        //}
        private bool AllowedFileSize(IFormFile file)
        {
            var maxSize = 4 * 1024 * 1024;
            if (file.Length > maxSize) // 4MB limit
            {
                return false;
            }
            return true;
        }
        private int AllowedCheckExtensions(IFormFile file, string fileType)
        {
            if (file != null && file.Length > 0)
            {
                if (fileType == "kml")
                {
                    var allowedExtensions = new[] { ".pdf" };
                    var fileExtension = Path.GetExtension(file.FileName);

                    if (!allowedExtensions.Contains(fileExtension.ToLower()))
                    {
                        return 0;
                    }
                }
                else
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".pdf" };
                    var fileExtension = Path.GetExtension(file.FileName);

                    if (!allowedExtensions.Contains(fileExtension.ToLower()))
                    {
                        return 0;
                    }
                }
                return 1;
            }
            else return 2;
        }

        [Obsolete]
        private string ProcessUploadedFile(IFormFile file,string prefixName)
        {
            string uniqueFileName = null;
            if (file != null && file.Length>0)
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
        [AllowAnonymous]
        public IActionResult GetSubDivisions(int divisionId)
        {
            var subDivision = _subDivisionRepo.GetAll();
            var filteredSubdivisions = subDivision.Where(c => c.DivisionId == divisionId).ToList();
            return Json(new SelectList(filteredSubdivisions, "Id", "Name"));
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult GetTehsilBlocks(int subDivisionId)
        {
            var tehsilBlock = _tehsilBlockRepo.GetAll().Where(c => c.SubDivisionId == subDivisionId).ToList();
            return Json(new SelectList(tehsilBlock, "Id", "Name"));
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult GetVillagess(int tehsilBlockId)
        {
            var village = _villageRpo.GetAll().Where(c => c.TehsilBlockId == tehsilBlockId).ToList();
            return Json(new SelectList(village, "Id", "Name"));
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetVillageDetail(int villageId)
        {
            var village = await _villageRpo.GetByIdAsync(villageId);
            return Json(village);
        }
    }
}
