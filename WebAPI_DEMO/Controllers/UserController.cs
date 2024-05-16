using Application.Features.UserFeatures.Commands;
using Application.Features.UserFeatures.Queries;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Demo.Filters.Authorizations;

namespace WebAPI_Demo.Controllers
{
    [Route("api/user/")]
    
    public class UserController : BaseApiController
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [Route("logout")]
        [IgnoreAntiforgeryToken]
        [ApplicationAuthorizedAttribute]
        public async Task<IActionResult> Logout(LogoutCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        
        [HttpGet]
        [Route("getme")]
        [IgnoreAntiforgeryToken]
        [ApplicationAuthorizedAttribute]
        public async Task<IActionResult> GetMe([FromQuery] GetmeQuery command)
        {
            return Ok(await Mediator.Send(command));
        }

    }
}
