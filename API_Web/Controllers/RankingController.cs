using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Repository.Repository;

namespace API_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RankingController : ControllerBase
	{
		private IRankingRepository _rankingRepository;

		public RankingController(IRankingRepository rankingRepository)
		{
			_rankingRepository = rankingRepository;
		}

		[HttpGet]
		public IActionResult GetRankings()
		{
			try
			{
				var ranks = _rankingRepository.GetRankings();
				return Ok(ranks);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpPost("update")]
		public IActionResult UpdateRankings()
		{
			try
			{
				_rankingRepository.UpdateRankings();
				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
	}
}
