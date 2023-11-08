using Azure.Messaging;
using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlayerController : ControllerBase
	{
		private IPlayerRepository _playerRepository;

		public PlayerController(IPlayerRepository playerRepository)
		{
			_playerRepository = playerRepository;
		}

		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				var players = _playerRepository.GetPlayers();
				return Ok(players);
			}catch (Exception)
			{

				return BadRequest();
			}
		}

		[HttpGet("Player/{id}")]
		public IActionResult GetById(int id)
		{
			try
			{
				var player = _playerRepository.GetPlayerById(id);
				return Ok(player);
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}



		[HttpGet("{clubId}")]
        public IActionResult GetByClubId(int clubId)
        {
            try
            {
                var players = _playerRepository.GetPlayersByClub(clubId);
                return Ok(players);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

		[HttpGet("Match/{clubId}")]
		public IActionResult GetByClubIdNotRegis(int clubId)
		{
			try
			{
				var players = _playerRepository.GetPlayersByClubIdNotRegis(clubId);
				return Ok(players);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public IActionResult CreatePlayer(PlayerDTO playerDTO)
		{
			try
			{
				_playerRepository.createPlayer(playerDTO);
				return Ok(playerDTO);
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}

		[HttpPut("{Id}")]
		public IActionResult Update(int id, PlayerDTO playerDTO)
		{
			playerDTO.PlayerId = id;
			try
			{
				_playerRepository.Update(playerDTO);
				return Ok();
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				_playerRepository.Delete(id);
				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
	}
}
