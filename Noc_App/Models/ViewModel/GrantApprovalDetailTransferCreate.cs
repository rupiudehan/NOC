using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantApprovalDetailTransferCreate
    {
        public string id { get; set; }
        [Display(Name = "Sub-Division")]
        public string SelectedSubDivisionId { get; set; }
        public IEnumerable<SelectListItem> SubDivisions { get; set; }
        [Required]
        [Display(Name = "Officer")]
        public string SelectedOfficerId { get; set; }
        public IEnumerable<SelectListItem> Officers { get; set; }
    }
}
