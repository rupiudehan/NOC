using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class OwnerViewModelCreate
    {
        public int OwnerTypeId { get; set; }
        public IEnumerable<SelectListItem> OwnerType { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string Address { get; set; }
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit mobile number.")]
        public string MobileNo { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }
    }
}
