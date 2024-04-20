using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class ForwardApplicationViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ApplicationID { get; set; }
        public string ApplicantName { get; set; }
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
        [Required]
        [Display(Name = "Officer")]
        public string SelectedOfficerId { get; set; }
        public IEnumerable<SelectListItem> Officers { get; set; }
        public string LocationDetails { get; set; }
    }
}
