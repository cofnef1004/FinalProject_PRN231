using BussinessObject.Models;
using DataAccess.Request;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class FavoClubController : Controller
	{
		private readonly HttpClient client = null;
		private string FavoClubApiUrl = "";
		private string CityApiUrl = "";
		private string StaApiUrl = "";
		private string ClubApiUrl = "";
		public FavoClubController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			FavoClubApiUrl = "http://localhost:5282/api/FavoClub";
			CityApiUrl = "http://localhost:5282/api/City";
			StaApiUrl = "http://localhost:5282/api/Stadium";
			ClubApiUrl = "http://localhost:5282/api/Club";
		}
		private async Task<List<City>> GetCities()
		{
			HttpResponseMessage clubResponse = await client.GetAsync(CityApiUrl);
			string clubStrData = await clubResponse.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<City> cities = System.Text.Json.JsonSerializer.Deserialize<List<City>>(clubStrData, options);
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
			List<Stadium> stadia = System.Text.Json.JsonSerializer.Deserialize<List<Stadium>>(couStrData, options);
			return stadia;
		}

		private async Task<List<Club>> GetClubs()
		{
			HttpResponseMessage couResponse = await client.GetAsync(ClubApiUrl);
			string couStrData = await couResponse.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Club> clubs = System.Text.Json.JsonSerializer.Deserialize<List<Club>>(couStrData, options);
			return clubs;
		}

		public async Task<IActionResult> Index()
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			int userId = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(x => x.Type == "id")?.Value);

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			if (role.Equals("User"))
			{
				HttpResponseMessage response = await client.GetAsync($"{FavoClubApiUrl}/{userId}");
				response.EnsureSuccessStatusCode();
				string clubStrData = await response.Content.ReadAsStringAsync();

				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
				};
				List<FavoriteClub> favoriteClubs = System.Text.Json.JsonSerializer.Deserialize<List<FavoriteClub>>(clubStrData, options);
				List<Stadium> stadia = await GetStadia();
				List<City> cities = await GetCities();
				List<Club> clubs = await GetClubs();
				foreach (var club in favoriteClubs)
				{
					Club matchedClub = clubs.FirstOrDefault(c => c.ClubId == club.ClubId);

					if (matchedClub != null)
					{
						Stadium stadium = stadia.FirstOrDefault(s => s.StadiumId == matchedClub.StadiumId);
						matchedClub.Stadium = stadium;
						City city = cities.FirstOrDefault(c => c.CityId == matchedClub.CityId);
						matchedClub.City = city;
					}
				}
				ViewBag.ErrorMessage = TempData["ErrorMessage"];
				TempData.Remove("ErrorMessage");
				return View(favoriteClubs);
			}
			else if (role.Equals("Admin"))
			{
				return RedirectToAction("TopClub");
			}
			return RedirectToAction("Index", "Login");
		}

		[HttpPost]
		public async Task<IActionResult> AddFavoriteClub(int userId, int clubId)
		{
			try
			{
				FavorRequest request = new FavorRequest();
				request.UserId = userId;
				request.ClubId = clubId;
				string token = HttpContext.Session.GetString("token");
				var tokenHandler = new JwtSecurityTokenHandler();
				var jwtToken = tokenHandler.ReadJwtToken(token);
				string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
				userId = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(x => x.Type == "id")?.Value);
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var jsonContent = System.Text.Json.JsonSerializer.Serialize(request);
				var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

				HttpResponseMessage response = await client.PostAsJsonAsync($"{FavoClubApiUrl}/{userId}/{clubId}", request);
				if (response.IsSuccessStatusCode)
				{
					return Redirect("/Club");
				}
				else
				{

					TempData["ErrorMessage"] = "You have added this club to your favorites list";
					return RedirectToAction("Index");
				}
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			HttpResponseMessage response = await client.DeleteAsync($"{FavoClubApiUrl}/{id}");
			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return NotFound();
		}

		public async Task<IActionResult> TopClub()
		{
			try
			{
				string token = HttpContext.Session.GetString("token");
				var tokenHandler = new JwtSecurityTokenHandler();
				var jwtToken = tokenHandler.ReadJwtToken(token);
				string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
				int userId = Convert.ToInt32(jwtToken.Claims.FirstOrDefault(x => x.Type == "id")?.Value);

				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				HttpResponseMessage response = await client.GetAsync($"{FavoClubApiUrl}/top");
				response.EnsureSuccessStatusCode();
				string clubStrData = await response.Content.ReadAsStringAsync();

				List<int> favoriteClubIds = System.Text.Json.JsonSerializer.Deserialize<List<int>>(clubStrData);

				List<Club> clubs = await GetClubs();
				List<Club> favoriteClubs = clubs.Where(c => favoriteClubIds.Contains(c.ClubId)).ToList();

				List<Stadium> stadiums = await GetStadia();
				List<City> cities = await GetCities();

				foreach (var club in favoriteClubs)
				{
					Stadium stadium = stadiums.FirstOrDefault(s => s.StadiumId == club.StadiumId);
					club.Stadium = stadium;

					City city = cities.FirstOrDefault(c => c.CityId == club.CityId);
					club.City = city;
				}

				ViewBag.ErrorMessage = TempData["ErrorMessage"];
				TempData.Remove("ErrorMessage");

				return View(favoriteClubs);
			}
			catch (Exception ex)
			{
				return View("Error"); // Thay thế "Error" bằng tên view lỗi mà bạn muốn hiển thị
			}
		}
	}
}
