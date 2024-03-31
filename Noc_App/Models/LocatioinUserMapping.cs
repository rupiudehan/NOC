using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    //public class LocationUserMapping
    //{
    //    public ApplicationUser Applicationuser { get; set; }
    //    public VillageDetails Village { get; set; }
    //    public TehsilBlockDetails TehsilBlock { get; set; }
    //    public SubDivisionDetails SubDivision { get; set; }
    //    public DivisionDetails Division { get; set; }
    //    [Key, Column(Order = 1)]
    //    public string UserID { get; set; }
    //    [Key, Column(Order = 2)]
    //    public int? DivisionID { get; set; } = null;
    //    [Key, Column(Order = 3)]
    //    public int? SubDivisionID { get; set; } = null;
    //    [Key, Column(Order = 4)]
    //    public int? TehsilBlockID { get; set; } = null;
    //    [Key, Column(Order = 5)]
    //    public int? VillageID { get; set; } = null;
    //}

    public class UserDivision
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int DivisionId { get; set; }
        public DivisionDetails Division { get; set; }
    }

    public class UserSubdivision
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int SubdivisionId { get; set; }
        public SubDivisionDetails Subdivision { get; set; }
    }

    public class UserTehsil
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int TehsilId { get; set; }
        public TehsilBlockDetails Tehsil { get; set; }
    }

    public class UserVillage
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int VillageId { get; set; }
        public VillageDetails Village { get; set; }
    }

}
