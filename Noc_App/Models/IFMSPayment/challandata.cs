using static Noc_App.Models.IFMSPayment.DepartmentModel;

namespace Noc_App.Models.IFMSPayment
{
    public class ifms_dataRes
    {
        public string chcksum { get; set; }
        public challandataRes challandata { get; set; }
    }
    public class challandataRes
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
        public BankRes bank_Res { get; set; }
    }
    public class BankRes
    {
        public string BankRefNo { get; set; }
        public string CIN { get; set; }
        public string dateOfPay { get; set; }
        public string status { get; set; }
        public string desc { get; set; }
    }
}
