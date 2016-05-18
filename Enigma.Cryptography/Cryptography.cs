using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Enigma.Cryptography
{
    public class Cryptography
    {
        private readonly SymmetricAlgorithm _mAesProvider;

        public Cryptography(byte[] bKeyBytes)
        {
            if (bKeyBytes.Length != 32) throw new CryptographicException("key length must be 256 bits!");

            _mAesProvider = new AesCryptoServiceProvider
            {
                KeySize = 256,
                Key = bKeyBytes
            };
        }
        public Cryptography(string sPassword, string sSalt)
        {
            if (sPassword.Length < 8) throw new CryptographicException("password must be at least 8 characters long!");
            if (sSalt.Length < 8) throw new CryptographicException("salt must be at least 8 characters long!");

            _mAesProvider = new AesCryptoServiceProvider
            {
                KeySize = 256
            };

            SHA256 sha256 = SHA256.Create();
            byte[] bKeyBytes = sha256.ComputeHash(Encoding.Unicode.GetBytes(sPassword + sSalt));
            _mAesProvider.Key = bKeyBytes;
        }

        public string EncryptAes(string sPlainData)
        {
            byte[] bDataBuffer = Encoding.Unicode.GetBytes(sPlainData);

            _mAesProvider.GenerateIV();

            ICryptoTransform pEncCryptoTransform = _mAesProvider.CreateEncryptor();
            byte[] bDataEncryptedBytes = pEncCryptoTransform.TransformFinalBlock(bDataBuffer, 0, bDataBuffer.Length);
            bDataEncryptedBytes = _mAesProvider.IV.Concat(bDataEncryptedBytes).ToArray();

            return Convert.ToBase64String(bDataEncryptedBytes);
        }
        public string DecryptAes(string sEncryptedData)
        {
            byte[] bDataEncryptedBytes = Convert.FromBase64String(sEncryptedData);

            _mAesProvider.IV = bDataEncryptedBytes.Take(16).ToArray();

            ICryptoTransform pDecCryptoTransform = _mAesProvider.CreateDecryptor();
            byte[] bDataDecryptedBytes = pDecCryptoTransform.TransformFinalBlock(bDataEncryptedBytes, 16, bDataEncryptedBytes.Length - 16);

            return Encoding.Unicode.GetString(bDataDecryptedBytes);
        }

        //-------------------------------------------------------------------------------------
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
            byte[] bDataEncryptedBytes = Convert.FromBase64String(sEncryptedData);

            DESCryptoServiceProvider aes = new DESCryptoServiceProvider
            {
                Key = Convert.FromBase64String(sKey),
                IV = Convert.FromBase64String(sKey)
            };

            ICryptoTransform pDecCryptoTransform = aes.CreateDecryptor();
            byte[] bDataDecryptedBytes = pDecCryptoTransform.TransformFinalBlock(bDataEncryptedBytes, 0, bDataEncryptedBytes.Length);
            pDecCryptoTransform.Dispose();
            
            return Encoding.ASCII.GetString(bDataDecryptedBytes);
        }
    }
}