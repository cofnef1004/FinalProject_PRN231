using BussinessObject.Models;
using Microsoft.AspNetCore.Http;
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
							.Include(p => p.Stadium).ToList();
		}

		public Club GetClubById(int id)
		{
			return _context.Clubs
							.Include(p => p.City)
							.Include(p => p.Stadium).FirstOrDefault(p => p.ClubId == id);
		}

		public void CreateClub(Club club)
		{
			_context.Clubs.Add(club);
			_context.SaveChanges();
		}

		public void UpdateClub(Club club)
		{
			var c = _context.Clubs.FirstOrDefault(p => p.ClubId == club.ClubId);
			if (c != null && c.Status == true)
			{
				c.Name = club.Name;
				c.StadiumId = club.StadiumId;
				c.CityId = club.CityId;
				_context.SaveChanges();
			}
		}

		private string SaveLogoFile(IFormFile logoFile)
		{
			if (logoFile != null && logoFile.Length > 0)
			{
				string uniqueFileName = Guid.NewGuid().ToString() + "_" + logoFile.FileName;
				string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
				if (!Directory.Exists(uploadPath))
				{
					Directory.CreateDirectory(uploadPath);
				}
				string filePath = Path.Combine(uploadPath, uniqueFileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					logoFile.CopyTo(stream);
				}

				return filePath;
			}

			return null;
		}

		public void DeleteClub(int clubId)
		{
			var club = _context.Clubs.Include(c => c.FavoriteClubs).Include(c => c.Players).FirstOrDefault(p => p.ClubId == clubId);
			if (club != null && club.Status == true)
			{
				_context.FavoriteClubs.RemoveRange(club.FavoriteClubs);
				_context.Players.RemoveRange(club.Players);
				_context.Clubs.Remove(club);
				_context.SaveChanges();
			}
		}

		public void ToggleClubStatus(int clubId)
		{
			var club = _context.Clubs.FirstOrDefault(p => p.ClubId == clubId);
			if (club != null)
			{
				club.Status = !club.Status;
				_context.SaveChanges();
			}
		}
	}
}
