using AutoMapper;
using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
	public class MatchRepository : IMatchRepository
	{
		FinalProPrn231Context _context;
		IMapper mapper;
		MatchDAO matchDAO;

		public MatchRepository(FinalProPrn231Context context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}

		public void DeleteMatch(int id)
		{
			matchDAO = new MatchDAO(_context);
			matchDAO.DeleteMatch(id);	
		}

		public void GenerateMatchesForRound(int round)
		{
			matchDAO = new MatchDAO(_context);
			matchDAO.GenerateMatchesForRound(round);
			var matches = matchDAO.GetMatchesByRound(round);
			var matchesDto = mapper.Map<List<MatchDTO>>(matches);
			foreach (var matchDto in matchesDto)
			{
				matchDAO.UpdateMatch(mapper.Map<Match>(matchDto));
			}
		}

		public MatchDTO GetMatchById(int id)
		{
			matchDAO = new MatchDAO(_context);
			MatchDTO matche = mapper.Map<MatchDTO>(matchDAO.GetMatchById(id));
			return matche;
		}

		public List<MatchDTO> GetMatches()
		{
			matchDAO = new MatchDAO(_context);
			List<MatchDTO> matches = mapper.Map<List<MatchDTO>>(matchDAO.GetMatches());
			return matches;
		}

		public List<MatchDTO> GetMatchesByRound(int round)
		{
			matchDAO = new MatchDAO(_context);
			List<MatchDTO> matches = mapper.Map<List<MatchDTO>>(matchDAO.GetMatchesByRound(round));
			return matches;
		}

		public void UpdateMatch(MatchDTO matchDTO)
        {
            matchDAO = new MatchDAO(_context);
            Match m = mapper.Map<Match>(matchDTO);
            matchDAO.UpdateMatch(m);
        }
    }
}
