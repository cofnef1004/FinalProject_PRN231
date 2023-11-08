using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class PlayerDAO
	{
		FinalProPrn231Context _context;

		public PlayerDAO(FinalProPrn231Context context)
		{
			_context = context;
		}

		public List<Player> GetAll()
		{
			return _context.Players
				.Include(p => p.Country)
								.Include(p => p.Club)
				.ThenInclude(c => c.Stadium)
                                .Include(p => p.Club)
                .ThenInclude(c => c.City)
                                .Include(p => p.Club)
                .ThenInclude(c => c.User).ThenInclude(c => c.Role).ToList();
		}

		public List<Player> GetPlayersByClubId(int clubId) 
		{
            return _context.Players
                .Include(p => p.Country)
                                .Include(p => p.Club)
                .ThenInclude(c => c.Stadium)
                                .Include(p => p.Club)
                .ThenInclude(c => c.City)
                                .Include(p => p.Club)
                .ThenInclude(c => c.User).ThenInclude(c => c.Role).Where(p => p.ClubId == clubId).ToList();
        }

		public List<Player> GetPlayersByClubIdNotRegis(int clubId)
		{
			return _context.Players
				.Include(p => p.Country)
								.Include(p => p.Club)
				.ThenInclude(c => c.Stadium)
								.Include(p => p.Club)
				.ThenInclude(c => c.City)
								.Include(p => p.Club)
				.ThenInclude(c => c.User).ThenInclude(c => c.Role).Where(p => p.ClubId == clubId && p.PlayerMatchRegistrations.Count == 0).ToList();
		}

		public Player GetPlayersById(int id)
		{
			return _context.Players
				.Include(p => p.Country)
								.Include(p => p.Club)
				.ThenInclude(c => c.Stadium)
								.Include(p => p.Club)
				.ThenInclude(c => c.City)
								.Include(p => p.Club)
				.ThenInclude(c => c.User).ThenInclude(c => c.Role).FirstOrDefault(p => p.PlayerId == id);
		}

		public void CreatePlayer(Player p)
		{
			_context.Players.Add(p);
			_context.SaveChanges();
		}

		public void Update(Player player)
		{
			var c = _context.Players.FirstOrDefault(p => p.PlayerId == player.PlayerId);
			if (c != null)
			{
				c.Name = player.Name;
				c.Position = player.Position;
				c.Number = player.Number;
				_context.SaveChanges();
			}
		}

		public void Delete(int playerId)
		{
			var player = _context.Players
				.Include(p => p.Goals)
				.Include(p => p.PlayerMatchRegistrations)
				.SingleOrDefault(p => p.PlayerId == playerId);
			if (player != null)
			{
				_context.Goals.RemoveRange(player.Goals);
				_context.PlayerMatchRegistrations.RemoveRange(player.PlayerMatchRegistrations);
				_context.Players.Remove(player);
				_context.SaveChanges();
			}
		}
	}
}
