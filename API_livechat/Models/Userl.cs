using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_livechat.Models
{
    [Table("Userl")]
    public class Userl
    {
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string Passwrd { get; set; } = null!;
    }
}
