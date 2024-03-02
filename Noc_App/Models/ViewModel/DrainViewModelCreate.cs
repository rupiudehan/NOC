namespace Noc_App.Models.ViewModel
{
    public class DrainViewModelCreate
    {
        public string Name { get; set; }
        public List<DrainCoordinatesViewModelCreate> DrainCoordinates { get; set; }
    }
}
