using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class FileUploadModel
    {
        public IFormFile File { get; set; }
        public string OwnerName { get; set; }
        public string Address { get; set; }
    }
    public class GrantRejectionShortfallSection
    {
        public int Id { get; set; }
        public long GrantApprovalId { get; set; }
        [ForeignKey(nameof(GrantApprovalId))]
        public GrantApprovalDetail GrantApprovalDetail { get; set; }
        public int SectionId { get; set; }
        [ForeignKey(nameof(SectionId))]
        public List<GrantSectionsDetails> GrantSectionsDetails { get; set; }
        public int IsCompleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
