using Microsoft.AspNetCore.Mvc;
using SteamClone.BLL.Services;

namespace SteamClone.API.Extensions
{
    public static class ControllerBaseExtesions
    {
        public static IActionResult GetResult(this ControllerBase controller, ServiceResponse response)
        {
            return response.IsSuccess
                ? controller.Ok(response)
                : controller.BadRequest(response);
        }
    }
}