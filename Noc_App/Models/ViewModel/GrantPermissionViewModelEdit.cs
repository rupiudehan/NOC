using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantPermissionViewModelEdit
    {
        public string permissionApplicationId { get; set; }
        public int permissionGrantId { get; set; }
        [Display(Name = "NOC Permission Type")]
        public int SelectedNocPermissionTypeID { get; set; }
        public IEnumerable<SelectListItem> NocPermissionType { get; set; }
        [Display(Name = "NOC Type")]
        public int SelectedNocTypeId { get; set; }
        public IEnumerable<SelectListItem> NocType { get; set; }
        public int IsExtension { get; set; }
        [Display(Name = "NOC Number")]
        public string? NocNumber { get; set; }
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PreviousDate { get; set; }
        public int permissionid { get; set; }
    }
}
