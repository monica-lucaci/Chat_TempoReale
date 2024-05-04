using API_livechat.DTO;
using API_livechat.Filter;
using API_livechat.Services;
using API_livechat.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_livechat.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class MessageController : Controller
    {
        #region service
        private readonly MessageService _service;
        public MessageController(MessageService service)
        {
            _service = service;
        }
        #endregion

        [HttpPost("sendMessage/{ChatRoomId}")]
        [AuthorizeUserRole("USER")]
        public IActionResult InsertMessage(MessageDTO m, string ChatRoomId)
        {
            try
            {
                var us = User.Claims.FirstOrDefault(u => u.Type == "Username")?.Value;
                if (us != null) {
                    m.Sder = User.Claims.First(u => u.Type == "Username").Value;
                    m.RRIF = new MongoDB.Bson.ObjectId(ChatRoomId);
                    if (_service.InsertMessage(m))
                        return Ok(new Response() { Status = "SUCCESS" });
                }
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }
    }
}
