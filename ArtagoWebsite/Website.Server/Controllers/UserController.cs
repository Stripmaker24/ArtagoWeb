using Appication.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Website.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        [HttpGet]
        public async Task<ActionResult<UserListSimpleDto>> Get() 
        {
            return Ok(await Mediator.Send(new ListUsersQuery()));
        }
    }
}
