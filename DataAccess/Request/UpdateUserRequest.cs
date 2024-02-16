using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Request
{
	public class UpdateUserRequest
	{
		public string Username { get; set; } = null!;

		public string Password { get; set; } = null!;
		public long? Phone { get; set; }
	}
}
