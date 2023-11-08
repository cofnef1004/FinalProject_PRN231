using DataAccess.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Repository.Repository;

namespace API_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MatchController : ControllerBase
	{
		private IMatchRepository _matchRepository;

		public MatchController(IMatchRepository matchRepository)
		{
			_matchRepository = matchRepository;
		}

		[HttpGet]
		public IActionResult Getmatch()
		{
			try
			{
				var matches = _matchRepository.GetMatches();
				return Ok(matches);
			}catch(Exception)
			{
				return BadRequest();
			}
		}

		[HttpGet("Matches")]
		public IActionResult GetmatchNotDetail()
		{
			try
			{
				var matches = _matchRepository.GetMatchNotDetail();
				return Ok(matches);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpGet("random")]
		public IActionResult GetRandomMatch()
		{
			try
			{
				var matches = _matchRepository.GetRandomMatches();
				return Ok(matches);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpGet("{id}")]
		public ActionResult<MatchDTO> GetMatchById(int id)
		{
			return _matchRepository.GetMatchById(id);
		}

		[HttpGet("match/{round}")]
		public IActionResult GetmatchsByRound(int round)
		{
			try
			{
				var matches = _matchRepository.GetMatchesByRound(round);
				return Ok(matches);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpPut("{id}")]
        public IActionResult UpdateMatch(int id, MatchDTO matchDTO)
        {
            matchDTO.MatchId = id;
            try
            {
                _matchRepository.UpdateMatch(matchDTO);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

		[HttpDelete("{id}")]
		public IActionResult DeleteMatch(int id)
		{
			try
			{
				_matchRepository.DeleteMatch(id);
				return Ok();
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}

		[HttpPost("generate")]
		public IActionResult GenerateMatches(int round)
		{
			try
			{
				_matchRepository.GenerateMatchesForRound(round);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		[HttpGet("home/{id}")]
		public IActionResult GetHome(int id)
		{
			try
			{
				int home = _matchRepository.Home(id);
				return Ok(home);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpGet("away/{id}")]
		public IActionResult GetAway(int id)
		{
			try
			{
				int away = _matchRepository.Away(id);
				return Ok(away);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
	}
}
