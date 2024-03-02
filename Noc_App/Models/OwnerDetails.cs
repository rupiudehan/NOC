using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models
{
    public class OwnerDetails
    {
        public int Id { get; set; }
        public int OwnerTypeId { get; set; }
        public OwnerTypeDetails OwnerType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MobileNo { get; set; }
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }
        public int GrantId { get; set; }
        public GrantDetails Grant { get; set; }
    }
}
