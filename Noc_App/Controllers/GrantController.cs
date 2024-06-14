using Microsoft.AspNetCore.Mvc;
using Noc_App.Models.interfaces;
using Noc_App.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Noc_App.Models.ViewModel;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Noc_App.Helpers;
using Noc_App.UtilityService;
using Noc_App.Models.Payment;
using Rotativa.AspNetCore;
using System.Collections.Generic;
using System.Security.Cryptography;
using Noc_App.Clients;
using System.Globalization;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;

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
        private readonly IRepository<SiteUnitMaster> _repoSiteUnitMaster;
        private readonly IEmailService _emailService;
        private readonly ICalculations _calculations;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IRepository<DistrictDetails> _districtRepo;
        private readonly IRepository<GrantRejectionShortfallSection> _grantrejectionRepository;
        private readonly IRepository<GrantSectionsDetails> _grantsectionRepository;
        private readonly IRepository<GrantApprovalMaster> _repoApprovalMaster;
        private readonly IRepository<DaysCheckMaster> _repoDaysCheckMaster;
        private readonly IRepository<PlanSanctionAuthorityMaster> _repoPlanSanctionAuthtoryMaster;
        [Obsolete]
        private readonly IHostingEnvironment _hostingEnvironment;
        [Obsolete]
        public GrantController(IRepository<GrantDetails> repo, IRepository<VillageDetails> villageRepo, IRepository<TehsilBlockDetails> tehsilBlockRepo, IRepository<SubDivisionDetails> subDivisionRepo,
            IRepository<DivisionDetails> divisionRepo, IRepository<ProjectTypeDetails> projectTypeRepo, IRepository<NocPermissionTypeDetails> nocPermissionTypeRepo,
            IRepository<NocTypeDetails> nocTypeRepo, IRepository<OwnerTypeDetails> ownerTypeRepo, IHostingEnvironment hostingEnvironment,
            IRepository<GrantKhasraDetails> khasraRepo, IRepository<SiteAreaUnitDetails> siteUnitsRepo, IEmailService emailService,
            IRepository<GrantPaymentDetails> grantPaymentRepo, IRepository<OwnerDetails> grantOwnersRepo, IRepository<DistrictDetails> districtRepo,
            IRepository<GrantApprovalDetail> repoApprovalDetail, IWebHostEnvironment hostEnvironment, IRepository<SiteUnitMaster> repoSiteUnitMaster
            , ICalculations calculations, IRepository<GrantRejectionShortfallSection> grantrejectionRepository
            , IRepository<GrantSectionsDetails> grantsectionRepository, IRepository<GrantApprovalMaster> repoApprovalMaster
            , IRepository<DaysCheckMaster> repoDaysCheckMaster, IRepository<PlanSanctionAuthorityMaster> repoPlanSanctionAuthtoryMaster)
        {
            _villageRpo = villageRepo;
            _tehsilBlockRepo = tehsilBlockRepo;
            _subDivisionRepo = subDivisionRepo;
            _divisionRepo = divisionRepo;
            _projectTypeRepo = projectTypeRepo;
            _nocPermissionTypeRepo = nocPermissionTypeRepo;
            _nocTypeRepo = nocTypeRepo;
            _ownerTypeRepo = ownerTypeRepo;
            _hostingEnvironment = hostingEnvironment;
            _repo = repo;
            _emailService = emailService;
            _khasraRepo = khasraRepo;
            _siteUnitsRepo = siteUnitsRepo;
            _grantPaymentRepo = grantPaymentRepo;
            _grantOwnersRepo = grantOwnersRepo;
            _repoApprovalDetail = repoApprovalDetail;
            _hostEnvironment = hostEnvironment;
            _repoSiteUnitMaster = repoSiteUnitMaster;
            _calculations = calculations;
            _districtRepo = districtRepo;
            _grantrejectionRepository = grantrejectionRepository;
            _grantsectionRepository = grantsectionRepository;
            _repoApprovalMaster = repoApprovalMaster;
            _repoDaysCheckMaster=repoDaysCheckMaster;
            _repoPlanSanctionAuthtoryMaster=repoPlanSanctionAuthtoryMaster;
        }

        [AllowAnonymous]
        public ViewResult Index(string Id)
        {
            try
            {
                var g = (from gr in _repo.GetAll()
                         join p in _grantPaymentRepo.GetAll() on gr.Id equals p.GrantID into payment
                         from pay in payment.DefaultIfEmpty()
                         where gr.ApplicationID.ToLower() == Id.Trim().ToLower()
                         select new
                         {
                             gr,
                             pay

                         }
                    ).FirstOrDefault();
                GrantDetails obj = g.gr;
                double total = 0;
                var units =  _repoSiteUnitMaster.Find(x => x.SiteAreaUnitId == g.gr.SiteAreaUnitId);
                SiteUnitMaster k = units.Where(x => x.UnitCode.ToUpper() == "K").FirstOrDefault();
                SiteUnitMaster m = units.Where(x => x.UnitCode.ToUpper() == "M").FirstOrDefault();
                SiteUnitMaster s = units.Where(x => x.UnitCode.ToUpper() == "S").FirstOrDefault();
                total = Math.Round(((from kh in _khasraRepo.GetAll()
                                     where kh.GrantID == g.gr.Id
                                     select new
                                     {
                                         TotalArea = ((kh.KanalOrBigha * k.UnitValue * k.Timesof) / k.DivideBy) + ((kh.MarlaOrBiswa * m.UnitValue * m.Timesof) / m.DivideBy) + ((kh.SarsaiOrBiswansi * s.UnitValue * s.Timesof) / s.DivideBy)

                                     }).Sum(d => d.TotalArea)), 4);
                double TotalPayment = 0;
                //string calculation = "";
                

                if (obj != null)
                {
                    var payment = g.pay;
                    if (payment == null)
                    {
                        GrantViewModel model = new GrantViewModel
                        {
                            Id = obj.Id,
                            ApplicationID = obj.ApplicationID,
                            OrderId = "0",
                            Message = "Payment is not successfull",
                            TotalAmount= TotalPayment
                        };
                        return View(model);
                    }
                    else
                    {
                        if (Convert.ToDouble(total) <= 0.50)
                        {
                            TotalPayment = 500;
                            //calculation = "For " + total.ToString() + " Acre, Amount is " + TotalPayment.ToString();
                        }
                        else if (Convert.ToDouble(total) > 0.50 && Convert.ToDouble(total) <= 1)
                        {
                            TotalPayment = 1000;
                            //calculation = "For " + total.ToString() + " Acre, Amount is " + TotalPayment.ToString();
                        }
                        else if (Convert.ToDouble(total) > 1)
                        {
                            double area = Convert.ToDouble(total) - 1;
                            string area2 = area.ToString("#.####");
                            TotalPayment = 1000;
                            //calculation = "For 1 Acre, Amount is " + TotalPayment.ToString();
                            int count = 0;
                            do
                            {
                                count++;
                                area = area - 1;
                            } while (area > 0);
                            // calculation += ". On Additional " + area2 + " Acres, Amount is " + 250.ToString() + " per/Acre";
                            TotalPayment = TotalPayment + (count * 250);
                            //calculation += ". For Total " + model.TotalArea.ToString() + " Acres, Amount is " + TotalPayment.ToString();
                        }
                        GrantPaymentDetails objPyment = payment;
                        GrantViewModel model = new GrantViewModel
                        {
                            Id = obj.Id,
                            ApplicationID = obj.ApplicationID,
                            OrderId = objPyment.PaymentOrderId,
                            Message = "Payment is successfull",
                            TotalAmount = TotalPayment
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
        public ViewResult UpdateIndex(string Id)
        {
            try
            {
                var g = (from gr in _repo.GetAll()
                         where gr.ApplicationID.ToLower() == Id.Trim().ToLower()
                         select new
                         {
                             gr

                         }
                    ).FirstOrDefault();
                GrantDetails obj = g.gr;
                if (obj != null)
                {
                    GrantViewModel model = new GrantViewModel
                    {
                        Id = obj.Id,
                        ApplicationID = obj.ApplicationID,
                        Message = "Payment is successfull"
                    };
                    return View(model);
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
            if (searchString != null && searchString.Trim() != "")
            {
                var model = (from g in _repo.GetAll()
                             join p in _grantPaymentRepo.GetAll() on g.Id equals p.GrantID into grantPayment
                             from payment in grantPayment.DefaultIfEmpty()
                             join app in _repoApprovalDetail.GetAll() on g.Id equals app.GrantID into grantApproval
                             from approval in grantApproval.DefaultIfEmpty()
                             where g.ApplicationID.ToLower() == searchString.Trim().ToLower()
                             select new GrantStatusViewModel
                             {
                                 ApplicationID = g.ApplicationID,
                                 IsApproved = g.IsApproved,
                                 CreatedOn = string.Format("{0:dd/MM/yyyy}", g.CreatedOn),
                                 ApplicationStatus = payment != null && payment.PaymentOrderId != "0" ? "Paid" : "Pending",
                                 ApprovalStatus = g.IsForwarded == false && g.IsShortFall == true && g.IsShortFallCompleted == false && g.IsRejected == false ? "Reverted to applicant for modification" : g.IsForwarded == true && g.IsShortFall == false && g.IsShortFallCompleted == true && g.IsRejected == false ? "Application modified by applicant" : g.IsPending == true ? g.IsRejected ? "Rejected" : g.IsForwarded ? approval != null ? "Pending With " + approval.ProcessedToRole : "UnProcessed" : "UnProcessed" : g.IsApproved ? "NOC Issued" : "UnProcessed",
                                 CertificateFilePath = g.CertificateFilePath
                             }
                         ).FirstOrDefault();
                if (model != null) return View(model);
                else
                {
                    return View(null);
                }
            }
            else
            {
                ModelState.AddModelError("", "Application ID field is required");
                return View();
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
                var domain = HttpContext.Request.Host.Value;
                var scheme = HttpContext.Request.Scheme;
                var g = (from gr in _repo.GetAll()
                         join v in _villageRpo.GetAll() on gr.VillageID equals v.Id
                         join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                         join sub in _subDivisionRepo.GetAll() on t.SubDivisionId equals sub.Id
                         join d in _divisionRepo.GetAll() on sub.DivisionId equals d.Id
                         join npt in _nocPermissionTypeRepo.GetAll() on gr.NocPermissionTypeID equals npt.Id
                         join npr in _nocTypeRepo.GetAll() on gr.NocTypeId equals npr.Id
                         join un in _siteUnitsRepo.GetAll() on gr.SiteAreaUnitId equals un.Id
                         join p in _grantPaymentRepo.GetAll() on gr.Id equals p.GrantID into paymentdetail
                         from pay in paymentdetail.DefaultIfEmpty()
                         where gr.ApplicationID.ToLower() == Id.Trim().ToLower()
                         select new
                         {
                             Grant = gr,
                             Payment = pay,
                             Village = v,
                             Tehsil = t,
                             SubDivision = sub,
                             Division = d,
                             PermissionType = npt,
                             NocType = npr,
                             Unit = un
                         }
                   ).FirstOrDefault();
                GrantDetails obj = g.Grant;
                VillageDetails village = g.Village;
                TehsilBlockDetails tehsil = g.Tehsil;
                SubDivisionDetails subDivision = g.SubDivision;
                DivisionDetails division = g.Division;
                NocPermissionTypeDetails permission = g.PermissionType;
                NocTypeDetails noctype = g.NocType;
                SiteAreaUnitDetails unit = g.Unit;
                List<GrantKhasraDetails> khasras = (await _khasraRepo.FindAsync(x => x.GrantID == obj.Id)).ToList();
                List<OwnerDetails> owners = (await _grantOwnersRepo.FindAsync(x => x.GrantId == obj.Id)).ToList();
                //List<SiteUnitMaster> unitMaster = (await _repoSiteUnitMaster.FindAsync(x => x.SiteAreaUnitId == obj.SiteAreaUnitId)).ToList();
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
                    totalArea = Math.Round(totalArea + units.KanalOrBigha + units.MarlaOrBiswa + units.SarsaiOrBiswansi, 4);
                }
                var payment = g.Payment;
                if (payment == null)
                {
                    GrantDetailViewModel model = new GrantDetailViewModel
                    {
                        Id = obj.Id,
                        ApplicationID = obj.ApplicationID,
                        PaymentOrderId = "0",
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
                }
                else
                {
                    GrantPaymentDetails objPyment = (payment);
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
                    model.Domain = scheme + "://" + domain;
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
                OwnerDetails owners = (await _grantOwnersRepo.FindAsync(x => x.GrantId == model.Id)).ToList().FirstOrDefault();
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
                    totalArea = Math.Round(totalArea + units.KanalOrBigha + units.MarlaOrBiswa + units.SarsaiOrBiswansi, 4);

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
                    ApplicationId = Id,
                    Hadbast = model.Hadbast,
                    PlotNo = model.PlotNo == null ? "" : model.PlotNo,
                    Division = division.Name,
                    SubDivision = subDivision.Name,
                    Tehsil = tehsil.Name,
                    Village = village.Name + "-" + village.PinCode.ToString(),
                    Pincode = village.PinCode.ToString(),
                    TehsilId = tehsil.Id.ToString(),
                    DistrictId = division.Id.ToString(),
                    PayerName = model.ApplicantName,
                    MobileNo = owners.MobileNo,
                    PhoneNumber = owners.MobileNo
                };
                //return RedirectToAction("Index", "Payment", paymentRequestDetail);
                return RedirectToAction("ProcessChallan", "Payment", paymentRequestDetail);
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
            var planAuth = _repoPlanSanctionAuthtoryMaster.GetAll();
            var viewModel = new GrantViewModelCreate
            {
                Divisions = new SelectList(divisions, "Id", "Name"),
                ProjectType = new SelectList(projectType, "Id", "Name"),
                PlanSanctionAuthorityMaster = new SelectList(planAuth, "Id", "Name"),
                NocPermissionType = new SelectList(nocPermission, "Id", "Name"),
                NocType = new SelectList(nocType, "Id", "Name"),
                Village = new SelectList(Enumerable.Empty<VillageDetails>(), "Id", "Name"),
                TehsilBlock = new SelectList(Enumerable.Empty<TehsilBlockDetails>(), "Id", "Name"),
                SubDivision = new SelectList(Enumerable.Empty<SubDivisionDetails>(), "Id", "Name"),
                SiteAreaUnit = new SelectList(siteUnits, "Id", "Name"),
                GrantKhasras = new List<GrantKhasraViewModelCreate> { new GrantKhasraViewModelCreate { KanalOrBigha = 0, KhasraNo = "", MarlaOrBiswa = 0, SarsaiOrBiswansi = 0/*,SelectedUnitId= siteUnits.OrderBy(x=>x.Name).Select(x=>x.Id).FirstOrDefault()*/ } },
                Owners = new List<OwnerViewModelCreate> { new OwnerViewModelCreate { Name = "", Address = "", MobileNo = "", Email = "", OwnerType = new SelectList(ownerType, "Id", "Name") } },
                IsOtherTypeSelected = 0,
                IsPaymentDone = false,
                IsConfirmed = false,
                IsExtension = 0,
                TotalAreaSqFeet = "0",
                TotalAreaSqMetre = "0"
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
                    var planAuth = _repoPlanSanctionAuthtoryMaster.GetAll();
                    var filteredSubdivisions = await _subDivisionRepo.FindAsync(c => c.DivisionId == model.SelectedDivisionId);
                    var filteredtehsilBlock = await _tehsilBlockRepo.FindAsync(c => c.SubDivisionId == model.SelectedSubDivisionId);
                    var fileteedvillage = await _villageRpo.FindAsync(c => c.TehsilBlockId == model.SelectedTehsilBlockId);
                    var selectedvillage = await _villageRpo.GetByIdAsync(model.SelectedVillageID);
                    string ErrorMessage = string.Empty;
                    if (filteredSubdivisions.Count() > 0 && filteredtehsilBlock.Count() > 0 && fileteedvillage.Count() > 0 && selectedvillage != null)
                    {
                        var viewModel = new GrantViewModelCreate
                        {
                            Village = new SelectList(fileteedvillage, "Id", "Name",model.SelectedVillageID),
                            TehsilBlock = new SelectList(filteredtehsilBlock, "Id", "Name", model.SelectedTehsilBlockId),
                            SubDivision = new SelectList(filteredSubdivisions, "Id", "Name", model.SelectedSubDivisionId),
                            Divisions = new SelectList(divisions, "Id", "Name", model.SelectedDivisionId),
                            PlanSanctionAuthorityMaster = new SelectList(planAuth, "Id", "Name",model.SelectedPlanSanctionAuthorityId),
                            ProjectType = new SelectList(projectType, "Id", "Name",model.SelectedProjectTypeId),
                            NocPermissionType = new SelectList(nocPermission, "Id", "Name",model.SelectedNocPermissionTypeID),
                            NocType = new SelectList(nocType, "Id", "Name",model.SelectedNocTypeId),
                            //OwnerType = new SelectList(ownerType, "Id", "Name"),
                            SiteAreaUnit = new SelectList(siteUnits, "Id", "Name"),
                            GrantKhasras = model.GrantKhasras,
                            Owners = model.Owners,
                            Pincode = selectedvillage.PinCode.ToString()
                        };
                        //model.OwnerType = viewModel.OwnerType;
                        if (model.SelectedSiteAreaUnitId > 0 && model.KMLFile != null && model.LayoutPlanFilePhoto != null && model.FaradFilePoto != null && model.KmlLinkName != null && model.KmlLinkName != "" && model.IDProofPhoto != null && model.AddressProofPhoto != null && model.AuthorizationLetterPhoto != null
                            && model.SelectedVillageID > 0 && model.SelectedProjectTypeId > 0 && model.SelectedNocPermissionTypeID > 0 && model.ApplicantName != null
                            && model.ApplicantEmailID != null && model.SelectedNocTypeId > 0 && model.SelectedPlanSanctionAuthorityId > 0 && model.IsConfirmed
                            )
                        {
                            int IdProofValidation = AllowedCheckExtensions(model.IDProofPhoto, "proof");
                            int AddressProofValidation = AllowedCheckExtensions(model.AddressProofPhoto, "proof");
                            int AuthorizationValidation = AllowedCheckExtensions(model.AuthorizationLetterPhoto, "proof");
                            int LayoutPlanValidation = AllowedCheckExtensions(model.LayoutPlanFilePhoto, "proof");
                            int FaradValidation = AllowedCheckExtensions(model.FaradFilePoto, "proof");
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
                            if (LayoutPlanValidation == 0)
                            {
                                ErrorMessage = $"Invalid layout plan file type. Please upload a JPG, PNG, or PDF file";
                                ModelState.AddModelError("", ErrorMessage);

                                return View(viewModel);

                            }
                            else if (LayoutPlanValidation == 2)
                            {
                                ErrorMessage = "Layout plan field is required";
                                ModelState.AddModelError("", ErrorMessage);
                                return View(viewModel);
                            }
                            if (FaradValidation == 0)
                            {
                                ErrorMessage = $"Invalid farad file type. Please upload a JPG, PNG, or PDF file";
                                ModelState.AddModelError("", ErrorMessage);

                                return View(viewModel);

                            }
                            else if (FaradValidation == 2)
                            {
                                ErrorMessage = "Farad field is required";
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
                            if (!AllowedFileSize(model.LayoutPlanFilePhoto))
                            {
                                ErrorMessage = "Layout plan file size exceeds the allowed limit of 4MB";
                                ModelState.AddModelError("", ErrorMessage);
                                return View(viewModel);
                            }
                            if (!AllowedFileSize(model.FaradFilePoto))
                            {
                                ErrorMessage = "Farad file size exceeds the allowed limit of 4MB";
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
                            string uniqueLayoutPlanFileName = ProcessUploadedFile(model.LayoutPlanFilePhoto, "LayoutPlan");
                            string uniqueFaradFileName = ProcessUploadedFile(model.FaradFilePoto, "Farad");
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
                                List<OwnerDetails> ownerList = new List<OwnerDetails>();
                                foreach (OwnerViewModelCreate item in viewModel.Owners)
                                {
                                    OwnerDetails owner = new OwnerDetails
                                    {
                                        Address = item.Address,
                                        Email = item.Email,
                                        MobileNo = item.MobileNo,
                                        Name = item.Name,
                                        OwnerTypeId = item.SelectedOwnerTypeID
                                    };
                                    ownerList.Add(owner);
                                }
                                List<GrantKhasraDetails> khasraList = new List<GrantKhasraDetails>();
                                int duplicates = viewModel.GrantKhasras.GroupBy(x => x.KhasraNo).Where(g => g.Count() > 1).Count();
                                if (duplicates <= 0)
                                {
                                    foreach (GrantKhasraViewModelCreate item in viewModel.GrantKhasras)
                                    {
                                        var khasraDeplicate = viewModel.GrantKhasras.Any(x => x.KhasraNo == item.KhasraNo);
                                        GrantKhasraDetails khasra = new GrantKhasraDetails
                                        {
                                            KanalOrBigha = item.KanalOrBigha,
                                            KhasraNo = item.KhasraNo,
                                            MarlaOrBiswa = item.MarlaOrBiswa,
                                            SarsaiOrBiswansi = item.SarsaiOrBiswansi,
                                            UnitId = model.SelectedSiteAreaUnitId,
                                            //GrantID = obj.Id
                                        };
                                        khasraList.Add(khasra);
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("", $"Please check duplicate khasra numbers");

                                    return View(viewModel);
                                }
                                GrantDetails obj = new GrantDetails
                                {
                                    Name = model.Name,
                                    SiteAreaUnitId = model.SelectedSiteAreaUnitId,
                                    IDProofPhotoPath = uniqueIDProofFileName,
                                    AddressProofPhotoPath = uniqueAddressProofFileName,
                                    AuthorizationLetterPhotoPath = uniqueAuthLetterFileName,
                                    VillageID = model.SelectedVillageID,
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
                                    LayoutPlanFilePath=uniqueLayoutPlanFileName,
                                    FaradFilePath=uniqueFaradFileName,
                                    PlanSanctionAuthorityId=model.SelectedPlanSanctionAuthorityId,
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
                                var idproof = fileUploadeSave(model.IDProofPhoto, uniqueIDProofFileName);
                                var addressproof = fileUploadeSave(model.AddressProofPhoto, uniqueAddressProofFileName);
                                var authletter = fileUploadeSave(model.AuthorizationLetterPhoto, uniqueAuthLetterFileName);
                                var kml = fileUploadeSave(model.KMLFile, uniqueKmlFileName);
                                obj.Owners = ownerList;
                                obj.Khasras = khasraList;
                                await _repo.UpdateAsync(obj);
                                //if (msg == "success")
                                //{
                                    var emailModel = new EmailModel(model.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantMessage(model.ApplicantName, model.ApplicationID));
                                    _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");
                                    double TotalPayment = 0;
                                
                                    string calculation = "", additionalcalculation = "", totalareacalculation="";
                                    if (Convert.ToDouble(model.TotalArea) <= 0.50)
                                    {
                                        TotalPayment = 500;
                                        calculation = "For " + model.TotalArea.ToString() + " Acre, Amount is ₹" + TotalPayment.ToString() + " (NON-REFUNDABLE)";
                                    }
                                    else if (Convert.ToDouble(model.TotalArea) > 0.50 && Convert.ToDouble(model.TotalArea) <= 1)
                                    {
                                        TotalPayment = 1000;
                                        calculation = "For " + model.TotalArea.ToString() + " Acre, Amount is ₹" + TotalPayment.ToString() + " (NON-REFUNDABLE)";
                                    }
                                    else if (Convert.ToDouble(model.TotalArea) > 1)
                                    {
                                        double area = Convert.ToDouble(model.TotalArea) - 1;
                                        string area2 = area.ToString("#.####");
                                        TotalPayment = 1000;
                                        calculation = "For 1 Acre, Amount is ₹" + TotalPayment.ToString();
                                        int count = 0;
                                        do
                                        {
                                            count++;
                                            area = area - 1;
                                        } while (area > 0);
                                    if(Convert.ToDouble(area2) < 0)
                                    {
                                        area2 = (Convert.ToDouble(area2) * -1).ToString();
                                    }
                                    additionalcalculation = "Additional Area Calculation With Amount ₹" + 250.ToString() + " per/Acre : On Additional " + area2 + " Acres, Amount is ₹" + (TotalPayment + (count * 250) - 1000).ToString();
                                        TotalPayment = TotalPayment + (count * 250);
                                    totalareacalculation = "Amount Calculation for Total " + model.TotalArea.ToString() + " Acres : ₹" + TotalPayment.ToString()+" (NON-REFUNDABLE)";
                                    }
                                    if (TotalPayment > 0)
                                    {
                                        var detail = (from v in _villageRpo.GetAll()
                                                      join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                                                      join s in _subDivisionRepo.GetAll() on t.SubDivisionId equals s.Id
                                                      join d in _divisionRepo.GetAll() on s.DivisionId equals d.Id
                                                      join dist in _districtRepo.GetAll() on d.DistrictId equals dist.Id
                                                      where v.Id == model.SelectedVillageID
                                                      select new
                                                      {
                                                          Village = v,
                                                          Tehsil = t,
                                                          SubDivision = s,
                                                          Division = d,
                                                          District = dist
                                                      }).FirstOrDefault();
                                        int grantid =  _repo.Find(x => x.ApplicationID == model.ApplicationID).FirstOrDefault().Id;
                                        PaymentRequest paymentRequestDetail = new PaymentRequest
                                        {
                                            Name = model.Name,
                                            PayerName = model.Owners.FirstOrDefault().Name,
                                            MobileNo = viewModel.Owners.FirstOrDefault().MobileNo,
                                            Email = model.ApplicantEmailID,
                                            Address = "Division:" + detail.Division.Name + ",Sub-Division:" + detail.SubDivision.Name + ",Tehsil/Block:" + detail.Tehsil.Name + ",Village:" + detail.Village.Name + ",Pincode:" + detail.Village.PinCode,
                                            Amount = TotalPayment,
                                            GrantId = grantid,
                                            ApplicationId = model.ApplicationID,
                                            DistrictId = detail.District.LGD_ID.ToString(),
                                            Hadbast = model.Hadbast,
                                            PhoneNumber = viewModel.Owners.FirstOrDefault().MobileNo,
                                            Pincode = detail.Village.PinCode.ToString(),
                                            PlotNo = model.PlotNo != null && model.PlotNo != "" ? model.PlotNo : "0",
                                            TehsilId = detail.Tehsil.LGD_ID.ToString(),
                                            Division = detail.Division.Name,
                                            SubDivision = detail.SubDivision.Name,
                                            Tehsil = detail.Tehsil.Name,
                                            Village = detail.Village.Name,
                                            AreaCalculation = calculation,
                                            AreaAdditionalCalculation=additionalcalculation,
                                            TotalAreaCalculation=totalareacalculation
                                        };
                                        return RedirectToAction("Index", "Payment", paymentRequestDetail);
                                    }
                                    else
                                    {
                                        return RedirectToAction("Index", "Grant", new { Id = model.ApplicationID });
                                    }
                                //}

                                //else
                                //{
                                //    ModelState.AddModelError("", "Something went wrong while saving details");
                                //}

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
                    ModelState.AddModelError("", $"All fields are required");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Modify(string id)
       {
            try
            {
                var grant = (from g in _repo.GetAll()
                             join p in _grantPaymentRepo.GetAll() on g.Id equals p.GrantID
                             join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                             join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                             join s in _subDivisionRepo.GetAll() on t.SubDivisionId equals s.Id
                             join d in _divisionRepo.GetAll() on s.DivisionId equals d.Id
                             join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                             join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                             join r in _grantrejectionRepository.GetAll() on a.Id equals r.GrantApprovalId
                             join pr in _grantsectionRepository.GetAll() on r.SectionId equals pr.Id
                             where g.ApplicationID.ToLower() == id.Trim().ToLower()
                             && m.Code.ToUpper() == "SF" 
                             && g.ShortFallLevel == a.ProcessLevel && g.IsShortFallCompleted==false
                             select new
                             {
                                 Grant = g,
                                 Payment = p,
                                 Village = v,
                                 Tehsil = t,
                                 SubDivision = s,
                                 Division = d
                             }
                         ).FirstOrDefault();
                if (grant != null)
                {
                    string startDateStr =  string.Format("{0:dd/MM/yyyy}", grant.Grant.ShortFallReportedOn);
                    string endDateStr = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                    DateTime startDate = ParseDate(startDateStr);
                    DateTime endDate = ParseDate(endDateStr);
                    double businessDays = CalculateBusinessDays(startDate, endDate);
                    double totalDays = (await _repoDaysCheckMaster.FindAsync(x => x.Code == "SF")).FirstOrDefault().NoOfDays;
                    if (businessDays <= totalDays)
                    {
                        if (grant.Grant.IsShortFallCompleted == false)
                        {
                            var projectType = _projectTypeRepo.GetAll();
                            var planAuth = _repoPlanSanctionAuthtoryMaster.GetAll();
                            var nocPermission = _nocPermissionTypeRepo.GetAll();
                            var nocType = _nocTypeRepo.GetAll();
                            var ownerType = _ownerTypeRepo.GetAll();
                            var siteUnits = _siteUnitsRepo.GetAll();
                            var villages = _villageRpo.Find(x => x.TehsilBlockId == grant.Village.TehsilBlockId);
                            var tehsils = _tehsilBlockRepo.Find(x => x.SubDivisionId == grant.Tehsil.SubDivisionId);
                            var subdivisions = _subDivisionRepo.Find(x => x.DivisionId == grant.SubDivision.DivisionId);
                            var divisions = _divisionRepo.GetAll();
                            var khasras = _khasraRepo.Find(x => x.GrantID == grant.Grant.Id);
                            var owners = (from o in _grantOwnersRepo.GetAll()
                                          join types in _ownerTypeRepo.GetAll() on o.OwnerTypeId equals types.Id
                                          where o.GrantId == grant.Grant.Id
                                          select new
                                          {
                                              Owner = o,
                                              OwnerType = types
                                          }
                                          ).ToList();


                            List<GrantKhasraViewModelCreate> khasralist = new List<GrantKhasraViewModelCreate>();
                            List<OwnerViewModelCreate> ownerlist = new List<OwnerViewModelCreate>();
                            List<OwnerTypeDetails> ownertype = _ownerTypeRepo.GetAll().ToList();
                            int count = 0;
                            foreach (var item in owners)
                            {
                                ownerlist.Add(new OwnerViewModelCreate
                                {
                                    OId = item.Owner.Id,
                                    SelectedOwnerTypeID = item.Owner.OwnerTypeId,
                                    RowId = ++count,
                                    OwnerType = new SelectList(ownertype, "Id", "Name", item.Owner.OwnerTypeId),
                                    Address = item.Owner.Address,
                                    Email = item.Owner.Email,
                                    MobileNo = item.Owner.MobileNo,
                                    Name = item.Owner.Name,
                                    OwnerTypeName = item.OwnerType.Name
                                });
                            }
                            double totalArea = 0;
                            count = 0;
                            foreach (GrantKhasraDetails item in khasras)
                            {
                                SiteUnitsViewModel unitDetails = new SiteUnitsViewModel
                                {
                                    KanalOrBigha = item.KanalOrBigha,
                                    MarlaOrBiswa = item.MarlaOrBiswa,
                                    SarsaiOrBiswansi = item.SarsaiOrBiswansi,
                                    SiteUnitId = grant.Grant.SiteAreaUnitId
                                };
                                var units = await _calculations.CalculateUnits(unitDetails);
                                totalArea = Math.Round(totalArea + units.KanalOrBigha + units.MarlaOrBiswa + units.SarsaiOrBiswansi, 4);
                                khasralist.Add(new GrantKhasraViewModelCreate { RowId = ++count, KId = item.Id, KanalOrBigha = item.KanalOrBigha, KhasraNo = item.KhasraNo, MarlaOrBiswa = item.MarlaOrBiswa, SarsaiOrBiswansi = item.SarsaiOrBiswansi });
                            }
                            List<SiteUnitMaster> unitMaster = (await _repoSiteUnitMaster.FindAsync(x => x.SiteAreaUnitId == grant.Grant.SiteAreaUnitId)).ToList();
                            var KanalOrBigha = unitMaster.Find(x => x.UnitCode.ToUpper() == "K");
                            var MarlaOrBiswa = unitMaster.Find(x => x.UnitCode.ToUpper() == "M");
                            var SarsaiOrBiswansi = unitMaster.Find(x => x.UnitCode.ToUpper() == "S");
                            GrantSections sections = new GrantSections();
                            var sectiondetail = (from p in _grantsectionRepository.GetAll()
                                                 join r in _grantrejectionRepository.GetAll() on p.Id equals r.SectionId
                                                 join a in _repoApprovalDetail.GetAll() on r.GrantApprovalId equals a.Id
                                                 join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                                                 where r.GrantApprovalId == a.Id && m.Code.ToUpper() == "SF" && a.GrantID == grant.Grant.Id && grant.Grant.ShortFallLevel == a.ProcessLevel
                                                 select new
                                                 {
                                                     project = p,
                                                     rejectedSection = r
                                                 }).ToList();
                            sections.project = 0;
                            sections.address = 0;
                            sections.kml = 0;
                            sections.applicant = 0;
                            sections.owner = 0;
                            sections.permission = 0;
                            foreach (var item in sectiondetail)
                            {
                                if (item.project.SectionCode.ToUpper() == "P")
                                {
                                    sections.project = 1;
                                    sections.isprojectCompleted = item.rejectedSection.IsCompleted;
                                    sections.projectid = item.rejectedSection.Id;
                                    sections.isPActive = "active"; sections.isADActive = "";
                                }
                                else if (item.project.SectionCode.ToUpper() == "AD")
                                {
                                    sections.address = 1;
                                    sections.isaddressCompleted = item.rejectedSection.IsCompleted;
                                    sections.addressid = item.rejectedSection.Id;
                                    sections.isADActive = sections.isPActive == "active" ? "" : "active";
                                }
                                else if (item.project.SectionCode.ToUpper() == "K")
                                {
                                    sections.kml = 1;
                                    sections.iskmlCompleted = item.rejectedSection.IsCompleted;
                                    sections.kmlid = item.rejectedSection.Id;
                                    sections.isKMLActive = sections.isPActive == "active" || sections.isADActive == "active" ? "" : "active";
                                }
                                else if (item.project.SectionCode.ToUpper() == "AP")
                                {
                                    sections.applicant = 1;
                                    sections.isapplicantCompleted = item.rejectedSection.IsCompleted;
                                    sections.applicantid = item.rejectedSection.Id;
                                    sections.isAPActive = sections.isPActive == "active" || sections.isADActive == "active" || sections.isKMLActive == "active" || sections.isPrActive == "active" ? "" : "active";
                                }
                                else if (item.project.SectionCode.ToUpper() == "OW")
                                {
                                    sections.owner = 1;
                                    sections.isownerCompleted = item.rejectedSection.IsCompleted;
                                    sections.ownerid = item.rejectedSection.Id;
                                    sections.isOActive = sections.isPActive == "active" || sections.isADActive == "active" || sections.isKMLActive == "active" || sections.isAPActive == "active" || sections.isPrActive == "active" ? "" : "active";
                                }
                                else if (item.project.SectionCode.ToUpper() == "PM")
                                {
                                    sections.permission = 1;
                                    sections.ispermissionCompleted = item.rejectedSection.IsCompleted;
                                    sections.permissionid = item.rejectedSection.Id;
                                    sections.isPrActive = sections.isPActive == "active" || sections.isADActive == "active" || sections.isKMLActive == "active" ? "" : "active";
                                }
                            }
                            int totalCompleted = (from g in _repo.GetAll()
                                                  join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                                                  join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                                                  join r in _grantrejectionRepository.GetAll() on a.Id equals r.GrantApprovalId
                                                  where g.ShortFallLevel == a.ProcessLevel && m.Code == "SF" && r.IsCompleted == 0 && g.Id == grant.Grant.Id
                                                  select new { rjectionid = r.Id }).Count();
                            var viewModel = new GrantViewModelEdit
                            {
                                Divisions = new SelectList(divisions, "Id", "Name", grant.Division.Id),
                                ProjectType = new SelectList(projectType, "Id", "Name", grant.Grant.ProjectTypeId),
                                PlanSanctionAuthorityMaster = new SelectList(planAuth, "Id", "Name", grant.Grant.PlanSanctionAuthorityId),
                                NocPermissionType = new SelectList(nocPermission, "Id", "Name", grant.Grant.NocPermissionTypeID),
                                NocType = new SelectList(nocType, "Id", "Name", grant.Grant.NocTypeId),
                                Village = new SelectList(villages, "Id", "Name", grant.Village.Id),
                                TehsilBlock = new SelectList(tehsils, "Id", "Name", grant.Tehsil.Id),
                                SubDivision = new SelectList(subdivisions, "Id", "Name", grant.SubDivision.Id),
                                SiteAreaUnit = new SelectList(siteUnits, "Id", "Name"),
                                GrantKhasras = khasralist,
                                Owners = ownerlist,
                                SelectedSiteAreaUnitId = grant.Grant.SiteAreaUnitId,
                                IsOtherTypeSelected = 0,
                                IsConfirmed = false,
                                IsExtension = 0,
                                TotalArea = totalArea.ToString("#.####"),
                                TotalAreaSqFeet = (totalArea * 43560).ToString("#.####"),
                                TotalAreaSqMetre = (totalArea * 4046.86).ToString("#.####"),
                                Name = grant.Grant.Name,
                                ApplicantEmailID = grant.Grant.ApplicantEmailID,
                                ApplicantName = grant.Grant.ApplicantName,
                                ApplicationID = grant.Grant.ApplicationID,
                                Hadbast = grant.Grant.Hadbast,
                                Id = grant.Grant.Id,
                                PId = grant.Grant.Id,
                                AdId = grant.Grant.Id,
                                KId = grant.Grant.Id,
                                FGrantId = grant.Grant.Id,
                                KmlGrantId = grant.Grant.Id,
                                PermisionGrantId = grant.Grant.Id,
                                OwnerGrantId = grant.Grant.Id,
                                ApplicantGrantId = grant.Grant.Id,
                                KmlLinkName = grant.Grant.KMLLinkName,
                                NocNumber = grant.Grant.NocNumber,
                                OtherProjectTypeDetail = grant.Grant.OtherProjectTypeDetail,
                                Pincode = grant.Village.PinCode.ToString(),
                                PlotNo = grant.Grant.PlotNo,
                                PreviousDate = grant.Grant.PreviousDate,
                                OwnerType = new SelectList(ownertype, "Id", "Name"),
                                SelectedDivisionId = grant.Division.Id,
                                SelectedNocPermissionTypeID = grant.Grant.NocPermissionTypeID,
                                SelectedNocTypeId = grant.Grant.NocTypeId,
                                SelectedProjectTypeId = grant.Grant.ProjectTypeId,
                                SelectedSubDivisionId = grant.SubDivision.Id,
                                SelectedTehsilBlockId = grant.Tehsil.Id,
                                SelectedVillageID = grant.Village.Id,
                                AddressProofPhotoPath = grant.Grant.AddressProofPhotoPath,
                                AuthorizationLetterPhotoPath = grant.Grant.AuthorizationLetterPhotoPath,
                                IDProofPhotoPath = grant.Grant.IDProofPhotoPath,
                                LayoutPlanFilePath = grant.Grant.LayoutPlanFilePath,
                                FaradFilePath = grant.Grant.FaradFilePath,
                                KMLFilePath = grant.Grant.KMLFilePath,
                                KUnitValue = KanalOrBigha.UnitValue,
                                KTimesof = KanalOrBigha.Timesof,
                                KDivideBy = KanalOrBigha.DivideBy,
                                MUnitValue = MarlaOrBiswa.UnitValue,
                                MTimesof = MarlaOrBiswa.Timesof,
                                MDivideBy = MarlaOrBiswa.DivideBy,
                                SUnitValue = SarsaiOrBiswansi.UnitValue,
                                STimesof = SarsaiOrBiswansi.Timesof,
                                SDivideBy = SarsaiOrBiswansi.DivideBy,
                                GrantSections = sections,
                                AreAllSectionCompleted = totalCompleted
                            };
                            if (viewModel != null && sectiondetail.Count > 0)
                                return View(viewModel);
                            else
                                return RedirectToAction("UnAuthorized");
                        }
                        else
                        {
                            return RedirectToAction("Expired");
                        }
                    }
                    else
                    {
                        return RedirectToAction("Expired");
                    }
                }
                else
                    return RedirectToAction("UnAuthorized");
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<GrantViewModelEdit> ModifyResult(string id)
        {
            try
            {
                var grant = (from g in _repo.GetAll()
                             join p in _grantPaymentRepo.GetAll() on g.Id equals p.GrantID
                             join v in _villageRpo.GetAll() on g.VillageID equals v.Id
                             join t in _tehsilBlockRepo.GetAll() on v.TehsilBlockId equals t.Id
                             join s in _subDivisionRepo.GetAll() on t.SubDivisionId equals s.Id
                             join d in _divisionRepo.GetAll() on s.DivisionId equals d.Id
                             join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                             join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                             join r in _grantrejectionRepository.GetAll() on a.Id equals r.GrantApprovalId
                             join pr in _grantsectionRepository.GetAll() on r.SectionId equals pr.Id
                             where g.ApplicationID.ToLower() == id.Trim().ToLower()
                             && m.Code.ToUpper() == "SF"
                             && g.ShortFallLevel == a.ProcessLevel && g.IsShortFallCompleted == false
                             select new
                             {
                                 Grant = g,
                                 Payment = p,
                                 Village = v,
                                 Tehsil = t,
                                 SubDivision = s,
                                 Division = d
                             }
                         ).FirstOrDefault();
                if (grant != null)
                {
                    string startDateStr = string.Format("{0:dd/MM/yyyy}", grant.Grant.ShortFallReportedOn);
                    string endDateStr = string.Format("{0:dd/MM/yyyy}", DateTime.Now);
                    DateTime startDate = ParseDate(startDateStr);
                    DateTime endDate = ParseDate(endDateStr);
                    double businessDays = CalculateBusinessDays(startDate, endDate);
                    double totalDays = (await _repoDaysCheckMaster.FindAsync(x => x.Code == "SF")).FirstOrDefault().NoOfDays;
                    if (businessDays <= totalDays)
                    {
                        if (grant.Grant.IsShortFallCompleted == false)
                        {
                            var projectType = _projectTypeRepo.GetAll();
                            var nocPermission = _nocPermissionTypeRepo.GetAll();
                            var nocType = _nocTypeRepo.GetAll();
                            var ownerType = _ownerTypeRepo.GetAll();
                            var siteUnits = _siteUnitsRepo.GetAll();
                            var villages = _villageRpo.Find(x => x.TehsilBlockId == grant.Village.TehsilBlockId);
                            var tehsils = _tehsilBlockRepo.Find(x => x.SubDivisionId == grant.Tehsil.SubDivisionId);
                            var subdivisions = _subDivisionRepo.Find(x => x.DivisionId == grant.SubDivision.DivisionId);
                            var divisions = _divisionRepo.GetAll();
                            var khasras = _khasraRepo.Find(x => x.GrantID == grant.Grant.Id);
                            var owners = (from o in _grantOwnersRepo.GetAll()
                                          join types in _ownerTypeRepo.GetAll() on o.OwnerTypeId equals types.Id
                                          where o.GrantId == grant.Grant.Id
                                          select new
                                          {
                                              Owner = o,
                                              OwnerType = types
                                          }
                                          ).ToList();


                            List<GrantKhasraViewModelCreate> khasralist = new List<GrantKhasraViewModelCreate>();
                            List<OwnerViewModelCreate> ownerlist = new List<OwnerViewModelCreate>();
                            List<OwnerTypeDetails> ownertype = _ownerTypeRepo.GetAll().ToList();
                            int count = 0;
                            foreach (var item in owners)
                            {
                                ownerlist.Add(new OwnerViewModelCreate
                                {
                                    OId = item.Owner.Id,
                                    SelectedOwnerTypeID = item.Owner.OwnerTypeId,
                                    RowId = ++count,
                                    OwnerType = new SelectList(ownertype, "Id", "Name", item.Owner.OwnerTypeId),
                                    Address = item.Owner.Address,
                                    Email = item.Owner.Email,
                                    MobileNo = item.Owner.MobileNo,
                                    Name = item.Owner.Name,
                                    OwnerTypeName = item.OwnerType.Name
                                });
                            }
                            double totalArea = 0;
                            count = 0;
                            foreach (GrantKhasraDetails item in khasras)
                            {
                                SiteUnitsViewModel unitDetails = new SiteUnitsViewModel
                                {
                                    KanalOrBigha = item.KanalOrBigha,
                                    MarlaOrBiswa = item.MarlaOrBiswa,
                                    SarsaiOrBiswansi = item.SarsaiOrBiswansi,
                                    SiteUnitId = grant.Grant.SiteAreaUnitId
                                };
                                var units = await _calculations.CalculateUnits(unitDetails);
                                totalArea = Math.Round(totalArea + units.KanalOrBigha + units.MarlaOrBiswa + units.SarsaiOrBiswansi, 4);
                                khasralist.Add(new GrantKhasraViewModelCreate { RowId = ++count, KId = item.Id, KanalOrBigha = item.KanalOrBigha, KhasraNo = item.KhasraNo, MarlaOrBiswa = item.MarlaOrBiswa, SarsaiOrBiswansi = item.SarsaiOrBiswansi });
                            }
                            List<SiteUnitMaster> unitMaster = (await _repoSiteUnitMaster.FindAsync(x => x.SiteAreaUnitId == grant.Grant.SiteAreaUnitId)).ToList();
                            var KanalOrBigha = unitMaster.Find(x => x.UnitCode.ToUpper() == "K");
                            var MarlaOrBiswa = unitMaster.Find(x => x.UnitCode.ToUpper() == "M");
                            var SarsaiOrBiswansi = unitMaster.Find(x => x.UnitCode.ToUpper() == "S");
                            GrantSections sections = new GrantSections();
                            var sectiondetail = (from p in _grantsectionRepository.GetAll()
                                                 join r in _grantrejectionRepository.GetAll() on p.Id equals r.SectionId
                                                 join a in _repoApprovalDetail.GetAll() on r.GrantApprovalId equals a.Id
                                                 join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                                                 where r.GrantApprovalId == a.Id && m.Code.ToUpper() == "SF" && a.GrantID == grant.Grant.Id && grant.Grant.ShortFallLevel == a.ProcessLevel
                                                 select new
                                                 {
                                                     project = p,
                                                     rejectedSection = r
                                                 }).ToList();
                            sections.project = 0;
                            sections.address = 0;
                            sections.kml = 0;
                            sections.applicant = 0;
                            sections.owner = 0;
                            sections.permission = 0;
                            foreach (var item in sectiondetail)
                            {
                                if (item.project.SectionCode.ToUpper() == "P")
                                {
                                    sections.project = 1;
                                    sections.isprojectCompleted = item.rejectedSection.IsCompleted;
                                    sections.projectid = item.rejectedSection.Id;
                                    sections.isPActive = "active"; sections.isADActive = "";
                                }
                                else if (item.project.SectionCode.ToUpper() == "AD")
                                {
                                    sections.address = 1;
                                    sections.isaddressCompleted = item.rejectedSection.IsCompleted;
                                    sections.addressid = item.rejectedSection.Id;
                                    sections.isADActive = sections.isPActive == "active" ? "" : "active";
                                }
                                else if (item.project.SectionCode.ToUpper() == "K")
                                {
                                    sections.kml = 1;
                                    sections.iskmlCompleted = item.rejectedSection.IsCompleted;
                                    sections.kmlid = item.rejectedSection.Id;
                                    sections.isKMLActive = sections.isPActive == "active" || sections.isADActive == "active" ? "" : "active";
                                }
                                else if (item.project.SectionCode.ToUpper() == "AP")
                                {
                                    sections.applicant = 1;
                                    sections.isapplicantCompleted = item.rejectedSection.IsCompleted;
                                    sections.applicantid = item.rejectedSection.Id;
                                    sections.isAPActive = sections.isPActive == "active" || sections.isADActive == "active" || sections.isKMLActive == "active" || sections.isPrActive == "active" ? "" : "active";
                                }
                                else if (item.project.SectionCode.ToUpper() == "OW")
                                {
                                    sections.owner = 1;
                                    sections.isownerCompleted = item.rejectedSection.IsCompleted;
                                    sections.ownerid = item.rejectedSection.Id;
                                    sections.isOActive = sections.isPActive == "active" || sections.isADActive == "active" || sections.isKMLActive == "active" || sections.isAPActive == "active" || sections.isPrActive == "active" ? "" : "active";
                                }
                                else if (item.project.SectionCode.ToUpper() == "PM")
                                {
                                    sections.permission = 1;
                                    sections.ispermissionCompleted = item.rejectedSection.IsCompleted;
                                    sections.permissionid = item.rejectedSection.Id;
                                    sections.isPrActive = sections.isPActive == "active" || sections.isADActive == "active" || sections.isKMLActive == "active" ? "" : "active";
                                }
                            }
                            int totalCompleted = (from g in _repo.GetAll()
                                                  join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                                                  join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                                                  join r in _grantrejectionRepository.GetAll() on a.Id equals r.GrantApprovalId
                                                  where g.ShortFallLevel == a.ProcessLevel && m.Code == "SF" && r.IsCompleted == 0 && g.Id == grant.Grant.Id
                                                  select new { rjectionid = r.Id }).Count();
                            var viewModel = new GrantViewModelEdit
                            {
                                Divisions = new SelectList(divisions, "Id", "Name", grant.Division.Id),
                                ProjectType = new SelectList(projectType, "Id", "Name", grant.Grant.ProjectTypeId),
                                NocPermissionType = new SelectList(nocPermission, "Id", "Name", grant.Grant.NocPermissionTypeID),
                                NocType = new SelectList(nocType, "Id", "Name", grant.Grant.NocTypeId),
                                Village = new SelectList(villages, "Id", "Name", grant.Village.Id),
                                TehsilBlock = new SelectList(tehsils, "Id", "Name", grant.Tehsil.Id),
                                SubDivision = new SelectList(subdivisions, "Id", "Name", grant.SubDivision.Id),
                                SiteAreaUnit = new SelectList(siteUnits, "Id", "Name"),
                                GrantKhasras = khasralist,
                                Owners = ownerlist,
                                SelectedSiteAreaUnitId = grant.Grant.SiteAreaUnitId,
                                IsOtherTypeSelected = 0,
                                IsConfirmed = false,
                                IsExtension = 0,
                                TotalArea = totalArea.ToString("#.####"),
                                TotalAreaSqFeet = (totalArea * 43560).ToString("#.####"),
                                TotalAreaSqMetre = (totalArea * 4046.86).ToString("#.####"),
                                Name = grant.Grant.Name,
                                ApplicantEmailID = grant.Grant.ApplicantEmailID,
                                ApplicantName = grant.Grant.ApplicantName,
                                ApplicationID = grant.Grant.ApplicationID,
                                Hadbast = grant.Grant.Hadbast,
                                Id = grant.Grant.Id,
                                PId = grant.Grant.Id,
                                AdId = grant.Grant.Id,
                                KId = grant.Grant.Id,
                                FGrantId = grant.Grant.Id,
                                KmlGrantId = grant.Grant.Id,
                                PermisionGrantId = grant.Grant.Id,
                                OwnerGrantId = grant.Grant.Id,
                                ApplicantGrantId = grant.Grant.Id,
                                KmlLinkName = grant.Grant.KMLLinkName,
                                NocNumber = grant.Grant.NocNumber,
                                OtherProjectTypeDetail = grant.Grant.OtherProjectTypeDetail,
                                Pincode = grant.Village.PinCode.ToString(),
                                PlotNo = grant.Grant.PlotNo,
                                PreviousDate = grant.Grant.PreviousDate,
                                OwnerType = new SelectList(ownertype, "Id", "Name"),
                                SelectedDivisionId = grant.Division.Id,
                                SelectedNocPermissionTypeID = grant.Grant.NocPermissionTypeID,
                                SelectedNocTypeId = grant.Grant.NocTypeId,
                                SelectedProjectTypeId = grant.Grant.ProjectTypeId,
                                SelectedSubDivisionId = grant.SubDivision.Id,
                                SelectedTehsilBlockId = grant.Tehsil.Id,
                                SelectedVillageID = grant.Village.Id,
                                AddressProofPhotoPath = grant.Grant.AddressProofPhotoPath,
                                AuthorizationLetterPhotoPath = grant.Grant.AuthorizationLetterPhotoPath,
                                IDProofPhotoPath = grant.Grant.IDProofPhotoPath,
                                KMLFilePath = grant.Grant.KMLFilePath,
                                KUnitValue = KanalOrBigha.UnitValue,
                                KTimesof = KanalOrBigha.Timesof,
                                KDivideBy = KanalOrBigha.DivideBy,
                                MUnitValue = MarlaOrBiswa.UnitValue,
                                MTimesof = MarlaOrBiswa.Timesof,
                                MDivideBy = MarlaOrBiswa.DivideBy,
                                SUnitValue = SarsaiOrBiswansi.UnitValue,
                                STimesof = SarsaiOrBiswansi.Timesof,
                                SDivideBy = SarsaiOrBiswansi.DivideBy,
                                GrantSections = sections,
                                AreAllSectionCompleted = totalCompleted
                            };
                            if (viewModel != null && sectiondetail.Count > 0)
                                return (viewModel);
                            
                        }
                        
                    }
                    
                }
                
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }



        public IActionResult UnAuthorized()
        {
            return View();
        }

        private DateTime ParseDate(string dateStr)
        {

            // Parse date in dd/MM/yyyy format
            return DateTime.ParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        private double CalculateBusinessDays(DateTime start, DateTime end)
        {
            if (start > end)
                throw new ArgumentException("Start date must be before end date");

            int businessDayCount = 0;

            // Loop through each day in the date range
            for (DateTime current = start; current <= end; current = current.AddDays(1))
            {
                // Check if the current day is not a weekend
                if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDayCount++;
                }
            }

            return businessDayCount;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Modify(GrantFinalSubmitViewModelEdit model)
        {
            var ViewMode = (await ModifyResult(model.FApplicationId));
            try
            {
                if (model != null)
                {
                    var grantDetail = await _repo.GetByIdAsync(model.FGrantId);
                    if (grantDetail == null)
                    {
                        ModelState.AddModelError("", $"Invalid grant detail");
                        return View(ViewMode);

                    }
                    bool isValid = true;
                    if (!model.IsConfirmed)
                    {
                        isValid = false;

                        ModelState.AddModelError("", $"Please check checkbox");
                        return View(ViewMode);

                        //return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    }
                    if (isValid)
                    {
                        grantDetail.IsForwarded = grantDetail.ProcessLevel == 0 ? false : true;
                        grantDetail.IsShortFallCompleted = true;
                        grantDetail.IsShortFall = false;
                        grantDetail.ShortFallCompletedOn = DateTime.Now;
                        await _repo.UpdateAsync(grantDetail);
                        var emailModel = new EmailModel(grantDetail.ApplicantEmailID, "Grant Application Status", EmailBody.EmailStringBodyForGrantUpdateMessage(grantDetail.ApplicantName, grantDetail.ApplicationID));
                        _emailService.SendEmail(emailModel, "Department of Water Resources, Punjab");
                        return RedirectToAction("UpdateIndex", "Grant", new { Id = grantDetail.ApplicationID });
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"All fields are required");
                }
                return View(ViewMode);


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View(ViewMode);


        }

        [HttpPost]
        public async Task<IActionResult> ModifyProject(GrantProjectViewModelEdit model)
        {
            try
            {
                if (model != null)
                {
                    var grantDetail = await _repo.GetByIdAsync(model.PId);
                    if (grantDetail == null)
                    {
                        ModelState.AddModelError("", $"Invalid grant detail");

                        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                    }
                    GrantRejectionShortfallSection grantrejctionSectionDetail = await _grantrejectionRepository.GetByIdAsync(model.projectid);

                    bool isValid = true;
                    var projectType = _projectTypeRepo.GetAll();
                    model.ProjectType = new SelectList(projectType, "Id", "Name");
                    if (model.Name == null || model.Name.Trim() == "")
                    {
                        isValid = false;

                        ModelState.AddModelError("", $"Project name field is required to fill");

                        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    }
                    if (model.IsOtherTypeSelected == 1)
                    {
                        if (model.OtherProjectTypeDetail == null)
                        {
                            isValid = false;
                            ModelState.AddModelError("", $"Other Detail is required to fill");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                            //return RedirectToAction("Modify", "Grant", new { id = grantDetail.ApplicationID });
                        }
                    }
                    if (isValid)
                    {
                        grantDetail.Name = model.Name;
                        grantDetail.ProjectTypeId = model.SelectedProjectTypeId ?? 0;
                        grantDetail.OtherProjectTypeDetail = model.OtherProjectTypeDetail;
                        await _repo.UpdateAsync(grantDetail);
                        grantrejctionSectionDetail.IsCompleted = 1;
                        grantrejctionSectionDetail.UpdatedOn = DateTime.Now;
                        await _grantrejectionRepository.UpdateAsync(grantrejctionSectionDetail); 
                        int totalCompleted = (from g in _repo.GetAll()
                                                join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                                                join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                                                join r in _grantrejectionRepository.GetAll() on a.Id equals r.GrantApprovalId
                                                where g.ShortFallLevel == a.ProcessLevel && m.Code == "SF" && r.IsCompleted == 0 && g.Id == model.PId
                                              select new { rjectionid = r.Id }).Count();
                        return Json(new { success = true, completed = totalCompleted });
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"All fields are required");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

        }
        
        [Obsolete]
        [HttpPost("uploadaddressproof")]
        [AllowAnonymous]
        public async Task<IActionResult> ModifyAddressDetail([FromForm] GrantAddressViewModelPostEdit model)
        {
            try
            {
                if (model != null)
                {
                    //var file = Request.Form; // Get the file from the request
                    var grantDetail = await _repo.GetByIdAsync(model.adId);
                    if (grantDetail == null)
                    {
                        ModelState.AddModelError("", $"Invalid grant detail");

                        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                    }
                    GrantRejectionShortfallSection grantrejctionSectionDetail = await _grantrejectionRepository.GetByIdAsync(model.addressid);
                    string ErrorMessage = "";
                    bool isValid = true;

                    if (model.hadbast == null && model.plotNo == null)
                    {
                        isValid = false;

                        ModelState.AddModelError("", $"Atleast one field is required to fill out of Hadbast/Plot No.");

                        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    }

                    if (model.selectedVillageID <= 0)
                    {
                        isValid = false;

                        ModelState.AddModelError("", $"Village field is required to fill");

                        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    }

                    if (model.selectedPlanSanctionAuthorityId <= 0)
                    {
                        isValid = false;

                        ModelState.AddModelError("", $"Plan Sanction Authority field is required to fill");

                        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    }

                    if (model.file == null)
                    {
                            isValid = false;
                            ModelState.AddModelError("", $"Please upload address proof");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    }

                    if (model.layoutPlanFilePhoto == null)
                    {
                        isValid = false;
                        ModelState.AddModelError("", $"Please upload layout plan");
                        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    }

                    if (model.faradFilePoto == null)
                    {
                        isValid = false;
                        ModelState.AddModelError("", $"Please upload farad");
                        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    }
                    if (isValid)
                    {
                        int AddressProofValidation = AllowedCheckExtensions(model.file, "proof");
                        if (AddressProofValidation == 0)
                        {
                            ErrorMessage = $"Invalid address proof file type. Please upload a JPG, PNG, or PDF file";
                            ModelState.AddModelError("", ErrorMessage);
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                        }
                        else if (AddressProofValidation == 2)
                        {
                            ErrorMessage = "Address proof field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                        string uniqueAddressProofFileName = ProcessUploadedFile(model.file, "Address");

                        int LayoutPlanValidation = AllowedCheckExtensions(model.layoutPlanFilePhoto, "proof");
                        if (LayoutPlanValidation == 0)
                        {
                            ErrorMessage = $"Invalid layout plan file type. Please upload a JPG, PNG, or PDF file";
                            ModelState.AddModelError("", ErrorMessage);

                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                        }
                        else if (LayoutPlanValidation == 2)
                        {
                            ErrorMessage = "Layout plan field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }

                        string uniqueLayoutPlanFileName = ProcessUploadedFile(model.layoutPlanFilePhoto, "LayoutPlan");

                        int FaradValidation = AllowedCheckExtensions(model.faradFilePoto, "proof");
                        if (FaradValidation == 0)
                        {
                            ErrorMessage = $"Invalid farad file type. Please upload a JPG, PNG, or PDF file";
                            ModelState.AddModelError("", ErrorMessage);

                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                        }
                        else if (FaradValidation == 2)
                        {
                            ErrorMessage = "Farad field is required";
                            ModelState.AddModelError("", ErrorMessage);
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                        string uniqueFaradFileName = ProcessUploadedFile(model.faradFilePoto, "Farad");

                        grantDetail.Hadbast = model.hadbast;
                        grantDetail.PlotNo = model.plotNo;
                        grantDetail.VillageID = model.selectedVillageID;
                        grantDetail.AddressProofPhotoPath = uniqueAddressProofFileName;
                        grantDetail.PlanSanctionAuthorityId = model.selectedPlanSanctionAuthorityId;
                        grantDetail.LayoutPlanFilePath = uniqueLayoutPlanFileName;
                        grantDetail.FaradFilePath = uniqueFaradFileName;
                        await _repo.UpdateAsync(grantDetail);
                        grantrejctionSectionDetail.IsCompleted = 1;
                        grantrejctionSectionDetail.UpdatedOn = DateTime.Now;
                        await _grantrejectionRepository.UpdateAsync(grantrejctionSectionDetail);

                        var addressproof = fileUploadeSave(model.file, uniqueAddressProofFileName);

                        int totalCompleted = (from g in _repo.GetAll()
                                              join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                                              join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                                              join r in _grantrejectionRepository.GetAll() on a.Id equals r.GrantApprovalId
                                              where g.ShortFallLevel == a.ProcessLevel && m.Code == "SF" && r.IsCompleted == 0 && g.Id == model.adId
                                              select new { rjectionid = r.Id }).Count();
                        string download=$"/Grant/Download?fileName={uniqueAddressProofFileName}";
                        return Json(new { success = true,filepath= download,completed= totalCompleted });
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"All fields are required");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ModifyAreaDetail(GrantKhasraViewModelEdit model)
        {
            try
            {
                if (ModelState.IsValid) {
                    if (model != null)
                    {
                        //var file = Request.Form; // Get the file from the request
                        GrantKhasraDetails grantDetail = new GrantKhasraDetails();
                        List<GrantKhasraDetails> grantKhasraDetail = new List<GrantKhasraDetails>();

                        bool isValid = true;

                        if (model.SelectedSiteAreaUnitId <= 0)
                        {
                            ModelState.AddModelError("", $"Site Area Unit field is required");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }

                        if (model.SarsaiOrBiswansi == 0)
                        {
                            ModelState.AddModelError("", $"Sarsai Or Biswansi field is required");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }

                        if (model.MarlaOrBiswa == 0)
                        {
                            ModelState.AddModelError("", $"Marla Or Biswa field is required");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }

                        if (model.KanalOrBigha == 0)
                        {
                            ModelState.AddModelError("", $"Kanal Or Bigha field is required");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }

                        if (model.KhasraNo == null)
                        {
                            ModelState.AddModelError("", $"Khasra No. field is required");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                        if (isValid)
                        {
                            grantDetail.KhasraNo = model.KhasraNo;
                            grantDetail.KanalOrBigha = model.KanalOrBigha;
                            grantDetail.MarlaOrBiswa = model.MarlaOrBiswa;
                            grantDetail.UnitId = model.SelectedSiteAreaUnitId;
                            grantDetail.SarsaiOrBiswansi = model.SarsaiOrBiswansi;
                            grantDetail.GrantID = model.KId;
                        }
                        if (model.KhasraId == 0) {
                            await _khasraRepo.CreateAsync(grantDetail);
                        }
                        else
                        {
                            grantDetail = await _khasraRepo.GetByIdAsync(model.KhasraId);
                            if (grantDetail == null)
                            {
                                ModelState.AddModelError("", $"Invalid khasra detail");

                                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                            }
                            grantDetail.KhasraNo = model.KhasraNo;
                            grantDetail.KanalOrBigha = model.KanalOrBigha;
                            grantDetail.MarlaOrBiswa = model.MarlaOrBiswa;
                            grantDetail.UnitId = model.SelectedSiteAreaUnitId;
                            grantDetail.SarsaiOrBiswansi = model.SarsaiOrBiswansi;
                            grantDetail.GrantID = model.KId;
                            grantDetail.Id = model.KhasraId;
                            await _khasraRepo.UpdateAsync(grantDetail);
                        }
                        grantKhasraDetail = (await _khasraRepo.FindAsync(x => x.GrantID == model.KId)).ToList();

                        return Json(new { success = true, grantkhasradetail = grantKhasraDetail });
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

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }
        [Obsolete]
        [HttpPost("uploadkml")]
        [AllowAnonymous]
        public async Task<IActionResult> ModifyKMLDetail(GrantKMLViewModelPostEdit model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        var grantDetail = await _repo.GetByIdAsync(model.kmlGrantId);
                        if (grantDetail == null)
                        {
                            ModelState.AddModelError("", $"Invalid grant detail");

                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                        }
                        GrantRejectionShortfallSection grantrejctionSectionDetail = await _grantrejectionRepository.GetByIdAsync(model.kmlid);

                        bool isValid = true;
                        if (model.KmlLinkName == null)
                        {
                            isValid = false;

                            ModelState.AddModelError("", $"KML Link field is required to fill");

                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                        if (model.kmlFile == null)
                        {
                            isValid = false;
                            ModelState.AddModelError("", $"Please upload KML file");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                        if (isValid)
                        {
                            string ErrorMessage = "";
                            int kmlFileValidation = AllowedCheckExtensions(model.kmlFile, "kml");
                            if (kmlFileValidation == 0)
                            {
                                ErrorMessage = $"Invalid KML file type. Please upload a PDF file only";
                                ModelState.AddModelError("", ErrorMessage);
                                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                            }
                            else if (kmlFileValidation == 2)
                            {
                                ErrorMessage = "KML File field is required";
                                ModelState.AddModelError("", ErrorMessage);
                                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                            }
                            string uniqueKmlFileName = ProcessUploadedFile(model.kmlFile, "kml");

                            grantDetail.KMLFilePath = uniqueKmlFileName;
                            grantDetail.KMLLinkName = model.KmlLinkName;
                            await _repo.UpdateAsync(grantDetail);
                            grantrejctionSectionDetail.IsCompleted = 1;
                            grantrejctionSectionDetail.UpdatedOn = DateTime.Now;
                            await _grantrejectionRepository.UpdateAsync(grantrejctionSectionDetail);

                            var kml = fileUploadeSave(model.kmlFile, uniqueKmlFileName);
                            string download = $"/Grant/Download?fileName={uniqueKmlFileName}";
                            int totalCompleted = (from g in _repo.GetAll()
                                                  join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                                                  join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                                                  join r in _grantrejectionRepository.GetAll() on a.Id equals r.GrantApprovalId
                                                  where g.ShortFallLevel == a.ProcessLevel && m.Code == "SF" && r.IsCompleted == 0 && g.Id == model.kmlGrantId
                                                  select new { rjectionid = r.Id }).Count();
                            return Json(new { success = true, filepath = download, completed = totalCompleted });
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

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ModifyPermissionDetail(GrantPermissionViewModelEdit model)
        {
            try
            {
                if (model != null)
                {
                    var grantDetail = await _repo.GetByIdAsync(model.permissionGrantId);
                    if (grantDetail == null)
                    {
                        ModelState.AddModelError("", $"Invalid grant detail");

                        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                    }
                    GrantRejectionShortfallSection grantrejctionSectionDetail = await _grantrejectionRepository.GetByIdAsync(model.permissionid);

                    bool isValid = true;
                    var nocPermission = _nocPermissionTypeRepo.GetAll();
                    var nocType = _nocTypeRepo.GetAll();
                    model.NocPermissionType = new SelectList(nocPermission, "Id", "Name");
                    model.NocType = new SelectList(nocType, "Id", "Name");
                    if (model.SelectedNocPermissionTypeID<=0)
                    {
                        isValid = false;

                        ModelState.AddModelError("", $"NOC permission field is required to fill");

                        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    }
                    if (model.SelectedNocTypeId <= 0)
                    {
                        isValid = false;

                        ModelState.AddModelError("", $"NOC type field is required to fill");

                        return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                    }
                    if (model.IsExtension == 1)
                    {
                        if (model.NocNumber == null) isValid = false;
                        if (model.PreviousDate == null) { isValid = false; }
                        if (!isValid)
                        {
                            ModelState.AddModelError("", $"Both NOC Number and Date both are required to fill");

                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                    }
                    else
                    {
                        model.PreviousDate = null;
                        model.NocNumber = null;
                    }
                    if (isValid)
                    {
                        grantDetail.NocPermissionTypeID = model.SelectedNocPermissionTypeID;
                        grantDetail.NocTypeId = model.SelectedNocTypeId;
                        grantDetail.NocNumber = model.NocNumber;
                        grantDetail.PreviousDate = model.PreviousDate;
                        await _repo.UpdateAsync(grantDetail);
                        grantrejctionSectionDetail.IsCompleted = 1;
                        grantrejctionSectionDetail.UpdatedOn = DateTime.Now;
                        await _grantrejectionRepository.UpdateAsync(grantrejctionSectionDetail);
                        int totalCompleted = (from g in _repo.GetAll()
                                              join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                                              join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                                              join r in _grantrejectionRepository.GetAll() on a.Id equals r.GrantApprovalId
                                              where g.ShortFallLevel == a.ProcessLevel && m.Code == "SF" && r.IsCompleted == 0 && g.Id== model.permissionGrantId
                                              select new { rjectionid = r.Id }).Count();
                        return Json(new { success = true, completed = totalCompleted });
                    }
                }
                else
                {
                    ModelState.AddModelError("", $"All fields are required");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [Obsolete]
        [HttpPost("uploadapplicant")]
        [AllowAnonymous]
        public async Task<IActionResult> ModifyApplicantDetail(GrantApplicantViewModelPostEdit model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        var grantDetail = await _repo.GetByIdAsync(model.applicantGrantId);
                        if (grantDetail == null)
                        {
                            ModelState.AddModelError("", $"Invalid grant detail");

                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                        }

                        var user = await _repo.GetAll().AnyAsync(x => x.ApplicantEmailID == model.ApplicantEmailID && x.Id != model.applicantGrantId && x.IsRejected != true);
                        if (user)
                        {
                            ModelState.AddModelError("", $"Email {model.ApplicantEmailID} is already in use");

                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                        
                        GrantRejectionShortfallSection grantrejctionSectionDetail = await _grantrejectionRepository.GetByIdAsync(model.applicantid);

                        bool isValid = true;
                        if (model.ApplicantName == null)
                        {
                            isValid = false;

                            ModelState.AddModelError("", $"Applicant name field is required to fill");

                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                        if (model.ApplicantEmailID == null)
                        {
                            isValid = false;

                            ModelState.AddModelError("", $"Applicant emailid field is required to fill");

                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                        if (model.idProofPhotoFile == null)
                        {
                            isValid = false;
                            ModelState.AddModelError("", $"Please upload identity proof file");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                        if (model.authorizationLetterPhotofile == null)
                        {
                            isValid = false;
                            ModelState.AddModelError("", $"Please upload authorization letter file");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                        if (isValid)
                        {
                            string ErrorMessage = "";
                            int IdProofValidation = AllowedCheckExtensions(model.idProofPhotoFile, "proof");
                            int AuthorizationValidation = AllowedCheckExtensions(model.authorizationLetterPhotofile, "proof");
                            if (IdProofValidation == 0)
                            {
                                ErrorMessage = $"Invalid ID proof file type. Please upload a JPG, PNG, or PDF file";
                                ModelState.AddModelError("", ErrorMessage);

                                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                            }
                            else if (IdProofValidation == 2)
                            {
                                ErrorMessage = "ID proof field is required";
                                ModelState.AddModelError("", ErrorMessage);
                                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                            }
                            if (AuthorizationValidation == 0)
                            {
                                ErrorMessage = $"Invalid authorization letter file type. Please upload a JPG, PNG, or PDF file";
                                ModelState.AddModelError("", ErrorMessage);
                                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                            }
                            else if (AuthorizationValidation == 2)
                            {
                                ErrorMessage = "Authorization letter field is required";
                                ModelState.AddModelError("", ErrorMessage);
                                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                            }
                            string uniqueIDProofFileName = ProcessUploadedFile(model.idProofPhotoFile, "IDProof");
                            string uniqueAuthLetterFileName = ProcessUploadedFile(model.authorizationLetterPhotofile, "AuthLetter");

                            grantDetail.IDProofPhotoPath = uniqueIDProofFileName;
                            grantDetail.AuthorizationLetterPhotoPath = uniqueAuthLetterFileName;
                            grantDetail.ApplicantName = model.ApplicantName;
                            grantDetail.ApplicantEmailID = model.ApplicantEmailID;
                            await _repo.UpdateAsync(grantDetail);
                            grantrejctionSectionDetail.IsCompleted = 1;
                            grantrejctionSectionDetail.UpdatedOn = DateTime.Now;
                            await _grantrejectionRepository.UpdateAsync(grantrejctionSectionDetail);

                            var idproof = fileUploadeSave(model.idProofPhotoFile, uniqueIDProofFileName);

                            var authletter = fileUploadeSave(model.authorizationLetterPhotofile, uniqueAuthLetterFileName);

                            string downloadid = $"/Grant/Download?fileName={uniqueIDProofFileName}";
                            string downloadauth = $"/Grant/Download?fileName={uniqueAuthLetterFileName}";
                            int totalCompleted = (from g in _repo.GetAll()
                                                  join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                                                  join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                                                  join r in _grantrejectionRepository.GetAll() on a.Id equals r.GrantApprovalId
                                                  where g.ShortFallLevel == a.ProcessLevel && m.Code == "SF" && r.IsCompleted == 0 && g.Id == model.applicantGrantId
                                                  select new { rjectionid = r.Id }).Count();
                            return Json(new { success = true, idfilePath = downloadid, authfilePath = downloadauth, completed = totalCompleted });
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

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ModifyOwnerDetail(GrantOwnersViewModelEdit model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (model != null)
                    {
                        //var file = Request.Form; // Get the file from the request
                        OwnerDetails grantDetail = new OwnerDetails();
                        List<GrantOwnersViewModelEdit> grantownerdetail = new List<GrantOwnersViewModelEdit>();

                        bool isValid = true;

                        if (model.SelectedOwnerTypeID <= 0)
                        {
                            ModelState.AddModelError("", $"Owner type field is required");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }

                        if (model.OwnerName == null)
                        {
                            ModelState.AddModelError("", $"Owner name field is required");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }

                        if (model.OwnerEmail == null)
                        {
                            ModelState.AddModelError("", $"Owner email field is required");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }

                        if (model.OwnerMobileNo == null)
                        {
                            ModelState.AddModelError("", $"Owner mobile no. field is required");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }

                        if (model.OwnerAddress == null)
                        {
                            ModelState.AddModelError("", $"Owner address field is required");
                            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
                        }
                        GrantRejectionShortfallSection grantrejctionSectionDetail = await _grantrejectionRepository.GetByIdAsync(model.ownersecid);
                        if (isValid)
                        {
                            grantDetail.OwnerTypeId = model.SelectedOwnerTypeID;
                            grantDetail.Name = model.OwnerName;
                            grantDetail.MobileNo = model.OwnerMobileNo;
                            grantDetail.Address = model.OwnerAddress;
                            grantDetail.Email = model.OwnerEmail;
                            grantDetail.GrantId = model.OwnerGrantId;
                        }
                        if (model.OwnerId == 0)
                        {
                            await _grantOwnersRepo.CreateAsync(grantDetail);
                        }
                        else
                        {
                            grantDetail = await _grantOwnersRepo.GetByIdAsync(model.OwnerId);
                            if (grantDetail == null)
                            {
                                ModelState.AddModelError("", $"Invalid khasra detail");

                                return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });

                            }
                            grantDetail.OwnerTypeId = model.SelectedOwnerTypeID;
                            grantDetail.Name = model.OwnerName;
                            grantDetail.MobileNo = model.OwnerMobileNo;
                            grantDetail.Address = model.OwnerAddress;
                            grantDetail.Email = model.OwnerEmail;
                            grantDetail.GrantId = model.OwnerGrantId;
                            grantDetail.Id = model.OwnerId;
                            await _grantOwnersRepo.UpdateAsync(grantDetail);
                        }
                        grantrejctionSectionDetail.IsCompleted = 1;
                        grantrejctionSectionDetail.UpdatedOn = DateTime.Now;
                        await _grantrejectionRepository.UpdateAsync(grantrejctionSectionDetail);
                        var detail = (from o in _grantOwnersRepo.GetAll()
                                      join t in _ownerTypeRepo.GetAll() on o.OwnerTypeId equals t.Id
                                      where o.GrantId == model.OwnerGrantId
                                      select new
                                      {
                                          OwnerDetail=o,
                                          OwnerType=t
                                      }).ToList();
                        foreach (var item in detail)
                        {
                            grantownerdetail.Add(new GrantOwnersViewModelEdit
                            {
                                SelectedOwnerTypeID = item.OwnerDetail.OwnerTypeId,
                                OwnerAddress=item.OwnerDetail.Address,
                                OwnerEmail=item.OwnerDetail.Email,
                                OwnerGrantId=item.OwnerDetail.GrantId,
                                OwnerId=item.OwnerDetail.Id,
                                OwnerMobileNo=item.OwnerDetail.MobileNo,
                                OwnerName=item.OwnerDetail.Name,
                                OwnerTypeName=item.OwnerType.Name
                            });
                        }
                        int totalCompleted = (from g in _repo.GetAll()
                                              join a in _repoApprovalDetail.GetAll() on g.Id equals a.GrantID
                                              join m in _repoApprovalMaster.GetAll() on a.ApprovalID equals m.Id
                                              join r in _grantrejectionRepository.GetAll() on a.Id equals r.GrantApprovalId
                                              where g.ShortFallLevel == a.ProcessLevel && m.Code == "SF" && r.IsCompleted == 0 && g.Id == model.OwnerGrantId
                                              select new { rjectionid = r.Id }).Count();
                        return Json(new { success = true, grantownerdetail = grantownerdetail, completed = totalCompleted });
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

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        public IActionResult Expired()
        {
            return View();
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
        private string ProcessUploadedFile(IFormFile file, string prefixName)
        {
            string uniqueFileName = null;
            if (file != null && file.Length > 0)
            {

                string uploadFolder = Path.Combine(_hostingEnvironment.WebRootPath, "Documents");
                uniqueFileName = prefixName + "_" + Guid.NewGuid().ToString() + "_" + file.FileName;
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

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailExists(string applicantEmailID)
        {
            var user = await _repo.GetAll().AnyAsync(x => x.ApplicantEmailID == applicantEmailID && x.IsRejected != true);

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
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetUnitsDetail(int unitId)
        {
            var obj = await _repoSiteUnitMaster.FindAsync(x => x.SiteAreaUnitId == unitId);
            return Json(obj);
        }
    }
}
