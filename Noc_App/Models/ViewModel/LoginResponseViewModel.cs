﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class ResponseViewModel
    {
        public string Status { get; set; }
        public string msg { get; set; }
    }
    public class LoginResponseViewModel
    {
        public string Status { get; set; }
        public string msg { get; set; }
        public user_info user_info { get; set; }
    }

    public class user_info
    {
        public string Name { get; set; }
        public string EmployeeName { get; set; }
        public string EmpID { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public int DistrictID { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string DivisionID { get; set; }
        public string SubDivision { get; set; }
        public int SubDivisionID { get; set; }
        public string Designation { get; set; }
        public int DesignationID { get; set; }
        public string Role { get; set; }
        public string RoleID { get; set; }
        public List<OfficeWiseRolesIds> OfficeWiseRoleID { get; set; }
        public List<OfficeWiseRolesNames> OfficeWiseRoleName { get; set; }
        public string Status { get; set; }
        public string DateOfRetirement { get; set; }
        public string IntialJoiningDate { get; set; }
        public string CurrentJoiningDate { get; set; }

    }

    public class OfficeWiseRolesIds
    {
        [Key]
        public int Id { get; set; }
        public int office_id { get; set; }
        public int role { get; set; }
    }
    public class OfficeWiseRolesNames
    {
        [Key]
        public int Id { get; set; }
        public string office_name { get; set; }
        public string role_name { get; set; }
    }
    public class OfficerResponseViewModel1
    {
        public string Status { get; set; }
        public string msg { get; set; }
        public List<officer_info> user_info { get; set; }
    }
    public class OfficerResponseViewModel
    {
        public string Status { get; set; }
        public string msg { get; set; }
        public officer_info user_info { get; set; }
    }

    public class officer_info
    {
        public string EmployeeName { get; set; }
        public string EmployeeId { get; set; }
        public string MobileNo { get; set; }
        public string email { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string DivisionName { get; set; }
        public string DivisionID { get; set; }
        public string SubdivisionName { get; set; }
        public int SubdivisionId { get; set; }
        public string DeesignationName { get; set; }
        public int DesignationID { get; set; }
        public string RoleName { get; set; }
        public string RoleID { get; set; }
        public string Status { get; set; }
        public string DateOfRetirement { get; set; }
        public string IntialJoiningDate { get; set; }
        public string CurrentJoiningDate { get; set; }
        public string UserName { get; set; }

    }
}
