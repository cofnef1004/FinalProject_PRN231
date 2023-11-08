using BussinessObject.Models;
using DataAccess.Request;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class UserDAO
	{
		FinalProPrn231Context _context;

		public UserDAO(FinalProPrn231Context context)
		{
			_context = context;
		}

		public User Login(string username, string password)
		{
			User user = _context.Users.Include(p => p.Role).FirstOrDefault(u => u.Username == username && u.Password == password);
			return user;
		}

		public static User? CheckUserLogin(LoginRequest request)
		{
			try
			{
				using var context = new FinalProPrn231Context();
				var user = context.Users.Include(x => x.Role).FirstOrDefault(x => x.Username == request.Username && x.Password == request.Password);
				return user;
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				throw;
			}
		}

	}
}
