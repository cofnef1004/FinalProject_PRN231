using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class PlayerMatchRegistration
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public int MatchId { get; set; }

    public string? Position { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}
