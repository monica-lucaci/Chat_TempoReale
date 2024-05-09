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
        public ChatRoomController(ChatRoomService service)
        {
            _service = service;
        }
        #endregion

        [HttpPost("newChatRoom/{user}")]
        public IActionResult NewChatRoom(ChatRoomDTO newRoom, string user)
        {
            if (_service.Insert(newRoom, user))
            {
                return Ok(new Response()
                {
                    Status = "SUCCESS",
                    Data = "Registrazione effettuata"
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
                if (_service.InsertUserIntoChatRoom(username, cr_code))
                {
                    return Ok(new Response()
                    { 
                        Status = "SUCCESS",
                        Data = GetUsersByRoom(cr_code)

                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpDelete("chat/removeUser/{cr_code}")]
        public IActionResult DeleteUserFromChatRoom(string cr_code, string username) 
        {
            try
            {
                if (_service.DeleteUserFromChatRoom(username, cr_code))
                {
                    return Ok(new Response()
                    {
                        Status = "SUCCESS",
                        Data = GetUsersByRoom(cr_code)

                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet("usersOfRoom/{cr_code}")]
        public IActionResult GetUsersByRoom(string cr_code)
        {
            return Ok(new Response()
            {
                Status = "SUCCESS",
                Data = _service.GetUsersByChatRoom(cr_code)
            });
        }
        [HttpDelete("chat/deleteChatRoom/{cr_code}")]
        public IActionResult DeleteChatRoom(string cr_code, string username)
        {
            return Ok(new Response()
            {
                Status = "SUCCESS",
                Data = _service.Delete(cr_code, username)
            });
        }
    }
}
