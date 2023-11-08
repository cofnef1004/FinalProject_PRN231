using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CityDAO
    {
        FinalProPrn231Context _context;

        public CityDAO(FinalProPrn231Context context)
        {
            _context = context;
        }

        public List<City> GetAll()
        {
            return _context.Cities.ToList();
        }
    }
}
