using BussinessObject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataAccess.DTO
{
	public class PlayerDTO
	{
		public int PlayerId { get; set; }

		public string Name { get; set; } = null!;

		public string Position { get; set; } = null!;

		public DateTime Dob { get; set; }

		public int ClubId { get; set; }

		public int CountryId { get; set; }

		public int Number { get; set; }

		public string? Avatar { get; set; }
		[AllowNull]
		[JsonIgnore]
		public virtual ClubDTO? Club { get; set; }
		[AllowNull]
		[JsonIgnore]
		public virtual CountryDTO? Country { get; set; }
	}
}
