using API_livechat.DTO;
using API_livechat.Filter;
using API_livechat.Models;
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
                if (m.Sder == null || m.Sder.Length == 0)
                {
                    return BadRequest(new Response()
                    {
                        Status = "ERROR",
                        Data = "E' richiesto username dell'utente che invia il messaggio"
                    });
                }
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

        [HttpDelete("chat/deleteMessage/{ms_code}")]
        public IActionResult DeleteMessage(string ms_code, string username)
        {
            if (!_service.UserReg(username))
            {
                return BadRequest(new Response()
                {
                    Status = "ERROR",
                    Data = "Utente non registrato"
                });
            }

            if (_service.GetMessByUser(username) == null || _service.GetMessByUser(username)!.Count == 0)
            {
                return BadRequest(new Response()
                {
                    Status = "ERROR",
                    Data = $"Non esistono messaggi di {username}"
                });
            }

            if (_service.DeleteMessage(ms_code, username))
            {
                return Ok(new Response()
                {
                    Status = "SUCCESS",
                    Data = "Eliminazione effettuata"
                });
            }
            return BadRequest(new Response()
            {
                Status = "ERROR",
                Data = "Eliminazione non riuscita"
            });
        }

        [HttpDelete("chat/deleteMessagesByUser/{username}")]
        public IActionResult DeleteMessages(string username)
        {
            if (!_service.UserReg(username))
            {
                return BadRequest(new Response()
                {
                    Status = "ERROR",
                    Data = "Utente non registrato"
                });
            }

            if (_service.GetMessByUser(username) == null || _service.GetMessByUser(username)!.Count == 0)
            {
                return BadRequest(new Response()
                {
                    Status = "ERROR",
                    Data = $"Non esistono messaggi di {username}"
                });
            }

            if (_service.DeleteMessagesOfUser(username))
            {
                return Ok(new Response()
                {
                    Status = "SUCCESS",
                    Data = "Eliminazione effettuata"
                });
            }

            return BadRequest(new Response()
            {
                Status = "ERROR",
                Data = "Eliminazione non riuscita"
            });
        }

        [HttpPost("UpdateMessage/{ms_code}")]
        public IActionResult UpdateMessage(string ms_code, string username, string textMessage)
        {
            if (_service.GetMessage(ms_code) == null)
            {
                return BadRequest(new Response()
                {
                    Status = "ERROR",
                    Data = $"Non esiste il messaggio cercato o il codice {ms_code} è errato"
                });
            }

            if (!_service.UserReg(username))
            {
                return BadRequest(new Response()
                {
                    Status = "ERROR",
                    Data = "L'utente che vuole effettuare la modifica non è registrato nel sistema"
                });
            }

            if (!_service.CheckUserSender(ms_code, username))
            {
                return BadRequest(new Response()
                {
                    Status = "ERROR",
                    Data = "L'utente che vuole effettuare la modifica non è proprietario del messaggio"
                });
            }

            if (_service.UpdateMessage(ms_code, textMessage))
            {
                return Ok(new Response()
                {
                    Status = "SUCCESS",
                    Data = _service.GetMessage(ms_code)
                });
            }
            return BadRequest(new Response()
            {
                Status = "ERROR",
                Data = "Messaggio non modificato"
            });
        }
    }
}