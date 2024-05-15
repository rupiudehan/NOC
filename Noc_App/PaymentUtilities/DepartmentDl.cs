using static Noc_App.Models.IFMSPayment.DepartmentModel;

namespace Noc_App.PaymentUtilities
{
    public class DepartmentDl
    {
        
        public Challandata FillChallanData()
        {

            Challandata challandata = new Challandata()
            {
                deptRefNo = "ABC12345",
                receiptNo = "",
                clientId = "LBCTP",
                challanDate = "2018-10-25T00:00:00",
                expiryDate = "2018-10-25T00:00:00",
                companyName = "Company LTd",
                deptCode = "PET",
                totalAmt = "250",
                trsyAmt = "150",
                nonTrsyAmt = "100",
                noOfTrans = "2",
                ddoCode = "0039",
                payLocCode = "CHD00",
                add1 = "Purpose1",
                add2 = "Purpose2",
                add3 = "Purpose3",
                add4 = "Purpose4",
                add5 = "Purpose5",
                sURL = "?succesfulURL",
                fURL = "?failureURL"
            };
            challandata.payee_info = new PayeeInfo()
            {
                payerName = "PayerName",
                teleNumber = "02874258742",
                mobNumber = "7845127485",
                emailId = "abc@abc.com",
                addLine1 = "Street Road",
                addLine2 = "Room/Flat Number",
                addPincode = "788777",
                district = "20",
                tehsil = "20"
            };
            challandata.trsyPayments = new List<trsyPayments>()
                                         {
                                         new trsyPayments(){
                                         Head= "0040-01-102-00-00",
                                         amt= "100" }
                                         ,
                                         new trsyPayments(){
                                         Head= "8443-01-116-00-00",
                                         amt= "50" }
                                         };
            challandata.nonTrsyPayments = new List<nonTrsyPayments>()
                                         {
                                         new nonTrsyPayments(){
                                         NonTrsy ="EXC",
                                        ntAmt= "70" },
                                         new nonTrsyPayments(){
                                         NonTrsy ="TRP",
                                         ntAmt= "30" },
                                         };

            //string output = JsonConvert.SerializeObject(deptIntigration);
            return challandata;
        }
    }
}
