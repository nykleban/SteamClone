// UpdateGameDto.cs
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SteamClone.BLL.Dtos.Game
{
    public class UpdateGameDto
    {
        [Required(ErrorMessage = "Id є обов'язковим")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва є обов'язковою")]
        public string Name { get; set; } = string.Empty;

        public DateTime ReleaseDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Ціна не може бути від'ємною")]
        public decimal Price { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Id розробника є обов'язковим")]
        public int DeveloperId { get; set; }

        public List<int> Genres { get; set; } = [];

        // null = не змінювати превью
        public IFormFile? PreviewImage { get; set; }

        // порожній список = не змінювати скріншоти
        public List<IFormFile> Images { get; set; } = [];
    }
}