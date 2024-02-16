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
	public class ClubDTO
	{
		public int ClubId { get; set; }

		public int UserId { get; set; }

		public string Name { get; set; } = null!;

		public int StadiumId { get; set; }

		public int CityId { get; set; }

		public string? Logo { get; set; }
		public bool? Status { get; set; }

		public virtual CityDTO? City { get; set; } = null!;

		public virtual StadiumDTO? Stadium { get; set; } = null!;
		[JsonIgnore]
		[AllowNull]
		public virtual UserDTO? User { get; set; } = null!;
	}
}
