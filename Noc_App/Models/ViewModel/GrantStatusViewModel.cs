﻿namespace Noc_App.Models.ViewModel
{
    public class GrantStatusViewModel
    {
        public string ApplicationID { get; set; }
        public string CreatedOn { get; set; }
        public string ApplicationStatus { get; set; }
        public string ApprovalStatus { get; set; }
        public string CertificateFilePath { get; set; }
        public bool IsApproved { get; set; }
        public bool IsUnderMasterPlan { get; set; }
        public int GrantId { get; set; }
        public string TransId { get; set; }
        public string ChallanDate { get; set; }
    }
}
