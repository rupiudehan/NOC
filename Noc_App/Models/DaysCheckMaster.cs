using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class DaysCheckMaster
    {
        [Key]
        public int Id { get; set; }
        public string CheckFor { get; set; }
        public string Code { get; set; }
        public int IsRelatedToForward { get; set; }
        public int IsRelatedToIssue { get; set; }
        public int NoOfDays { get; set; }
        public int UserRoleID { get; set; }
        [ForeignKey(nameof(UserRoleID))]
        public UserRoleDetails UserRole { get; set; }
    }
}
