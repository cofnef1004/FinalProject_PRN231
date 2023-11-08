using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.DTO;
using DataAccess.Request;
using DataAccess.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepository;
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

		public UserController(IUserRepository userRepository, IConfiguration configuration)
		{
			_userRepository = userRepository;
			_jwtSetting = configuration.GetSection("Jwt").Get<JwtSetting>();
			_configuration = configuration;
		}

		/*[HttpGet("{username}/{password}")]
        public IActionResult Login(string username, string password)
		{
			var user = _userRepository.Login(username, password);
			if (user != null)
			{
				return Ok(user);
			}
			return Unauthorized("Invalid username or password");
		}*/

		/*[HttpPost]
		public IActionResult Login([FromBody] LoginRequest request)
		{
			var adminAcc = _configuration.GetSection("Admin").Get<LoginRequest>();
			var userLogin = _userRepository.CheckUserLogin(request);
			if (userLogin != null || (string.Equals(request.Username, adminAcc.Username, StringComparison.CurrentCultureIgnoreCase) && request.Password == adminAcc.Password))
			{
				var token = GenerateJwtToken(userLogin);
				return Ok(new LoginResponse() { Token = token });
			}
			return Unauthorized(new { message = "Invalid" });
		}
		private string GenerateJwtToken(UserDTO? userLogin)
		{
			var adminAcc = _configuration.GetSection("Admin").Get<LoginRequest>();
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSetting.Key);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("username", userLogin == null ? adminAcc.Username : userLogin.Username) }),
				Expires = DateTime.UtcNow.AddMinutes(_jwtSetting.DurationInMinutes),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
				Audience = "abc",
				Issuer = "abc"
			};
			if (userLogin == null)
				tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, "Admin"));
			else
				tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, userLogin.Role.RoleName));
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}*/

		[HttpPost]
		public IActionResult Login([FromBody] LoginRequest request)
		{
			var userLogin = _userRepository.CheckUserLogin(request);
			if (userLogin != null)
			{
				var token = GenerateJwtToken(userLogin);
				return Ok(new LoginResponse() { Token = token });
			}
			return Unauthorized(new { message = "Invalid" });
		}

		private string GenerateJwtToken(UserDTO userLogin)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtSetting.Key);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[] { new Claim("username", userLogin.Username) }),
				Expires = DateTime.UtcNow.AddMinutes(_jwtSetting.DurationInMinutes),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
				Audience = "abc",
				Issuer = "abc"
			};
			tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, userLogin.Role.RoleName));
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
