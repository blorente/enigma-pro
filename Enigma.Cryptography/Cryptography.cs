using System;
using System.Security.Cryptography;
using System.Text;

namespace Enigma.Cryptography
{
    public class Cryptography
    {
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr destination, int length);

        public static string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DES.Create();
            return Convert.ToBase64String(desCrypto.Key);
        }

        public static string AES_Encrypt(string sPlainData, string sKey)
        {
            byte[] bDataBuffer = Encoding.ASCII.GetBytes(sPlainData);

            DESCryptoServiceProvider aes = new DESCryptoServiceProvider
            {
                Key = Convert.FromBase64String(sKey),
                IV = Convert.FromBase64String(sKey)
            };

            ICryptoTransform pEncCryptoTransform = aes.CreateEncryptor();
            byte[] bDataEncryptedBytes = pEncCryptoTransform.TransformFinalBlock(bDataBuffer, 0, bDataBuffer.Length);
            pEncCryptoTransform.Dispose();

            return Convert.ToBase64String(bDataEncryptedBytes);
        }
        public static string AES_Decrypt(string sEncryptedData, string sKey)
        {
            byte[] bDataBuffer = Convert.FromBase64String(sEncryptedData);

            DESCryptoServiceProvider aes = new DESCryptoServiceProvider
            {
                Key = Convert.FromBase64String(sKey),
                IV = Convert.FromBase64String(sKey)
            };

            ICryptoTransform pDecCryptoTransform = aes.CreateDecryptor();
            byte[] bDataDecryptedBytes = pDecCryptoTransform.TransformFinalBlock(bDataBuffer, 0, bDataBuffer.Length);
            pDecCryptoTransform.Dispose();
            
            return Encoding.ASCII.GetString(bDataDecryptedBytes);
        }
    }
}