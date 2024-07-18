using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Noc_App.Helpers;

namespace Noc_App.Models.ViewModel
{
    public class GrantViewModelEdit
    {
        public int Id { get; set; }
        public int PId { get; set; }
        public int AdId { get; set; }
        public int KmlGrantId { get; set; }
        public int PermisionGrantId { get; set; }
        public int OwnerGrantId { get; set; }
        public int ApplicantGrantId { get; set; }
        public int KId { get; set; }
        public int FGrantId { get; set; }
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        public string Name { get; set; }
        [Display(Name = "Unit of Site Area")]
        public int SelectedSiteAreaUnitId { get; set; }
        public GrantSections GrantSections { get; set; }
        public IEnumerable<SelectListItem> SiteAreaUnit { get; set; }
        public List<GrantKhasraViewModelCreate> GrantKhasras { get; set; }
        public GrantKhasraViewModelEdit Khasras { get; set; }
        [Display(Name = "Total Area")]
        public string TotalArea { get; set; }
        [Display(Name = "Total Area in Sq Feet")]
        public string TotalAreaSqFeet { get; set; }
        [Display(Name = "Total Area in Sq Metre")]
        public string TotalAreaSqMetre { get; set; }
        [Display(Name = "Project Type")]
        public int? SelectedProjectTypeId { get; set; }
        public IEnumerable<SelectListItem> ProjectType { get; set; }
        [Display(Name = "Specify Other Detail")]
        public string? OtherProjectTypeDetail { get; set; }
        public int IsOtherTypeSelected { get; set; }
        [MaxLength(50, ErrorMessage = "Hadbast cannot exceed 50 characters")]
        public string? Hadbast { get; set; }
        [MaxLength(50, ErrorMessage = "Plot No. cannot exceed 50 characters")]
        [Display(Name = "Plot No.")]
        public string? PlotNo { get; set; }
        [Display(Name = "Pin Code")]
        public string Pincode { get; set; }
        public string VillageName { get; set; }
        //[Display(Name = "Village/Town/City")]
        //public int SelectedVillageID { get; set; }
        //public IEnumerable<SelectListItem> Village { get; set; }
        [Display(Name = "Drainage Division")]
        public int SelectedDivisionId { get; set; }
        public IEnumerable<SelectListItem> Divisions { get; set; }
        [Display(Name = "Drainage Sub-Division")]
        public int SelectedSubDivisionId { get; set; }
        public IEnumerable<SelectListItem> SubDivision { get; set; }
        [Display(Name = "Tehsil/Block")]
        public int SelectedTehsilBlockId { get; set; }
        public IEnumerable<SelectListItem> TehsilBlock { get; set; }
        [Display(Name = "Address Proof")]
        public IFormFile AddressProofPhoto { get; set; }
        public string AddressProofPhotoPath { get; set; }
        [Required]
        [Display(Name = "Plan Sanction Authority")]
        public int SelectedPlanSanctionAuthorityId { get; set; }
        public IEnumerable<SelectListItem> PlanSanctionAuthorityMaster { get; set; }
        [Required]
        [Display(Name = "Layout Plan")]
        public IFormFile LayoutPlanFilePhoto { get; set; }
        [Required]
        [Display(Name = "Farad")]
        public IFormFile FaradFilePoto { get; set; }
        
        public string LayoutPlanFilePath { get; set; }
        
        public string FaradFilePath { get; set; }
        public IFormFile KMLFile { get; set; }
        public string KMLFilePath { get; set; }
        [RegularExpression(@"https:\/\/earth\.google\.com\/web\/\S+", ErrorMessage = "Invalid link")]
        [Display(Name = "KML Link")]
        public string KmlLinkName { get; set; }
        [Display(Name = "Applicant Name")]
        public string ApplicantName { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
        [Display(Name = "Identity Proof")]
        public IFormFile IDProofPhoto { get; set; }
        public string IDProofPhotoPath { get; set; }
        [Display(Name = "Authorization Letter")]
        public IFormFile AuthorizationLetterPhoto { get; set; }
        public string AuthorizationLetterPhotoPath { get; set; }
        public List<OwnerViewModelCreate> Owners { get; set; }
        [Display(Name = "NOC Permission Type")]
        public int? SelectedNocPermissionTypeID { get; set; }
        public IEnumerable<SelectListItem> NocPermissionType { get; set; }
        [Display(Name = "NOC Type")]
        public int? SelectedNocTypeId { get; set; }
        public IEnumerable<SelectListItem> NocType { get; set; }
        public int IsExtension { get; set; }
        [Display(Name = "NOC Number")]
        public string? NocNumber { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? PreviousDate { get; set; }
        [Display(Name = "Confirm")]
        public bool IsConfirmed { get; set; }
        public string ApplicationID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int KhasraId { get; set; }
        [MaxLength(50, ErrorMessage = "Khasra cannot exceed 50 characters")]
        public string KhasraNo { get; set; }
        //public int SelectedUnitId { get; set; }
        public IEnumerable<SelectListItem> Units { get; set; }
        [Display(Name = "Marla/Biswa")]
        [NumericValidation(typeof(double))]
        public double MarlaOrBiswa { get; set; }
        [Display(Name = "Kanal/")]
        [NumericValidation(typeof(double))]
        public double KanalOrBigha { get; set; }
        [Display(Name = "Sarsai/Biswansi")]
        [NumericValidation(typeof(double))]
        public double SarsaiOrBiswansi { get; set; }
        public int OwnerId { get; set; }
        [Display(Name = "Owner Type")]
        public int SelectedOwnerTypeID { get; set; }
        public IEnumerable<SelectListItem> OwnerType { get; set; }
        [Display(Name = "Name")]
        public string OwnerName { get; set; }
        [Display(Name = "Address")]
        [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string OwnerAddress { get; set; }
        [Display(Name = "Mobile No.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit mobile number.")]
        public string OwnerMobileNo { get; set; }
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        public string OwnerEmail { get; set; }
        public double KUnitValue { get; set; }
        public double KTimesof { get; set; }
        public double KDivideBy { get; set; }
        public double MUnitValue { get; set; }
        public double MTimesof { get; set; }
        public double MDivideBy { get; set; }
        public double SUnitValue { get; set; }
        public double STimesof { get; set; }
        public double SDivideBy { get; set; }
        public int AreAllSectionCompleted { get; set; }
    }
}
