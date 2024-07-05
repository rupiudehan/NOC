using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class ForwardApplicationViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ApplicationID { get; set; }
        public string ApplicantName { get; set; }
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
        public string ForwardToRole { get; set; }
        [Display(Name = "Sub-Division")]
        public string SelectedSubDivisionId { get; set; }
        public IEnumerable<SelectListItem> SubDivisions { get; set; }
        [Required]
        [Display(Name = "Officer")]
        public string SelectedOfficerId { get; set; }
        public IEnumerable<SelectListItem> Officers { get; set; }
        public string LocationDetails { get; set; }
        public double TotalArea { get; set; }
        public string LoggedInRole { get; set; }
        [Required]
        [Display(Name = "Site Condition Report")]
        public IFormFile SiteConditionReportFile { get; set; }
        [Required]
        [Display(Name = "Catchment Area & Flow")]
        public IFormFile CatchmentAreaFile { get; set; }
        [Required]
        [Display(Name = "Distance From the Creek")]
        public IFormFile DistanceFromCreekFile { get; set; }
        [Required]
        [Display(Name = "GIS Report/DWS Report")]
        public IFormFile GisOrDwsFile { get; set; }
        [Display(Name = "Is KML Provided by Applicant Valid?")]
        public bool IsKMLByApplicantValid { get; set; }
        //[Required]
        //[Display(Name = "KML File Report")]
        //public IFormFile KmlFile { get; set; }
        [Required]
        [Display(Name = "Cross-Section/Calculation Sheets")]
        public IFormFile CrossSectionOrCalculationFile { get; set; }
        [Required]
        [Display(Name = "L-Section of the Drain if Sanctioned")]
        public IFormFile LSectionOfDrainFile { get; set; }
        public bool IsForwarded { get; set; }
        [Display(Name = "Recommendation")]
        public int SelectedRecommendationId { get; set; }
        public IEnumerable<SelectListItem> Recommendations { get; set; }
        public string Remarks { get; set; }
        [Display(Name = "Is Drain Notified?")]
        public bool IsDrainNotified { get; set; }
        public int TypeOfWidth { get; set; }
        [Required]
        [Display(Name = "Width")]
        public double DrainWidth { get; set; }
    }
}
