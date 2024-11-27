using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models
{
    public class UserSessionDetails
    {
        [Key]
        public Guid Id { get; set; }
        public string Hrms { get; set; }
        public string EmpId { get; set; }
        public string LastSessionId { get; set; }  // Store the session ID
        public DateTime LastLoginTime { get; set; }
    }
}
