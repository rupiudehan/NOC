using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class GrantUnprocessedAppDetails
    {
        public int Id { get; set; }
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
    }
}
