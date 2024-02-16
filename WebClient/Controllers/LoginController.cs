using DataAccess.Request;
using DataAccess.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using BussinessObject.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Net.Http;

namespace WebClient.Controllers
{
	public class LoginController : Controller
	{
		private readonly HttpClient client = null;
		private string LoginApiUrl = "";

		public LoginController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			LoginApiUrl = "http://localhost:5282/api/User";
		}

		[HttpGet]
		public IActionResult Index()
		{
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			TempData.Remove("ErrorMessage");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login()
		{
			LoginRequest request = new LoginRequest();
			request.Username = HttpContext.Request.Form["username"];
			request.Password = HttpContext.Request.Form["password"];
			var jsonContent = System.Text.Json.JsonSerializer.Serialize(request);
			var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await client.PostAsync($"{LoginApiUrl}", content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMessage"] = "Login Failed";
				return Redirect("/Login");
			}
			else
			{
				string strData = await response.Content.ReadAsStringAsync();
				var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(strData);
				var token = loginResponse.Token;
				HttpContext.Session.SetString("token", token);
			}
			return Redirect("/homepage");
		}
	}
}
