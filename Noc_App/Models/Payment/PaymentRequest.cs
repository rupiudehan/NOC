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
        public string Hadbast { get; set; }
        public string PlotNo { get; set; }
        public string Division { get; set; }
        public string SubDivision { get; set; }
        public string Tehsil { get; set; }
        public string Village { get; set; }
        public string TehsilId { get; set; }
        public string DistrictId { get; set; }
        public string Pincode { get; set; }
        public string PayerName { get; set; }
        public string MobileNo { get; set; }
        public string AreaCalculation { get; set; }
        public string AreaAdditionalCalculation { get; set; }
        public string TotalAreaCalculation { get; set; }
        [Required]
        public int GrantId { get; set; }
        [Required]
        public string ApplicationId { get; set; }
    }
}
