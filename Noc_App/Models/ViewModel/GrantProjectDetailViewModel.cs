using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantProjectDetailViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "Name cannot exceed 250 characters")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Project Type")]
        public int? SelectedProjectTypeId { get; set; }
    }
}
