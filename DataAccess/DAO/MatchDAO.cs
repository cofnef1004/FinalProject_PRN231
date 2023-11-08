using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class MatchDAO
	{
		private FinalProPrn231Context _context;

		public MatchDAO(FinalProPrn231Context context)
		{
			_context = context;
		}

		public List<Match> GetMatches()
		{
			return _context.Matches.Include(p => p.AwayNavigation)
				.Include(p => p.HomeNavigation).ToList();
		}

		public List<Match> GetRandomMatches()
		{
			return _context.Matches.Include(p => p.AwayNavigation)
				.Include(p => p.HomeNavigation)
				.Where(m => m.Status == false)
				.OrderBy(m => Guid.NewGuid())
				.Take(3)
				.ToList();
		}

		public List<Match> GetMatchesByRound(int round)
		{
			return _context.Matches.Include(p => p.AwayNavigation)
				.Include(p => p.HomeNavigation).Where(p => p.Round == round).ToList();
		}

		public List<Match> GetMatchNotDone()
		{
			return _context.Matches
				.Include(p => p.AwayNavigation)
				.Include(p => p.HomeNavigation)
				.Include(p => p.MatchDetails)
				.Where(p => p.MatchDetails.Count == 0 && p.Status == true)
				.ToList();
		}

		public int Home(int id)
		{
			Match match = GetMatchById(id);
			int home = match.Home;
			return home;
		}

		public int Away(int id)
		{
			Match match = GetMatchById(id);
			int away = match.Away;
			return away;
		}

		public Match GetMatchById(int id)
		{
			return _context.Matches.Include(p => p.AwayNavigation)
				.Include(p => p.HomeNavigation).FirstOrDefault(p => p.MatchId == id);
		}

		public void UpdateMatch(Match match)
		{
			var m = _context.Matches.FirstOrDefault(p => p.MatchId == match.MatchId);
			if (m != null)
			{
				m.RefId = match.RefId;
				m.Result = match.Result;
				_context.SaveChanges();
			}
		}

		public void DeleteMatch(int id)
		{
			var match = _context.Matches.FirstOrDefault(p => p.MatchId == id);
			if (match != null)
			{
				_context.Matches.Remove(match);
				_context.SaveChanges();
			}
		}

		public void GenerateMatchesForRound(int round)
		{
			var existingRound = _context.Matches.Any(m => m.Round == round);
			if (existingRound)
			{
				Console.WriteLine($"Round {round} already exists. Generating matches for this round is not allowed.");
				return;
			}

			var previousRoundsComplete = _context.Matches
				.Where(m => m.Round < round)
				.All(m => m.Status == true);

			if (!previousRoundsComplete)
			{
				Console.WriteLine($"Previous rounds are not complete. Generating matches for round {round} is not allowed.");
				return;
			}

			var teams = _context.Clubs.ToList();
			var playedTeams = new List<int>();
			var previousMatches = _context.Matches.Where(m => m.Round < round).ToList();
			var randomTeams = teams.OrderBy(t => Guid.NewGuid()).ToList();

			foreach (var homeTeam in randomTeams)
			{
				if (playedTeams.Contains(homeTeam.ClubId))
				{
					continue;
				}
				var awayTeams = teams.Where(t =>
					t.ClubId != homeTeam.ClubId &&
					!playedTeams.Contains(t.ClubId) &&
					!previousMatches.Any(m =>
						(m.Home == homeTeam.ClubId && m.Away == t.ClubId) ||
						(m.Home == t.ClubId && m.Away == homeTeam.ClubId)
					)
				);
				foreach (var awayTeam in awayTeams)
				{
					var match = new Match()
					{
						Round = round,
						Home = homeTeam.ClubId,
						Away = awayTeam.ClubId,
						Date = DateTime.Now,
						StadiumId = homeTeam.StadiumId,
						RefId = GetRandomRefereeForRound(round),
						Result = "-",
						Status = false
					};
					_context.Matches.Add(match);
					playedTeams.Add(homeTeam.ClubId);
					playedTeams.Add(awayTeam.ClubId);
					break;
				}
			}
			_context.SaveChanges();
		}

		public int GetRandomRefereeForRound(int round)
		{
			var referees = _context.Referees.ToList();
			var assignedRefs = _context.Matches
				.Where(m => m.Round == round && m.RefId.HasValue)
				.Select(m => m.RefId.Value)
				.ToList();

			var availableRefs = referees.Where(r => !assignedRefs.Contains(r.RefId)).ToList();

			if (availableRefs.Count == 0)
			{
				throw new Exception("No available referees for the round.");
			}
			var random = new Random();
			var index = random.Next(availableRefs.Count);
			var refereeId = availableRefs[index].RefId;
			return refereeId;
		}
	}
}
