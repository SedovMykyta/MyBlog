namespace MyBlog.Service.Helpers.EncryptDecrypt;

public interface IPasswordManager
{
    public string Encrypt(string text);

    public string Decrypt(string text);
}