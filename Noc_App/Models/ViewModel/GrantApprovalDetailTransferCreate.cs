using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantApprovalDetailTransferCreate
    {
        public string id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ApplicationID { get; set; }
        public string ApplicantName { get; set; }
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
        public string ForwardToRole { get; set; }
        [Display(Name = "Division")]
        public string SelectedDivisionId { get; set; }
        public IEnumerable<SelectListItem> Divisions { get; set; }
        [Required]
        [Display(Name = "Officer")]
        public string SelectedOfficerId { get; set; }
        public IEnumerable<SelectListItem> Officers { get; set; }
        public string LocationDetails { get; set; }
        public string CurrentOfficer { get; set; }
        public long ApprovalId { get; set; }
    }
}
