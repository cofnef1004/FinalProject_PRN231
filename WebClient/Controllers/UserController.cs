using BussinessObject.Models;
using DataAccess.DTO;
using DataAccess.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class UserController : Controller
	{
		private readonly HttpClient client = null;
		private string UserApiUrl = "";

		public UserController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			UserApiUrl = "http://localhost:5282/api/User";
		}

		public async Task<IActionResult> Index()
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			int userId = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(x => x.Type == "id")?.Value);
			if (role.Equals("User"))
			{
				HttpResponseMessage Response = await client.GetAsync($"{UserApiUrl}/{userId}");
				if (Response.IsSuccessStatusCode)
				{
					string StrData = await Response.Content.ReadAsStringAsync();
					var options = new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
					};
					User user = JsonSerializer.Deserialize<User>(StrData, options);
					ViewBag.Message = TempData["Message"];
					TempData.Remove("Message");
					return View(user);
				}
			}
			if (role.Equals("Admin"))
			{
				return RedirectToAction("ViewUser");
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, UserDTO user)
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			int userId = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(x => x.Type == "id")?.Value);
			HttpResponseMessage response = await client.PutAsJsonAsync($"{UserApiUrl}/{userId}", user);
			if (response.IsSuccessStatusCode)
			{
				TempData["Message"] = "Update information success";
				return RedirectToAction("Index");
			}
			return BadRequest();
		}


		public async Task<IActionResult> ViewUser()
		{
			HttpResponseMessage Response = await client.GetAsync($"{UserApiUrl}");
			if (Response.IsSuccessStatusCode)
			{
				string StrData = await Response.Content.ReadAsStringAsync();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				List<User> user = JsonSerializer.Deserialize<List<User>>(StrData, options);
				return View(user);
			}
			return NotFound();
		}
	}
}
