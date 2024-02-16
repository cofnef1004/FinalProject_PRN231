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

		public UserDTO? Register(RegisRequest request)
		{
			userDAO = new UserDAO(_context);
			return mapper.Map<UserDTO>(UserDAO.Register(request));
		}

		public void Update(UserDTO userDTO)
		{
			userDAO = new UserDAO(_context);
			User p = mapper.Map<User>(userDTO);
			userDAO.Edit(p);
		}

		public UserDTO? GetUserById(int id)
		{
			userDAO = new UserDAO(_context);
			UserDTO user = mapper.Map<UserDTO>(userDAO.GetUserById(id));
			return user;
		}
	}
}
