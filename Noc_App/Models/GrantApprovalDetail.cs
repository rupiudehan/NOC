using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class GrantApprovalDetail
    {
        public long Id { get; set; }
        public int GrantID { get; set; }
        [ForeignKey(nameof(GrantID))]
        public GrantDetails Grant { get; set; }
        public int ApprovalID { get; set; }
        [ForeignKey(nameof(ApprovalID))]
        public GrantApprovalMaster GrantApproval { get; set; }
        public List<GrantApprovalProcessDocumentsDetails> GrantApprovalProcessDocuments { get; set; }
        public List<GrantRejectionShortfallSection> GrantRejectionShortfallSection { get; set; }
        public string Remarks { get; set; }
        public int ProcessLevel { get; set; }
        public string ProcessedToUser { get; set; }
        public string ProcessedToRole { get; set; }
        public string ProcessedToName { get; set; }
        public string ProcessedBy { get; set; }
        public string ProcessedByRole { get; set; }
        public string ProcessedByName { get; set; }
        public DateTime ProcessedOn { get; set; } = DateTime.Now;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public int RecommendationID { get; set; }
        [ForeignKey(nameof(RecommendationID))]
        public RecommendationDetail RecommendationDetail { get; set; }
    }
}
