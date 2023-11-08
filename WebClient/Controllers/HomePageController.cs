using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class HomePageController : Controller
	{
		private readonly HttpClient client = null;
		private string MatchApiUrl = "";
		private string ClubApiUrl = "";
		private string StaApiUrl = "";
		private string RefApiUrl = "";

		public HomePageController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			MatchApiUrl = "http://localhost:5282/api/Match";
			ClubApiUrl = "http://localhost:5282/api/Club";
			StaApiUrl = "http://localhost:5282/api/Stadium";
			RefApiUrl = "http://localhost:5282/api/Ref";
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
			HttpResponseMessage response = await client.GetAsync($"{MatchApiUrl}/random");
			string StrData = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Match> matches = JsonSerializer.Deserialize<List<Match>>(StrData, options);
			List<Club> clubs = await GetClubs();
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
			ViewData["matches"] = matches;
			return View(matches);
		}
	}
}
