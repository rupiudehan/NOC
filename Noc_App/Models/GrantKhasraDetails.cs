using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class GrantKhasraDetails
    {
        [Key]
        public int Id { get; set; }
        public string KhasraNo { get; set; }
        public int UnitId { get; set; }
        [ForeignKey(nameof(UnitId))]
        public SiteAreaUnitDetails Units { get; set; }
        public int GrantID { get; set; }
        [ForeignKey(nameof(GrantID))]
        public GrantDetails Grant { get; set; }
        public double MarlaOrBiswansi { get; set; }
        public double KanalOrBiswa { get; set; }
        public double SarsaiOrBigha { get; set; }
    }
}
