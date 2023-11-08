using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class ManagerClub
{
    public int ManagerId { get; set; }

    public int ClubId { get; set; }

    public string Role { get; set; } = null!;

    public virtual Club Club { get; set; } = null!;

    public virtual Manager Manager { get; set; } = null!;
}
