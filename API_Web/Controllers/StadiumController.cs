using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace API_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StadiumController : ControllerBase
    {
        private IStadiumRepository _stadiumRepository;

        public StadiumController(IStadiumRepository stadiumRepository)
        {
            _stadiumRepository = stadiumRepository;
        }

        [HttpGet]
        public IActionResult GetStadiums()
        {
            try
            {
                var stadiums = _stadiumRepository.GetStadiums();
                return Ok(stadiums);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
