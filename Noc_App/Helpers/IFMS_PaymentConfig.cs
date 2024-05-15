namespace Noc_App.Helpers
{
    public class IFMS_PaymentConfig
    {
        public string IpAddress { get; set; }
        public string IntegratingAgency { get; set; }
        public string headerClientId { get; set; }
        public string clientSecret { get; set; }
        public string clientId { get; set; }
        public string ChecksumKey { get; set; }
        public string edKey { get; set; }
        public string edIV { get; set; }
        public string ddoCode { get; set; }
        public string companyName { get; set; }
        public string deptCode { get; set; }
        public string payLocCode { get; set; }
        public string trsyPaymentHead { get; set; }
        public string PostUrl { get; set; }

        public IFMS_PaymentConfig(string _IpAddress, string _IntegratingAgency,string _clientSecret,string _clientId,
            string _ChecksumKey,string _edKey,string _edIV,string _ddoCode,string _companyName,string _deptCode,string _payLocCode
            ,string _trsyPaymentHead,string _PostUrl,string _headerClientId)
        {
            IpAddress = _IpAddress;
            IntegratingAgency = _IntegratingAgency;
            clientSecret = _clientSecret;
            clientId=_clientId;
            ChecksumKey = _ChecksumKey;
            edKey = _edKey;
            edIV = _edIV;
            ddoCode = _ddoCode;
            companyName = _companyName;
            deptCode = _deptCode;
            payLocCode = _payLocCode;
            trsyPaymentHead= _trsyPaymentHead;
            PostUrl= _PostUrl;
            headerClientId = _headerClientId;
        }
    }
}
