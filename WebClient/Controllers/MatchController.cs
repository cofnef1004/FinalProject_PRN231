using Azure;
using BussinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Numerics;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class MatchController : Controller
	{
		private readonly HttpClient client = null;
		private string MatchApiUrl = "";
		private string ClubApiUrl = "";
		private string StaApiUrl = "";
		private string RefApiUrl = "";
		private string MatchDetailApiUrl = "";

		public MatchController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			MatchApiUrl = "http://localhost:5282/api/Match";
			ClubApiUrl = "http://localhost:5282/api/Club";
			StaApiUrl = "http://localhost:5282/api/Stadium";
			RefApiUrl = "http://localhost:5282/api/Ref";
			MatchDetailApiUrl = "http://localhost:5282/api/MatchDetail";
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

		private async Task<List<Referee>> GetReferees()
		{
			HttpResponseMessage Response = await client.GetAsync(RefApiUrl);
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Referee> referees = JsonSerializer.Deserialize<List<Referee>>(StrData, options);
			return referees;
		}

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

		public async Task<IActionResult> Index(int? round)
		{
			if (TempData.ContainsKey("ErrorMessage"))
			{
				ViewBag.ErrorMessage = TempData["ErrorMessage"];
			}
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			TempData["Role"] = role;
			HttpResponseMessage response;
			if (round.HasValue && round.Value != 0)
			{
				response = await client.GetAsync($"{MatchApiUrl}/match/{round}");
			}
			else
			{
				response = await client.GetAsync(MatchApiUrl);
			}
			string StrData = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Match> matches = JsonSerializer.Deserialize<List<Match>>(StrData, options);
			List<Club> clubs = await GetClubs();
			List<Stadium> stadia = await GetStadia();
			List<Referee> referees = await GetReferees();
			foreach (var home in matches)
			{
				Club club = clubs.FirstOrDefault(c => c.ClubId == home.Home);
				home.HomeNavigation = club;
			}
			foreach (var away in matches)
			{
				Club club = clubs.FirstOrDefault(c => c.ClubId == away.Away);
				away.AwayNavigation = club;
			}
			foreach (var match in matches)
			{
				Stadium stadium = stadia.FirstOrDefault(c => c.StadiumId == match.StadiumId);
				match.Stadium = stadium;

				Referee referee = referees.FirstOrDefault(c => c.RefId == match.RefId);
				match.Ref = referee;
			}
			ViewData["matches"] = matches;
			List<int> rounds = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 36, 37, 38 };
			ViewBag.CurrentRound = round;
			ViewBag.Rounds = new SelectList(rounds);
			return View(matches);
		}

		public async Task<IActionResult> GetMatchesByClub(int clubId)
		{
			HttpResponseMessage Response = await client.GetAsync($"{MatchApiUrl}/Club/{clubId}");
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Match> matches = JsonSerializer.Deserialize<List<Match>>(StrData, options);
			List<Club> clubs = await GetClubs();
			List<Stadium> stadia = await GetStadia();
			List<Referee> referees = await GetReferees();
			foreach (var home in matches)
			{
				Club club = clubs.FirstOrDefault(c => c.ClubId == home.Home);
				home.HomeNavigation = club;
			}
			foreach (var away in matches)
			{
				Club club = clubs.FirstOrDefault(c => c.ClubId == away.Away);
				away.AwayNavigation = club;
			}
			foreach (var match in matches)
			{
				Stadium stadium = stadia.FirstOrDefault(c => c.StadiumId == match.StadiumId);
				match.Stadium = stadium;

				Referee referee = referees.FirstOrDefault(c => c.RefId == match.RefId);
				match.Ref = referee;
			}
			return View(matches);
		}

		public async Task<IActionResult> Edit(int id)
		{
            string token = HttpContext.Session.GetString("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				List<Referee> referees = await GetReferees();
				ViewData["Referees"] = new SelectList(referees, "RefId", "Name");
				HttpResponseMessage Response = await client.GetAsync($"{MatchApiUrl}/{id}");
				if (Response.IsSuccessStatusCode)
				{
					string StrData = await Response.Content.ReadAsStringAsync();
					var options = new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
					};
					Match match = JsonSerializer.Deserialize<Match>(StrData, options);
					return View(match);
				}
			}
            return RedirectToAction("Index", "Denied");
        }

		[HttpPost]
		public async Task<IActionResult> Edit(int id, Match match)
        {
            string token = HttpContext.Session.GetString("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.PutAsJsonAsync($"{MatchApiUrl}/{id}", match);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
			}
            return RedirectToAction("Index", "Denied");
        }

		[HttpPost]
		public async Task<IActionResult> GenerateMatches(int round)
		{
            string token = HttpContext.Session.GetString("token");
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.PostAsync($"{MatchApiUrl}/generate?round={round}", null);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
				else
				{
					ViewBag.ErrorMessage1 = "All previous round matches must be completed before a new round can be created";
					return StatusCode((int)response.StatusCode);
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
				HttpResponseMessage response = await client.DeleteAsync($"{MatchApiUrl}/{id}");
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
			}
            return RedirectToAction("Index", "Denied");
        }
	}
}
