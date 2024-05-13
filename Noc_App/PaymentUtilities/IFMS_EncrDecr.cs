using Microsoft.Extensions.Options;
using Noc_App.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace Noc_App.PaymentUtilities
{
    public class IFMS_EncrDecr
    {
        private string ChecksumKey { get; set; }
        private string Key { get; set; }
        private string IV { get; set; }
        //private IFMS_PaymentConfig _settings;
        //public IFMS_EncrDecr(IOptions<IFMS_PaymentConfig> settings)
        //{
        //    _settings = settings.Value;
        //}
        public IFMS_EncrDecr(string checksumKey, string key, string iV)
        {
            ChecksumKey = checksumKey;// 
            //ConfigurationManager.AppSettings["ChecksumKey"].ToString();
            Key = key;// 
            //ConfigurationManager.AppSettings["edKey"].ToString();
            IV = iV;//ConfigurationManager.AppSettings["edIV"].ToString();
        }
        public string CheckSum(string text)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            string hex = "";
            byte[] hashValue = new
           HMACSHA512(encoder.GetBytes(ChecksumKey)).ComputeHash(encoder.GetBytes(text));
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex.ToLower();
        }
        [Obsolete]
        public string Encrypt(string textToEncrypt)
        {
            int _Keysize = 128;
            RijndaelManaged rijndaelCipher = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = _Keysize,
                BlockSize = _Keysize,
            };
            rijndaelCipher.Key = Encoding.UTF8.GetBytes(Key);
            rijndaelCipher.IV = Encoding.UTF8.GetBytes(IV);
            ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
            byte[] plainText = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
            return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0,
           plainText.Length));
        }
        [Obsolete]
        public string Decrypt(string textToDecrypt)
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged()
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 128,
                BlockSize = 128
            };
            Byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
            rijndaelCipher.Key = Encoding.UTF8.GetBytes(Key);
            rijndaelCipher.IV = Encoding.UTF8.GetBytes(IV);
            Byte[] plainText =
           rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0,
           encryptedData.Length);
            string a = System.Text.Encoding.UTF8.GetString(plainText);
            return a;
        }
        protected byte[] GetFileBytes(string filepath)
        {
            System.IO.FileStream fileStream = new System.IO.FileStream(filepath,
           FileMode.Open, FileAccess.Read);
            int length = (int)fileStream.Length;
            byte[] buffer = new byte[length];
            try
            {
                int count = 0;
                int sum = 0;
                while (((count = fileStream.Read(buffer, sum, (length - sum))) > 0))
                {
                    sum = (sum + count);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }
    }
}
