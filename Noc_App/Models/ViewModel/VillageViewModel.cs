namespace Noc_App.Models.ViewModel
{
    public class VillageViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PinCode { get; set; }

        public int DivisionId { get; set; }
        public string DivisionName { get; set; }

        public int SubDivisionId { get; set; }
        public string SubDivisionName { get; set; }
        public int TehsilBlockId { get; set; }
        public string TehsilBlockName { get; set; }
    }
}
