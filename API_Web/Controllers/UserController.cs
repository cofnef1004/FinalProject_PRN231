using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.Request;
using DataAccess.Response;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepository;
using Repository.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_Web.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private IUserRepository _userRepository;
		private JwtSetting _jwtSetting;
		private IConfiguration _configuration;
		private readonly FinalProPrn231Context context;

		public UserController(IUserRepository userRepository, IConfiguration configuration, FinalProPrn231Context context)
		{
			_userRepository = userRepository;
			_jwtSetting = configuration.GetSection("Jwt").Get<JwtSetting>();
			_configuration = configuration;
			this.context = context;
		}

		[HttpGet("{id}")]
		public ActionResult<UserDTO> GetUserById(int id)
		{
			return _userRepository.GetUserById(id);
		}

		[HttpGet]
		public List<User> GetAll()
		{ 
			var users = context.Users.ToList();
			return users;
		}

		[HttpPost]
		public IActionResult Login([FromBody] LoginRequest request)
		{
			var adminAcc = _configuration.GetSection("Admin").Get<LoginRequest>();
			var userLogin = _userRepository.CheckUserLogin(request);
			if (userLogin != null || (string.Equals(request.Username, adminAcc.Username, StringComparison.CurrentCultureIgnoreCase) && request.Password == adminAcc.Password))
			{
				var token = GenerateJwtToken(userLogin);
				LoginResponse loginResponse = new LoginResponse();
				loginResponse.Token = token;
				return Ok(loginResponse);
			}
			return Unauthorized(new { message = "Invalid credentials" });
		}
		private string GenerateJwtToken(UserDTO? userLogin)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSetting.Key);

			var claims = new List<Claim>();
			if (userLogin == null)
			{
				var adminAcc = _configuration.GetSection("Admin").Get<LoginRequest>();
				claims.Add(new Claim("username", adminAcc.Username));
				claims.Add(new Claim(ClaimTypes.Role, "Admin"));
			}
			else
			{
				claims.Add(new Claim("username", userLogin.Username));
				claims.Add(new Claim(ClaimTypes.Role, "User"));
				claims.Add(new Claim("id", userLogin.UserId.ToString()));
			}
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(_jwtSetting.DurationInMinutes),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
				Audience = "abc",
				Issuer = "abc"
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}

		[HttpPost("Register")]
		public IActionResult Register([FromBody] RegisRequest request)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				_userRepository.Register(request);
				return Ok(new { message = "Success" });
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error" });
			}
		}

		[HttpPut("{id}")]
		public IActionResult UpdateClub(int id, UserDTO userDTO)
		{
			userDTO.UserId = id;
			try
			{
				_userRepository.Update(userDTO);
				return Ok();
			}
			catch (Exception)
			{

				return BadRequest();
			}
		}
	}
}
