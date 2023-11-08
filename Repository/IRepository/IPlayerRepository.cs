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

		void createPlayer(PlayerDTO player);
	}
}
