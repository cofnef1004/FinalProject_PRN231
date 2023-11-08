using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
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

		public async Task<IActionResult> Index()
		{
			HttpResponseMessage Response = await client.GetAsync(RankApiUrl);
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Ranking> rankings = JsonSerializer.Deserialize<List<Ranking>>(StrData, options);
			List<Club> clubs = await GetClubs();
			foreach (var rank in rankings)
			{
				Club club = clubs.FirstOrDefault(c => c.ClubId == rank.ClubId);
				rank.Club = club;
			}
			return View(rankings);
		}

		[HttpPost]
		public async Task<IActionResult> UpdateRankings()
		{
			HttpResponseMessage response = await client.PostAsync(RankApiUrl + "/update", null);
			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			else
			{
				return BadRequest();
			}
		}
	}
}
