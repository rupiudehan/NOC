using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class VillageViewModelEdit
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }
        [Required]
        [DataType(DataType.PostalCode)]
        //[MaxLength(6, ErrorMessage = "Please enter a valid 6-digit PIN code.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Please enter a valid 6-digit PIN code.")]
        public int PinCode { get; set; }
        [Required]
        [Display(Name = "District")]
        public int SelectedDistrictId { get; set; }
        public IEnumerable<SelectListItem> Districts { get; set; }
        //[Required]
        //[Display(Name = "Division")]
        //public int SelectedDivisionId { get; set; }
        //public IEnumerable<SelectListItem> Divisions { get; set; }
        //[Required]
        //[Display(Name = "Sub-Division")]
        //public int SelectedSubDivisionId { get; set; }
        //public IEnumerable<SelectListItem> SubDivisions { get; set; }
        [Required]
        [Display(Name = "Tehsil/Block")]
        public int SelectedTehsilBlockId { get; set; }
        public IEnumerable<SelectListItem> TehsilBlock { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
