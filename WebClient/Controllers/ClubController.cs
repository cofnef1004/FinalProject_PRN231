using AutoMapper.Execution;
using BussinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Controllers
{
	public class ClubController : Controller
	{
		private readonly HttpClient client = null;
		private string ClubApiUrl = "";
		private string CityApiUrl = "";
		private string StaApiUrl = "";

		private async Task<List<City>> GetCities()
		{
			HttpResponseMessage clubResponse = await client.GetAsync(CityApiUrl);
			string clubStrData = await clubResponse.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			List<City> cities = JsonSerializer.Deserialize<List<City>>(clubStrData, options);
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
			List<Stadium> stadia = JsonSerializer.Deserialize<List<Stadium>>(couStrData, options);
			return stadia;
		}

		public ClubController()
		{
			client = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			client.DefaultRequestHeaders.Accept.Add(contentType);
			ClubApiUrl = "http://localhost:5282/api/Club";
			CityApiUrl = "http://localhost:5282/api/City";
			StaApiUrl = "http://localhost:5282/api/Stadium";
		}

		public async Task<IActionResult> Index()
		{
			if (TempData.ContainsKey("ErrorMessage"))
			{
				ViewBag.ErrorMessage = TempData["ErrorMessage"];
			}
			List<Club> clubs = new List<Club>();
			List<Stadium> stadia = await GetStadia();
			List<City> cities = await GetCities();
			HttpResponseMessage clubResponse = await client.GetAsync($"{ClubApiUrl}");
			string clubStrData = await clubResponse.Content.ReadAsStringAsync();

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			clubs = JsonSerializer.Deserialize<List<Club>>(clubStrData, options);
			foreach (var club in clubs)
			{
				Stadium stadium = stadia.FirstOrDefault(c => c.StadiumId == club.StadiumId);
				club.Stadium = stadium;

				City city = cities.FirstOrDefault(c => c.CityId == club.CityId);
				club.City = city;
			}
			return View(clubs);
		}

		public async Task<IActionResult> Create()
		{
			List<Stadium> stadia = await GetStadia();
			List<City> cities = await GetCities();
			ViewData["City"] = new SelectList(cities, "CityId", "Name");
			ViewData["Stadium"] = new SelectList(stadia, "StadiumId", "Name");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Club club)
		{
			HttpResponseMessage response = await client.PostAsJsonAsync(ClubApiUrl, club);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return BadRequest();
		}

		public async Task<IActionResult> Edit(int id)
		{
			List<Stadium> stadia = await GetStadia();
			List<City> cities = await GetCities();
			ViewData["City"] = new SelectList(cities, "CityId", "Name");
			ViewData["Stadium"] = new SelectList(stadia, "StadiumId", "Name");

			HttpResponseMessage Response = await client.GetAsync($"{ClubApiUrl}/{id}");
			if (Response.IsSuccessStatusCode)
			{
				string StrData = await Response.Content.ReadAsStringAsync();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true,
				};
				Club club = JsonSerializer.Deserialize<Club>(StrData, options);
				return View(club);
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> Edit(int id, Club club)
		{
			HttpResponseMessage response = await client.PutAsJsonAsync($"{ClubApiUrl}/{id}", club);

			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			return BadRequest();
		}


		//Delete
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			HttpResponseMessage response = await client.DeleteAsync($"{ClubApiUrl}/{id}");
			if (response.IsSuccessStatusCode)
			{
				return RedirectToAction("Index");
			}
			TempData["ErrorMessage"] = "Cannot delete the club until the end of league";
			return NotFound();
		}
	}
}
