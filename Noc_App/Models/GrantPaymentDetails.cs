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
        //public string referenceId { get; set; }
        public decimal? TotalAmount { get; set; }

        public string paymentid { get; set; }

        //public string? sessionid { get; set; }
        public string? Paymentstatus { get; set; }
        public string? PaymentOrderId { get; set; }
        //public string PayerName { get; set; }
        //public string PayerEmail { get; set; }
        //public string PayerId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
