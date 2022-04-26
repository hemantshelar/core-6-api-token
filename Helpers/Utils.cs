using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using core_6_api_token.DTO;
using Microsoft.IdentityModel.Tokens;

namespace core_6_api_token.Helpers
{
  public class Utils
  {

    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSault)
    {
      using (var hmac = new HMACSHA512())
      {

        passwordSault = hmac.Key;
        var passwordInBytes = System.Text.Encoding.UTF8.GetBytes(password);
        passwordHash = hmac.ComputeHash(passwordInBytes);
      }

    }
    public bool VerifyPassword(string password,  byte[] actualHashedPassword,  byte[] passwordSault)
    {
        var result = false;
      using (var hmac = new HMACSHA512(passwordSault))
      {

        passwordSault = hmac.Key;
        var passwordInBytes = System.Text.Encoding.UTF8.GetBytes(password);
        var passwordHash = hmac.ComputeHash(passwordInBytes);

        result = passwordHash.SequenceEqual(actualHashedPassword);
      }

      return result;
    }

    public string CreateToken(User user,string signingToken)
    {
        
        List<Claim> claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name,user.Username));
        claims.Add(new Claim(ClaimTypes.Country,"IND"));

        var signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingToken);
        var key = new SymmetricSecurityKey(signingKeyBytes);

        var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);

        var jwtSecToken = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: cred
        );


        var jwt = new JwtSecurityTokenHandler().WriteToken(jwtSecToken);
        return jwt;
    }
  }
}