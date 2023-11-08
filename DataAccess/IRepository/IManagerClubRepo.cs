using BussinessObject.Models;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
	public interface IManagerClubRepo
	{
		List<ManagerClubDTO> GetAll();

		List<ManagerClubDTO> GetByClubId(int clubId);
	}
}
