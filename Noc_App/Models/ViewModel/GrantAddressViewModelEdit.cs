using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantAddressViewModelPostEdit
    {
        public string addressApplicationId { get; set; }
        public int adId { get; set; }
        [MaxLength(50, ErrorMessage = "Hadbast cannot exceed 50 characters")]
        public string? hadbast { get; set; }
        [MaxLength(50, ErrorMessage = "Plot No. cannot exceed 50 characters")]
        [Display(Name = "Plot No.")]
        public string? plotNo { get; set; }
        [Display(Name = "Village/Town/City")]
        public int selectedVillageID { get; set; }
        [Display(Name = "Address Proof")]
        public IFormFile file { get; set; }
        public int addressid { get; set; }
        [Display(Name = "Plan Sanction Authority")]
        public int selectedPlanSanctionAuthorityId { get; set; }
        [Display(Name = "Layout Plan")]
        public IFormFile layoutPlanFilePhoto { get; set; }
        [Display(Name = "Farad")]
        public IFormFile faradFilePoto { get; set; }
    }
}
