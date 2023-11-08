using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class ManagerDAO
	{
		private readonly FinalProPrn231Context context;

		public ManagerDAO(FinalProPrn231Context context)
		{
			this.context= context;
		}

		public List<Manager> GetAll() 
		{
			return context.Managers.ToList();
		}
	}
}
