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
    public class StadiumRepository : IStadiumRepository
    {
        FinalProPrn231Context _context;
        IMapper mapper;
        StadiumDAO stadiumDAO;

        public StadiumRepository(FinalProPrn231Context context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }
        public List<StadiumDTO> GetStadiums()
        {
            stadiumDAO = new StadiumDAO(_context);
            List<StadiumDTO> stadia = mapper.Map<List<StadiumDTO>>(stadiumDAO.GetAll());
            return stadia;
        }
    }
}
