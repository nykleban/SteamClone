using System.ComponentModel.DataAnnotations;

namespace SteamClone.BLL.Dtos.Genre
{
    public class UpdateGenreDto
    {
        [Required(ErrorMessage = "Id є обов'язковим")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Назва є обов'язковою")]
        public string Name { get; set; } = string.Empty;
    }
}
