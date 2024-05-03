using API_livechat.DTO;
using API_livechat.Filter;
using API_livechat.Repositories;
using API_livechat.Services;
using API_livechat.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

            if(_service.Register(user))
            {
                return Ok(new Response()
                {
                    Status = "SUCCESS",
                    Data = _service.GetByUsername(user)
                });
            }
            else
            {
                return BadRequest(new Response()
                {
                    Status = "ERROR",
                    Data = "Registrazione non effettuata"
                });
            }
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

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(UserDTO userDTO, string newPassword) {
            if (userDTO.User.Trim().Equals(""))
            {
                return BadRequest(new Response()
                {
                    Status = "ERROR",
                    Data = "Il campo è vuoto"
                });
            }
            if (_service.CheckUserLog(userDTO))
            {
                return Ok(new Response()
                {
                    Status = "SUCCESS",
                    Data = _service.UpdateUserPassword(userDTO, newPassword)
                });
            }else
            {
                return Ok(new Response()
                {
                    Status = "ERROR",
                    Data = "Utente non esistente o credenziali errate"
                });
            }
        }

        #endregion


    }
}
