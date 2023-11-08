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
    public class CountryRepository : ICountryRepository
    {
        FinalProPrn231Context _context;
        IMapper mapper;
        CountryDAO countryDAO;

        public CountryRepository(FinalProPrn231Context context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }
        public List<CountryDTO> GetCountries()
        {
            countryDAO = new CountryDAO(_context);
            List<CountryDTO> countries = mapper.Map<List<CountryDTO>>(countryDAO.GetAll());
            return countries;
        }
    }
}
