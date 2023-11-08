using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Repository.Repository;

namespace API_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ManagerClubController : ControllerBase
	{
		private IManagerClubRepo repo;

		public ManagerClubController(IManagerClubRepo repo)
		{
			this.repo = repo;
		}

		[HttpGet]
		public IActionResult Get()
		{
			try
			{
				var managerClubs = repo.GetAll();
				return Ok(managerClubs);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpGet("GetManagerByClub/{clubId}")]
		public IActionResult GetByClub(int clubId)
		{
			try
			{
				var managerClubs = repo.GetByClubId(clubId);
				return Ok(managerClubs);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
	}
}
