using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantOwnersViewModelEdit
    {
        public string OwnerApplicationId { get; set; }
        public int OwnerGrantId { get; set; }
        public int OwnerId { get; set; }
        [Required]
        [Display(Name = "Owner Type")]
        public int SelectedOwnerTypeID { get; set; }
        public IEnumerable<SelectListItem> OwnerType { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string OwnerName { get; set; }
        [Display(Name = "Owner Type")]
        public string OwnerTypeName { get; set; }
        [Required]
        [Display(Name = "Address")]
        [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string OwnerAddress { get; set; }
        [Required]
        [Display(Name = "Mobile No.")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit mobile number.")]
        public string OwnerMobileNo { get; set; }
        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        public string OwnerEmail { get; set; }
        public int ownersecid { get; set; }
    }
}
