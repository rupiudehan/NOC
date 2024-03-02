using Microsoft.AspNetCore.Identity;

namespace Noc_App.Models
{
    public class ApplicationUser:IdentityUser
    {
        //public int DivisionId { get; set; }
        //public DivisionDetails Division { get; set; }
        //public int SubDivisionId { get; set; }
        //public SubDivisionDetails SubDivision { get; set; }
        //public int TehsilBlockId { get; set; }
        //public TehsilBlockDetails TehsilBlock { get; set; }
        //public int VillageId { get; set; }
        ////public VillageDetails Village { get; set; }
        //public virtual ICollection<DivisionDetails> Divisions { get; set; }
        //public virtual ICollection<SubDivisionDetails> SubDivisions { get; set; }
        //public virtual ICollection<TehsilBlockDetails> TehsilBlocks { get; set; }
        //public virtual ICollection<VillageDetails> Villages { get; set; }

        // Foreign key relationship with Village
        public int? VillageId { get; set; }
        public VillageDetails Village { get; set; }
        public int? TehsilBlockId { get; set; }
        public TehsilBlockDetails TehsilBlock { get; set; }
        public int? SubDivisionId { get; set; }
        public SubDivisionDetails SubDivision { get; set; }
        public int? DivisionId { get; set; }
        public DivisionDetails Division { get; set; }

        public ICollection<DivisionDetails> Divisions { get; set; }
        public ICollection<SubDivisionDetails> SubDivisions { get; set; }
        public ICollection<TehsilBlockDetails> TehsilBlocks { get; set; }
        public ICollection<VillageDetails> Villages { get; set; }
        public ICollection<DrainCoordinatesDetails> DrainCoordinates { get; set; }
        public ICollection<DrainDetails> Drains { get; set; }

        public ICollection<DivisionDetails> Divisions2 { get; set; }
        public ICollection<SubDivisionDetails> SubDivisions2 { get; set; }
        public ICollection<TehsilBlockDetails> TehsilBlocks2 { get; set; }
        public ICollection<VillageDetails> Villages2 { get; set; }
        public ICollection<DrainCoordinatesDetails> DrainCoordinates2 { get; set; }
        public ICollection<DrainDetails> Drains2 { get; set; }
    }
}
