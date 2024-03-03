using Noc_App.Helpers;

namespace Noc_App.Models.ViewModel
{
    public class DrainCoordinatesViewModel
    {
        public int Id { get; set; }
        [NumericValidation(typeof(double))]
        public double Latitude { get; set; }
        [NumericValidation(typeof(double))]
        public double Longitude { get; set; }
    }
}
