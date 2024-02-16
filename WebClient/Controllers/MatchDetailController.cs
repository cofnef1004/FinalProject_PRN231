using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class MatchDetailController : Controller
	{
		private readonly HttpClient client = null;
		private string MatchApiUrl = "";
		private string MatchDetailApiUrl = "";
		private string GoalApiUrl = "";
		private string PlayerApiUrl = "";

		public MatchDetailController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			MatchApiUrl = "http://localhost:5282/api/Match";
			MatchDetailApiUrl = "http://localhost:5282/api/MatchDetail";
			GoalApiUrl = "http://localhost:5282/api/Goal";
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

		private async Task<List<Match>> GetMatches()
		{
			HttpResponseMessage Response = await client.GetAsync($"{MatchApiUrl}/Matches");
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Match> matches = JsonSerializer.Deserialize<List<Match>>(StrData, options);
			return matches;
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

		public async Task<IActionResult> Index(int matchId)
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			TempData["Role"] = role;
			if (matchId == 0)
			{
				ViewBag.Message = "Invalid match ID.";
				return View();
			}
			HttpResponseMessage response = await client.GetAsync($"{MatchDetailApiUrl}/{matchId}");
			if (!response.IsSuccessStatusCode)
			{
				ViewBag.Message = "Match detail not found.";
				return View();
			}
			string data = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			MatchDetail matchDetail = JsonSerializer.Deserialize<MatchDetail>(data, options);
			if (matchDetail == null)
			{
				ViewBag.Message = "Match detail not found.";
				return View();
			}
			return View(matchDetail);
		}

        public IActionResult Add()
        {
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				var matches = GetMatches().Result;
				ViewData["Matches"] = matches.Select(m => new SelectListItem { Value = m.MatchId.ToString(), Text = $"{m.HomeNavigation.Name} - {m.AwayNavigation.Name} - Round : {m.Round}" });
				return View();
			}
            return RedirectToAction("Index", "Denied");
        }

        [HttpPost]
		public async Task<IActionResult> Add(MatchDetail matchDetail)
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.PostAsJsonAsync(MatchDetailApiUrl, matchDetail);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
				else
				{
					ViewBag.Message = "Failed to add MatchDetail.";
					return View();
				}
			}
            return RedirectToAction("Index", "Denied");
        }

		public async Task<IActionResult> Edit(int id)
        {
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage Response = await client.GetAsync($"{MatchDetailApiUrl}/MatchDetail/{id}");
				if (Response.IsSuccessStatusCode)
				{
					string StrData = await Response.Content.ReadAsStringAsync();
					var options = new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
					};
					MatchDetail matchDetail = JsonSerializer.Deserialize<MatchDetail>(StrData, options);
					return View(matchDetail);
				}
			}
            return RedirectToAction("Index", "Denied");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, MatchDetail matchDetail)
        {
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.PutAsJsonAsync($"{MatchDetailApiUrl}/{id}", matchDetail);
				if (response.IsSuccessStatusCode)
				{
					ViewBag.SuccessMessage = "Match detail updated successfully.";
					return View(matchDetail);
				}

				ViewBag.ErrorMessage = "Failed to update match detail.";
				return View(matchDetail);
			}
            return RedirectToAction("Index", "Denied");
        }
    }
}
