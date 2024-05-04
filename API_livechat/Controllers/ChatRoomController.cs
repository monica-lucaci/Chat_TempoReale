using API_livechat.DTO;
using API_livechat.Filter;
using API_livechat.Services;
using API_livechat.Utils;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace API_livechat.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class ChatRoomController : Controller
    {
        #region service
        private readonly ChatRoomService _service;
        private ChatRoomController(ChatRoomService service)
        {
            _service = service;
        }
        #endregion

        [HttpPost]
        [AuthorizeUserRole("ROLE")]
        public IActionResult NewChatRoom(ChatRoomDTO newRoom)
        {
            try
            {
                if(User.Claims.FirstOrDefault(u => u.Type == "Username")?.Value != null)
                {
                    string username = User.Claims.First(u => u.Type == "Username").Value;
                    if (_service.Insert(newRoom, username))
                        return Ok(new Response() { Status = "SUCCESS" });
                }
            }catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }

            return BadRequest();
        }

        [HttpGet("chat/{chatRoomId}")]
        public IActionResult GetRoomAndMessages(string chatRoomId)
        {
            return Ok(new Response()
            {
                Status = "SUCCESS",
                Data = _service.GetById(new ObjectId(chatRoomId))
            });
        }

        [HttpPost("chat/addUser/{id}")]
        public IActionResult AddUserToChatRoom(string id, string username)
        {
            try
            {
                if (_service.InsertUserIntoChatRoom(username, new ObjectId(id))) return Ok(new Response() { Status = "SUCCESS" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }
        [HttpGet("userOfRoom/{id}")]
        public IActionResult GetUserByRoom(string chatRoomId)
        {
            return Ok(new Response()
            {
                Status = "SUCCESS",
                Data = _service.GetUsersByChatRoom(new ObjectId(chatRoomId))
            });
        }
    }
}
