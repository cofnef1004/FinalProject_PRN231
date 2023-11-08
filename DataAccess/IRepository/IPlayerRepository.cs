using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
	public interface IPlayerRepository
	{
		List<PlayerDTO> GetPlayers();
		List<PlayerDTO> GetPlayersByClub(int clubId);
		List<PlayerDTO> GetPlayersByClubIdNotRegis(int clubId);

		PlayerDTO GetPlayerById(int id);

		void createPlayer(PlayerDTO player);
		void Update(PlayerDTO p);

		void Delete(int playerId);

	}
}
