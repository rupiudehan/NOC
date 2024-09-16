namespace Noc_App.Models.ViewModel
{
    public class LoginRoleViewModel
    {
        public string Name { get; set; }
        public string Designation { get; set; }
        public string EmpID { get; set; }
        public string DistrictID { get; set; }
        public string DivisionID { get; set; }
        public string RoleID { get; set; }
        public string EmployeeName { get; set; }
        public string RoleWithOffice { get; set; }
        public string DivisionName { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string Success { get; set; }
        public string Errors { get; set; }
        public List<UserRoleDetailsViewModel> Roles { get; set; }
    }
}
