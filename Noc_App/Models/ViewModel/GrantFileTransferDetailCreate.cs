using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantFileTransferDetailCreate
    {
        public string Name { get; set; }
        public string ApplicationID { get; set; }
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
        [Required]
        [Display(Name = "Officer")]
        public string SelectedOfficerId { get; set; }
        public IEnumerable<SelectListItem> Officers { get; set; }
        public string LocationDetails { get; set; }
        public string CurrentOfficer { get; set; }
        public string Remarks { get; set; }
        [Display(Name = "Division")]
        public string SelectedDivisionId { get; set; }
        public IEnumerable<SelectListItem> Divisions { get; set; }
        public string ForwardToRole { get; set; }
    }
}
