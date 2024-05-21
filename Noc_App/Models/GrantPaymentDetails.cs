using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class GrantPaymentDetails
    {
        [Key]
        public int Id { get; set; }
        public int GrantID { get; set; }
        [ForeignKey(nameof(GrantID))]
        public GrantDetails Grant { get; set; }
        public decimal? TotalAmount { get; set; }
        public string deptRefNo { get; set; }
        public string? PaymentOrderId { get; set; }
        public string PayerName { get; set; }
        public string PayerEmail { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
