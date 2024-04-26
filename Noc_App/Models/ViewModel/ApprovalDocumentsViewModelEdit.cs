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
    }
}
