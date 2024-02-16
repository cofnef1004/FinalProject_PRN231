using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class FavoClubDao
	{
		FinalProPrn231Context _context;

		public FavoClubDao(FinalProPrn231Context context)
		{
			_context = context;
		}
		public List<FavoriteClub> GetFavoriteClubs(int userId)
		{
			var favoriteClubs = _context.FavoriteClubs
				.Include(fc => fc.Club)
					.ThenInclude(c => c.City)
				.Include(fc => fc.Club)
					.ThenInclude(c => c.Stadium)
				.Where(fc => fc.UserId == userId)
				.ToList();

			return favoriteClubs;
		}

		public void AddFavoriteClub(int userId, int clubId)
		{
			bool isExisting = _context.FavoriteClubs
				.Any(fc => fc.UserId == userId && fc.ClubId == clubId);

			if (isExisting)
			{
				throw new Exception("Club is already added to favorites.");
			}
			var favoriteClub = new FavoriteClub
			{
				UserId = userId,
				ClubId = clubId,
			};
			_context.FavoriteClubs.Add(favoriteClub);
			_context.SaveChanges();
		}

		public void RemoveFavoriteClub(int id)
		{
			var favoriteClub = _context.FavoriteClubs.FirstOrDefault(p=>p.Id == id);
			if (favoriteClub != null)
			{
				_context.FavoriteClubs.Remove(favoriteClub);
				_context.SaveChanges();
			}
		}
	}
}
