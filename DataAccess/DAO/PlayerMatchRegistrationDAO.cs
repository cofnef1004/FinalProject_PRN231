using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class PlayerMatchRegistrationDAO
	{
		FinalProPrn231Context _context;

		public PlayerMatchRegistrationDAO(FinalProPrn231Context context)
		{
			_context = context;
		}

		public List<PlayerMatchRegistration> GetRegisteredPlayersForClubAndMatch(int clubId, int matchId)
		{
			var registeredPlayers = _context.PlayerMatchRegistrations
				.Where(p => p.MatchId == matchId && p.Player.ClubId == clubId)
				.OrderBy(p => p.Player.Position == "GK" ? 0 : p.Player.Position == "DF" ? 1 : p.Player.Position == "MF" ? 2 : 3)
				.Take(11)
				.ToList();
			return registeredPlayers;
		}

		public void Add(PlayerMatchRegistration playerMatchRegistration)
		{
			_context.PlayerMatchRegistrations.Add(playerMatchRegistration);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var player = _context.PlayerMatchRegistrations.FirstOrDefault(p => p.Id == id);
			if (player != null)
			{
				_context.PlayerMatchRegistrations.Remove(player);
				_context.SaveChanges();
			}
		}
	}
}
