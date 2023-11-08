using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
	public class ManagerDTO
	{
		public int ManagerId { get; set; }

		public string Name { get; set; } = null!;

		public DateTime Dob { get; set; }

		public int CountryId { get; set; }

		public virtual CountryDTO Country { get; set; } = null!;
	}
}
