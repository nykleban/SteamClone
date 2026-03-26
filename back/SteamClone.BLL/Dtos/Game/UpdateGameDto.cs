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

        public List<int> GenreIds { get; set; } = [];
    }
}
