using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class City
{
    public int CityId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Club> Clubs { get; set; } = new List<Club>();
}
