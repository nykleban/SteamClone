using Microsoft.AspNetCore.Mvc;
using SteamClone.API.Extensions;
using SteamClone.BLL.Dtos.Game;
using SteamClone.BLL.Services;

namespace SteamClone.API.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        // GET api/game
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _gameService.GetAllAsync();
            return this.GetResult(response);
        }

        // GET api/game/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var response = await _gameService.GetByIdAsync(id);
            return this.GetResult(response);
        }

        // POST api/game
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateGameDto dto)
        {
            var response = await _gameService.CreateAsync(dto);
            return this.GetResult(response);
        }

        // PUT api/game
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateGameDto dto)
        {
            var response = await _gameService.UpdateAsync(dto);
            return this.GetResult(response);
        }

        // DELETE api/game/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _gameService.DeleteAsync(id);
            return this.GetResult(response);
        }
    }
}
