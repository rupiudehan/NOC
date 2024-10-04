using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class GrantFileTransferDetails
    {
        [Key]
        public int Id { get; set; }
        public int GrantId { get; set; }
        [ForeignKey(nameof(GrantId))]
        public GrantDetails GrantDetail { get; set; }
        public string FromAuthorityId { get; set; }
        public string FromName { get; set; }
        public string FromDesignationName { get; set; }
        public string ToAuthorityId { get; set; }
        public string ToName { get; set; }
        public string ToDesignationName { get; set; }
        public string Remarks { get; set; }
        public DateTime TransferedOn { get; set; } = DateTime.Now;
        public int FromLocationId { get; set; }
        public int ToLocationId { get; set; }
    }
}
