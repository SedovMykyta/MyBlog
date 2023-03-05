using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace MyBlog.Service.Areas.Mailing;

public class EmailService : IEmailService
{
    private readonly string _email;
    private readonly string _password;

    public EmailService(IConfiguration config)
    {
        _email = config["EmailInfo:Email"]!;
        _password = config["EmailInfo:Password"]!;
    }

    public async Task SendMessageAsync(string message, string recipientEmail)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress("My blog", _email));
        mimeMessage.To.Add(new MailboxAddress("",recipientEmail));
        mimeMessage.Subject = message;
        mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };

        using (var client = new SmtpClient()) 
        {
            await client.ConnectAsync("smtp.gmail.com", 465, true);
            await client.AuthenticateAsync(_email, _password);
            await client.SendAsync(mimeMessage);

            await client.DisconnectAsync(true);
        }
    } 
}