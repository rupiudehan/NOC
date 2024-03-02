using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Noc_App.Models.ViewModel
{
    public class UserWithRolesViewModel
    {
        public ApplicationUser User { get; set; }
        //public int SelectedRoleId { get; set; }
        //public IEnumerable<SelectListItem> Roles { get; set; }
        public List<string> Roles { get; set; }
    }
}
