using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class RegisterEmployeeViewModel
    {
        [Required]
        [EmailAddress]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        [Remote(action: "IsEmailExists", controller: "Account")]
        public string Email { get; set; }
        //[Required]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }
        //public string RoleName { get; set; }
        public string UserName { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        [Required]
        [Display(Name = "Division")]
        public int SelectedDivisionId { get; set; }
        public IEnumerable<SelectListItem> Divisions { get; set; }
        [Display(Name = "Sub-Division")]
        public int? SelectedSubDivisionId { get; set; }
        public IEnumerable<SelectListItem> SubDivision { get; set; }
        [Display(Name = "Tehsil/Block")]
        public int? SelectedTehsilBlockId { get; set; }
        public IEnumerable<SelectListItem> TehsilBlock { get; set; }
        [Display(Name = "Village")]
        public int? SelectedVillageId { get; set; }
        public IEnumerable<SelectListItem> Village { get; set; }
        //public int? VillageId { get; set; }
        //public int? TehsilBlockId { get; set; }
        //public int? SubDivisionId { get; set; }
        //public int? DivisionId { get; set; }
    }
}
