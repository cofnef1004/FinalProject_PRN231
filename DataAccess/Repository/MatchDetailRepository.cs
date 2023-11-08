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
	public class MatchDetailRepository : IMatchDetailRepository
	{
		FinalProPrn231Context _context;
		IMapper mapper;
		MatchDetailDAO matchDetailDAO;

		public MatchDetailRepository(FinalProPrn231Context context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}

		public void AddMatchDetail(MatchDetailDTO matchDetail)
		{
			matchDetailDAO = new MatchDetailDAO(_context);
			var matchDetailEntity = mapper.Map<MatchDetail>(matchDetail);
			matchDetailDAO.AddMatchDetail(matchDetailEntity);
		}

		public void EditMatchDetail(MatchDetailDTO matchDetail)
		{
			matchDetailDAO = new MatchDetailDAO(_context);
			var matchDetailEntity = mapper.Map<MatchDetail>(matchDetail);
			matchDetailDAO.EditMatchDetail(matchDetailEntity);
		}

        public MatchDetailDTO GetMatchDetailbyId(int id)
        {
            matchDetailDAO = new MatchDetailDAO(_context);
            MatchDetailDTO matchDetail = mapper.Map<MatchDetailDTO>(matchDetailDAO.GetMatchDetailbyId(id));
            return matchDetail;
        }

        public MatchDetailDTO GetMatchDetailbyMatchId(int matchId)
		{
			matchDetailDAO = new MatchDetailDAO(_context);
			MatchDetailDTO matchDetails = mapper.Map<MatchDetailDTO>(matchDetailDAO.GetMatchDetailbyMatchId(matchId));
			return matchDetails;
		}
	}
}
