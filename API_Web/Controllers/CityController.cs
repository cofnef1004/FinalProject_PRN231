using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;

namespace API_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private ICityRepository _cityRepository;

        public CityController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            try
            {
                var cities = _cityRepository.GetCities();
                return Ok(cities);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
