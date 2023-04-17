using Common.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagement.Common.Mapper;
using UserManagement.Database.DataAccess;
using UserManagement.Database.Repositories;
using UserManagement.Database.Repositories.Contracts; 
using UserManagement.Services;
using UserManagement.Services.Contracts;
using static UserManagement.Common.Constants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<UserDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:7264",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY_VALUE))
        };
    });

builder.Services.AddAutoMapper(typeof(MappingProfiles));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtHelper, JwtHelper>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
//app.UseRouting();
app.UseAuthorization();
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllers();
//});


app.MapControllers();
app.UseMiddleware<GlobalExceptionHandler>();
app.Run();
