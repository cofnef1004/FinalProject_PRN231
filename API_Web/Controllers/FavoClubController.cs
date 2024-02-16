using BussinessObject.Models;
using DataAccess.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FavoClubController : ControllerBase
	{
		private readonly IFavoClubRepo _favoClubRepo;
		private readonly FinalProPrn231Context _context;

		public FavoClubController(IFavoClubRepo favoClubRepo, FinalProPrn231Context context)
		{
			_favoClubRepo = favoClubRepo;
			_context = context;
		}

		[HttpGet("top")]
		public IActionResult GetTopFavoriteClubIds()
		{
			try
			{
				var topFavoriteClubs = _context.FavoriteClubs.Include(fc => fc.Club)
					.ThenInclude(c => c.City)
				.Include(fc => fc.Club)
					.ThenInclude(c => c.Stadium)
					.GroupBy(fc => fc.ClubId)
					.OrderByDescending(group => group.Count())
					.Take(5)
					.Select(group => group.Key)
					.ToList();

				return Ok(topFavoriteClubs);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet("{userId}")]
		public IActionResult GetFavoriteClubs(int userId)
		{
			try
			{
				var favoriteClubs = _favoClubRepo.GetFavoriteClubs(userId);
				return Ok(favoriteClubs);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPost("{userId}/{clubId}")]
		public IActionResult AddFavoriteClub(int userId, int clubId)
		{
			try
			{
				_favoClubRepo.AddFavoriteClub(userId, clubId);
				return Ok("Club added to favorites successfully.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		public IActionResult RemoveFavoriteClub(int id)
		{
			try
			{
				_favoClubRepo.RemoveFavoriteClub(id);
				return Ok("Club removed from favorites successfully.");
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
