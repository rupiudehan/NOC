using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class GrantDetails
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SiteAreaUnitId { get; set; }
        [ForeignKey(nameof(SiteAreaUnitId))]
        public SiteAreaUnitDetails SiteAreaUnits { get; set; }

        public int ProjectTypeId { get; set; }
        [ForeignKey(nameof(ProjectTypeId))]
        public ProjectTypeDetails ProjectType { get; set; }
        public string? OtherProjectTypeDetail { get; set; }
        public List<GrantKhasraDetails> Khasras { get; set; }
        public string? Hadbast { get; set; }
        public string? PlotNo { get; set; }
        public int SubDivisionId { get; set; }
        [ForeignKey(nameof(SubDivisionId))]
        public SubDivisionDetails SubDivisions { get; set; }
        public string VillageName { get; set; }
        public string PinCode { get; set; }
        public int TehsilID { get; set; }
        [ForeignKey(nameof(TehsilID))]
        public TehsilBlockDetails Tehsil { get; set; }
        //[Required]
        public string AddressProofPhotoPath { get; set; }
        public int PlanSanctionAuthorityId { get; set; }
        [ForeignKey(nameof(PlanSanctionAuthorityId))]
        public PlanSanctionAuthorityMaster PlanSanctionAuthorityMaster { get; set; }
        public string LayoutPlanFilePath { get; set; }
        public string FaradFilePath { get; set; }
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
        public List<GrantFileTransferDetails> GrantFileTransferDetails { get; set; }
        //[Required]
        public int? NocPermissionTypeID { get; set; }
        [ForeignKey(nameof(NocPermissionTypeID))]
        public NocPermissionTypeDetails NocPermissionType { get; set; }
        //[Required]
        public int? NocTypeId { get; set; }
        [ForeignKey(nameof(NocTypeId))]
        public NocTypeDetails NocType { get; set; }
        public bool? IsExtension { get; set; }
        public string? NocNumber { get; set; }
        public DateTime? PreviousDate { get; set; }
        public bool IsConfirmed { get; set; }
        public string ApplicationID { get; set; }
        public bool IsPending { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsRejected { get; set; }
        public bool IsApproved { get; set; }
        public bool IsForwarded { get; set; }
        public int ProcessLevel { get; set; }
        public bool IsSentBack { get; set; }
        public int SentBackLevel { get; set; }
        public bool IsShortFall { get; set; }
        public int ShortFallLevel { get; set; }
        public string ShortFallReportedById { get; set; }
        public string ShortFallReportedByRole { get; set; }
        public string ShortFallReportedByName { get; set; }
        public DateTime ShortFallReportedOn { get; set; }
        public bool IsShortFallCompleted { get; set; }
        public DateTime ShortFallCompletedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string CertificateFilePath { get; set; }
        public DateTime UploadedOn { get; set; }
        public string UploadedByRole { get; set; }
        public string UploadedBy { get; set; }
        public bool IsUnderMasterPlan { get; set; }
        public int? MasterPlanId { get; set; }
        [ForeignKey(nameof(MasterPlanId))]
        public MasterPlanDetails MasterPlanDetails { get; set; }
    }
}
