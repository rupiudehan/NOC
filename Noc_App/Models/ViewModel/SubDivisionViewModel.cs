using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class SubDivisionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
    }
}
