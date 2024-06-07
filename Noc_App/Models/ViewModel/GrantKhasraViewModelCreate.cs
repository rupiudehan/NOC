using Microsoft.AspNetCore.Mvc.Rendering;
using Noc_App.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantKhasraViewModelCreate
    {
        public int KId { get; set; }
        public int RowId { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Khasra cannot exceed 50 characters")]
        public string KhasraNo { get; set; }
        //public int SelectedUnitId { get; set; }
        public IEnumerable<SelectListItem> Units { get; set; }
        [Display(Name = "Marla/Biswa")]
        [NumericValidation(typeof(double))]
        public double MarlaOrBiswa { get; set; }
        [Display(Name = "Kanal/Bigha")]
        [NumericValidation(typeof(double))]
        public double KanalOrBigha { get; set; }
        [Display(Name = "Sarsai/Biswansi")]
        [NumericValidation(typeof(double))]
        public double SarsaiOrBiswansi { get; set; }
    }
}
