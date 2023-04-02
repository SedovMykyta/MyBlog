using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MyBlog.Infrastructure;
using MyBlog.Infrastructure.Entities.Enum;
using MyBlog.Service.Areas.Mailing.Models;
using MyBlog.Service.Exception;
using MyBlog.Service.Helpers.ConfigurationParser;

namespace MyBlog.Service.Areas.Mailing;

public class MailingService : IMailingService
{
    private readonly MyBlogContext _context;
    private readonly MailingSettings _mailingSettings;
    private readonly SmtpClient _client;
    
    public MailingService(MyBlogContext context, IConfiguration configuration, IMailingParser mailingParser)
    {
        _context = context;
        _mailingSettings = mailingParser.ParseToMailSettings(configuration);
        
        _client = new SmtpClient();
        _client.ConnectAsync(_mailingSettings.Host, _mailingSettings.Port, _mailingSettings.IsUseSSL).GetAwaiter().GetResult();
        _client.AuthenticateAsync(_mailingSettings.From, _mailingSettings.Password).GetAwaiter().GetResult();
    }

    public async Task SendEmailToSubscribedUsersAsync(string message)
    {
        var recipientEmails = await GetSubscribedEmailsAsync();

        foreach (var email in recipientEmails)
        {
            await SendEmailToUserAsync(email, message);
        }
    }

    public async Task SendEmailToUserAsync(string recipientEmail, string message)
    {
        await ThrowIfEmailNotFound(recipientEmail);

        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(_mailingSettings.DisplayName, _mailingSettings.From));
        mimeMessage.To.Add(new MailboxAddress(string.Empty,recipientEmail));
        mimeMessage.Subject = message;
        mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };

        await _client.SendAsync(mimeMessage);
    }
    
    public async Task SubscribeAsync(int id)
    {
        var userSubscription = await _context.Subscriptions.FirstOrDefaultAsync(user => user.UserId == id)
                               ?? throw new NotFoundException($"User with Id: {id} not found");

        userSubscription.IsSubscribedToEmail = true;

        _context.Subscriptions.Update(userSubscription);
        await _context.SaveChangesAsync();
    }

    public async Task UnsubscribeAsync(int id)
    {
        var userSubscription = await _context.Subscriptions.FirstOrDefaultAsync(user => user.UserId == id)
                               ?? throw new NotFoundException($"User with Id: {id} not found");

        userSubscription.IsSubscribedToEmail = false;

        _context.Subscriptions.Update(userSubscription);
        await _context.SaveChangesAsync();
    }


    private async Task ThrowIfEmailNotFound(string email)
    {
        if (! await _context.Users.AnyAsync(user => user.Email == email))
        {
            throw new NotFoundException($"Email: {email} not found in DB");
        }
    }

    private async Task<List<string>> GetSubscribedEmailsAsync()
    {
        var subscribedEmails = await _context.Users
            .Include(user => user.Subscription)
            .Where(user => user.Subscription.IsSubscribedToEmail)
            .Where(user => user.Role == Role.User)
            .Select(user => user.Email)
            .ToListAsync();

        return subscribedEmails;
    }
}