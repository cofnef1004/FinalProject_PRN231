using AutoMapper;
using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class CityRepository : ICityRepository
    {
        FinalProPrn231Context _context;
        IMapper mapper;
        CityDAO cityDAO;

        public CityRepository(FinalProPrn231Context context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }
        public List<CityDTO> GetCities()
        {
            cityDAO = new CityDAO(_context);
            List<CityDTO> cities = mapper.Map<List<CityDTO>>(cityDAO.GetAll());
            return cities;
        }
    }
}
