using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class Manager
{
    public int ManagerId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime Dob { get; set; }

    public int CountryId { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<ManagerClub> ManagerClubs { get; set; } = new List<ManagerClub>();
}
