using DataAccess.DAO;
using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
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

		void Create(ClubDTO clubDTO);

		void Update(ClubDTO clubDTO);
		void Delete(int clubId);

		void ToggleClubStatus(int clubId);
	}
}
