using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class ClubDAO
	{
		FinalProPrn231Context _context;

		public ClubDAO(FinalProPrn231Context context)
		{
			_context = context;
		}

		public List<Club> GetAll()
		{
			return _context.Clubs
							.Include(p => p.City)
							.Include(p => p.User).ThenInclude(p => p.Role)
							.Include(p => p.Stadium).ToList();
		}

		public List<Club> GetClubByUserId(int userId)
		{
			return _context.Clubs
							.Include(p => p.City)
							.Include(p => p.User).ThenInclude(p => p.Role)
							.Include(p => p.Stadium).Where(p => p.UserId == userId).ToList();
		}

		public Club GetClubById(int id)
		{
			return _context.Clubs
							.Include(p => p.City)
							.Include(p => p.User).ThenInclude(p => p.Role)
							.Include(p => p.Stadium).FirstOrDefault(p => p.ClubId == id);
		}

		public void CreateClub(Club club)
		{
			_context.Clubs.Add(club);
			_context.SaveChanges();
		}

		public void UpdateClub(Club club)
		{
			var c = _context.Clubs.FirstOrDefault(p => p.ClubId== club.ClubId);
			if (c != null) 
			{
				c.Logo = club.Logo;
				c.Name = club.Name;
				c.StadiumId = club.StadiumId;
				c.CityId = club.CityId;
				_context.SaveChanges();
			}
		}

		public void DeleteClub(int clubId)
		{
			var club = _context.Clubs.Include(c => c.Players).FirstOrDefault(p => p.ClubId == clubId);
			if (club != null)
			{
				var hasRound19Matches = _context.Matches.Any(m => m.Round == 19 && (m.Home == clubId || m.Away == clubId));
				var hasRound19MatchesWithTrueStatus = _context.Matches.Any(m => m.Round == 19 && (m.Home == clubId || m.Away == clubId) && m.Status == true);

				if (hasRound19Matches && hasRound19MatchesWithTrueStatus)
				{
					return;
				}
				_context.Players.RemoveRange(club.Players);
				_context.Clubs.Remove(club);
				_context.SaveChanges();
			}
		}
	}
}
