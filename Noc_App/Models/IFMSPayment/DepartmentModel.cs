namespace Noc_App.Models.IFMSPayment
{
    public class DepartmentModel
    {
        public class hrmsView
        {
            public string encdata { get; set; }
            public string json { get; set; }
            public string status { get; set; }
        }
        public class RequestData
        {
            public string data { get; set; }
        }
        public class ifms_verifydata
        {
            public string chcksum { get; set; }
            public Challanverifydata challandata { get; set; }
        }
        public class Challanverifydata
        {
            public string deptRefNo { get; set; }
            public string clientId { get; set; }
            public string deptCode { get; set; }
            //public DateTime challanDate { get; set; }
            public string challanDate { get; set; }
        }
        public class ifms_data
        {
            public string chcksum { get; set; }
            public Challandata challandata { get; set; }
        }
        public class PayeeInfo
        {
            public string payerName { get; set; }
            public string teleNumber { get; set; }
            public string mobNumber { get; set; }
            public string emailId { get; set; }
            public string addLine1 { get; set; }
            public string addLine2 { get; set; }
            public string addPincode { get; set; }
            public string district { get; set; }
            public string tehsil { get; set; }
        }
        public class trsyPayments
        {
            public string Head { get; set; }
            public string amt { get; set; }
        }
        public class nonTrsyPayments
        {
            public string NonTrsy { get; set; }
            public string ntAmt { get; set; }
        }
        public class Challandata
        {
            public string deptRefNo { get; set; }
            public string receiptNo { get; set; }
            public string clientId { get; set; }
            public string challanDate { get; set; }
            public string expiryDate { get; set; }
            public string companyName { get; set; }
            public string deptCode { get; set; }
            public string totalAmt { get; set; }
            public string trsyAmt { get; set; }
            public string nonTrsyAmt { get; set; }
            public string noOfTrans { get; set; }
            public string ddoCode { get; set; }
            public string payLocCode { get; set; }
            public string add1 { get; set; }
            public string add2 { get; set; }
            public string add3 { get; set; }
            public string add4 { get; set; }
            public string add5 { get; set; }
            public string sURL { get; set; }
            public string fURL { get; set; }
            public List<trsyPayments> trsyPayments { get; set; }
            public List<nonTrsyPayments> nonTrsyPayments { get; set; }
            public PayeeInfo payee_info { get; set; }
        }
        public class Checkdata
        {
            public string encData { get; set; }
            //public string clientHeader { get; set; }
            public string clientId { get; set; }
            public string clientSecret { get; set; }
            public string integratingAgency { get; set; }
            public string ipAddress { get; set; }
            public string transactionID { get; set; }
            public string dateTime { get; set; } = null;
        }
        public class IFMSHeader
        {
            public string clientId { get; set; }
            public string clientSecret { get; set; }
            public string integratingAgency { get; set; }
            public string ipAddress { get; set; }
            public string transactionID { get; set; }
            public string dateTime { get; set; }
            public string ApplicationId { get; set; }
            public string encdata { get; set; }
            public long ChallanId { get; set; }
        }
        public class ClientHeader
        {
            public string clientSecret { get; set; }
            public string integratingAgency { get; set; }
            public string ipAddress { get; set; }
            public string clientId { get; set; }
            public string txnId { get; set; }
        }

        public class ClientResponse
        {
            public string chcksum { get; set; }
            public ClientChallanResponse challandata { get; set; }
        }

        public class ClientChallanResponse
        {
            public string receiptNo { get; set;}
            public string deptRefNo { get; set; }
            public string clientId { get; set; }
            public string challanDate { get; set; }
            public string expiryDate { get; set; }
            public string companyName { get; set; }
            public string deptCode { get; set; }
            public string totalAmt { get; set; }
            public string trsyAmt { get; set; }
            public string nonTrsyAmt { get; set; }
            public string noOfTrans { get; set; }
            public string ddoCode { get; set; }
            public string payLocCode { get; set; }
            public string add1 { get; set; }
            public string add2 { get; set; }
            public string add3 { get; set; }
            public string add4 { get; set; }
            public string add5 { get; set; }
            public BankDetails bank_Res { get; set; }
        }
        public class BankDetails
        {
            public string BankRefNo { get; set; }
            public string CIN { get; set; }
            public string dateOfPay { get; set; }
            public string status { get; set; }
            public string desc { get; set; }
        }

    }
}
