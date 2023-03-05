namespace MyBlog.Service.Areas.Mailing;

public interface IEmailService
{
    public Task SendMessageAsync(string message, string recipientEmail);
}