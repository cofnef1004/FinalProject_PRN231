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
	public class ManagerRepo : IManagerRepo
	{
		FinalProPrn231Context _context;
		IMapper mapper;
		ManagerDAO managerDAO;

		public ManagerRepo(FinalProPrn231Context context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}
		public List<ManagerDTO> GetManagers()
		{
			managerDAO = new ManagerDAO(_context);
			List<ManagerDTO> managers = mapper.Map<List<ManagerDTO>>(managerDAO.GetAll());
			return managers;
		}
	}
}
