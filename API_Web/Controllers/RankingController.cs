using BussinessObject.Models;
using DataAccess.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using Repository.Repository;

namespace API_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class RankingController : ControllerBase
	{
		private IRankingRepository _rankingRepository;
		private readonly FinalProPrn231Context context;

		public RankingController(IRankingRepository rankingRepository, FinalProPrn231Context context)
		{
			_rankingRepository = rankingRepository;
			this.context = context;
		}

		[HttpGet]
		public IActionResult GetRankings()
		{
			try
			{
				var ranks = _rankingRepository.GetRankings();
				return Ok(ranks);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpPost("add")]
		public IActionResult AddClubToRanking([FromBody] int clubId)
		{
			try
			{
				bool allStatusTrue = context.Rankings.All(r => r.Status == true);
				if (allStatusTrue)
				{
					Club club = context.Clubs.FirstOrDefault(c => c.ClubId == clubId);
					if (club != null)
					{
						Ranking ranking = new Ranking
						{
							ClubId = club.ClubId,
							Point = 0,
							Difference = 0,
							MatchPlayed = 0,
							Win = 0,
							Draw = 0,
							Loose = 0,
							Status = true
						};
						context.Rankings.Add(ranking);
						context.SaveChanges();
						return Ok();
					}
					else
					{
						return NotFound("Club not found");
					}
				}
				else
				{
					return BadRequest("Cannot add club to ranking when not all statuses are true");
				}
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpDelete("delete/{clubId}")]
		public IActionResult DeleteClubFromRanking(int clubId)
		{
			try
			{
				bool allStatusTrue = context.Rankings.All(r => r.Status == true);
				if (allStatusTrue)
				{
					Club club = context.Clubs.FirstOrDefault(c => c.ClubId == clubId);
					if (club != null)
					{
						Ranking ranking = context.Rankings.FirstOrDefault(r => r.ClubId == clubId);
						if (ranking != null)
						{
							context.Rankings.Remove(ranking);
							context.SaveChanges();
							return Ok();
						}
						else
						{
							return NotFound("Club not found in the ranking");
						}
					}
					else
					{
						return NotFound("Club not found");
					}
				}
				else
				{
					return BadRequest("Cannot delete club from ranking when not all statuses are true");
				}
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpGet("GetByClub/{clubId}")]
		public IActionResult GetDetailByClub(int clubId)
		{
			try
			{
				var rank = _rankingRepository.GetDetail(clubId);
				return Ok(rank);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpGet("GetRankedClubs")]
		public IActionResult GetRankedClubs()
		{
			try
			{
				List<Club> rankedClubs;
				using (var dbContext = new FinalProPrn231Context())
				{
					rankedClubs = dbContext.Rankings
						.Where(r => r.Club.Name != null)
						.Select(r => r.Club)
						.ToList();
				}
				return Ok(rankedClubs);
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpPost("toggle-status")]
		public IActionResult ToggleStatus()
		{
			try
			{
				var rankings = context.Rankings.ToList();
				foreach (var ranking in rankings)
				{
					ranking.Status = !ranking.Status;
				}
				context.SaveChanges();
				return Ok();
			}
			catch (Exception)
			{
				return BadRequest();
			}
		}

		[HttpPost("updatemanual")]
		public IActionResult UpdateAllRankings(List<Ranking> newRankings)
		{
			try
			{
				newRankings = context.Rankings.ToList();
				foreach (var newRanking in newRankings)
				{
					var existingRanking = context.Rankings.FirstOrDefault(r => r.ClubId == newRanking.ClubId);

					if (existingRanking != null)
					{
						existingRanking.MatchPlayed = newRanking.MatchPlayed;
						existingRanking.Win = newRanking.Win;
						existingRanking.Draw = newRanking.Draw;
						existingRanking.Loose = newRanking.Loose;
						existingRanking.Difference = newRanking.Difference;
						existingRanking.Point = newRanking.Point;
					}
				}
				context.SaveChanges();
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest($"Đã xảy ra lỗi: {ex.Message}");
			}
		}

		[HttpPost("update")]
		public IActionResult UpdateRankings()
		{
			bool allStatusTrue = context.Rankings.All(r => r.Status == false);
			if (allStatusTrue)
			{
				try
				{
					_rankingRepository.UpdateRankings();
					return Ok();
				}
				catch (Exception)
				{
					return BadRequest();
				}
			}
			return BadRequest();
		}


		[HttpPost("reset")]
		public IActionResult ResetRankings()
		{
			try
			{
				bool allStatusTrue = context.Rankings.All(r => r.Status == true);
				if (allStatusTrue)
				{
					var rankings = context.Rankings.ToList();
					foreach (var ranking in rankings)
					{
						ranking.Point = 0;
						ranking.Difference = 0;
						ranking.MatchPlayed = 0;
						ranking.Win = 0;
						ranking.Draw = 0;
						ranking.Loose = 0;
					}
					context.SaveChanges();
					return Ok();
				}
				else
				{
					return BadRequest("Cannot reset rankings when not all statuses are true");
				}
			}
			catch (Exception ex)
			{
				return BadRequest($"An error occurred: {ex.Message}");
			}
		}
	}
}
