using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
	public class MatchDTO
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
		public virtual ClubDTO? AwayNavigation { get; set; } = null!;
		public virtual ClubDTO? HomeNavigation { get; set; } = null!;
		[JsonIgnore]
		[AllowNull]
		public virtual StadiumDTO? Stadium { get; set; } = null!;
		[JsonIgnore]
		[AllowNull]
		public virtual RefereeDTO? Ref { get; set; }
	}
}
