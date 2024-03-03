using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models
{
    public class GrantDetails
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double SiteAreaOrSizeInFeet { get; set; }
        [Required]
        public double SiteAreaOrSizeInInches { get; set; }
        [Required]
        public int ProjectTypeId { get; set; }
        public ProjectTypeDetails ProjectType { get; set; }
        public string? OtherProjectTypeDetail { get; set; }
        public string? Khasra { get; set; }
        public string? Hadbast { get; set; }
        public string? PlotNo { get; set; }
        public int VillageID { get; set; }
        public VillageDetails Village { get; set; }
        [Required]
        public string AddressProofPhotoPath { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitute { get; set; }
        [Required]
        [Display(Name = "Applicant Name")]
        public string ApplicantName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
        [Required]
        public string IDProofPhotoPath { get; set; }
        [Required]
        public string AuthorizationLetterPhotoPath { get; set; }
        public List<OwnerDetails> Owners { get; set; }
        [Required]
        public int NocPermissionTypeID { get; set; }
        public NocPermissionTypeDetails NocPermissionType { get; set; }
        [Required]
        public int NocTypeId { get; set; }
        public NocTypeDetails NocType { get; set; }
        public bool IsExtension { get; set; }
        public string? NocNumber { get; set; }
        public DateTime? PreviousDate { get; set; }
        public bool IsConfirmed { get; set; }
        public string ApplicationID { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
