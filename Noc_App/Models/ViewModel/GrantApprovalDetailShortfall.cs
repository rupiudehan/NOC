using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantApprovalDetailShortfall
    {
        public string id { get; set; }
        [Required]
        [Display(Name ="Section Name")]
        public List<int> SelectedGrantSectionIDs { get; set; }
        public IEnumerable<GrantSectionsDetails> GrantSectionsDetailsList { get; set; }
        [Required]
        public string Remarks { get; set; }
    }
}
