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
	public class RefRepository : IRefRepository
	{
		FinalProPrn231Context _context;
		IMapper mapper;
		RefDAO refDAO;

		public RefRepository(FinalProPrn231Context context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}
		public List<RefereeDTO> GetReferees()
		{
			refDAO = new RefDAO(_context);
			List<RefereeDTO> referees = mapper.Map<List<RefereeDTO>>(refDAO.GetAll());
			return referees;
		}
	}
}
