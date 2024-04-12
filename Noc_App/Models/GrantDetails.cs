using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class GrantDetails
    {
        [Key]
        public int Id { get; set; }
        //[Required]
        public string Name { get; set; }
        public int SiteAreaUnitId { get; set; }
        [ForeignKey(nameof(SiteAreaUnitId))]
        public SiteAreaUnitDetails SiteAreaUnits { get; set; }
        //[Required]
        //public double SiteAreaOrSizeInFeet { get; set; }
        //[Required]
        //public double SiteAreaOrSizeInInches { get; set; }
        [Required]
        public int ProjectTypeId { get; set; }
        public ProjectTypeDetails ProjectType { get; set; }
        public string? OtherProjectTypeDetail { get; set; }
        public List<GrantKhasraDetails> Khasras { get; set; }
        //public string? Khasra { get; set; }
        public string? Hadbast { get; set; }
        public string? PlotNo { get; set; }
        public int VillageID { get; set; }
        public VillageDetails Village { get; set; }
        //[Required]
        public string AddressProofPhotoPath { get; set; }
        public string KMLFilePath { get; set; }
        public string KMLLinkName { get; set; }
        //[Required]
        [Display(Name = "Applicant Name")]
        public string ApplicantName { get; set; }
        //[Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
        //[Required]
        public string IDProofPhotoPath { get; set; }
        //[Required]
        public string AuthorizationLetterPhotoPath { get; set; }
        public List<OwnerDetails> Owners { get; set; }
        //[Required]
        public int NocPermissionTypeID { get; set; }
        public NocPermissionTypeDetails NocPermissionType { get; set; }
        //[Required]
        public int NocTypeId { get; set; }
        public NocTypeDetails NocType { get; set; }
        public bool IsExtension { get; set; }
        public string? NocNumber { get; set; }
        public DateTime? PreviousDate { get; set; }
        public bool IsConfirmed { get; set; }
        public string ApplicationID { get; set; }
        public bool IsPending { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsRejected { get; set; }
        public bool IsApproved { get; set; }
        public bool IsForwarded { get; set; }
        public int ForwardLevel { get; set; }
        public bool IsSentBack { get; set; }
        public int SentBackLevel { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
