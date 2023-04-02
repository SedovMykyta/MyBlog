using Microsoft.Extensions.Configuration;
using MyBlog.Service.Areas.Mailing.Models;

namespace MyBlog.Service.Helpers.ConfigurationParser;

public interface IMailingParser
{
    public MailingSettings ParseToMailSettings(IConfiguration configuration);
}