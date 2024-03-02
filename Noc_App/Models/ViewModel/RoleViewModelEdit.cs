using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class RoleViewModelEdit
    {
        //public RoleViewModelEdit()
        //{
        //    Users = new List<string>();
        //}
        public string Id { get; set; }
        [Required(ErrorMessage = "Role Name is required")]
        public string RoleName { get; set; }
        //public List<string> Users { get; set; }
    }
}
