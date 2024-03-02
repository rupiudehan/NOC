using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250,ErrorMessage ="Name cannot exceed 250 characters")]
        public string Name { get; set; }
        [Required]
        public Dept? Department { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        [Display(Name = "Office Email")]
        public string Email { get; set; }
        [Required]
        public string PhotoPath { get; set; }
    }
}
