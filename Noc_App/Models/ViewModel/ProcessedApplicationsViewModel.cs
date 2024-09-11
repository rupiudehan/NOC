using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class ProcessedApplicationsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProjectTypeId { get; set; }
        public string OtherProjectTypeDetail { get; set; }
        public string Hadbast { get; set; }
        public string PlotNo { get; set; }
        public int VillageID { get; set; }
        public string ApplicantName { get; set; }
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
        public int NocPermissionTypeID { get; set; }
        public int NocTypeId { get; set; }
        public string NocNumber { get; set; }
        public DateTime? PreviousDate { get; set; }
        public string ApplicationID { get; set; }
        public string VillageName { get; set; }
        public string LocationDetails { get; set; }
        public bool IsForwarded { get; set; }
        public bool isshortfall { get; set; }
        public bool isshortfallcompleted { get; set; }
        public bool ispending { get; set; }
        public bool isapproved { get; set; }
        public bool isrejected { get; set; }
        public bool IsShortFall { get; set; }
        public int ProcessedLevel { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CertificateFilePath { get; set; }
        public int IsRecommended { get; set; }
        public bool IsUnderMasterPlan { get; set; }
        public string ProcessedBy { get; set; }
        public string ProcessedByName { get; set; }
        public string ProcessedByRole { get; set; }
        public DateTime ProcessedOn { get; set; }
        public string ProcessedToUser { get; set; }
        public string ProcessedToName { get; set; }
        public string ProcessedToRole { get; set; }
        public string CurrentProcessedBy { get; set; }
        public string CurrentProcessedByName { get; set; }
        public string CurrentProcessedByRole { get; set; }
        public DateTime CurrentProcessedOn { get; set; }
        public string CurrentProcessedToUser { get; set; }
        public string CurrentProcessedToName { get; set; }
        public string CurrentProcessedToRole { get; set; }
    }
}
