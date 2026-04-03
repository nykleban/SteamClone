using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SteamClone.BLL.Dtos.Game
{
    public class CreateGameDto
    {
        [Required(ErrorMessage = "Назва є обов'язковою")]
        public string Name { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; } = DateTime.UtcNow;

        [Range(0, double.MaxValue, ErrorMessage = "Ціна не може бути від'ємною")]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Id розробника є обов'язковим")]
        public int DeveloperId { get; set; }

        public List<string> Genres { get; set; } = [];
        public IFormFile? PreviewImage { get; set; }
        public List<IFormFile> Images { get; set; } = [];
    }

}
