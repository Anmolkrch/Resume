using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;


namespace Tastehub.Utility.Helper
{
    public static class SecurityHelper
    {
        #region Utilities

        private static byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            var mStream = new MemoryStream();
            var cStream = new CryptoStream(mStream, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write);
            byte[] toEncrypt = new UnicodeEncoding().GetBytes(data);
            cStream.Write(toEncrypt, 0, toEncrypt.Length);
            cStream.FlushFinalBlock();
            byte[] ret = mStream.ToArray();
            cStream.Close();
            mStream.Close();
            return ret;
        }

        private static string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            var msDecrypt = new MemoryStream(data);
            var csDecrypt = new CryptoStream(msDecrypt, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read);
            var sReader = new StreamReader(csDecrypt, new UnicodeEncoding());
            return sReader.ReadLine();
        }

        #endregion
        /// <summary>
        /// Creates a password hash
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="salt">Salt</param>
        /// <returns>Password hash</returns>
        public static string CreatePasswordHash(string password, string salt)
        {
            ////MD5, SHA1
            //string passwordFormat = IoC.Resolve<ISettingManager>().GetSettingValue("Security.PasswordFormat");
            //if (String.IsNullOrEmpty(passwordFormat))
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(password));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();

            //string passwordFormat = "SHA1";
            //string passwordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password + salt, passwordFormat);
            //return passwordHash;
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        /// <summary>
        /// Creates a salt
        /// </summary>
        /// <param name="size">A salt size</param>
        /// <returns>A salt</returns>
        public static string CreateSalt(int size)
        {
            var provider = new RNGCryptoServiceProvider();
            byte[] data = new byte[size];
            provider.GetBytes(data);
            return Convert.ToBase64String(data);
        }
        #region Methods
        /// <summary>
        /// Decrypts text
        /// </summary>
        /// <param name="cipherText">Cipher text</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string cipherText)
        {
            string encryptionPrivateKey = "1234567890123451";// GetSettingValue("Security.EncryptionPrivateKey");
            return Decrypt(cipherText, encryptionPrivateKey);
        }

        /// <summary>
        /// Decrypts text
        /// </summary>
        /// <param name="cipherText">Cipher text</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Decrypted string</returns>
        public static string Decrypt(string cipherText, string encryptionPrivateKey)
        {
            if (String.IsNullOrEmpty(cipherText))
                return cipherText;

            var tDESalg = new TripleDESCryptoServiceProvider();
            tDESalg.Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16));
            tDESalg.IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8));

            byte[] buffer = Convert.FromBase64String(cipherText);
            string result = DecryptTextFromMemory(buffer, tDESalg.Key, tDESalg.IV);
            return result;
        }
        /// <summary>
        /// Encrypts text
        /// </summary>
        /// <param name="plainText">Plaint text</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string plainText)
        {
            string encryptionPrivateKey = "1234567890123451"; //GetSettingValue("Security.EncryptionPrivateKey");
            return Encrypt(plainText, encryptionPrivateKey);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public static string CreateMD5(string TextToHash)
        {
            //Check wether data was passed
            MD5 md5 = MD5CryptoServiceProvider.Create();
            byte[] dataMd5 = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(TextToHash));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dataMd5.Length; i++)
                sb.AppendFormat("{0:x2}", dataMd5[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Encrypts text
        /// </summary>
        /// <param name="plainText">Plaint text</param>
        /// <param name="encryptionPrivateKey">Encryption private key</param>
        /// <returns>Encrypted string</returns>
        public static string Encrypt(string plainText, string encryptionPrivateKey)
        {
            if (String.IsNullOrEmpty(plainText))
                return plainText;

            var tDESalg = new TripleDESCryptoServiceProvider();
            tDESalg.Key = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(0, 16));
            tDESalg.IV = new ASCIIEncoding().GetBytes(encryptionPrivateKey.Substring(8, 8));

            byte[] encryptedBinary = EncryptTextToMemory(plainText, tDESalg.Key, tDESalg.IV);
            string result = Convert.ToBase64String(encryptedBinary);
            return result;
        }

        public static string ConvertStringToHex(String input, System.Text.Encoding encoding)
        {
            Byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }
        public static string ConvertHexToString(String hexInput, System.Text.Encoding encoding)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }
        #endregion
    }
}
