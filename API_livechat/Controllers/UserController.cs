using API_livechat.DTO;
using API_livechat.Filter;
using API_livechat.Repositories;
using API_livechat.Services;
using API_livechat.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_livechat.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserController : Controller
    {
        #region service
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }
        #endregion

        #region HTTP requests

        [HttpPost("register")]
        public IActionResult Register([FromBody]UserDTO user)
        {
            if(user.User.Trim().Equals("") || user.Pass.Trim().Equals("")) {
                return BadRequest(new Response()
                {
                    Status = "ERROR",
                    Data = "Riempire i campi"
                });
            }

            return Ok(new Response()
            {
                Status = "SUCCESS",
                Data = _service.Register(user)
            });
        }

        [HttpGet("ListOfUsers")]
        public IActionResult GetListOfUsers(){
            return Ok(new Response()
            {
                Status = "SUCCESS",
                Data = _service.GetListOfUsers()
            });
        }

        [HttpGet("UserProfile")]
        [AuthorizeUserRole("USER")]
        public IActionResult GetUserProfile()
        {
            var name = User.Claims.FirstOrDefault(n => n.Type == "Username")?.Value;

            if(name != null)
            {
                return Ok(new Response()
                {
                    Status = "SUCCESS",
                    Data = _service.GetUser(name)
                });
            }
            return Ok(new Response()
            {
                Status = "ERROR"
            });
        }

        /*
        [HttpPost("update")]
        public IActionResult UpdateUserPassword(UserlDTO user, string newPassword) {
            if (user.User.Trim().Equals(""))
            {
                return BadRequest(new Status()
                {
                    Status = "SUCCESS",
                    Data = "Il campo è vuoto"
                });
            }

            return Ok(new Status()
            {
                Status = "SUCCESS",
                Data = _service.UpdateUserPassword(user)
            });
        }
        */
        #endregion


    }
}
