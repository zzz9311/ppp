using MailService;
using Rebus.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers(x =>
{
    x.Filters.Add<ExceptionFilter>();
});

builder.Services.AddRebus(configure =>
{
    return configure
        .Logging(l => l.ColoredConsole())
        .Transport(t =>
            t.UsePostgreSql(
                "Server=localhost;Port=5432;Database=rebustest;User Id=Admin;Password=qwerty;",
                "service_bus", "messages-to-send"));
});
builder.Services.AutoRegisterHandlersFromAssembly(typeof(Program).Assembly);

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