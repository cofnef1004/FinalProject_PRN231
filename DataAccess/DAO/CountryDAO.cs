using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
    public class CountryDAO
    {
        FinalProPrn231Context _context;

        public CountryDAO(FinalProPrn231Context context)
        {
            _context = context;
        }

        public List<Country> GetAll()
        {
            return _context.Countries.ToList();
        }
    }
}
