using AutoMapper;
using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.Request;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
	public class UserRepository : IUserRepository
	{
		FinalProPrn231Context _context;
		IMapper mapper;
		UserDAO userDAO;

		public UserRepository(FinalProPrn231Context context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}
		public UserDTO Login(string username, string password)
		{
			userDAO = new UserDAO(_context);
			UserDTO user = mapper.Map<UserDTO>(userDAO.Login(username,password));
			return user;
		}

		public UserDTO? CheckUserLogin(LoginRequest request)
		{
			userDAO = new UserDAO(_context);
			return mapper.Map<UserDTO>(UserDAO.CheckUserLogin(request));
		}
	}
}
