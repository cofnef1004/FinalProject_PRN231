using DataAccess.DTO;
using DataAccess.IRepository;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoalController : ControllerBase
    {
        private IGoalRepo goalRepo;

        public GoalController(IGoalRepo goalRepo)
        {
            this.goalRepo = goalRepo;
        }

        [HttpGet("{matchId}")]
        public IActionResult GetById(int matchId)
        {
            var goal = goalRepo.GetGoalsInAMatch(matchId);
            if (goal == null)
            {
                return NotFound();
            }
            return Ok(goal);
        }

        [HttpPost]
        public IActionResult AddMatchDetail(GoalDTO goal)
        {
            goalRepo.Add(goal);
            return Ok();
        }
    }
}
