using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class RoleViewModelCreate
    {
        [Required(ErrorMessage ="Role Name is required")]

        public string RoleName { get; set; }
    }
}
