namespace MyBlog.Service.Areas.Mailing;

public interface IMailingService
{
    public Task SendEmailToUserAsync(string message, string recipientEmail);

    public Task SendEmailToSubscribedUsersAsync(string message);

    public Task ChangeSubscribeOnMailingAsync(bool isSubscribeToEmail, int id);
}