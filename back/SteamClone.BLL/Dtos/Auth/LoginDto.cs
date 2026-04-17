using System;
using System.Collections.Generic;
using System.Text;

namespace SteamClone.BLL.Dtos.Auth
{
    public class LoginDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
