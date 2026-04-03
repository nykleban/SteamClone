// GameDto.cs
using SteamClone.BLL.Dtos.Genre;

namespace SteamClone.BLL.Dtos.Game
{
    public class GameDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int DeveloperId { get; set; }
        public List<GenreDto> Genres { get; set; } = [];
        public string? PreviewImage { get; set; }
        public List<string> Images { get; set; } = [];
    }
}