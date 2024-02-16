using BussinessObject.Models;
using DataAccess.DAO;
using DataAccess.IRepository;
using DataAccess.Mapping;
using DataAccess.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepository;
using Repository.Repository;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddTransient<IPlayerRepository, PlayerRepository>()
	.AddDbContext<FinalProPrn231Context>(opt =>
	builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IClubRepository, ClubRepository>()
    .AddDbContext<FinalProPrn231Context>(opt =>
    builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<ICountryRepository, CountryRepository>()
    .AddDbContext<FinalProPrn231Context>(opt =>
    builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<ICityRepository, CityRepository>()
    .AddDbContext<FinalProPrn231Context>(opt =>
    builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IStadiumRepository, StadiumRepository>()
    .AddDbContext<FinalProPrn231Context>(opt =>
    builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IUserRepository, UserRepository>()
	.AddDbContext<FinalProPrn231Context>(opt =>
	builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IRankingRepository, RankingRepository>()
	.AddDbContext<FinalProPrn231Context>(opt =>
	builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IMatchRepository, MatchRepository>()
	.AddDbContext<FinalProPrn231Context>(opt =>
	builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IRefRepository, RefRepository>()
	.AddDbContext<FinalProPrn231Context>(opt =>
	builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IPlayerMatchRegistrationRepository, PlayerMatchRegistrationRepository>()
    .AddDbContext<FinalProPrn231Context>(opt =>
    builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IMatchDetailRepository, MatchDetailRepository>()
	.AddDbContext<FinalProPrn231Context>(opt =>
	builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IGoalRepo, GoalRepo>()
    .AddDbContext<FinalProPrn231Context>(opt =>
    builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IManagerClubRepo, ManagerClubRepo>()
	.AddDbContext<FinalProPrn231Context>(opt =>
	builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IManagerRepo, ManagerRepo>()
	.AddDbContext<FinalProPrn231Context>(opt =>
	builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<IFavoClubRepo, FavoRepo>()
	.AddDbContext<FinalProPrn231Context>(opt =>
	builder.Configuration.GetConnectionString("DB"));
builder.Services.AddTransient<FavoClubDao>();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
			RoleClaimType = ClaimTypes.Role
		};
	});

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.PropertyNamingPolicy = null;
	options.JsonSerializerOptions.DictionaryKeyPolicy = null;
});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("RequireLoggedIn", policy =>
		policy.RequireAuthenticatedUser());
});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("RequireAdminRole", policy =>
		policy.RequireRole("Admin"));

	options.AddPolicy("RequireUserRole", policy =>
		policy.RequireClaim(ClaimTypes.Role));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
