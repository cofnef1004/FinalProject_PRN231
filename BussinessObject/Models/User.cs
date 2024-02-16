using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    public long? Phone { get; set; }

    public virtual ICollection<FavoriteClub> FavoriteClubs { get; set; } = new List<FavoriteClub>();

    public virtual Role Role { get; set; } = null!;
}
