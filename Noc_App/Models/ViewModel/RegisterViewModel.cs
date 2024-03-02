using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Remote(action: "IsEmailExists",controller:"Account")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password",ErrorMessage ="Password and confirm password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
