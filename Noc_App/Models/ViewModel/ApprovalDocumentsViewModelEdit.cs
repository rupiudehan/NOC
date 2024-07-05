using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class ApprovalDocumentsViewModelEdit
    {
        [Required]
        public long GrantApprovalId { get; set; }
        public long GrantApprovalDocId { get; set; }
        [Display(Name = "Site Condition Report")]
        public IFormFile SiteConditionReportFile { get; set; }
        [Display(Name = "Catchment Area & Flow")]
        public IFormFile CatchmentAreaFile { get; set; }
        [Display(Name = "Distance From the Creek")]
        public IFormFile DistanceFromCreekFile { get; set; }
        [Display(Name = "GIS Report/DWS Report")]
        public IFormFile GisOrDwsFile { get; set; }
        [Display(Name = "Is KML Provided by Applicant Valid?")]
        public bool IsKMLByApplicantValid { get; set; }
        [Display(Name = "Cross-Section/Calculation Sheets")]
        public IFormFile CrossSectionOrCalculationFile { get; set; }
        [Display(Name = "L-Section of the Drain if Sanctioned")]
        public IFormFile LSectionOfDrainFile { get; set; }

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
        [Display(Name = "Is Drain Notified?")]
        public bool IsDrainNotified { get; set; }
        public int TypeOfWidth { get; set; }
        [Display(Name = "Width")]
        public double DrainWidth { get; set; }
    }
}
