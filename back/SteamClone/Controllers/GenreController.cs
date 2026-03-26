using Microsoft.AspNetCore.Mvc;
using SteamClone.API.Extensions;
using SteamClone.BLL.Dtos.Genre;
using SteamClone.BLL.Services;

namespace SteamClone.API.Controllers
{
    [ApiController]
    [Route("api/genre")]
    public class GenreController : ControllerBase
    {
        private readonly GenreService _genreService;

        public GenreController(GenreService genreService)
        {
            _genreService = genreService;
        }

        // GET api/genre
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _genreService.GetAllAsync();
            return this.GetResult(response);
        }

        // GET api/genre/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var response = await _genreService.GetByIdAsync(id);
            return this.GetResult(response);
        }

        // POST api/genre
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateGenreDto dto)
        {
            var response = await _genreService.CreateAsync(dto);
            return this.GetResult(response);
        }

        // PUT api/genre
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateGenreDto dto)
        {
            var response = await _genreService.UpdateAsync(dto);
            return this.GetResult(response);
        }

        // DELETE api/genre/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _genreService.DeleteAsync(id);
            return this.GetResult(response);
        }
    }
}
