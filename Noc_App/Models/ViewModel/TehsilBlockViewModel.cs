namespace Noc_App.Models.ViewModel
{
    public class TehsilBlockViewModel
    {
        public int Id { get; set; }
        public int LGD_ID { get; set; }
        public string Name { get; set; }

        public int DistrictId { get; set; }
        public string DistrictName { get; set; }

        public int DivisionId { get; set; }
        public string DivisionName { get; set; }

        //public int SubDivisionId { get; set; }
        //public string SubDivisionName { get; set; }
    }
}
