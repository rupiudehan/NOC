using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class MasterPlanDetails
    {
        public int Id { get; set; }
        public string MainPlanName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int DistrictId { get; set; }
        [ForeignKey(nameof(DistrictId))]
        public DistrictDetails Districts { get; set; }
    }
}
