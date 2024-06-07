namespace Noc_App.Models
{
    public class GrantSectionsDetails
    {
        public int Id { get; set; }
        public string SectionName { get; set; }
        public string SectionCode { get; set; }

        public GrantRejectionShortfallSection GrantRejectionShortfallSection { get; set; }
    }
}
