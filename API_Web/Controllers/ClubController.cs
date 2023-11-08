using BussinessObject.Models;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace API_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private IClubRepository _clubRepository;
		private IPlayerRepository _playerRepository;

		public ClubController(IClubRepository clubRepository, IPlayerRepository playerRepository)
		{
			_clubRepository = clubRepository;
			_playerRepository = playerRepository;
		}

		[HttpGet]
        public IActionResult GetClubs()
        {
            try
            {
                var clubs = _clubRepository.GetClubs();
                return Ok(clubs);
            }catch(Exception)
            {
                return BadRequest();
            }
        }

		[HttpGet("{id}")]
		public ActionResult<ClubDTO> GetClubById(int id)
		{
			return _clubRepository.GetClubById(id);
		}

		[HttpGet("user/{userId}")]
		public IActionResult GetClubByUserId(int userId)
		{
			try
			{
				var club = _clubRepository.GetClubByUserId(userId);
				if (club == null)
				{
					return NotFound();
				}
				return Ok(club);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpGet("{clubId}/players")]
		public IActionResult GetClubPlayers(int clubId)
		{
            List<PlayerDTO> players = _playerRepository.GetPlayersByClub(clubId);
			return Ok(players);
		}


		[HttpPost]
		public IActionResult CreateClub(ClubDTO clubDTO)
		{
			try
			{
				_clubRepository.Create(clubDTO);
				return Ok(clubDTO);
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}

		[HttpPut("{Id}")]
		public IActionResult UpdateClub(int id, ClubDTO clubDTO)
		{
			clubDTO.ClubId = id;
			try
			{
				_clubRepository.Update(clubDTO);
				return Ok();
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteClub(int clubId)
		{
			try
			{
				_clubRepository.Delete(clubId);
				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
	}
}
