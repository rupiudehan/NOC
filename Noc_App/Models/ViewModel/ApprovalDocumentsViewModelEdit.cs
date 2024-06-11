using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class ApprovalDocumentsViewModelEdit
    {
        [Required]
        public long GrantApprovalDocId { get; set; }

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
        [Required]
        [Display(Name = "KML File Report")]
        public IFormFile KmlFile { get; set; }
        [Required]
        [Display(Name = "Cross-Section/Calculation Sheets")]
        public IFormFile CrossSectionOrCalculationFile { get; set; }
        [Required]
        [Display(Name = "L-Section of the Drain if Sanctioned")]
        public IFormFile LSectionOfDrainFile { get; set; }

        [Display(Name = "Site Condition Report")]
        public string SiteConditionReportFilePath { get; set; }
        
        [Display(Name = "Catchment Area & Flow")]
        public string CatchmentAreaFilePath { get; set; }
        [Required]
        [Display(Name = "Distance From the Creek")]
        public string DistanceFromCreekFilePath { get; set; }
        [Required]
        [Display(Name = "GIS Report/DWS Report")]
        public string GisOrDwsFilePath { get; set; }
        [Required]
        [Display(Name = "KML File Report")]
        public string KmlFilePath { get; set; }
        [Required]
        [Display(Name = "Cross-Section/Calculation Sheets")]
        public string CrossSectionOrCalculationFilePath { get; set; }
        [Required]
        [Display(Name = "L-Section of the Drain if Sanctioned")]
        public string LSectionOfDrainFilePath { get; set; }
    }
}
