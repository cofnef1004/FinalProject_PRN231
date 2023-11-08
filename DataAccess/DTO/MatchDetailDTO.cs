using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
	public class MatchDetailDTO
	{
		public int MatchDetailId { get; set; }

		public int MatchId { get; set; }

		public int? HomePossesion { get; set; }

		public int? AwayPossesion { get; set; }

		public int? HomeShot { get; set; }

		public int? AwayShot { get; set; }

		public int? HomeShotOnTarget { get; set; }

		public int? AwayShotOnTarget { get; set; }

		public int? HomeConers { get; set; }

		public int? AwayConers { get; set; }

		public int? HomeYcard { get; set; }

		public int? HomeRcard { get; set; }

		public int? HomePass { get; set; }

		public int? AwayPass { get; set; }

		public int? AwayYcard { get; set; }

		public int? AwayRcard { get; set; }
	}
}
