namespace Noc_App.Models.ViewModel
{
    public class LoginResponseViewModel
    {
        public string Status { get; set; }
        public user_info user_info { get; set; }
    }

    public class user_info
    {
        public string Name { get; set; }
        public string Emp_ID { get; set; }
        public string Mobile_No { get; set; }
        public string Email_Id { get; set; }
        public int DistrictID { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public int DivisionID { get; set; }
        public string Sub_Division { get; set; }
        public int Sub_DivisionID { get; set; }
        public string Designation { get; set; }
        public int DesignationID { get; set; }
        public string Role { get; set; }
        public string InitialJoiningDate { get; set; }
        public string RetirementDate { get; set; }

    }
}
