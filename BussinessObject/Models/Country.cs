using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class Country
{
    public int CountryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Manager> Managers { get; set; } = new List<Manager>();

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();
}
