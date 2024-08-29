using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models
{
    public class EstablishmentOfficeDetails
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
