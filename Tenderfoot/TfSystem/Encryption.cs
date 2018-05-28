using System;
using Tenderfoot.TfSystem.Diagnostics;
using Tenderfoot.Tools;

namespace Tenderfoot.TfSystem
{
    public static class Encryption
    {
        public static string Encrypt(string input, bool getFromConfig = false)
        {
            if (getFromConfig && TfSettings.Encryption.Active == false)
            {
                return input;
            }

            StringCipher stringCipher = new StringCipher
            {
                PasswordHash = TfSettings.Encryption.PasswordHash,
                SaltKey = TfSettings.Encryption.SaltKey,
                VIKey = TfSettings.Encryption.VIKey
            };

            try
            {
                return stringCipher.Encrypt(input);
            }
            catch
            {
                TfDebug.WriteLog(
                    TfSettings.Logs.System,
                    $"Ignored Invalid Ecryption - {DateTime.Now}",
                    $"Value: {input}{Environment.NewLine}");

                return null;
            }
        }

        public static string Decrypt(string input, bool getFromConfig = false)
        {
            if (getFromConfig && TfSettings.Encryption.Active == false)
            {
                return input;
            }

            StringCipher stringCipher = new StringCipher
            {
                PasswordHash = TfSettings.Encryption.PasswordHash,
                SaltKey = TfSettings.Encryption.SaltKey,
                VIKey = TfSettings.Encryption.VIKey
            };
            
            try
            {
                return stringCipher.Decrypt(input);
            }
            catch
            {
                TfDebug.WriteLog(
                    TfSettings.Logs.System,
                    $"Ignored Invalid Decryption - {DateTime.Now}",
                    $"Value: {input}{Environment.NewLine}");

                return null;
            }
        }
    }
}
