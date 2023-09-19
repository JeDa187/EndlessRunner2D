using UnityEngine;
using System.Text;
using System.Security.Cryptography;
using System;

public static class SecurePlayerPrefs
{
    private static readonly string secretKey = "6D1gJ4kY9wZ2rU3xS8vL0pT5lE7hQ6bN"; // This key is unique.


    // Set a string value in the PlayerPrefs
    public static void SetString(string key, string value)
    {
        // Encrypt the value
        string encryptedValue = Encrypt(value, secretKey);

        // Save the encrypted value in PlayerPrefs
        PlayerPrefs.SetString(key, encryptedValue);
    }

    // Get a string value from the PlayerPrefs
    public static string GetString(string key, string defaultValue = "")
    {
        if (PlayerPrefs.HasKey(key))
        {
            // Retrieve the encrypted value from PlayerPrefs
            string encryptedValue = PlayerPrefs.GetString(key);

            // Decrypt the value
            string decryptedValue = Decrypt(encryptedValue, secretKey);

            return decryptedValue;
        }
        else
        {
            return defaultValue;
        }
    }

    // Set an integer value in the PlayerPrefs
    public static void SetInt(string key, int value)
    {
        // Convert the integer to string and then encrypt it
        SetString(key, value.ToString());
    }

    // Get an integer value from the PlayerPrefs
    public static int GetInt(string key, int defaultValue = 0)
    {
        string value = GetString(key, defaultValue.ToString());
        return int.Parse(value);
    }
    public static void DeleteKey(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
    }
    public static void Save()
    {
        PlayerPrefs.Save();
    }

    // Helper function to encrypt a string using a given key
    private static string Encrypt(string toEncrypt, string key)
    {
        byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

        // Generate a hash of the given key
        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

        // Set up encryption settings
        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;

        // Perform encryption
        ICryptoTransform cTransform = tdes.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

        return Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    // Helper function to decrypt a string using a given key
    private static string Decrypt(string toDecrypt, string key)
    {
        byte[] toDecryptArray = Convert.FromBase64String(toDecrypt);

        // Generate a hash of the given key
        MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
        byte[] keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

        // Set up decryption settings
        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        tdes.Key = keyArray;
        tdes.Mode = CipherMode.ECB;
        tdes.Padding = PaddingMode.PKCS7;

        // Perform decryption
        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);

        return UTF8Encoding.UTF8.GetString(resultArray);
    }
}
