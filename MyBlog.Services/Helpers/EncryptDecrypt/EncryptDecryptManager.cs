using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace MyBlog.Service.Helpers.EncryptDecrypt;

public class EncryptDecryptManager: IEncryptDecryptManager
{
    private readonly string? _key;
    
    public EncryptDecryptManager(IConfiguration config)
    {
        _key = config["PasswordKey:Key"];
    }
    
    public string Encrypt(string text)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (var aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(_key);
            aes.IV = iv;
            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (var ms = new MemoryStream())
            {
                using(var cryptoStream = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(text);
                    }
        
                    array = ms.ToArray();
                }
            }
        }
        
        return Convert.ToBase64String(array);
    }

    public string Decrypt(string text)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(text);
        
        using var aes = Aes.Create();
        aes.Key = Encoding.UTF8.GetBytes(_key);
        aes.IV = iv;
        
        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        
        using var ms = new MemoryStream(buffer);
        using var cryptoStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cryptoStream);
        
        return sr.ReadToEnd();
    }
}