using Microsoft.AspNetCore.Mvc.Rendering;
using Noc_App.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantKhasraViewModelCreate
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Khasra cannot exceed 50 characters")]
        public string KhasraNo { get; set; }
        //public int SelectedUnitId { get; set; }
        public IEnumerable<SelectListItem> Units { get; set; }
        [Display(Name = "Marla/Biswansi")]
        [NumericValidation(typeof(double))]
        public double MarlaOrBiswansi { get; set; }
        [Display(Name = "Kanal/Biswa")]
        [NumericValidation(typeof(double))]
        public double KanalOrBiswa { get; set; }
        [Display(Name = "Sarsai/Bigha")]
        [NumericValidation(typeof(double))]
        public double SarsaiOrBigha { get; set; }
    }
}
