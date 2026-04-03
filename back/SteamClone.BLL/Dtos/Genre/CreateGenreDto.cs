using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SteamClone.BLL.Dtos.Genre
{
    public class CreateGenreDto
    {
        [Required(ErrorMessage = "Назва є обов'язковою")]
        public string Name { get; set; } = string.Empty;
        public IFormFile? Image { get; set; }
    }
}
