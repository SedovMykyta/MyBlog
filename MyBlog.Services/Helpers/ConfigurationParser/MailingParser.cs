using Microsoft.Extensions.Configuration;
using MyBlog.Service.Areas.Mailing.Models;

namespace MyBlog.Service.Helpers.ConfigurationParser;

public class MailingParser : IMailingParser
{
    public MailingSettings ParseToMailSettings(IConfiguration configuration)
    {
        int.TryParse(configuration["MailSettings:Port"], out int port);
        bool.TryParse(configuration["MailSettings:UseSSL"], out bool isUseSSL);

        return new MailingSettings
        {
            DisplayName = configuration["MailSettings:DisplayName"]!,
            From = configuration["MailSettings:From"]!,
            Host = configuration["MailSettings:Host"]!,
            Password = configuration["MailSettings:Password"]!,
            Port = port,
            IsUseSSL = isUseSSL
        };
    }
}