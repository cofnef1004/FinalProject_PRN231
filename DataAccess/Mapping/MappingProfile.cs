using AutoMapper;
using AutoMapper.Execution;
using BussinessObject.Models;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<Role, RoleDTO>().ReverseMap();
			CreateMap<User, UserDTO>().ReverseMap();
			CreateMap<Player, PlayerDTO>().ReverseMap();
			CreateMap<City, CityDTO>().ReverseMap();
			CreateMap<Country, CountryDTO>().ReverseMap();
			CreateMap<Ranking, RankingDTO>().ReverseMap();
			CreateMap<Club, ClubDTO>().ReverseMap();
			CreateMap<Manager, ManagerDTO>().ReverseMap();
			CreateMap<ManagerClub, ManagerClubDTO>().ReverseMap();
			CreateMap<Match, MatchDTO>().ReverseMap();
			CreateMap<Stadium, StadiumDTO>().ReverseMap();
			CreateMap<Referee, RefereeDTO>().ReverseMap();
            CreateMap<PlayerMatchRegistration, PlayerMatchRegistrationDTO>().ReverseMap();
			CreateMap<MatchDetail, MatchDetailDTO>().ReverseMap();
            CreateMap<Goal, GoalDTO>().ReverseMap();
        }
	}
}
