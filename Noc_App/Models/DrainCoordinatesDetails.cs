using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models
{
    public class DrainCoordinatesDetails
    {
        public int Id { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public int DrainId { get; set; }
        public DrainDetails Drain { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; } // Assuming it's a user ID
        public ApplicationUser User { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public ApplicationUser User2 { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
