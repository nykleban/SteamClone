using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SteamClone.BLL.Dtos.Developer
{
    public class UpdateDeveloperDto
    {
        [Required(ErrorMessage = "Id є обов'язковим")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ім'я є обов'язковим")]
        public string Name { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}
