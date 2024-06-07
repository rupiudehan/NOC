using Microsoft.AspNetCore.Mvc.Rendering;
using Noc_App.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantKhasraViewModelEdit
    {
        public string AreaApplicationId { get; set; }
        public int KId { get; set; }
        public int KhasraId { get; set; }
        [Required]
        [Display(Name = "Khasra No.")]
        [MaxLength(50, ErrorMessage = "Khasra cannot exceed 50 characters")]
        public string KhasraNo { get; set; }
        [Required]
        [Display(Name = "Site Area Unit")]
        public int SelectedSiteAreaUnitId { get; set; }
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        [Required]
        [Display(Name = "Marla/Biswa")]
        [NumericValidation(typeof(double))]
        public double MarlaOrBiswa { get; set; }
        [Display(Name = "Kanal/Bigha")]
        [NumericValidation(typeof(double))]
        [Required]
        public double KanalOrBigha { get; set; }
        [Required]
        [Display(Name = "Sarsai/Biswansi")]
        [NumericValidation(typeof(double))]
        public double SarsaiOrBiswansi { get; set; }
    }
}
