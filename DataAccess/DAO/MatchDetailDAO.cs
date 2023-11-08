using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class MatchDetailDAO
	{
		FinalProPrn231Context _context;

		public MatchDetailDAO(FinalProPrn231Context context)
		{
			_context = context;
		}

		public List<MatchDetail> GetAll()
		{
			return _context.MatchDetails.ToList();
		}

		public MatchDetail GetMatchDetailbyMatchId(int matchId) 
		{
			return _context.MatchDetails.FirstOrDefault(p => p.MatchId == matchId);
		}

        public MatchDetail GetMatchDetailbyId(int id)
        {
            return _context.MatchDetails.FirstOrDefault(p => p.MatchDetailId == id);
        }

        public void AddMatchDetail(MatchDetail matchDetail)
		{
			_context.MatchDetails.Add(matchDetail);
			_context.SaveChanges();
		}

		public void EditMatchDetail(MatchDetail matchDetail)
		{
			var m = _context.MatchDetails.FirstOrDefault(p => p.MatchDetailId == matchDetail.MatchDetailId);
			if (m != null)
			{
				if (matchDetail.HomeShotOnTarget > matchDetail.HomeShot || matchDetail.AwayShotOnTarget > matchDetail.AwayShot)
				{
					return;
				}
				m.HomePossesion = matchDetail.HomePossesion;
				m.AwayPossesion = 100 - matchDetail.HomePossesion;
				m.HomeShot = matchDetail.HomeShot;
				m.AwayShot = matchDetail.AwayShot;
				m.HomeShotOnTarget = matchDetail.HomeShotOnTarget;
				m.AwayShotOnTarget = matchDetail.AwayShotOnTarget;
				m.HomeConers = matchDetail.HomeConers;
				m.AwayConers = matchDetail.AwayConers;
				m.HomePass = matchDetail.HomePass;
				m.AwayPass = matchDetail.AwayPass;
				m.HomeYcard = matchDetail.HomeYcard;
				m.AwayYcard = matchDetail.AwayYcard;
				m.HomeRcard = matchDetail.HomeRcard;
				m.AwayRcard = matchDetail.AwayRcard;
				_context.SaveChanges();
			}
		}
	}
}
