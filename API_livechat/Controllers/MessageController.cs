using API_livechat.DTO;
using API_livechat.Filter;
using API_livechat.Services;
using API_livechat.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

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

        [HttpPost("sendMessage/{cr_code}")]
        public IActionResult InsertMessage(MessageDTO m, string cr_code)
        {
            try
            {
                if (_service.InsertMessage(m, cr_code))
                {
                    return Ok(new Response()
                    {
                        Status = "SUCCESS",
                        Data = "Messaggio inviato"
                    });
                }
                else
                {
                    return BadRequest(new Response()
                    {
                        Status = "ERROR",
                        Data = "Messaggio non inviato"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }

        [HttpGet("MessagesOfARoom/{cr_code}")]
        public IActionResult GetMessagesOfRoom(string cr_code) 
        {
            return Ok(new Response()
            {
                Status = "SUCCESS",
                Data = _service.GetMessagesOfRoom(cr_code)
            });
        }
        /*
        [HttpDelete("chat/deleteMessage/{ms_code}")]
        public IActionResult DeleteChatRoom(string ms_code, string username)
        {
            if (_service.DeleteByCode(ms_code, username))
            {
                return Ok(new Response()
                {
                    Status = "SUCCESS",
                    Data = "Eliminazione effettuata"
                });
            }
            else
            {

            }
            return Ok(new Response()
            {
                Status = "Error",
                Data = "Eliminazione non riuscita"
            });
        }
        */
    }
}
