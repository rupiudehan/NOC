using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class PartiallyApproveViewModelCreate
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ApplicationID { get; set; }
        public string LocationDetails { get; set; }
        public bool IsUnderMasterPlan { get; set; }
        [Required]
        [Display(Name = "Officer")]
        public string SelectedOfficerId { get; set; }
        public IEnumerable<SelectListItem> Officers { get; set; }
        [Display(Name = "Division")]
        public string SelectedDivisionId { get; set; }
        public IEnumerable<SelectListItem> Divisions { get; set; }
        public string Remarks { get; set; }
        public string ForwardToRole { get; set; }
        public string LoggedInRole { get; set; }
        public int FromLocationId { get; set; }
        public int ToLocationId { get; set; }
    }
}
