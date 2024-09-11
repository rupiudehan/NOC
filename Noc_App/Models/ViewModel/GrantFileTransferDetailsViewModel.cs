using System.ComponentModel.DataAnnotations.Schema;

namespace Noc_App.Models.ViewModel
{
    public class GrantFileTransferDetailsViewModel
    {
        public int Id { get; set; }
        public int GrantId { get; set; }
        public string FromAuthorityId { get; set; }
        public string FromName { get; set; }
        public string FromDesignationName { get; set; }
        public string ToAuthorityId { get; set; }
        public string ToName { get; set; }
        public string ToDesignationName { get; set; }
        public string Remarks { get; set; }
        public string TransferedOn { get; set; }
    }
}
