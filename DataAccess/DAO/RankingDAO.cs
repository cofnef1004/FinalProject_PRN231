using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class RankingDAO
	{
		FinalProPrn231Context _context;

		public RankingDAO(FinalProPrn231Context context)
		{
			_context = context;
		}

		public List<Ranking> GetAll()
		{
			return _context.Rankings
			  .Include(p => p.Club)
			  .ThenInclude(p => p.City)
			  .Include(p => p.Club)
			  .ThenInclude(p => p.Stadium)
			  .Include(p => p.Club)
			  .ThenInclude(p => p.User)
			  .ThenInclude(p => p.Role)
			  .OrderByDescending(r => r.Point)
			  .ThenByDescending(r => r.Difference)
			  .ThenBy(r => r.Club.Name)
			  .ToList();
		}

		public void UpdateRankings()
		{
			List<Match> matches = _context.Matches.Where(m => m.Status == false).ToList();

			foreach (var match in matches)
			{
				if (match.Status == true || match.Result.Equals("-"))
				{
					continue;
				}
				List<Ranking> homeRankings = _context.Rankings.Where(r => r.ClubId == match.Home).ToList();
				List<Ranking> awayRankings = _context.Rankings.Where(r => r.ClubId == match.Away).ToList();

				string[] score = match.Result.Split('-');
				if (score.Length != 2)
				{
					continue;
				}

				int homeScore, awayScore;
				if (!int.TryParse(score[0], out homeScore) || !int.TryParse(score[1], out awayScore))
				{
					continue;
				}

				foreach (var ranking in homeRankings)
				{
					ranking.MatchPlayed += 1;
					ranking.Win += homeScore > awayScore ? 1 : 0;
					ranking.Draw += homeScore == awayScore ? 1 : 0;
					ranking.Loose += homeScore < awayScore ? 1 : 0;
					ranking.Difference += homeScore - awayScore;

					if (homeScore > awayScore)
					{
						ranking.Point += 3;
					}
					else if (homeScore == awayScore)
					{
						ranking.Point += 1;
					}
				}

				foreach (var ranking in awayRankings)
				{
					ranking.MatchPlayed += 1;
					ranking.Win += homeScore < awayScore ? 1 : 0;
					ranking.Draw += homeScore == awayScore ? 1 : 0;
					ranking.Loose += homeScore > awayScore ? 1 : 0;
					ranking.Difference += awayScore - homeScore;

					if (homeScore < awayScore)
					{
						ranking.Point += 3;
					}
					else if (homeScore == awayScore)
					{
						ranking.Point += 1;
					}
				}
				_context.SaveChanges();
				match.Status = true;
				_context.SaveChanges();
			}
		}
	}
}
