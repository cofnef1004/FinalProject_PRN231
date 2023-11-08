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
    public class PlayerMatchRegistrationDTO
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public int MatchId { get; set; }
		public string? Position { get; set; }
		[JsonIgnore]
		[AllowNull]
		public virtual MatchDTO? Match { get; set; } = null!;
		[JsonIgnore]
		[AllowNull]
		public virtual PlayerDTO? Player { get; set; } = null!;
    }
}
