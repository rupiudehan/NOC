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
using System.Text.RegularExpressions;
using Noc_App.Models.Payment;
using Microsoft.AspNetCore.Http;
using Rotativa.AspNetCore;
using System;
using Razorpay.Api;
using System.Collections.ObjectModel;

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
        private readonly IRepository<GrantPaymentDetails> _grantPaymentRepo;
        private readonly IRepository<OwnerDetails> _grantOwnersRepo;
        private readonly IRepository<GrantApprovalDetail> _repoApprovalDetail;
        private readonly IEmailService _emailService;
        private readonly IWebHostEnvironment _hostEnvironment;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        [Obsolete]
        public GrantController(IRepository<GrantDetails> repo,IRepository<VillageDetails> villageRepo, IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<SubDivisionDetails> subDivisionRepo, 
            IRepository<DivisionDetails> divisionRepo, IRepository<ProjectTypeDetails> projectTypeRepo, IRepository<NocPermissionTypeDetails> nocPermissionTypeRepo, 
            IRepository<NocTypeDetails> nocTypeRepo,IRepository<OwnerTypeDetails> ownerTypeRepo, IHostingEnvironment hostingEnvironment,
            IRepository<GrantKhasraDetails> khasraRepo, IRepository<SiteAreaUnitDetails> siteUnitsRepo, IEmailService emailService, 
            IRepository<GrantPaymentDetails> grantPaymentRepo, IRepository<OwnerDetails> grantOwnersRepo, 
            IRepository<GrantApprovalDetail> repoApprovalDetail, IWebHostEnvironment hostEnvironment)
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
            _grantPaymentRepo = grantPaymentRepo;
            _grantOwnersRepo = grantOwnersRepo;
            _repoApprovalDetail = repoApprovalDetail;
            _hostEnvironment= hostEnvironment;
        }
        [AllowAnonymous]
        public async Task<ViewResult> Index(string Id)
        {
            try
            {
                GrantDetails obj = (await _repo.FindAsync(x=> x.ApplicationID.ToLower() == Id.ToLower())).FirstOrDefault();
                if (obj != null)
                {
                    var payment = await _grantPaymentRepo.FindAsync(x => x.GrantID == obj.Id);
                    if (payment == null || payment.Count() == 0)
                    {
                        GrantViewModel model = new GrantViewModel
                        {
                            Id = obj.Id,
                            ApplicationID = obj.ApplicationID,
                            OrderId = "0",
                            Message = "Payment is not successfull"
                        };
                        return View(model);
                    }
                    else
                    {
                        GrantPaymentDetails objPyment = (payment).FirstOrDefault();
                        GrantViewModel model = new GrantViewModel
                        {
                            Id = obj.Id,
                            ApplicationID = obj.ApplicationID,
                            OrderId = objPyment.PaymentOrderId,
                            Message = "Payment is successfull"
                        };
                        return View(model);
                    }
                }
                else
                {
                    return View(null);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        [AllowAnonymous]
        public IActionResult Download(string fileName)
        {
            // Replace "path_to_your_file" with the actual path to your file
            string relativeFilePath = "../wwwroot/Documents/" + fileName;
            string filePath = Path.Combine(_hostEnvironment.WebRootPath, relativeFilePath);
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
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Search(string searchString)
        {
            var model = (from g in _repo.GetAll()
                     join p in _grantPaymentRepo.GetAll() on g.Id equals p.GrantID into grantPayment
                     from payment in grantPayment.DefaultIfEmpty()
                     join app in _repoApprovalDetail.GetAll() on g.Id equals app.GrantID into grantApproval
                     from approval in grantApproval.DefaultIfEmpty()
                     where g.ApplicationID.ToLower()==searchString.ToLower()
                     select new GrantStatusViewModel
                     {
                         ApplicationID = g.ApplicationID,
                         IsApproved=g.IsApproved,
                         CreatedOn = string.Format("{0:dd/MM/yyyy}", g.CreatedOn),
                         ApplicationStatus = payment != null && payment.PaymentOrderId != "0" ? "Paid" : "Pending",
                         ApprovalStatus = g.IsPending==true? g.IsRejected?"Rejected": g.IsForwarded?approval != null ? "Pending With "+approval.ProcessedToRole : "Pending" : "Pending" : g.IsApproved ? "NOC Issued" : "Pending",
                         CertificateFilePath= g.CertificateFilePath
                     }
                     ).FirstOrDefault();
            if (model != null) return View(model);
            //GrantDetails obj = (await _repo.FindAsync(x => x.ApplicationID.ToLower() == searchString.ToLower())).FirstOrDefault();
            //if (obj != null)
            //{
            //    GrantPaymentDetails payment = (await _grantPaymentRepo.FindAsync(x => x.GrantID == obj.Id)).FirstOrDefault();

            //    GrantStatusViewModel model = new GrantStatusViewModel
            //    {
            //        ApplicationID = obj.ApplicationID,
            //        CreatedOn = string.Format("{0:dd/MM/yyyy}", obj.CreatedOn),
            //        ApplicationStatus = payment != null && payment.PaymentOrderId != "0" ? "Paid" : "Pending",
            //    };
            //    return View(model);
            //}
            else
            {
                return View(null);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult TrackStatus()
        {
            return View();
        }

        [AllowAnonymous]
        public async Task<ViewResult> GeneratePdf(string Id)
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
                    double marla = 0,kanal=0,sarsai=0,biswansi=0,biswa=0,bigha=0;
                    if(unit.Name== @"Marla/Kanal/Sarsai")
                    {
                        marla = item.MarlaOrBiswa / 160;
                        kanal = item.KanalOrBigha / 8;
                        sarsai = item.SarsaiOrBiswansi / 1440;
                    }
                    else
                    {
                        biswansi = item.SarsaiOrBiswansi * 0.000625;
                        biswa = item.MarlaOrBiswa * 0.0125;
                        bigha = item.KanalOrBigha * 0.25;
                    }
                    totalArea= Math.Round(totalArea+marla + kanal+ sarsai+ biswansi+ biswa+ bigha,4);
                }
                var payment = await _grantPaymentRepo.FindAsync(x => x.GrantID == obj.Id);
                if (payment == null || payment.Count() == 0)
                {
                    GrantDetailViewModel model = new GrantDetailViewModel
                    {
                        Id = obj.Id,
                        ApplicationID = obj.ApplicationID,
                        PaymentOrderId = "0",
                        Name=obj.Name,
                        VillageName= village.Name,
                        TehsilBlockName=tehsil.Name,
                        SubDivisionName=subDivision.Name,
                        DivisionName= division.Name,
                        AddressProofPhotoPath=obj.AddressProofPhotoPath,
                        ApplicantEmailID=obj.ApplicantEmailID,
                        ApplicantName=obj.ApplicantName,
                        AuthorizationLetterPhotoPath=obj.AuthorizationLetterPhotoPath,
                        Hadbast=obj.Hadbast,
                        IDProofPhotoPath=obj.IDProofPhotoPath,
                        KMLFilePath=obj.KMLFilePath,
                        KmlLinkName=obj.KMLLinkName,
                        NocNumber=obj.NocNumber,
                        NocPermissionTypeName= permission.Name,
                        NocTypeName= noctype.Name,
                        OtherProjectTypeDetail=obj.OtherProjectTypeDetail,
                        Pincode=village.PinCode.ToString(),
                        PlotNo=obj.PlotNo,
                        PreviousDate=string.Format("{0:dd/MM/yyyy}", obj.PreviousDate),
                        ProjectTypeName= obj.Name,
                        SiteAreaUnitName= unit.Name,
                        TotalArea= totalArea.ToString(),
                        TotalAreaSqFeet = (totalArea* 43560).ToString(),
                        TotalAreaSqMetre = (totalArea* 4046.86).ToString(),
                        Owners = owners,
                        Khasras=khasras
                    };
                    return new ViewAsPdf(model);
                    //return new ViewAsPdf(model)
                    //{
                    //    FileName = model.ApplicationID + ".pdf"
                    //};
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
                        Khasras = khasras
                    };
                    return new ViewAsPdf(model);
                    //return new ViewAsPdf(model)
                    //{
                    //    FileName = model.ApplicationID + ".pdf"
                    //};
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> PayNow(string Id)
        {
            GrantDetails model = (await _repo.FindAsync(x => x.ApplicationID.ToLower() == Id.ToLower())).FirstOrDefault();
            if (model != null)
            {
                VillageDetails village = await _villageRpo.GetByIdAsync(model.VillageID);
                TehsilBlockDetails tehsil = await _tehsilBlockRepo.GetByIdAsync(village.TehsilBlockId);
                SubDivisionDetails subDivision = await _subDivisionRepo.GetByIdAsync(tehsil.SubDivisionId);
                DivisionDetails division = await _divisionRepo.GetByIdAsync(subDivision.DivisionId);
                SiteAreaUnitDetails unit = await _siteUnitsRepo.GetByIdAsync(model.SiteAreaUnitId);
                List<GrantKhasraDetails> khasras = (await _khasraRepo.FindAsync(x => x.GrantID == model.Id)).ToList();
                double totalArea = 0;
                foreach (GrantKhasraDetails item in khasras)
                {
                    double marla = 0, kanal = 0, sarsai = 0, biswansi = 0, biswa = 0, bigha = 0;
                    if (unit.Name == @"Marla/Kanal/Sarsai")
                    {
                        marla = item.MarlaOrBiswa / 160;
                        kanal = item.KanalOrBigha / 8;
                        sarsai = item.SarsaiOrBiswansi / 1440;
                    }
                    else
                    {
                        biswansi = item.SarsaiOrBiswansi * 0.000625;
                        biswa = item.MarlaOrBiswa * 0.0125;
                        bigha = item.KanalOrBigha * 0.25;
                    }
                    totalArea = totalArea + marla + kanal + sarsai + biswansi + biswa + bigha;
                }
                double TotalPayment = 0;
                if (Convert.ToDouble(totalArea) <= 0.50)
                {
                    TotalPayment = 500;
                }
                else if (Convert.ToDouble(totalArea) > 0.50 && Convert.ToDouble(totalArea) <= 1)
                {
                    TotalPayment = 1000;
                }
                else if (Convert.ToDouble(totalArea) > 1)
                {
                    double area = Convert.ToDouble(totalArea) - 1;
                    TotalPayment = 1000;
                    int count = 0;
                    do
                    {
                        count++;
                        area = area - 1;
                    } while (area > 0);
                    TotalPayment = TotalPayment + (count * 250);
                }
                PaymentRequest paymentRequestDetail = new PaymentRequest
                {
                    Name = model.ApplicantName,
                    Email = model.ApplicantEmailID,
                    Address = "Division:" + division.Name + ", Sub-Division:" + subDivision.Name + ", Tehsil/Block:" + tehsil.Name + ", Village:" + village.Name + ", Pincode:" + village.PinCode,
                    Amount = TotalPayment,
                    GrantId = model.Id,
                    ApplicationId=Id
                };
                return RedirectToAction("Index", "Payment", paymentRequestDetail);
            }
            else
            {
                return View(model);
            }
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
                //OwnerType = new SelectList(ownerType, "Id", "Name"),
                Village = new SelectList(Enumerable.Empty<VillageDetails>(), "Id", "Name"),
                TehsilBlock = new SelectList(Enumerable.Empty<TehsilBlockDetails>(), "Id", "Name"),
                SubDivision = new SelectList(Enumerable.Empty<SubDivisionDetails>(), "Id", "Name"),
                SiteAreaUnit = new SelectList(siteUnits, "Id", "Name"),
                GrantKhasras = new List<GrantKhasraViewModelCreate> { new GrantKhasraViewModelCreate { KanalOrBigha = 0, KhasraNo = "", MarlaOrBiswa = 0, SarsaiOrBiswansi = 0/*,SelectedUnitId= siteUnits.OrderBy(x=>x.Name).Select(x=>x.Id).FirstOrDefault()*/ } },
                Owners = new List<OwnerViewModelCreate> { new OwnerViewModelCreate { Name = "", Address = "",MobileNo="",Email="",OwnerType= new SelectList(ownerType, "Id", "Name") } },
                IsOtherTypeSelected=0,
                IsPaymentDone = false,
                IsConfirmed =false,
                IsExtension=0,
                TotalAreaSqFeet="0",
                TotalAreaSqMetre="0"
            };

            return View(viewModel);
        }

        [HttpPost]
        [Obsolete]
        [AllowAnonymous]
        public async Task<IActionResult> Create(GrantViewModelCreate model)
        {
            try
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
                    var filteredSubdivisions = await _subDivisionRepo.FindAsync(c => c.DivisionId == model.SelectedDivisionId);
                    var filteredtehsilBlock = await _tehsilBlockRepo.FindAsync(c => c.SubDivisionId == model.SelectedSubDivisionId);
                    var fileteedvillage = await _villageRpo.FindAsync(c => c.TehsilBlockId == model.SelectedTehsilBlockId);
                    var selectedvillage = await _villageRpo.GetByIdAsync(model.SelectedVillageID ?? 0);
                    string ErrorMessage = string.Empty;
                    if (filteredSubdivisions.Count() > 0 && filteredtehsilBlock.Count() > 0 && fileteedvillage.Count() > 0 && selectedvillage != null)
                    {
                        var viewModel = new GrantViewModelCreate
                        {
                            Village = new SelectList(fileteedvillage, "Id", "Name"),
                            TehsilBlock = new SelectList(filteredtehsilBlock, "Id", "Name"),
                            SubDivision = new SelectList(filteredSubdivisions, "Id", "Name"),
                            Divisions = new SelectList(divisions, "Id", "Name"),
                            ProjectType = new SelectList(projectType, "Id", "Name"),
                            NocPermissionType = new SelectList(nocPermission, "Id", "Name"),
                            NocType = new SelectList(nocType, "Id", "Name"),
                            OwnerType = new SelectList(ownerType, "Id", "Name"),
                            SiteAreaUnit = new SelectList(siteUnits, "Id", "Name"),
                            GrantKhasras = model.GrantKhasras,
                            Owners = model.Owners,
                            Pincode = selectedvillage.PinCode.ToString()
                        };
                        if (model.SelectedSiteAreaUnitId > 0 && model.KMLFile != null && model.KmlLinkName != null && model.KmlLinkName != "" && model.IDProofPhoto != null && model.AddressProofPhoto != null && model.AuthorizationLetterPhoto != null
                            && model.SelectedVillageID > 0 && model.SelectedProjectTypeId > 0 && model.SelectedNocPermissionTypeID > 0 && model.ApplicantName != null
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
                            if (model.Hadbast == null && model.PlotNo == null)
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
                                var grant = _repo.GetAll().OrderByDescending(x => x.Id).Select(x => x.Id).FirstOrDefault();
                                if (grant > 0)
                                {
                                    int grantId = grant;/* Convert.ToInt32((from g in grant
                                                               select new { id = g.Id }
                                            ).AsEnumerable().Max(x => x.id));*/
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
                                    IDProofPhotoPath = uniqueIDProofFileName,
                                    AddressProofPhotoPath = uniqueAddressProofFileName,
                                    AuthorizationLetterPhotoPath = uniqueAuthLetterFileName,
                                    VillageID = model.SelectedVillageID ?? 0,
                                    ProjectTypeId = model.SelectedProjectTypeId ?? 0,
                                    Hadbast = model.Hadbast,
                                    PlotNo = model.PlotNo,
                                    ApplicantName = model.ApplicantName,
                                    ApplicantEmailID = model.ApplicantEmailID,
                                    NocPermissionTypeID = model.SelectedNocPermissionTypeID ?? 0,
                                    NocTypeId = model.SelectedNocTypeId ?? 0,
                                    IsExtension = Convert.ToBoolean(model.IsExtension),
                                    KMLFilePath = uniqueKmlFileName,
                                    KMLLinkName = model.KmlLinkName,
                                    NocNumber = model.NocNumber,
                                    PreviousDate = model.PreviousDate,
                                    IsConfirmed = model.IsConfirmed,
                                    ApplicationID = model.ApplicationID,
                                    OtherProjectTypeDetail = model.OtherProjectTypeDetail,
                                    CreatedOn = DateTime.Now,
                                    IsPending = true
                                };
                                await _repo.CreateAsync(obj);
                                //model.SelectedOwnerTypeID
                                List<OwnerDetails> ownerList = new List<OwnerDetails>();
                                foreach (OwnerViewModelCreate item in viewModel.Owners)
                                {
                                    OwnerDetails owner = new OwnerDetails
                                    {
                                        Address = item.Address,
                                        Email = item.Email,
                                        GrantId = obj.Id,
                                        MobileNo = item.MobileNo,
                                        Name = item.Name,
                                        OwnerTypeId = item.SelectedOwnerTypeID
                                    };
                                    ownerList.Add(owner);
                                }
                                List<GrantKhasraDetails> khasraList = new List<GrantKhasraDetails>();
                                foreach (GrantKhasraViewModelCreate item in viewModel.GrantKhasras)
                                {
                                    GrantKhasraDetails khasra = new GrantKhasraDetails
                                    {
                                        KanalOrBigha = item.KanalOrBigha,
                                        KhasraNo = item.KhasraNo,
                                        MarlaOrBiswa = item.MarlaOrBiswa,
                                        SarsaiOrBiswansi = item.SarsaiOrBiswansi,
                                        UnitId = obj.SiteAreaUnitId,
                                        GrantID = obj.Id
                                    };
                                    khasraList.Add(khasra);
                                }
                                obj.Owners = ownerList;
                                obj.Khasras = khasraList;
                                await _repo.UpdateAsync(obj);
                                var emailModel = new EmailModel(model.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessage(model.ApplicantName, model.ApplicationID));
                                _emailService.SendEmail(emailModel, "Punjab Irrigation Department");
                                double TotalPayment = 0;
                                //if (Convert.ToDouble(model.TotalArea) <= 0.25)
                                //{
                                //    TotalPayment = 250;
                                //}
                                //else 
                                if (Convert.ToDouble(model.TotalArea) <= 0.50)
                                {
                                    TotalPayment = 500;
                                }
                                else if (Convert.ToDouble(model.TotalArea) > 0.50 && Convert.ToDouble(model.TotalArea) <= 1)
                                {
                                    TotalPayment = 1000;
                                }
                                else if (Convert.ToDouble(model.TotalArea) > 1)
                                {
                                    double area = Convert.ToDouble(model.TotalArea) - 1;
                                    TotalPayment = 1000;
                                    int count = 0;
                                    do
                                    {
                                        count++;
                                        area = area - 1;
                                    } while (area > 0);
                                    TotalPayment = TotalPayment + (count * 250);
                                }
                                if (TotalPayment > 0)
                                {
                                    VillageDetails applicantVillage = await _villageRpo.GetByIdAsync(model.SelectedVillageID ?? 0);
                                    TehsilBlockDetails applicantteh = await _tehsilBlockRepo.GetByIdAsync(model.SelectedTehsilBlockId ?? 0);
                                    SubDivisionDetails applicantsubdiv = await _subDivisionRepo.GetByIdAsync(model.SelectedSubDivisionId ?? 0);
                                    DivisionDetails applicantdiv = await _divisionRepo.GetByIdAsync(model.SelectedDivisionId ?? 0);
                                    PaymentRequest paymentRequestDetail = new PaymentRequest
                                    {
                                        Name = model.ApplicantName,
                                        Email = model.ApplicantEmailID,
                                        Address = "Division:" + applicantdiv.Name + ",Sub-Division:" + applicantsubdiv.Name + ",Tehsil/Block:" + applicantteh.Name + ",Village:" + applicantVillage.Name + ",Pincode:" + applicantVillage.PinCode,
                                        Amount = TotalPayment,
                                        GrantId = obj.Id,
                                        ApplicationId = obj.ApplicationID
                                    };
                                    return RedirectToAction("Index", "Payment", paymentRequestDetail);
                                }
                                else
                                {
                                    return RedirectToAction("Index", "Grant", new { Id = obj.ApplicationID });
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
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
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
            var user = await _repo.GetAll().AnyAsync(x=>x.ApplicantEmailID == applicantEmailID && x.IsRejected!=true);

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
