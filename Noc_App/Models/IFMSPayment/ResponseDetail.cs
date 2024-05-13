namespace Noc_App.Models.IFMSPayment
{
    public class ResponseDetail
    {
        public string deptRefNo { get; set; }
        public string clientId { get; set; }
        public string deptCode { get; set; }
        public DateTime challanDate { get; set; }
        public string encData { get; set; }
        public string clientSecret { get; set; }
        public string integratingAgency { get; set; }
        public string ipAddress { get; set; }
        public string transactionID { get; set; }
        public string statusCode { get; set; }
        public string msg { get; set; }
    }
}
