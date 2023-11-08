using BussinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO
{
	public class ManagerClubDAO
	{
		private readonly FinalProPrn231Context context;

		public ManagerClubDAO(FinalProPrn231Context context)
		{
			this.context = context;
		}

		public List<ManagerClub> GetAll() 
		{
			return context.ManagerClubs.Include(p => p.Manager).Include(p => p.Club).ToList();
		}

		public List<ManagerClub> GetByClubId(int clubId)
		{
			return context.ManagerClubs.Include(p=>p.Manager).Include(p=>p.Club).Where(p=>p.ClubId == clubId).ToList();
		}
	}
}
