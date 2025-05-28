﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace UserManagementService.Utility
{
    public class EncDecHelper
    {
        public static string Encrypt(string input, string EncryptionKey = null)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                // string key = UMSResources.GetEncryptionKey("encryptionKey");
                string key = "umsumstLoanNcrKe";
                if (!string.IsNullOrWhiteSpace(key))
                {
                    byte[] inputArray = Encoding.UTF8.GetBytes(input);
                    TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                    tripleDES.Key = Encoding.UTF8.GetBytes(key);
                    tripleDES.Mode = CipherMode.ECB;
                    tripleDES.Padding = PaddingMode.PKCS7;
                    ICryptoTransform cTransform = tripleDES.CreateEncryptor();
                    byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                    tripleDES.Clear();
                    result = Convert.ToBase64String(resultArray, 0, resultArray.Length);
                }
            }
            return result;
        }

        public static string Decrypt(string input, string EncryptionKey = null)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(input))
            {
                try
                {
                    // string key = UMSResources.configuration.GetValue<string>("encryptionKey");
                    // string key = UMSResources.GetEncryptionKey("encryptionKey");
                    string key = "umsumstLoanNcrKe";
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        byte[] inputArray = Convert.FromBase64String(input);
                        TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                        tripleDES.Key = Encoding.UTF8.GetBytes(key);
                        tripleDES.Mode = CipherMode.ECB;
                        tripleDES.Padding = PaddingMode.PKCS7;
                        ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                        byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                        tripleDES.Clear();
                        result = Encoding.UTF8.GetString(resultArray);
                    }
                }
                catch (Exception ex)
                {
                    result = string.Empty;
                }
            }
            return result;
        }
    }
}
