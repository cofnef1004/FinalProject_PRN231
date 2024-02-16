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

		public User GetUserById(int id)
		{
			return _context.Users.FirstOrDefault(p => p.UserId == id);
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

		public void Edit(User user)
		{
			var c = _context.Users.FirstOrDefault(p => p.UserId == user.UserId);
			if (c != null)
			{
				c.Username = user.Username;
				c.Password = user.Password;
				c.Phone = user.Phone;
				_context.SaveChanges();
			}
		}

		public static User Register(RegisRequest request)
		{
			try
			{
				using var context = new FinalProPrn231Context();
				var existingUser = context.Users.FirstOrDefault(u => u.Username == request.Username);
				if (existingUser != null)
				{
					throw new Exception("Username already exists.");
				}
				if (request.Username.ToLower() == "admin")
				{
					throw new Exception("Username cannot be 'Admin'.");
				}
				var user = new User
				{
					Username = request.Username,
					Password = request.Password,
					Phone = request.Phone,
					RoleId = request.RoleId
				};

				context.Users.Add(user);
				context.SaveChanges();

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
