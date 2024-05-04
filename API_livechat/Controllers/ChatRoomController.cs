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

        [HttpPost("newChatRoom")]
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

        [HttpGet("chat/viewList")]
        public IActionResult GetListChatRooms()
        {
            return Ok(new Response
            {
                Status = "SUCCESS",
                Data = _service.GetAllChatRooms()
            });
        }

        [HttpGet("chat/{cr_code}")]
        public IActionResult GetChatRoomAndMessages(string cr_code)
        {
            return Ok(new Response()
            {
                Status = "SUCCESS",
                Data = _service.GetByCode(cr_code)
            });
        }

        [HttpPost("chat/addUser/{cr_code}")]
        public IActionResult AddUserToChatRoom(string cr_code, string username)
        {
            try
            {
                if (_service.InsertUserIntoChatRoom(username, cr_code)) return Ok(new Response() { Status = "SUCCESS" });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }
        [HttpGet("userOfRoom/{cr_code}")]
        public IActionResult GetUserByRoom(string cr_code)
        {
            return Ok(new Response()
            {
                Status = "SUCCESS",
                Data = _service.GetUsersByChatRoom(cr_code)
            });
        }
        [HttpDelete("chat/deleteChatRoom/{cr_code}")]
        public IActionResult DeleteChatRoom(string cr_code, string Username)
        {
            return Ok(new Response()
            {
                Status = "SUCCESS",
                Data = _service.Delete(cr_code, Username)
            });
        }
    }
}
