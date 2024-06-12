using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class DashboardDropdownViewModelView: ReportApplicationCountViewModel
    {
        public int hdnDivisionId { get; set; }
        public int hdnSubDivisionId { get; set; }
        [Display(Name = "Division")]
        public int SelectedDivisionId { get; set; }
        public IEnumerable<SelectListItem> Divisions { get; set; }
        [Display(Name = "Sub-Division")]
        public int SelectedSubDivisionId { get; set; }
        public IEnumerable<SelectListItem> SubDivisions { get; set; }
        public string RoleName { get; set; }
        public long Pending { get; set; }
        public string LoggedInRole { get; set; }
    }
}
