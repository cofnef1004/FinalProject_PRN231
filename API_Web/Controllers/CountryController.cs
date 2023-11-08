using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepository;
using Repository.Repository;

namespace API_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }


        [HttpGet]
        public IActionResult GetCountry()
        {
            try
            {
                var countries = _countryRepository.GetCountries();
                return Ok(countries);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
