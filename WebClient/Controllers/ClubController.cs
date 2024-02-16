using AutoMapper.Execution;
using BussinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class ClubController : Controller
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly HttpClient client = null;
		private string ClubApiUrl = "";
		private string CityApiUrl = "";
		private string StaApiUrl = "";

		private async Task<List<City>> GetCities()
		{
			HttpResponseMessage clubResponse = await client.GetAsync(CityApiUrl);
			string clubStrData = await clubResponse.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<City> cities = JsonSerializer.Deserialize<List<City>>(clubStrData, options);
			return cities;
		}
		private async Task<List<Stadium>> GetStadia()
		{
			HttpResponseMessage couResponse = await client.GetAsync(StaApiUrl);
			string couStrData = await couResponse.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Stadium> stadia = JsonSerializer.Deserialize<List<Stadium>>(couStrData, options);
			return stadia;
		}

		private async Task<List<Club>> GetClub()
		{
			HttpResponseMessage couResponse = await client.GetAsync(ClubApiUrl);
			string couStrData = await couResponse.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Club> clubs = JsonSerializer.Deserialize<List<Club>>(couStrData, options);
			return clubs;
		}

		public ClubController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			ClubApiUrl = "http://localhost:5282/api/Club";
			CityApiUrl = "http://localhost:5282/api/City";
			StaApiUrl = "http://localhost:5282/api/Stadium";
		}

		public async Task<IActionResult> Index()
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			int userId = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(x => x.Type == "id")?.Value);
			HttpContext.Session.SetInt32("UserId", userId);
			TempData["Role"] = role;
			ViewBag.Role = HttpContext.Session.GetString("Role");
			if (role.Equals("Admin") || role.Equals("User"))
			{
				HttpResponseMessage clubResponse = await client.GetAsync($"{ClubApiUrl}");
				List<Club> clubs = await GetClub();
				List<Stadium> stadia = await GetStadia();
				List<City> cities = await GetCities();
				string clubStrData = await clubResponse.Content.ReadAsStringAsync();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
				};
				clubs = JsonSerializer.Deserialize<List<Club>>(clubStrData, options);
				foreach (var club in clubs)
				{
					Stadium stadium = stadia.FirstOrDefault(c => c.StadiumId == club.StadiumId);
					club.Stadium = stadium;

					City city = cities.FirstOrDefault(c => c.CityId == club.CityId);
					club.City = city;
				}
				ViewBag.ErrorMessage = TempData["ErrorMessage"];
				TempData.Remove("ErrorMessage");
				ViewBag.Message = TempData["Message"];
				TempData.Remove("Message");
				return View(clubs);
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> ToggleClubStatus(int id)
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.PutAsync($"{ClubApiUrl}/togglestatus/{id}", null);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
				return BadRequest();
			}
            return RedirectToAction("Index", "Denied");
        }

		public async Task<IActionResult> Create()
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				List<Stadium> stadia = await GetStadia();
				List<City> cities = await GetCities();
				ViewData["City"] = new SelectList(cities, "CityId", "Name");
				ViewData["Stadium"] = new SelectList(stadia, "StadiumId", "Name");
				return View();
			}
            return RedirectToAction("Index", "Denied");
        }

		[HttpPost]
		public async Task<IActionResult> Create(Club club)
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.PostAsJsonAsync(ClubApiUrl, club);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
				return BadRequest();
			}
			return RedirectToAction("Index", "Denied");
		}

		public async Task<IActionResult> Edit(int id)
		{
			List<Stadium> stadia = await GetStadia();
			List<City> cities = await GetCities();
			ViewData["City"] = new SelectList(cities, "CityId", "Name");
			ViewData["Stadium"] = new SelectList(stadia, "StadiumId", "Name");
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage Response = await client.GetAsync($"{ClubApiUrl}/{id}");
				if (Response.IsSuccessStatusCode)
				{
					string StrData = await Response.Content.ReadAsStringAsync();
					var options = new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
					};
					Club club = JsonSerializer.Deserialize<Club>(StrData, options);
					return View(club);
				}
			}
            return RedirectToAction("Index", "Denied");
        }

		[HttpPost]
		public async Task<IActionResult> Edit(int id, Club club)
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.PutAsJsonAsync($"{ClubApiUrl}/{id}", club);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
			}
            return RedirectToAction("Index", "Denied");
        }

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.DeleteAsync($"{ClubApiUrl}/{id}");
				if (response.IsSuccessStatusCode)
				{
					TempData["Message"] = "Delete Success";
					return RedirectToAction("Index");
				}
			}
			else
			{
				return RedirectToAction("Index", "Denied");
			}
			TempData["ErrorMessage"] = "Cannot delete the club while the league are in proccess";
			return RedirectToAction("Index");
		}
	}
}
