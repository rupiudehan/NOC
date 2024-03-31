using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class RegisterEmployeeViewModelEdit
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        [EmailAddress]
        //[Remote(action: "IsEmailUnique", controller: "Account", AdditionalFields = "Id")]
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
        public List<int> SelectedDivisionId { get; set; }
        public IEnumerable<DivisionDetails> Divisions { get; set; }
        [Display(Name = "Sub-Division")]
        public List<int> SelectedSubDivisionId { get; set; }
        public IEnumerable<SubDivisionDetails> SubDivision { get; set; }
        [Display(Name = "Tehsil/Block")]
        public List<int> SelectedTehsilBlockId { get; set; }
        public IEnumerable<TehsilBlockDetails> TehsilBlock { get; set; }
        [Display(Name = "Village")]
        public List<int> SelectedVillageId { get; set; }
        public IEnumerable<VillageDetails> Village { get; set; }
    }
}
