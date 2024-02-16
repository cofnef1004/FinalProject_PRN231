using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class RankingController : Controller
	{
		private readonly HttpClient client = null;
		private string RankApiUrl = "";
		private string ClubApiUrl = "";

		public RankingController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			RankApiUrl = "http://localhost:5282/api/Ranking";
			ClubApiUrl = "http://localhost:5282/api/Club";
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

		private async Task<List<Club>> GetClubsInRank()
		{
			HttpResponseMessage clubResponse = await client.GetAsync($"{RankApiUrl}/GetRankedClubs");
			string clubStrData = await clubResponse.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Club> clubs = JsonSerializer.Deserialize<List<Club>>(clubStrData, options);
			return clubs;
		}

		public async Task<IActionResult> Index()
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			TempData["Role"] = role;
			HttpResponseMessage Response = await client.GetAsync(RankApiUrl);
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Ranking> rankings = JsonSerializer.Deserialize<List<Ranking>>(StrData, options);
			List<Club> clubs = await GetClubs();
			List<Club> clubsInRank = await GetClubsInRank();
			List<Club> unrankedClubs = clubs.Where(c => !clubsInRank.Any(cr => cr.ClubId == c.ClubId)).ToList();
			ViewData["Club"] = new SelectList(unrankedClubs, "ClubId", "Name");
			foreach (var rank in rankings)
			{
				Club club = clubs.FirstOrDefault(c => c.ClubId == rank.ClubId);
				rank.Club = club;
			}
			ViewBag.Message = TempData["Message"];
			TempData.Remove("Message");
			ViewBag.ErrorMessage = TempData["ErrorMessage"];
			TempData.Remove("ErrorMessage");
			return View(rankings);
		}

		[HttpPost]
		public async Task<IActionResult> AddClubToRanking(int clubId)
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				var jsonContent = new StringContent(clubId.ToString(), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.PostAsync($"{RankApiUrl}/add", jsonContent);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
			}
			else
			{
				return RedirectToAction("Index", "Denied");
			}
			TempData["ErrorMessage"] = "Cannot Add the club while the league are in proccess";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> GetDetail(int clubId)
		{
			HttpResponseMessage response = await client.GetAsync($"{RankApiUrl}/GetByClub/{clubId}");
			string strData = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			Ranking ranking = JsonSerializer.Deserialize<Ranking>(strData, options);
			List<Club> clubs = await GetClubs();

			Club club = clubs.FirstOrDefault(c => c.ClubId == ranking.ClubId);
			ranking.Club = club;

			return View(ranking);
		}

		[HttpPost]
		public async Task<IActionResult> DeleteClubFromRanking(int clubId)
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.DeleteAsync($"{RankApiUrl}/delete/{clubId}");
				if (response.IsSuccessStatusCode)
				{
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

		[HttpPost]
		public async Task<IActionResult> ToggleAllStatus()
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.PostAsync($"{RankApiUrl}/toggle-status", null);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
			}
			return RedirectToAction("Index", "Denied");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateRankings()
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.PostAsync(RankApiUrl + "/update", null);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
			}
			else
			{
				return RedirectToAction("Index", "Denied");
			}
			TempData["ErrorMessage"] = "Cannot Update result while the league are pause";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> UpdateAllRankings()
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage Response = await client.GetAsync($"{RankApiUrl}");
				if (Response.IsSuccessStatusCode)
				{
					string StrData = await Response.Content.ReadAsStringAsync();
					var options = new JsonSerializerOptions
					{
						PropertyNameCaseInsensitive = true,
					};
					List<Ranking> rankings = JsonSerializer.Deserialize<List<Ranking>>(StrData, options);
					return View(rankings);
				}
			}
			return RedirectToAction("Index", "Denied");
		}

		[HttpPost]
		public async Task<IActionResult> UpdateAllRankings(List<Ranking> newRankings)
		{
			try
			{
				string apiUrl = "http://localhost:5282/api/Ranking/updatemanual";
				var jsonContent = new StringContent(JsonSerializer.Serialize(newRankings), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await client.PostAsync(apiUrl, jsonContent);
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("Index");
				}
				else
				{
					return BadRequest();
				}
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpPost]
		public async Task<IActionResult> Reset()
		{
			string token = HttpContext.Session.GetString("token");
			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);
			string role = jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value;
			if (role.Equals("Admin"))
			{
				HttpResponseMessage response = await client.PostAsync(RankApiUrl + "/reset", null);
				if (response.IsSuccessStatusCode)
				{
					TempData["Message"] = "Reset success";
					return RedirectToAction("Index");
				}
			}
			else
			{
				return RedirectToAction("Index", "Denied");
			}
			TempData["ErrorMessage"] = "Cannot Reset result while the league are pause";
			return RedirectToAction("Index");
		}
	}
}
