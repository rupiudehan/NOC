using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class SubDivisionDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }
        public int DivisionId { get; set; }
        [ForeignKey(nameof(DivisionId))]
        public DivisionDetails Division { get; set; }
        public List<TehsilBlockDetails> TehsilBlock { get; set; }
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } // Assuming it's a user ID
        //public ApplicationUser User { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        //public ApplicationUser User2 { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
