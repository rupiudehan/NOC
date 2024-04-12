namespace Noc_App.Models.Payment
{
    public class OrderCompletionDetail
    {
        public int GrantId { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
    }
}
