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
	public class FavoRepo : IFavoClubRepo
	{
		FinalProPrn231Context _context;
		IMapper mapper;
		FavoClubDao favoClubDao;

		public FavoRepo(FinalProPrn231Context context, FavoClubDao favoClubDao, IMapper mapper)
		{
			_context = context;
			this.favoClubDao = favoClubDao;
			this.mapper = mapper;
		}

		public List<FavoDto> GetFavoriteClubs(int userId)
		{
			favoClubDao = new FavoClubDao(_context);
			List<FavoDto> favos = mapper.Map<List<FavoDto>>(favoClubDao.GetFavoriteClubs(userId));
			return favos;
		}

		public void AddFavoriteClub(int userId, int clubId)
		{
			favoClubDao = new FavoClubDao(_context);
			favoClubDao.AddFavoriteClub(userId, clubId);
		}

		public void RemoveFavoriteClub(int id)
		{
			favoClubDao = new FavoClubDao(_context);
			favoClubDao.RemoveFavoriteClub(id);
		}
	}
}
