using DataAccess.Request;
using DataAccess.Response;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using BussinessObject.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

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
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Index(string username, string password)
		{
			var jwt = await LoginAsync(username, password);
			if (!string.IsNullOrEmpty(jwt))
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var token = tokenHandler.ReadJwtToken(jwt);
				var roles = token.Claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();
				if (roles.Contains("Admin"))
				{
					var cookieOptions = new CookieOptions
					{
						HttpOnly = true,
						Expires = DateTime.UtcNow.AddHours(1)
					}; 
					Response.Cookies.Append("jwt", jwt, cookieOptions);
					HttpContext.Session.SetString("UserRole", "Admin");
					return Redirect("/homepage");
				}
				else if (roles.Contains("User"))
				{
					var cookieOptions = new CookieOptions
					{
						HttpOnly = true,
						Expires = DateTime.UtcNow.AddHours(1)
					};
					Response.Cookies.Append("jwt", jwt, cookieOptions);
					HttpContext.Session.SetString("UserRole", "User");
					return Redirect("/homepage");
				}
				else
				{
					ViewBag.ErrorMessage = "Invalid Role";
					return View("Index");
				}
			}
			else
			{
				ViewBag.ErrorMessage = "Invalid Username or Password";
				return View("Index");
			}
		}

		public async Task<string> LoginAsync(string username, string password)
		{
			var loginRequest = new LoginRequest()
			{
				Username = username,
				Password = password
			};
			var jsonContent = new StringContent(JsonSerializer.Serialize(loginRequest), Encoding.UTF8, "application/json");
			var response = await client.PostAsync(LoginApiUrl, jsonContent);
			var strData = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions()
			{
				PropertyNameCaseInsensitive = true
			};
			var loginResponse = JsonSerializer.Deserialize<LoginResponse>(strData, options);
			return loginResponse.Token;
		}
	}
}
