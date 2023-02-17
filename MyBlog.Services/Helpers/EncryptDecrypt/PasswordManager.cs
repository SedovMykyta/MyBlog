using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MyBlog.Service.Helpers.EncryptDecrypt;

public class PasswordManager: IPasswordManager
{
    private readonly string? _key;
    
    public PasswordManager(IConfiguration config)
    {
        _key = config["PasswordKey:Key"];
    }
    
    public string Encrypt(string text)
    {
        byte[] aesInitializationVector = new byte[16];
        byte[] array;

        using (var aesImplementation = Aes.Create())
        {
            aesImplementation.Key = Encoding.UTF8.GetBytes(_key);
            aesImplementation.IV = aesInitializationVector;
            var encryptor = aesImplementation.CreateEncryptor(aesImplementation.Key, aesImplementation.IV);
            using (var memoryStream = new MemoryStream())
            {
                using(var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(text);
                    }
        
                    array = memoryStream.ToArray();
                }
            }
        }
        
        return Convert.ToBase64String(array);
    }

    public string Decrypt(string text)
    {
        byte[] aesInitializationVector = new byte[16];
        byte[] buffer = Convert.FromBase64String(text);
        
        using var aesImplementation = Aes.Create();
        aesImplementation.Key = Encoding.UTF8.GetBytes(_key);
        aesImplementation.IV = aesInitializationVector;
        
        var decryptor = aesImplementation.CreateDecryptor(aesImplementation.Key, aesImplementation.IV);
        
        using var memoryStream = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(cryptoStream);
        
        return streamReader.ReadToEnd();
    }
}