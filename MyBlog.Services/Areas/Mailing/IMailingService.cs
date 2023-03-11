namespace MyBlog.Service.Areas.Mailing;

public interface IMailingService
{
    public Task SendEmailToUserAsync(string recipientEmail, string message);

    public Task SendEmailToSubscribedUsersAsync(string message);
    
    public Task SubscribeOnMailingAsync(int id);
    
    public Task UnsubscribeOnMailingAsync(int id);
}