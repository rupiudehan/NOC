using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models
{
    public class DrainDetails
    {
        public int Id { get; set; }
        [Required]
        [Remote(action: "IsNameDuplicate", controller: "Drain")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }
        public List<DrainCoordinatesDetails> DrainCoordinates { get; set; }
        public string CreatedBy { get; set; } // Assuming it's a user ID
        public ApplicationUser User { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public ApplicationUser User2 { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
