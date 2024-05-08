using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class SiteUnitMaster
    {
        [Key]
        public int Id { get; set; }
        public int SiteAreaUnitId { get; set; }
        [ForeignKey(nameof(SiteAreaUnitId))]
        public SiteAreaUnitDetails SiteAreaUnits { get; set; }
        public string UnitName { get; set; }
        public string UnitCode { get; set; }
        public double UnitValue { get; set; }
        public double Timesof { get; set; }
        public double DivideBy { get; set; }
    }
}
