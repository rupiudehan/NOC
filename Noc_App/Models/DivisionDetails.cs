using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class DivisionDetails
    {
        public int Id { get; set; }
        [Required]
        [Remote(action: "IsNameDuplicate", controller: "Division")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }
        public List<SubDivisionDetails> SubDivision { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } // Assuming it's a user ID
        public ApplicationUser User { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public ApplicationUser User2 { get; set; }
        //[NotMapped]
        //public ApplicationUser UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}
