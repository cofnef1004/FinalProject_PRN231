using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class Stadium
{
    public int StadiumId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}
