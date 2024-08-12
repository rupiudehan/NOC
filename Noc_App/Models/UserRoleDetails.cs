using Noc_App.Models.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models
{
    public class UserRoleDetails
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string AppRoleName { get; set; }
        public int RoleLevel { get; set; }
        public List<OfficeWiseRolesIds> OfficeWiseRoleID { get; set; }
    }
}
