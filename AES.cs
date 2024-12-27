using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class AESExample
{
    public static string Encrypt(string plainText, string key)  // Шифрування повідомлення
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[16];

            using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
            {
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    public static string Decrypt(string cipherText, string key) // Розшифрування повідомлення
    {
        using (Aes aesAlg = Aes.Create())
        {
            aesAlg.Key = Encoding.UTF8.GetBytes(key);
            aesAlg.IV = new byte[16];

            using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
            {
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

    static void Main()
    {
        string key = "mysecretkey12345";  // Ключ

        Console.Write("Введіть повідомлення для шифрування: ");
        string originalMessage = Console.ReadLine();

        string encryptedMessage = Encrypt(originalMessage, key);
        Console.WriteLine("Зашифроване повідомлення: " + encryptedMessage);

        string decryptedMessage = Decrypt(encryptedMessage, key);
        Console.WriteLine("Розшифроване повідомлення: " + decryptedMessage);
    }
}