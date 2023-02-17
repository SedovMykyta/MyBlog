namespace MyBlog.Service.Helpers.EncryptDecrypt;

public interface IEncryptDecryptManager
{
    public string Encrypt(string text);

    public string Decrypt(string text);
}