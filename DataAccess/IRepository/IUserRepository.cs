using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
	public interface IUserRepository
	{
		UserDTO Login(string username, string password);
		public UserDTO? CheckUserLogin(LoginRequest request);

		public UserDTO? Register(RegisRequest request);

		public UserDTO? GetUserById(int id);

		void Update(UserDTO userDTO);
	}
}
