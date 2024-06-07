using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantProjectViewModelEdit
    {
        public string ProjectApplicationId { get; set; }
        public int PId { get; set; }
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        public string Name { get; set; }
        [Display(Name = "Project Type")]
        public int? SelectedProjectTypeId { get; set; }
        public IEnumerable<SelectListItem> ProjectType { get; set; }
        [Display(Name = "Specify Other Detail")]
        public string? OtherProjectTypeDetail { get; set; }
        public int IsOtherTypeSelected { get; set; }
        public int projectid { get; set; }
    }
}
