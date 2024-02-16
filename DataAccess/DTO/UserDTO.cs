using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
	public class UserDTO
	{
		public int UserId { get; set; }

		public string Username { get; set; } = null!;

		public string Password { get; set; } = null!;
		public long? Phone { get; set; }
		[AllowNull]
		public int? RoleId { get; set; }

		[AllowNull]
		public virtual RoleDTO? Role { get; set; } = null!;
	}
}
