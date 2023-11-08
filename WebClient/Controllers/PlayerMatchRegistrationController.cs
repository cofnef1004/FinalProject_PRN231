using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class PlayerMatchRegistrationController : Controller
	{
		private readonly HttpClient client = null;
		private string PlayerRegisApiUrl = "";
		private string PlayerApiUrl = "";
		private int clubId;
		private int matchId;

		public PlayerMatchRegistrationController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			PlayerRegisApiUrl = "http://localhost:5282/api/PlayerMatchRegistration";
			PlayerApiUrl = "http://localhost:5282/api/Player";
		}

		private async Task<List<Player>> GetPlayers()
		{
			HttpResponseMessage Response = await client.GetAsync(PlayerApiUrl);
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Player> players = System.Text.Json.JsonSerializer.Deserialize<List<Player>>(StrData, options);
			return players;
		}

		private async Task<List<Player>> GetPlayersByClubId(int clubId)
		{
			HttpResponseMessage Response = await client.GetAsync($"{PlayerApiUrl}/{clubId}");
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Player> players = System.Text.Json.JsonSerializer.Deserialize<List<Player>>(StrData, options);
			return players;
		}

		public async Task<IActionResult> Index(int clubId, int matchId)
		{
			HttpResponseMessage Response = await client.GetAsync($"{PlayerRegisApiUrl}/{clubId}/{matchId}");
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<PlayerMatchRegistration> playersRegis = System.Text.Json.JsonSerializer.Deserialize<List<PlayerMatchRegistration>>(StrData, options);
			List<Player> players = await GetPlayers();
			foreach (var playerRegis in playersRegis)
			{
				Player player = players.FirstOrDefault(c => c.PlayerId == playerRegis.PlayerId);
				playerRegis.Player = player;
			}
			TempData["ClubId"] = clubId;
			TempData["MatchId"] = matchId;
			this.clubId = clubId;
			this.matchId = matchId;
			ViewBag.MatchId = matchId;
			return View(playersRegis);
		}

		public async Task<IActionResult> Add(int matchId)
		{
			int clubId = (int)TempData["ClubId"];
			ViewData["MatchId"] = matchId;
			ViewData["ClubId"] = clubId;
			List<Player> players = await GetPlayersByClubId(clubId);
			ViewData["Player"] = new SelectList(players, "PlayerId", "Name");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(PlayerMatchRegistration playerMatchRegistration)
		{
			HttpResponseMessage response = await client.PostAsJsonAsync(PlayerRegisApiUrl, playerMatchRegistration);
			if (response.IsSuccessStatusCode)
			{
				int matchId = (int)TempData["MatchId"];
				int clubId = (int)TempData["ClubId"];
				return RedirectToAction("Index", new { clubId = clubId, matchId = matchId });
			}
			else
			{
				ViewBag.Message = "Failed to register player for the match.";
				return View();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			HttpResponseMessage response = await client.DeleteAsync($"{PlayerRegisApiUrl}/{id}");
			if (response.IsSuccessStatusCode)
			{
				int matchId = (int)TempData["MatchId"];
				int clubId = (int)TempData["ClubId"];
				return RedirectToAction("Index", new { clubId = clubId, matchId = matchId });
			}
			return NotFound();
		}
	}
}