using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class RefDAO
	{
		FinalProPrn231Context _context;

		public RefDAO(FinalProPrn231Context context)
		{
			_context = context;
		}

		public List<Referee> GetAll()
		{
			return _context.Referees.ToList();
		}
	}
}
