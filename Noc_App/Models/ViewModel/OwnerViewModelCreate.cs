using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class OwnerViewModelCreate
    {
        public int OId { get; set; }
        public int RowId { get; set; }
        public string OwnerTypeName { get; set; }
        [Required]
        [Display(Name = "Owner Type")]
        public int SelectedOwnerTypeID { get; set; }
        public IEnumerable<SelectListItem> OwnerType { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        [MinLength(3, ErrorMessage = "Name cannot be less than 3 characters")]
        public string Name { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        [MinLength(3, ErrorMessage = "Name cannot be less than 3 characters")]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit mobile number.")]
        public string MobileNo { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }
    }
}
