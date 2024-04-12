namespace Noc_App.Models.Payment
{
    public class GrantNocPaymentOrderDetail
    {
        public string OrderId { get; set; }
        public string RazorpayKey { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int GrantId { get; set; }
    }
}
