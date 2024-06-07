using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class DistrictDetails
    {
        [Key]
        public int Id { get; set; }
        public int LGD_ID { get; set; }
        [Required]
        [Remote(action: "IsNameDuplicate", controller: "Division")]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }
        public List<DivisionDetails> Division { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
