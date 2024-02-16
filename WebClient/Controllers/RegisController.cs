using DataAccess.Request;
using DataAccess.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace WebClient.Controllers
{
	public class RegisController : Controller
	{
		private readonly HttpClient client = null;
		private string RegisApiUrl = "";

		public RegisController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			RegisApiUrl = "http://localhost:5282/api/User";
		}

		[HttpGet]
		public IActionResult Index()
		{
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			TempData.Remove("ErrorMessage");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register()
		{
			string username = HttpContext.Request.Form["username"];
			string password = HttpContext.Request.Form["password"];
			string confirmPassword = HttpContext.Request.Form["confirmPassword"];
			long phone = Int32.Parse(HttpContext.Request.Form["phone"]);
			int roleId = Int32.Parse(HttpContext.Request.Form["role"]);

			if (password != confirmPassword)
			{
				TempData["ErrorMessage"] = "Password and Confirm Password must match.";
				return Redirect("/Regis");
			}

			RegisRequest request = new RegisRequest();
			request.Username = username;
			request.Password = password;
			request.Phone = phone;
			request.RoleId = roleId;

			var jsonContent = System.Text.Json.JsonSerializer.Serialize(request);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync($"{RegisApiUrl}/Register", content);

			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMessage"] = "Invalid Username or Password";
				return Redirect("/Regis");
			}

			return Redirect("/Login");
		}
	}
}
