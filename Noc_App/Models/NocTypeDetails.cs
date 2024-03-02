namespace Noc_App.Models
{
    public class NocTypeDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GrantDetails> Grants { get; set; }
    }
}
