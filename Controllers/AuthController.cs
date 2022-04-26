using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using core_6_api_token.DTO;
using core_6_api_token.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace core_6_api_token.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class AuthController : Controller
  {
    private readonly ILogger<AuthController> _logger;
    private readonly Utils utils;
    private readonly IConfiguration config;
    public static User user = new User();

    public AuthController(
        ILogger<AuthController> logger, 
        Utils utils,
        IConfiguration config)
    {
      _logger = logger;
      this.utils = utils;
      this.config = config;
    }

    [HttpPost("Register")]
    public async Task<ActionResult> Register(UserDto userDto)
    {
      this.utils.CreatePasswordHash(userDto.Password, out byte[] passwordHash, out byte[] passwordSault);
      user.PasswordHash = passwordHash;
      user.PasswordSault = passwordSault;
      user.Username = userDto.Username;
      return Ok(user);
    }



    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login(UserDto userDto)
    {
        var token = "token1";
        var signingKey = config.GetSection("AppSettings:SigningKey").Value;

        var result = utils.VerifyPassword(userDto.Password,user.PasswordHash,user.PasswordSault);

        token = utils.CreateToken(user,signingKey);

        return token;
    }
  }
}