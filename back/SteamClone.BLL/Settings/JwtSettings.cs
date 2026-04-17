using System;
using System.Collections.Generic;
using System.Text;

namespace SteamClone.BLL.Settings
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpireHours { get; set; }
    }
}
