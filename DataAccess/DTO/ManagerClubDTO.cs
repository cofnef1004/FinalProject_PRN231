using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
	public class ManagerClubDTO
	{
		public int ManagerId { get; set; }

		public int ClubId { get; set; }

		public string Role { get; set; } = null!;

		public virtual ClubDTO Club { get; set; } = null!;

		public virtual ManagerDTO Manager { get; set; } = null!;
	}
}
