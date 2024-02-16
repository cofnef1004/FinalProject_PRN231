using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
	public interface IMatchRepository
	{
		List<MatchDTO> GetMatches();
		List<MatchDTO> GetMatchesByRound(int round);

		List<MatchDTO> GetMatchesByClub(int clubId);

		List<MatchDTO> GetRandomMatches();

		List<MatchDTO> GetMatchNotDetail();
		void UpdateMatch(MatchDTO matchDTO);

		void GenerateMatchesForRound(int round);

		void DeleteMatch(int id);

		int Home(int id);
		int Away(int id);

		MatchDTO GetMatchById(int id);
	}
}
