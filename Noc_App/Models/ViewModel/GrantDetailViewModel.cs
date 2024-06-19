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
        public string TotalAreaSqFeet { get; set; }
        public string TotalAreaSqMetre { get; set; }
        [Display(Name = "Project Type")]
        public string ProjectTypeName { get; set; }
        [Display(Name = "Specify Other Detail")]
        public string OtherProjectTypeDetail { get; set; }
        public string Hadbast { get; set; }
        [Display(Name = "Plot No.")]
        public string PlotNo { get; set; }
        [Display(Name = "Pin Code")]
        public string Pincode { get; set; }
        [Display(Name = "Plan Sanction Authority")]
        public string PlanSanctionAuthorityName { get; set; }
        public string LayoutPlanFilePath { get; set; }
        public string FaradFilePath { get; set; }
        [Display(Name = "Village/Town/City")]
        public string VillageName { get; set; }
        [Display(Name = "Drainage Division")]
        public string DivisionName { get; set; }
        [Display(Name = "Drainage Sub-Division")]
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
        public List<GrantInspectionDocuments> GrantInspectionDocumentsDetail { get; set; }
        public string LocationDetail { get; set; }
        public string Domain { get; set; }
    }

    public class GrantInspectionDocuments
    {
        [Display(Name = "Site Condition Report")]
        public string SiteConditionReportFilePath { get; set; }

        [Display(Name = "Catchment Area & Flow")]
        public string CatchmentAreaFilePath { get; set; }

        [Display(Name = "Distance From the Creek")]
        public string DistanceFromCreekFilePath { get; set; }

        [Display(Name = "GIS Report/DWS Report")]
        public string GisOrDwsFilePath { get; set; }

        [Display(Name = "Cross-Section/Calculation Sheets")]
        public string CrossSectionOrCalculationFilePath { get; set; }

        [Display(Name = "L-Section of the Drain if Sanctioned")]
        public string LSectionOfDrainFilePath { get; set; }
        [Display(Name = "Is KML Provided by Applicant Valid")]
        public bool IsKMLByApplicantValid { get; set; }
        public string UploadedByRole { get; set; }
        public string UploadedByName { get; set; }
    }
}
