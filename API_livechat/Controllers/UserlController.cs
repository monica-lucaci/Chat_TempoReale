using API_livechat.DTO;
using API_livechat.Repositories;
using API_livechat.Services;
using API_livechat.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_livechat.Controllers
{
    [ApiController]
    [Route("User")]
    public class UserlController : Controller
    {
        #region service
        private readonly UserlService _service;

        public UserlController(UserlService service)
        {
            _service = service;
        }
        #endregion

        #region HTTP requests

        [HttpPost("register")]
        public IActionResult Register(UserlDTO user)
        {
            if(user.User.Trim().Equals("") || user.Pass.Trim().Equals("")) {
                return BadRequest(new Status()
                {
                    Stato = "ERROR",
                    Data = "Riempire i campi"
                });
            }

            return Ok(new Status()
            {
                Stato = "SUCCESS",
                Data = _service.Register(user)
            });
        }

        [HttpGet]
        public IActionResult GetListOfUsers(){
            return Ok(new Status()
            {
                Stato = "SUCCESS",
                Data = _service.GetListOfUsers()
            });
        }
        /*
        [HttpPost("update")]
        public IActionResult UpdateUserPassword(UserlDTO user, string newPassword) {
            if (user.User.Trim().Equals(""))
            {
                return BadRequest(new Status()
                {
                    Stato = "SUCCESS",
                    Data = "Il campo è vuoto"
                });
            }

            return Ok(new Status()
            {
                Stato = "SUCCESS",
                Data = _service.UpdateUserPassword(user)
            });
        }
        */
        #endregion


    }
}
