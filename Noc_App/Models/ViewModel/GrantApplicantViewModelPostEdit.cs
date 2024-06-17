using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantApplicantViewModelPostEdit
    {
        public string applicantApplicationId { get; set; }
        public int applicantGrantId { get; set; }
        [Required]
        [Display(Name = "Applicant Name")]
        public string ApplicantName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
       // [Required]
        [Display(Name = "Identity Proof")]
        public IFormFile idProofPhotoFile { get; set; }
        //[Required]
        [Display(Name = "Authorization Letter")]
        public IFormFile authorizationLetterPhotofile { get; set; }
        public int applicantid { get; set; }
    }
}
