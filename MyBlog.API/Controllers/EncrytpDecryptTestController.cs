using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Service.Helpers.EncryptDecrypt;

namespace MyBlog.Controllers;

[Route("api/encrypt-decrypt")]
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
public class EncryptDecryptController : ControllerBase
{
    private readonly IEncryptDecryptManager _encryptDecrypt;

    public EncryptDecryptController(IEncryptDecryptManager encryptDecrypt)
    {
        _encryptDecrypt = encryptDecrypt;
    }

    [HttpGet("encrypt")]
    public string Encrypt(string text)
    {
        var encStr = _encryptDecrypt.Encrypt(text);

        return encStr;
    }

    [HttpGet("decrypt")]
    public string Decrypt(string text)
    {
        var decStr = _encryptDecrypt.Decrypt(text);

        return decStr;
    }
}