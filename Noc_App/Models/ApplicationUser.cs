using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models
{
    public class ApplicationUser:IdentityUser
    {

        public ICollection<DivisionDetails> Divisions { get; set; }
        public ICollection<SubDivisionDetails> SubDivisions { get; set; }
        public ICollection<TehsilBlockDetails> TehsilBlocks { get; set; }
        public ICollection<VillageDetails> Villages { get; set; }

        public ICollection<UserDivision> UserDivisions { get; set; }
        public ICollection<UserSubdivision> UserSubdivisions { get; set; }
        public ICollection<UserTehsil> UserTehsils { get; set; }
        public ICollection<UserVillage> UserVillages { get; set; }

        public ICollection<DivisionDetails> Divisions2 { get; set; }
        public ICollection<SubDivisionDetails> SubDivisions2 { get; set; }
        public ICollection<TehsilBlockDetails> TehsilBlocks2 { get; set; }
        public ICollection<VillageDetails> Villages2 { get; set; }
    }
}
