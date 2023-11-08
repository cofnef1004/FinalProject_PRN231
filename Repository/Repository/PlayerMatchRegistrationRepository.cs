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

        public PlayerMatchRegistrationRepository(FinalProPrn231Context context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

		public void Add(int clubId, int matchId)
		{

		}

		public List<PlayerMatchRegistrationDTO> GetRegisteredPlayersForClubAndMatch(int clubId, int matchId)
        {
            playerMatchRegistrationDAO = new PlayerMatchRegistrationDAO(_context);
            List<PlayerMatchRegistrationDTO> playerMatchRegistrations = mapper.Map<List<PlayerMatchRegistrationDTO>>(playerMatchRegistrationDAO.GetRegisteredPlayersForClubAndMatch(clubId,matchId));
            return playerMatchRegistrations;
        }
    }
}
