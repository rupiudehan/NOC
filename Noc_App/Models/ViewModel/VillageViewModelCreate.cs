using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class VillageViewModelCreate
    {
        [Required]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }
        [Required]
        public int PinCode { get; set; }
        [Required]
        [Display(Name = "Division")]
        public int SelectedDivisionId { get; set; }
        public IEnumerable<SelectListItem> Divisions { get; set; }
        [Required]
        [Display(Name = "Sub-Division")]
        public int SelectedSubDivisionId { get; set; }
        public IEnumerable<SelectListItem> SubDivision { get; set; }
        [Required]
        [Display(Name = "Tehsil/Block")]
        public int SelectedTehsilBlockId { get; set; }
        public IEnumerable<SelectListItem> TehsilBlock { get; set; }
        public bool IsActive { get; set; }
        public List<ApplicationUser> CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
