namespace Noc_App.Models
{
    public class NocPermissionTypeDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GrantDetails> Grants { get; set; }
    }
}
