using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class GrantApprovalProcessDocumentsDetails
    {
        public long Id { get; set; }
        public long GrantApprovalID { get; set; }
        [ForeignKey(nameof(GrantApprovalID))]
        public GrantApprovalDetail GrantApproval { get; set; }
        public string SiteConditionReportPath { get; set; }
        public string CatchmentAreaAndFlowPath { get; set; }
        public string DistanceFromCreekPath { get; set; }
        public string GISOrDWSReportPath { get; set; }
        public bool IsKMLByApplicantValid { get; set; }
        //public string KmlFileVerificationReportPath { get; set; }
        public string CrossSectionOrCalculationSheetReportPath { get; set; }
        public string DrainLSectionPath { get; set; }
        public string ProcessedBy { get; set; }
        public string ProcessedByRole { get; set; }
        public string ProcessedByName { get; set; }
        public DateTime ProcessedOn { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public string UpdatedByRole { get; set; }
        public string UpdatedByName { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
    }
}
