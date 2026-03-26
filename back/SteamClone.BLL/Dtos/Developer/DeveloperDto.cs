using System;
using System.Collections.Generic;
using System.Text;

namespace SteamClone.BLL.Dtos.Developer
{
    public class DeveloperDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }
    }
}
