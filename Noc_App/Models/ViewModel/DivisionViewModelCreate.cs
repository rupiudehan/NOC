using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class DivisionViewModelCreate
    {
        [Required]
        [Display(Name = "Name")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string DivisionName { get; set; }
        public bool IsActive { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
