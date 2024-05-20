using System;
using System.Collections.Generic;

namespace API_livechat.Models;

public partial class UserProfile
{
    public int UserId { get; set; }

    public string? Code { get; set; } = Guid.NewGuid().ToString().ToUpper();

    public string Username { get; set; } = null!;

    public string Passwrd { get; set; } = null!;

    public string? UsImg { get; set; }

    public string? Email { get; set; }

    public bool? IsDeleted { get; set; }

    public List<string>? ChatRoomsCode { get; set; }

    public string? UsRole { get; set; }
   
}
