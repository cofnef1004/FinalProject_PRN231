using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class GoalController : Controller
	{
		private readonly HttpClient client = null;
		private string MatchApiUrl = "";
		private string GoalApiUrl = "";
		private string PlayerApiUrl = "";

		public GoalController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			MatchApiUrl = "http://localhost:5282/api/Match";
			GoalApiUrl = "http://localhost:5282/api/Goal";
			PlayerApiUrl = "http://localhost:5282/api/Player";
		}

		private async Task<List<Match>> GetMatches()
		{
			HttpResponseMessage Response = await client.GetAsync(MatchApiUrl);
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Match> matches = JsonSerializer.Deserialize<List<Match>>(StrData, options);
			return matches;
		}

		private async Task<int> GetHomeFromMatch(int matchId)
		{
			HttpResponseMessage response = await client.GetAsync($"{MatchApiUrl}/home/{matchId}");
			string responseData = await response.Content.ReadAsStringAsync();
			int home = int.Parse(responseData);
			return home;
		}

		private async Task<int> GetAwayFromMatch(int matchId)
		{
			HttpResponseMessage response = await client.GetAsync($"{MatchApiUrl}/away/{matchId}");
			string responseData = await response.Content.ReadAsStringAsync();
			int away = int.Parse(responseData);
			return away;
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

		public async Task<IActionResult> Index(int matchId)
		{
			HttpResponseMessage Response = await client.GetAsync($"{GoalApiUrl}/{matchId}");
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Goal> goalInMatches = System.Text.Json.JsonSerializer.Deserialize<List<Goal>>(StrData, options);
			List<Player> players = await GetPlayers();
			foreach (var goal in goalInMatches)
			{
				Player player = players.FirstOrDefault(c => c.PlayerId == goal.PlayerId);
				goal.Player = player;
			}
			TempData["MatchId"] = matchId;
			ViewBag.MatchId = matchId;
			ViewBag.GoalsInMatch = goalInMatches;
			return View(goalInMatches);
		}

		public async Task<IActionResult> Add(int matchId)
		{
			var matches = GetMatches().Result;
			int home = await GetHomeFromMatch(1);
			int away = await GetAwayFromMatch(1);
			List<Player> homeplayers = await GetPlayersByClubId(home);
			List<Player> awayplayers = await GetPlayersByClubId(away);
			ViewData["HomePlayer"] = new SelectList(homeplayers, "PlayerId", "Name");
			ViewData["AwayPlayer"] = new SelectList(awayplayers, "PlayerId", "Name");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(Goal goal)
		{
			HttpResponseMessage response = await client.PostAsJsonAsync(GoalApiUrl, goal);
			if (response.IsSuccessStatusCode)
			{
				TempData["SuccessMessage"] = "Player registration added successfully.";
				return Redirect("/match");
			}
			else
			{
				ViewBag.Message = "Failed to add goal for the match.";
				return View();
			}
		}
	}
}
