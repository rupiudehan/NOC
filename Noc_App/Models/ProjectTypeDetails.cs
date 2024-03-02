namespace Noc_App.Models
{
    public class ProjectTypeDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GrantDetails> Grants { get; set; }
    }
}
