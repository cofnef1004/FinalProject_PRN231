using DataAccess.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Repository.Repository;

namespace API_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PlayerMatchRegistrationController : ControllerBase
	{
		private IPlayerMatchRegistrationRepository playerMatchRegistrationRepository;

		public PlayerMatchRegistrationController(IPlayerMatchRegistrationRepository playerMatchRegistrationRepository)
		{
			this.playerMatchRegistrationRepository = playerMatchRegistrationRepository;
		}

		[HttpGet("{clubId}/{matchId}")]
		public IActionResult GetRegisteredPlayersForClubAndMatch(int clubId, int matchId)
		{
			var registeredPlayers = playerMatchRegistrationRepository.GetRegisteredPlayersForClubAndMatch(clubId, matchId);

			if (registeredPlayers == null)
			{
				return NotFound();
			}
			return Ok(registeredPlayers);
		}

		[HttpPost]
		public IActionResult Add(PlayerMatchRegistrationDTO playerMatchRegistration)
		{
			playerMatchRegistrationRepository.Add(playerMatchRegistration);
			return Ok();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				playerMatchRegistrationRepository.Delete(id);
				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
	}
}
