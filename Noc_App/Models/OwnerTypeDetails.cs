namespace Noc_App.Models
{
    public class OwnerTypeDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<OwnerDetails> Owners { get; set; }
    }
}
