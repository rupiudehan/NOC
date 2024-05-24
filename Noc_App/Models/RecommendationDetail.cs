using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models
{
    public class RecommendationDetail
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
