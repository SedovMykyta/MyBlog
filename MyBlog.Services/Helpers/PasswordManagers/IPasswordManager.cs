namespace MyBlog.Service.Helpers.PasswordManagers;

public interface IPasswordManager
{
    public string Encrypt(string text);

    public string Decrypt(string text);
}