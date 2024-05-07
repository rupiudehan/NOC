using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantApprovalDetailReject
    {
        public string id { get; set; }
        [Required]
        public string Remarks { get; set; }
    }
}
