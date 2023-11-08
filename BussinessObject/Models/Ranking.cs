using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class Ranking
{
    public int ClubId { get; set; }

    public int Round { get; set; }

    public int Win { get; set; }

    public int Draw { get; set; }

    public int Loose { get; set; }

    public int Difference { get; set; }

    public int Point { get; set; }

    public int? MatchPlayed { get; set; }

    public virtual Club Club { get; set; } = null!;
}
