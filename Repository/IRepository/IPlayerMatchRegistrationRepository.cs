using BussinessObject.Models;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface IPlayerMatchRegistrationRepository
    {
        List<PlayerMatchRegistrationDTO> GetRegisteredPlayersForClubAndMatch(int clubId, int matchId);
        public void Add(int clubId, int matchId);

	}
}
