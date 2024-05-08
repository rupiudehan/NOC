using Noc_App.Models.interfaces;
using Noc_App.Models;
using Noc_App.Models.ViewModel;

namespace Noc_App.UtilityService
{
    public class Calculations : ICalculations
    {
        private readonly IRepository<SiteUnitMaster> _repoSiteUnitMaster;
        public Calculations(IRepository<SiteUnitMaster> repoSiteUnitMaster)
        {
            _repoSiteUnitMaster = repoSiteUnitMaster;
        }
        public async Task<SiteUnitsViewModel> CalculateUnits(SiteUnitsViewModel units)
        {
            List<SiteUnitMaster> unitMaster = (await _repoSiteUnitMaster.FindAsync(x => x.SiteAreaUnitId == units.SiteUnitId)).ToList();
            var KanalOrBigha = unitMaster.Find(x => x.UnitCode.ToUpper() == "K");
            var MarlaOrBiswa = unitMaster.Find(x => x.UnitCode.ToUpper() == "M");
            var SarsaiOrBiswansi = unitMaster.Find(x => x.UnitCode.ToUpper() == "S");
            SiteUnitsViewModel obj=new SiteUnitsViewModel();
            obj.SiteUnitId=units.SiteUnitId;
            obj.KanalOrBigha = Convert.ToDouble((units.KanalOrBigha * KanalOrBigha.UnitValue * KanalOrBigha.Timesof) / KanalOrBigha.DivideBy);
            obj.MarlaOrBiswa = Convert.ToDouble((units.MarlaOrBiswa * MarlaOrBiswa.UnitValue * MarlaOrBiswa.Timesof) / MarlaOrBiswa.DivideBy);
            obj.SarsaiOrBiswansi = Convert.ToDouble((units.SarsaiOrBiswansi * SarsaiOrBiswansi.UnitValue * SarsaiOrBiswansi.Timesof) / SarsaiOrBiswansi.DivideBy);
            return obj;
        }
    }
}
