using Microsoft.AspNetCore.Rewrite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class CircleDivisionMapping
    {
        [Key]
        public int Id { get; set; }
        public int CircleId { get; set; }
        public int DivisionId { get; set; }
        [ForeignKey(nameof(CircleId))]
        public CircleDetails Circle { get; set; }
        [ForeignKey(nameof(DivisionId))]
        public DivisionDetails Divisions { get; set; }
    }
}
