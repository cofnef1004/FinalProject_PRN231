using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class Referee
{
    public int RefId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();
}
