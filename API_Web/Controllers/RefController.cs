using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace API_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RefController : ControllerBase
	{
		private IRefRepository refRepository;

		public RefController(IRefRepository refRepository)
		{
			this.refRepository = refRepository;
		}

		[HttpGet]
		public IActionResult GetRef() 
		{
			try
			{
				var reference = refRepository.GetReferees();
				return Ok(reference);
			}
			catch(Exception) 
			{
				return BadRequest();	
			}
		}
	}
}
