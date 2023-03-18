using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities.Enum;
using MyBlog.Service.Areas.Mailing.Models;
using MyBlog.Service.Exception;
using MyBlog.Service.Helpers.ExtensionMethods;

namespace MyBlog.Service.Areas.Mailing;

public class MailingService : IMailingService
{
    private readonly MailSettings _settings;
    private readonly MyBlogContext _context;
    
    public MailingService(IOptions<MailSettings> settings, MyBlogContext context)
    {
        _context = context;
        _settings = settings.Value;
    }

    public async Task SendEmailToSubscribedUsersAsync(string message)
    {
        var recipientEmails = await GetSubscribedEmailsAsync();

        foreach (var email in recipientEmails)
        {
            await SendEmailToUserAsync(message, email);
        }
    }

    public async Task SendEmailToUserAsync(string recipientEmail, string message)
    {
        await ThrowIfEmailNotFound(recipientEmail);
        
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
        mimeMessage.To.Add(new MailboxAddress("",recipientEmail));
        mimeMessage.Subject = message;
        mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using (var client = new SmtpClient()) 
        {
            await client.ConnectAsync(_settings.Host, _settings.Port, _settings.UseSSL);
            await client.AuthenticateAsync(_settings.From, _settings.Password );
            await client.SendAsync(mimeMessage);

            await client.DisconnectAsync(true);
        }
    }
    
    public async Task SubscribeAsync(int id)
    {
        var userSubscription = await _context.UserSubscriptions.FirstOrDefaultAsync(user => user.UserId == id)
                               ?? throw new NotFoundException($"User with Id: {id} not found");

        userSubscription.IsSubscribedToEmail = true;

        _context.UserSubscriptions.Update(userSubscription);
        await _context.SaveChangesAsync();
    }

    public async Task UnsubscribeAsync(int id)
    {
        var userSubscription = await _context.UserSubscriptions.FirstOrDefaultAsync(user => user.UserId == id)
                               ?? throw new NotFoundException($"User with Id: {id} not found");

        userSubscription.IsSubscribedToEmail = false;

        _context.UserSubscriptions.Update(userSubscription);
        await _context.SaveChangesAsync();
    }


    private async Task ThrowIfEmailNotFound(string email)
    {
        if (!await _context.Users.AnyAsync(user => user.Email == email))
        {
            throw new NotFoundException($"Email: {email} not found in DB");
        }
    }

    private async Task<List<string>> GetSubscribedEmailsAsync()
    {
        var emailsSubscribedUsers = await _context.Users.
            Include(user => user.Subscription).
            Where(user => user.Subscription.IsSubscribedToEmail).
            Where(user => user.Role == Role.User).
            Select(user => user.Email).
            ToListAsync();

        return emailsSubscribedUsers;
    }
}