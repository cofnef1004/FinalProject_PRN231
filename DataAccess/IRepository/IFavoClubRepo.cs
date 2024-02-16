using BussinessObject.Models;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
	public interface IFavoClubRepo
	{
		List<FavoDto> GetFavoriteClubs(int userId);
		void AddFavoriteClub(int userId, int clubId);
		void RemoveFavoriteClub(int id);
	}
}
