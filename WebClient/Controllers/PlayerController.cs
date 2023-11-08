using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class PlayerController : Controller
	{
		private readonly HttpClient client = null;
		private string PlayerApiUrl = "";
		private string ClubApiUrl = "";
		private string CouApiUrl = "";

		private async Task<List<Club>> GetClubs()
		{
			HttpResponseMessage clubResponse = await client.GetAsync(ClubApiUrl);
			string clubStrData = await clubResponse.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Club> clubs = JsonSerializer.Deserialize<List<Club>>(clubStrData, options);
			return clubs;
		}

		private async Task<List<Country>> GetCountries()
		{
			HttpResponseMessage couResponse = await client.GetAsync(CouApiUrl);
			string couStrData = await couResponse.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Country> countries = JsonSerializer.Deserialize<List<Country>>(couStrData, options);
			return countries;
		}

		public PlayerController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			PlayerApiUrl = "http://localhost:5282/api/Player";
			ClubApiUrl = "http://localhost:5282/api/Club";
			CouApiUrl = "http://localhost:5282/api/Country";
		}


		public async Task<IActionResult> Index(int clubId)
		{
			HttpResponseMessage playerResponse = await client.GetAsync($"{PlayerApiUrl}/{clubId}");
			string playerStrData = await playerResponse.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Player> players = JsonSerializer.Deserialize<List<Player>>(playerStrData, options);
			List<Club> clubs = await GetClubs();
			foreach (var player in players)
			{
				Club club = clubs.FirstOrDefault(c => c.ClubId == player.ClubId);
				player.Club = club;
			}
			List<Country> countries = await GetCountries();
			foreach (var player in players)
			{
				Country country = countries.FirstOrDefault(c => c.CountryId == player.CountryId);
				player.Country = country;
			}
			TempData["ClubId"] = clubId;
			return View(players);
		}

		//Create
		public async Task<IActionResult> Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Player player)
		{
			HttpResponseMessage response = await client.PostAsJsonAsync(PlayerApiUrl, player);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return BadRequest();
		}

		public async Task<IActionResult> Edit(int id)
		{
			HttpResponseMessage Response = await client.GetAsync($"{PlayerApiUrl}/Player/{id}");
			if (Response.IsSuccessStatusCode)
			{
				string StrData = await Response.Content.ReadAsStringAsync();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
				};
				Player player = JsonSerializer.Deserialize<Player>(StrData, options);
				return View(player);
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, Player player)
		{
			HttpResponseMessage response = await client.PutAsJsonAsync($"{PlayerApiUrl}/{id}", player);

			if (response.IsSuccessStatusCode)
			{
				int clubId = (int)TempData["ClubId"];
				return RedirectToAction("Index", new { clubId = clubId });
			}
			return BadRequest();
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			HttpResponseMessage response = await client.DeleteAsync($"{PlayerApiUrl}/{id}");
			if (response.IsSuccessStatusCode)
			{
				int clubId = (int)TempData["ClubId"];
				return RedirectToAction("Index", new { clubId = clubId});
			}
			return NotFound();
		}
	}
}
