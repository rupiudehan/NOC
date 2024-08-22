using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models
{
    public class CircleDetails
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(500, ErrorMessage = "Name cannot exceed 500 characters")]
        public string Name { get; set; }
        public string Code { get; set; }
        public List<CircleDivisionMapping> CircleDivisionMappings { get; set; }
    }
}
