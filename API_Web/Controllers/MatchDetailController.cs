using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MatchDetailController : ControllerBase
	{
		private IMatchDetailRepository matchDetailRepository;

		public MatchDetailController(IMatchDetailRepository matchDetailRepository)
		{
			this.matchDetailRepository = matchDetailRepository;
		}

		[HttpGet("{matchId}")]
		public IActionResult GetByMatchId(int matchId)
		{
			var matchDetail = matchDetailRepository.GetMatchDetailbyMatchId(matchId);
			if (matchDetail == null)
			{
				return NotFound();
			}
			return Ok(matchDetail);
		}

        [HttpGet("MatchDetail/{id}")]
        public IActionResult GetById(int id)
        {
            var matchDetail = matchDetailRepository.GetMatchDetailbyId(id);
            if (matchDetail == null)
            {
                return NotFound();
            }
            return Ok(matchDetail);
        }

        [HttpPost]
		public IActionResult AddMatchDetail(MatchDetailDTO matchDetail)
		{
			matchDetailRepository.AddMatchDetail(matchDetail);
			return Ok();
		}

		[HttpPut("{id}")]
		public IActionResult EditMatchDetail(MatchDetailDTO matchDetail,int id)
		{
            matchDetail.MatchDetailId = id;
            matchDetailRepository.EditMatchDetail(matchDetail);
			return Ok();
		}
	}
}
