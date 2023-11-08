using BussinessObject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class ManagerClubController : Controller
	{
		private readonly HttpClient client = null;
		private string ManagerClubAPI = "";
		private string ClubApiUrl = "";
		private string ManagerAPI = "";

		public ManagerClubController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			ManagerClubAPI = "http://localhost:5282/api/ManagerClub";
			ClubApiUrl = "http://localhost:5282/api/Club";
			ManagerAPI = "http://localhost:5282/api/Manager";
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

		private async Task<List<Manager>> GetManagers()
		{
			HttpResponseMessage Response = await client.GetAsync(ManagerAPI);
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<Manager> managers = JsonSerializer.Deserialize<List<Manager>>(StrData, options);
			return managers;
		}

		public async Task<IActionResult> Index(int clubId)
		{
			List<ManagerClub> managerClubs = new List<ManagerClub>();
			HttpResponseMessage Response = await client.GetAsync($"{ManagerClubAPI}/GetManagerByClub/{clubId}");
			string StrData = await Response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			managerClubs = JsonSerializer.Deserialize<List<ManagerClub>>(StrData, options);
			List<Club> clubs = await GetClubs();
			List<Manager> managers = await GetManagers();
			foreach (var managerClub in managerClubs)
			{
				Club club = clubs.FirstOrDefault(c => c.ClubId == managerClub.ClubId);
				managerClub.Club = club;

				Manager manager = managers.FirstOrDefault(c => c.ManagerId == managerClub.ManagerId);
				managerClub.Manager = manager;
			}
			TempData["ClubId"] = clubId;
			return View(managerClubs);
		}
	}
}
