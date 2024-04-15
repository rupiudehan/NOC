using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Noc_App.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantViewModelCreate
    {
        [Required]
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Unit of Site Area")]
        public int SelectedSiteAreaUnitId { get; set; }
        public IEnumerable<SelectListItem> SiteAreaUnit { get; set; }
        public List<GrantKhasraViewModelCreate> GrantKhasras { get; set; }
        [Display(Name = "Total Area")]
        public string TotalArea { get; set; }
        [Display(Name = "Total Area in Sq Feet")]
        public string TotalAreaSqFeet { get; set; }
        [Display(Name = "Total Area in Sq Metre")]
        public string TotalAreaSqMetre { get; set; }
        //[Required]
        //[Display(Name ="Site Size in Feet")]
        //[NumericValidation(typeof(double))]
        //public double SiteAreaOrSizeInFeet { get; set; }
        //[Required]
        //[Display(Name = "Site Size in Inches")]
        //[NumericValidation(typeof(double))]
        //public double SiteAreaOrSizeInInches { get; set; }
        [Required]
        [Display(Name = "Project Type")]
        public int? SelectedProjectTypeId { get; set; }
        public IEnumerable<SelectListItem> ProjectType { get; set; }
        [Display(Name ="Specify Other Detail")]
        public string? OtherProjectTypeDetail { get; set; }
        public int IsOtherTypeSelected { get; set; }
        [MaxLength(50, ErrorMessage = "Hadbast cannot exceed 50 characters")]
        public string? Hadbast { get; set; }
        [MaxLength(50, ErrorMessage = "Plot No. cannot exceed 50 characters")]
        [Display(Name ="Plot No.")]
        public string? PlotNo { get; set; }
        [Required]
        [Display(Name = "Owner Type")]
        //public int? SelectedOwnerTypeID { get; set; }
        public IEnumerable<SelectListItem> OwnerType { get; set; }
        [Display(Name="Pin Code")]
        public string Pincode { get; set; }
        [Required]
        [Display(Name = "Village/Town/City")]
        public int? SelectedVillageID { get; set; }
        public IEnumerable<SelectListItem> Village { get; set; }
        [Required]
        [Display(Name = "Division")]
        public int? SelectedDivisionId { get; set; }
        public IEnumerable<SelectListItem> Divisions { get; set; }
        [Required]
        [Display(Name = "Sub-Division")]
        public int? SelectedSubDivisionId { get; set; }
        public IEnumerable<SelectListItem> SubDivision { get; set; }
        [Required]
        [Display(Name = "Tehsil/Block")]
        public int? SelectedTehsilBlockId { get; set; }
        public IEnumerable<SelectListItem> TehsilBlock { get; set; }
        [Required]
        [Display(Name = "Address Proof")]
        public IFormFile AddressProofPhoto { get; set; }
        [Required]
        public IFormFile KMLFile { get; set; }
        [Required]
        [RegularExpression(@"https:\/\/earth\.google\.com\/web\/\S+", ErrorMessage = "Invalid link")]
        [Display(Name = "KML Link")]
        public string KmlLinkName { get; set; }
        [Required]
        [Display(Name = "Applicant Name")]
        public string ApplicantName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        [Display(Name = "Applicant Email")]
        [Remote(action: "IsEmailExists",controller:"Grant")]
        public string ApplicantEmailID { get; set; }
        [Required]
        [Display(Name = "Identity Proof")]
        public IFormFile IDProofPhoto { get; set; }
        [Required]
        [Display(Name = "Authorization Letter")]
        public IFormFile AuthorizationLetterPhoto { get; set; }
        public List<OwnerViewModelCreate> Owners { get; set; }
        [Required]
        [Display(Name = "NOC Permission Type")]
        public int? SelectedNocPermissionTypeID { get; set; }
        public IEnumerable<SelectListItem> NocPermissionType { get; set; }
        [Required]
        [Display(Name = "NOC Type")]
        public int? SelectedNocTypeId { get; set; }
        public IEnumerable<SelectListItem> NocType { get; set; }
        public int IsExtension { get; set; }
        [Display(Name = "NOC Number")]
        public string? NocNumber { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PreviousDate { get; set; }
        [Required]
        [Display(Name ="Confirm")]
        public bool IsConfirmed { get; set; }
        public bool IsPaymentDone { get; set; }
        public string ApplicationID { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
