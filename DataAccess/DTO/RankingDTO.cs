using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
	public class RankingDTO
	{
		public int ClubId { get; set; }

		public int Round { get; set; }

		public int Win { get; set; }

		public int Draw { get; set; }

		public int Loose { get; set; }

		public int Difference { get; set; }

		public int Point { get; set; }

		public int MatchPlayed { get; set; }

		public virtual ClubDTO Club { get; set; } = null!;
	}
}
