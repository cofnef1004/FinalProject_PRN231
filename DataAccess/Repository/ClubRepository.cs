using AutoMapper;
using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
	public class ClubRepository : IClubRepository
	{
		FinalProPrn231Context _context;
		IMapper mapper;
		ClubDAO clubDAO;

		public ClubRepository(FinalProPrn231Context context, IMapper mapper)
		{
			_context = context;
			this.mapper = mapper;
		}

		public void Create(ClubDTO clubDTO)
		{
			clubDAO = new ClubDAO(_context);
			Club p = mapper.Map<Club>(clubDTO);
			clubDAO.CreateClub(p);
		}

		public void Delete(int id)
		{
			clubDAO = new ClubDAO(_context);
			clubDAO.DeleteClub(id);
		}

		public ClubDTO GetClubById(int id)
		{
			clubDAO = new ClubDAO(_context);
			ClubDTO clubDTO = mapper.Map<ClubDTO>(clubDAO.GetClubById(id));
			return clubDTO;
		}

		public List<ClubDTO> GetClubs()
		{
			clubDAO = new ClubDAO(_context);
			List<ClubDTO> clubDTOs= mapper.Map<List<ClubDTO>>(clubDAO.GetAll());
			return clubDTOs;
		}

		public void ToggleClubStatus(int clubId)
		{
			clubDAO = new ClubDAO(_context);
			clubDAO.ToggleClubStatus(clubId);
		}

		public void Update(ClubDTO clubDTO)
		{
			clubDAO = new ClubDAO(_context);
			Club p = mapper.Map<Club>(clubDTO);
			clubDAO.UpdateClub(p);
		}
		/*		public void Update(ClubDTO clubDTO, IFormFile logoFile)
				{
					clubDAO = new ClubDAO(_context);
					Club p = mapper.Map<Club>(clubDTO);
					clubDAO.UpdateClub(p, logoFile);
				}*/
	}
}
