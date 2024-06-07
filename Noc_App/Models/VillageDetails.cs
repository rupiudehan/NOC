using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class VillageDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }
        public int TehsilBlockId { get; set; }
        [ForeignKey(nameof(TehsilBlockId))]
        public TehsilBlockDetails TehsilBlock { get; set; }
        public int PinCode { get; set; }
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } // Assuming it's a user ID
        //public ApplicationUser User { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public List<GrantDetails> Grants { get; set; }
    }

}
