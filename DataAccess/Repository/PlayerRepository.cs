using AutoMapper;
using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
	public class PlayerRepository : IPlayerRepository
	{
		FinalProPrn231Context _context;
		IMapper mapper;
		PlayerDAO playerDAO;

		public PlayerRepository(FinalProPrn231Context context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}

		public void createPlayer(PlayerDTO player)
		{
			playerDAO = new PlayerDAO(_context);
			Player p = mapper.Map<Player>(player);
			playerDAO.CreatePlayer(p);
		}

		public void Delete(int playerId)
		{
			playerDAO = new PlayerDAO(_context);
			playerDAO.Delete(playerId);
		}

		public PlayerDTO GetPlayerById(int id)
		{
			playerDAO = new PlayerDAO(_context);
			PlayerDTO playerDTO = mapper.Map<PlayerDTO>(playerDAO.GetPlayersById(id));
			return playerDTO;
		}

		public List<PlayerDTO> GetPlayers()
		{
			playerDAO = new PlayerDAO(_context);
			List<PlayerDTO> playerDTOs = mapper.Map<List<PlayerDTO>>(playerDAO.GetAll());
			return playerDTOs;
		}

        public List<PlayerDTO> GetPlayersByClub(int clubId)
        {
            playerDAO = new PlayerDAO(_context);
            List<PlayerDTO> playerDTOs = mapper.Map<List<PlayerDTO>>(playerDAO.GetPlayersByClubId(clubId));
            return playerDTOs;
        }

		public List<PlayerDTO> GetPlayersByClubIdNotRegis(int clubId)
		{
			playerDAO = new PlayerDAO(_context);
			List<PlayerDTO> playerDTOs = mapper.Map<List<PlayerDTO>>(playerDAO.GetPlayersByClubIdNotRegis(clubId));
			return playerDTOs;
		}

		public void Update(PlayerDTO p)
		{
			playerDAO = new PlayerDAO(_context);
			Player player = mapper.Map<Player>(p);
			playerDAO.Update(player);
		}
	}
}
