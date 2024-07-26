using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantKMLViewModelPostEdit
    {
        public string kmlApplicationId { get; set; }
        public int kmlGrantId { get; set; }
        //[RegularExpression(@"https:\/\/earth\.google\.com\/web\/\S+", ErrorMessage = "Invalid link")]
        [Display(Name = "KML Link")]
        public string KmlLinkName { get; set; }
        [Display(Name = "KML File")]
        public IFormFile kmlFile { get; set; }
        public int kmlid { get; set; }
    }
}
