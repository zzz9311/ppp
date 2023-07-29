using Core.DependencyInjectionExtensions;
using PetPPP;
using Rebus.Config;
using Rebus.Routing.TypeBased;
using ServiceContracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSelfRegistered(typeof(Program).Assembly);

builder.Services.AddControllers(x =>
{
    x.Filters.Add<ExceptionFilter>();
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

#region redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Redis");
    
    options.Configuration = connectionString;
    options.InstanceName = "local";
});
#endregion

#region MemoryCache

builder.Services.AddMemoryCache();

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();