using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class Club
{
    public int ClubId { get; set; }

    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public int StadiumId { get; set; }

    public int CityId { get; set; }

    public string? Logo { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual ICollection<ManagerClub> ManagerClubs { get; set; } = new List<ManagerClub>();

    public virtual ICollection<Match> MatchAwayNavigations { get; set; } = new List<Match>();

    public virtual ICollection<Match> MatchHomeNavigations { get; set; } = new List<Match>();

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual ICollection<Ranking> Rankings { get; set; } = new List<Ranking>();

    public virtual Stadium Stadium { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
