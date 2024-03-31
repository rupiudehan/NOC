using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class TehsilBlockDetails
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }
        public int SubDivisionId { get; set; }
        public SubDivisionDetails SubDivision { get; set; }
        public List<VillageDetails> Village { get; set; }
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; } // Assuming it's a user ID
        public ApplicationUser User { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public ApplicationUser User2 { get; set; }
        //public ApplicationUser UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        //public ICollection<ApplicationUser> ApplicationUsers { get; set; }

        //public List<LocationUserMapping> LocatioinUserMapping { get; set; }
        public ICollection<UserTehsil> UserTehsils { get; set; }

    }
}
