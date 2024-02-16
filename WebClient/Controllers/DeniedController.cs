using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers
{
	public class DeniedController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
