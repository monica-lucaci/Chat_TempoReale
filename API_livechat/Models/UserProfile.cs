using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_livechat.Models
{
    [Table("UserProfile")]
    public class UserProfile : UserLogin
    {
        public string? UsImg { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
