using BussinessObject.Models;
using DataAccess.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepository
{
	public interface IMatchDetailRepository
	{
		public MatchDetailDTO GetMatchDetailbyMatchId(int matchId);

        public MatchDetailDTO GetMatchDetailbyId(int id);
        void AddMatchDetail(MatchDetailDTO matchDetail);
		void EditMatchDetail(MatchDetailDTO matchDetail);
	}
}
