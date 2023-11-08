using System;
using System.Collections.Generic;

namespace BussinessObject.Models;

public partial class Match
{
    public int MatchId { get; set; }

    public DateTime Date { get; set; }

    public int Round { get; set; }

    public int Home { get; set; }

    public int Away { get; set; }

    public int StadiumId { get; set; }

    public string Result { get; set; } = null!;

    public int? RefId { get; set; }

    public bool? Status { get; set; }

    public virtual Club AwayNavigation { get; set; } = null!;

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual Club HomeNavigation { get; set; } = null!;

    public virtual ICollection<MatchDetail> MatchDetails { get; set; } = new List<MatchDetail>();

    public virtual ICollection<PlayerMatchRegistration> PlayerMatchRegistrations { get; set; } = new List<PlayerMatchRegistration>();

    public virtual Referee? Ref { get; set; }

    public virtual Stadium Stadium { get; set; } = null!;
}
