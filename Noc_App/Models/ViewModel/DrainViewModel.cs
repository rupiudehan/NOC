namespace Noc_App.Models.ViewModel
{
    public class DrainViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DrainCoordinatesViewModel> DrainCoordinates { get; set; }
    }
}
