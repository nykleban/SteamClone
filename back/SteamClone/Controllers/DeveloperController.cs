using Microsoft.AspNetCore.Mvc;
using SteamClone.API.Extensions;
using SteamClone.BLL.Dtos.Developer;
using SteamClone.BLL.Services;

namespace SteamClone.API.Controllers
{
    [ApiController]
    [Route("api/developer")]
    public class DeveloperController : ControllerBase
    {
        private readonly DeveloperService _developerService;

        public DeveloperController(DeveloperService developerService)
        {
            _developerService = developerService;
        }

        // api/developer
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var response = await _developerService.GetAllAsync();
            return this.GetResult(response);
        }

        // api/developer/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var response = await _developerService.GetByIdAsync(id);
            return this.GetResult(response);
        }

        // POST api/developer
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] CreateDeveloperDto dto)
        {
            var response = await _developerService.CreateAsync(dto);
            return this.GetResult(response);
        }

        // PUT api/developer
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromForm] UpdateDeveloperDto dto)
        {
            var response = await _developerService.UpdateAsync(dto);
            return this.GetResult(response);
        }

        // DELETE api/developer/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var response = await _developerService.DeleteAsync(id);
            return this.GetResult(response);
        }
    }

}
