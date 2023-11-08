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
		void UpdateMatch(MatchDTO matchDTO);

		void GenerateMatchesForRound(int round);

		void DeleteMatch(int id);

		MatchDTO GetMatchById(int id);
	}
}
