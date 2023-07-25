using Core;
using Core.DependencyInjectionExtensions;
using DAL;
using DAL.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PetPPP.BLL;
using PetPPP.Extensions;
using PetPPP.JWT;
using PetPPP.Mapper;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using ServiceContracts;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSelfRegistered(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.UseDatabase(builder.Configuration);
builder.Services.UseServices();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT").GetValue<string>("Secret")))
        };
    });


builder.Services.AddRebus(configure =>
{
    return configure
        .Logging(l => l.ColoredConsole())
        .Transport(t =>
            t.UsePostgreSqlAsOneWayClient(
                "Server=localhost;Port=5432;Database=rebustest;User Id=Admin;Password=qwerty;", "service_bus"))
        .Routing(x => x.TypeBased().MapAssemblyOf<UserRegisteredEvent>("messages-to-send"));
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis");
    
    options.Configuration = connectionString;
    options.InstanceName = "local";
});

builder.Services.Configure<JWTSettings>(act => builder.Configuration.GetSection("JWT").Bind(act));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.ApplyMigrations();

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.Run();