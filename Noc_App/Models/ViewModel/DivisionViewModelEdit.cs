using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class DivisionViewModelEdit
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        [MaxLength(150, ErrorMessage = "Division Name cannot exceed 150 characters")]
        public string DivisionName { get; set; }
        public ApplicationUser UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
