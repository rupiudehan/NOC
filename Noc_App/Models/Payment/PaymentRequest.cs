using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.Payment
{
    public class PaymentRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public int GrantId { get; set; }
        [Required]
        public string ApplicationId { get; set; }
    }
}
