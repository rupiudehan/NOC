using Microsoft.Extensions.Options;
using Noc_App.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace Noc_App.UtilityService
{
    public class PasswordEncryptionService
    {
        private PasswordEncryption _settings;
        public PasswordEncryptionService(IOptions<PasswordEncryption> settings)
        {
            _settings = settings.Value;
        }

        public string Encrypt(string plainText)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportRSAPublicKey(Convert.FromBase64String(_settings.SecretKey), out _);
                var encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(plainText), RSAEncryptionPadding.Pkcs1);
                return Convert.ToBase64String(encryptedData);
            }
        }

        public string Decrypt(string encryptedText)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(_settings.IVvalue), out _);
                var decryptedData = rsa.Decrypt(Convert.FromBase64String(encryptedText), RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        public string EncryptPassword(string password)
        {
            try
            {
                string secretKeyBase64 = _settings.SecretKey;
                string ivBase64 = _settings.IVvalue;

                byte[] secretKeyBytes = Convert.FromBase64String(secretKeyBase64);
                byte[] ivBytes = Convert.FromBase64String(ivBase64);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = secretKeyBytes;
                    aesAlg.IV = ivBytes;
                    aesAlg.Padding = PaddingMode.PKCS7;  // Ensure padding mode is set

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(password); // Write the password to be encrypted
                            }
                        }
                        byte[] encrypted = msEncrypt.ToArray();
                        return Convert.ToBase64String(encrypted); // Return Base64-encoded encrypted data
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Encryption failed: {ex.Message}";
            }
        }


        public string DecryptPassword2(string encryptedPassword)
        {
            try
            {
                string secretKeyBase64 = _settings.SecretKey;
                string ivBase64 = _settings.IVvalue;

                byte[] secretKeyBytes = Convert.FromBase64String(secretKeyBase64);
                byte[] ivBytes = Convert.FromBase64String(ivBase64);

                // Make sure padding is correct for Base64
                string validEncryptedPassword = encryptedPassword.PadRight(encryptedPassword.Length + (4 - encryptedPassword.Length % 4) % 4, '=');

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = secretKeyBytes;
                    aesAlg.IV = ivBytes;
                    aesAlg.Padding = PaddingMode.PKCS7;  // Match encryption padding

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(validEncryptedPassword)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd(); // Decrypted password
                            }
                        }
                    }
                }
            }
            catch (FormatException fe)
            {
                return "Decryption error: Invalid Base64 string format.";
            }
            catch (CryptographicException ce)
            {
                return "Decryption error: Invalid key, IV, or padding.";
            }
            catch (Exception ex)
            {
                return $"Decryption error: {ex.Message}";
            }
        }
        public string DecryptPassword(string encryptedText)
        {
            try
            {
                //Secret Key.
                string secretKey = _settings.SecretKey;

                //Secret Bytes.
                byte[] secretBytes = Encoding.UTF8.GetBytes(secretKey);

                //Encrypted Bytes.
                byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

                //Decrypt with AES Alogorithm using Secret Key.
                using (Aes aes = Aes.Create())
                {
                    aes.Key = secretBytes;
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7;

                    byte[] decryptedBytes = null;
                    using (ICryptoTransform decryptor = aes.CreateDecryptor())
                    {
                        decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    }

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch (FormatException)
            {
                return "Decryption error: Invalid Base64 format.";
            }
            catch (CryptographicException)
            {
                return "Decryption error: Invalid key, IV, or padding.";
            }
            catch (Exception ex)
            {
                return $"Decryption error: {ex.Message}";
            }
        }
        public string Decrypt3(string cypherText)
        {
            try
            {


                //function encryptPassword(password)
                //{
                //    const secretKey = '@PasswordEncryptConfig.Value.SecretKey';
                //    var key = CryptoJS.enc.Utf8.parse('@PasswordEncryptConfig.Value.SecretKey');
                //    var iv = CryptoJS.enc.Utf8.parse('@PasswordEncryptConfig.Value.IVvalue');
                //    // Encrypt the password using the secret key
                //    const encryptedPassword = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(password), secretKey, {
                //    keySize: 128,
                //iv: iv,
                //mode: CryptoJS.mode.CBC,
                //padding: CryptoJS.pad.Pkcs7
                //    });
                //    return encryptedPassword.toString();
                //}

                var key = Encoding.UTF8.GetBytes("1234567890789456"); // Must be 16 bytes for AES-128
                var iv = Encoding.UTF8.GetBytes("7894561230123456"); // Must be 16 bytes for AES
                byte[] encryptedBytes = Convert.FromBase64String(cypherText);

                using (var aes = new RijndaelManaged())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.KeySize = 128; // Set explicitly if needed
                    aes.BlockSize = 128; // Set block size for AES
                    aes.Key = key;
                    aes.IV = iv;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var msDecrypt = new MemoryStream(encryptedBytes))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
            catch (FormatException)
            {
                return "Decryption error: Invalid Base64 format.";
            }
            catch (CryptographicException)
            {
                return "Decryption error: Invalid key, IV, or padding.";
            }
            catch (Exception ex)
            {
                return $"Decryption error: {ex.Message}";
            }
        }


    }
}
