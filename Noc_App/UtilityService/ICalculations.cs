using Noc_App.Models;
using Noc_App.Models.ViewModel;

namespace Noc_App.UtilityService
{
    public interface ICalculations
    {
        Task<SiteUnitsViewModel> CalculateUnits(SiteUnitsViewModel units);
    }
}
