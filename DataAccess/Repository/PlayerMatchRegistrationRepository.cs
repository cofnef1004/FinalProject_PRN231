using AutoMapper;
using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class PlayerMatchRegistrationRepository : IPlayerMatchRegistrationRepository
    {
        FinalProPrn231Context _context;
        IMapper mapper;
        PlayerMatchRegistrationDAO playerMatchRegistrationDAO;
        PlayerDAO playerDAO;

        public PlayerMatchRegistrationRepository(FinalProPrn231Context context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

		public void Add(PlayerMatchRegistrationDTO playerMatchRegistration)
		{
			playerMatchRegistrationDAO = new PlayerMatchRegistrationDAO(_context);
			var playerRegis = mapper.Map<PlayerMatchRegistration>(playerMatchRegistration);
			playerMatchRegistrationDAO.Add(playerRegis);
		}

		public void Delete(int id)
		{
			playerMatchRegistrationDAO = new PlayerMatchRegistrationDAO(_context);
            playerMatchRegistrationDAO.Delete(id);
		}

		public List<PlayerMatchRegistrationDTO> GetRegisteredPlayersForClubAndMatch(int clubId, int matchId)
        {
            playerMatchRegistrationDAO = new PlayerMatchRegistrationDAO(_context);
            List<PlayerMatchRegistrationDTO> playerMatchRegistrations = mapper.Map<List<PlayerMatchRegistrationDTO>>(playerMatchRegistrationDAO.GetRegisteredPlayersForClubAndMatch(clubId,matchId));
            return playerMatchRegistrations;
        }
    }
}
