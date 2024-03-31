using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class GrantApprovalDetail
    {
        public int Id { get; set; }
        public int GrantID { get; set; }
        [ForeignKey(nameof(GrantID))]
        public GrantDetails Grant { get; set; }
        public int ApprovalID { get; set; }
        [ForeignKey(nameof(ApprovalID))]
        public GrantApprovalMaster GrantApproval { get; set; }
        public int ApprovalLevel { get; set; }
        public string ProcessedBy { get; set; }
        public DateTime ProcessedOn { get; set; } = DateTime.Now;
    }
}
