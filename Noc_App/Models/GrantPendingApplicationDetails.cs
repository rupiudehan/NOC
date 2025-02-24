﻿using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models
{
    public class GrantPendingApplicationDetails
    {
        public int Id { get; set; }
        public int rejectionmust { get; set; }
        //[Required]
        public string Name { get; set; }
        public int SiteAreaUnitId { get; set; }
        public int ProjectTypeId { get; set; }
        public string OtherProjectTypeDetail { get; set; }
        public string Hadbast { get; set; }
        public string PlotNo { get; set; }
        public int VillageID { get; set; }
        public string ApplicantName { get; set; }
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
        public int NocPermissionTypeID { get; set; }
        public int NocTypeId { get; set; }
        public string NocNumber { get; set; }
        public DateTime? PreviousDate { get; set; }
        public string ApplicationID { get; set; }
        public string VillageName { get; set; }
        public string TehsilBlockName { get; set; }
        public string SubDivisionName { get; set; }
        public string DivisionName { get; set; }
        public string LocationDetails { get; set; }
        public int DivisionId { get; set; }
        public int SubDivisionId { get; set; }
        public bool IsForwarded { get; set; }
        public bool IsShortFall { get; set; }
        public string LoggedInRole { get; set; }
        public long GrantApprovalId { get; set; }
        public string ProcessedToUser { get; set; }
        public string ProcessedToRole { get; set; }
        public long GrantApprovalDocId { get; set; }
        public string LastForwardedByRole { get; set; }
        public int PreviousProcessLevel { get; set; }
        public int ProcessedLevel { get; set; }
        public DateTime CreatedOn { get; set; }
        public double TotalArea { get; set; }
        public string Remarks { get; set; }
        public string CertificateFilePath { get; set; }
        public int IsRecommended { get; set; }
        public bool IsUnderMasterPlan { get; set; }
        public string LastForwardedByName { get; set; }
        public string ProcessedToName { get; set; }
        public bool IsPartiallyApproved { get; set; }
        public bool IsSiteWithin150m { get; set; }
    }
}
