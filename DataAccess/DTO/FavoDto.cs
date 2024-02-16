using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
	public class FavoDto
	{
		public int Id { get; set; }

		public int ClubId { get; set; }

		public int UserId { get; set; }
		public virtual ClubDTO Club { get; set; } = null!;

		public virtual UserDTO User { get; set; } = null!;
	}
}
