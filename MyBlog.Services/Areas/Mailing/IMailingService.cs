namespace MyBlog.Service.Areas.Mailing;

public interface IMailingService
{
    public Task SendEmailToSubscribedUsersAsync(string message);
    
    public Task SendEmailToUserAsync(string recipientEmail, string message);
    
    public Task SubscribeAsync(int id);
    
    public Task UnsubscribeAsync(int id);
}