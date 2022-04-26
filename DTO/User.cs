using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace core_6_api_token.DTO
{
    public class User
    {
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSault { get; set; }
    }
}