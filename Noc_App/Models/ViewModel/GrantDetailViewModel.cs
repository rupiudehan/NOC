using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Unit of Site Area")]
        public string SiteAreaUnitName { get; set; }
        public string TotalArea { get; set; }
        [Display(Name = "Project Type")]
        public string ProjectTypeName { get; set; }
        [Display(Name = "Specify Other Detail")]
        public string OtherProjectTypeDetail { get; set; }
        public string Hadbast { get; set; }
        [Display(Name = "Plot No.")]
        public string PlotNo { get; set; }
        [Display(Name = "Pin Code")]
        public string Pincode { get; set; }
        [Display(Name = "Village/Town/City")]
        public string VillageName { get; set; }
        [Display(Name = "Division")]
        public string DivisionName { get; set; }
        [Display(Name = "Sub-Division")]
        public string SubDivisionName { get; set; }
        [Display(Name = "Tehsil/Block")]
        public string TehsilBlockName { get; set; }
        [Display(Name = "Address Proof")]
        public string AddressProofPhotoPath { get; set; }
        public string KMLFilePath { get; set; }
        [Display(Name = "KML Link Name")]
        public string KmlLinkName { get; set; }
        [Display(Name = "Applicant Name")]
        public string ApplicantName { get; set; }
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
        [Display(Name = "Identity Proof")]
        public string IDProofPhotoPath { get; set; }
        [Display(Name = "Authorization Letter")]
        public string AuthorizationLetterPhotoPath { get; set; }
        [Display(Name = "NOC Permission Type")]
        public string NocPermissionTypeName { get; set; }
        [Display(Name = "NOC Type")]
        public string NocTypeName { get; set; }
        [Display(Name = "NOC Number")]
        public string NocNumber { get; set; }
        [Display(Name = "Date")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string PreviousDate { get; set; }
        public string PaymentOrderId { get; set; }
        public string ApplicationID { get; set; }
        public List<OwnerDetails> Owners { get; set; }
        public List<GrantKhasraDetails> Khasras { get; set; }
    }
}
