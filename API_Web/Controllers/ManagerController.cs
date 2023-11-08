using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ManagerController : ControllerBase
	{
		private readonly IManagerRepo repo;

		public ManagerController(IManagerRepo repo)
		{
			this.repo = repo;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			try
			{
				var manager = repo.GetManagers();
				return Ok(manager);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}
	}
}
