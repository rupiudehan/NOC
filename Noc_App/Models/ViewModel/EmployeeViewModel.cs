using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class EmployeeViewModel
    {
        [Required]
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        public string Name { get; set; }
        [Required]
        public Dept? Department { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        [Display(Name = "Office Email")]
        public string Email { get; set; }
        public IFormFile Photo { get; set; }
    }
}
