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

        [HttpPost("sendMessage/{cr_Id}")]
        public IActionResult InsertMessage(MessageDTO m, string cr_Id)
        {
            try
            {
                ObjectId chatRoomId = ObjectId.Parse(cr_Id);

                if (_service.InsertMessage(m, chatRoomId))
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
    }
}
