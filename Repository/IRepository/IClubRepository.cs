using DataAccess.DAO;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
	public interface IClubRepository
	{
		List<ClubDTO> GetClubs();

		ClubDTO GetClubById(int id);

		List<ClubDTO> GetClubByUserId(int userId);

		void Create(ClubDTO clubDTO);

		void Update(ClubDTO clubDTO);
		void Delete(int id);
	}
}
