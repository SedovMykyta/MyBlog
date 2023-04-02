namespace MyBlog.Service.Areas.Mailing.Models;

public class MailingSettings
{
    public string DisplayName { get; set; }
    public string From { get; set; }
    public string Host { get; set; }
    public string Password { get; set; }
    public int Port { get; set; }
    public bool IsUseSSL { get; set; }
}