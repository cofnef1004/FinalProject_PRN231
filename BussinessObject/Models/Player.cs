using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class Player
{
    public int PlayerId { get; set; }

    public string Name { get; set; } = null!;

    public string Position { get; set; } = null!;

    public DateTime Dob { get; set; }

    public int ClubId { get; set; }

    public int CountryId { get; set; }

    public int Number { get; set; }

    public string? Avatar { get; set; }

    public virtual Club Club { get; set; } = null!;

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual ICollection<PlayerMatchRegistration> PlayerMatchRegistrations { get; set; } = new List<PlayerMatchRegistration>();
}
