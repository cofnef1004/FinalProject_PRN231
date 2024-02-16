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
	public class RankingRepository : IRankingRepository
	{
		FinalProPrn231Context _context;
		IMapper mapper;
		RankingDAO rankingDAO;

		public RankingRepository(FinalProPrn231Context context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}

		public void AddClubToRanking(ClubDTO club)
		{
			rankingDAO = new RankingDAO(_context);

			Club clubToAdd = mapper.Map<Club>(club);

			rankingDAO.AddClubToRanking(clubToAdd);
			_context.SaveChanges();
		}

		public RankingDTO GetDetail(int clubId)
		{
			rankingDAO = new RankingDAO(_context);
			RankingDTO ranking = mapper.Map<RankingDTO>(rankingDAO.GetByClub(clubId));
			return ranking;
		}

		public List<RankingDTO> GetRankings()
		{
			rankingDAO = new RankingDAO(_context);
			List<RankingDTO> rankings = mapper.Map<List<RankingDTO>>(rankingDAO.GetAll());
			return rankings;
		}

		public void UpdateRankings()
		{
			rankingDAO = new RankingDAO(_context);
			rankingDAO.UpdateRankings();
			_context.SaveChanges();
		}
	}
}
