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
	public class ManagerClubRepo : IManagerClubRepo
	{
		FinalProPrn231Context _context;
		IMapper mapper;
		ManagerClubDAO managerClubDAO;

		public ManagerClubRepo(FinalProPrn231Context context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}
		public List<ManagerClubDTO> GetAll()
		{
			managerClubDAO = new ManagerClubDAO(_context);
			List<ManagerClubDTO> managerClubs = mapper.Map<List<ManagerClubDTO>>(managerClubDAO.GetAll());
			return managerClubs;
		}

		public List<ManagerClubDTO> GetByClubId(int clubId)
		{
			managerClubDAO = new ManagerClubDAO(_context);
			List<ManagerClubDTO> managerClubs = mapper.Map<List<ManagerClubDTO>>(managerClubDAO.GetByClubId(clubId));
			return managerClubs;
		}
	}
}
