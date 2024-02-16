using Microsoft.AspNetCore.Mvc;

namespace WebClient.Controllers.Models
{
	public class LogOutController : Controller
	{
		public IActionResult Index()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Index", "Login");
		}
	}
}
