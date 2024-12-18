using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class IssueNocViewModelCreate
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ApplicationID { get; set; }
        public string LocationDetails { get; set; }
        //[Required]
        [Display(Name = "Upload Certificate")]
        public IFormFile CertificateFile { get; set; }
        public bool IsUnderMasterPlan { get; set; }
    }
}
