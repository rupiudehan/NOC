using Noc_App.Helpers;

namespace Noc_App.Models.ViewModel
{
    public class DrainCoordinatesViewModelCreate
    {
        [NumericValidation(typeof(double))]
        public double Latitude { get; set; }
        [NumericValidation(typeof(double))]
        public double Longitude { get; set; }
    }
}
