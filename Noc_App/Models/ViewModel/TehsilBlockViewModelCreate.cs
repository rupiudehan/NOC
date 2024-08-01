using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class TehsilBlockViewModelCreate
    {
        [Required]
        public int LGD_ID { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "District")]
        public int SelectedDistrictId { get; set; }
        public IEnumerable<SelectListItem> Districts { get; set; }
        [Required]
        [Display(Name = "Division")]
        public int SelectedDivisionId { get; set; }
        public IEnumerable<SelectListItem> Divisions { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
