using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Hemrika.SharePresence.WebSite.Modules.GateKeeper
{
    public class Encryption
    {
        /// <summary>
        /// Read more about DPAPI at http://msdn.microsoft.com/en-us/library/ms995355.aspx
        /// </summary>
        private static string encryptionKey = Environment.MachineName;

        public static string Encrypt(string plaintext)
        {
            //GateKeeperModule.log.Debug("Entering Encryption | Encrypt");
            if (string.IsNullOrEmpty(plaintext)) { return string.Empty; }
            try
            {
                byte[] encodedPlaintext = Encoding.UTF8.GetBytes(plaintext);
                byte[] encodedEntropy = Encoding.UTF8.GetBytes(encryptionKey);
                byte[] ciphertext = ProtectedData.Protect(encodedPlaintext, encodedEntropy, DataProtectionScope.LocalMachine);
                return Convert.ToBase64String(ciphertext);
            }
            catch (Exception)
            { return string.Empty; }
        }

        public static string Decrypt(string base64Ciphertext)
        {
            //GateKeeperModule.log.Debug("Entering Encryption | Decrypt");
            if (string.IsNullOrEmpty(base64Ciphertext)) { return string.Empty; }
            try
            {
                byte[] ciphertext = Convert.FromBase64String(base64Ciphertext);
                byte[] encodedEntropy = Encoding.UTF8.GetBytes(encryptionKey);

                byte[] encodedPlaintext = ProtectedData.Unprotect(ciphertext, encodedEntropy, DataProtectionScope.LocalMachine);
                return Encoding.UTF8.GetString(encodedPlaintext);
            }
            catch (Exception)
            { return string.Empty; }
        }
    }
}
