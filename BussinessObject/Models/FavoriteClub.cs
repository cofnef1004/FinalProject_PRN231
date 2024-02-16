using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class FavoriteClub
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ClubId { get; set; }

    public virtual Club Club { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
