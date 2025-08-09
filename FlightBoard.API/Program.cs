// Purpose: Configures dependency injection, EF Core, SignalR, AutoMapper, controllers, logging, and validation.
using AutoMapper;
using FlightBoard.API.Hubs;
using FlightBoard.Application.Services;
using FlightBoard.Application.Validation;
using FlightBoard.Infrastructure.Data;
using FlightBoard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FlightBoardDbConnectionString")));

builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddSingleton<FlightStatusCalculator>();
builder.Services.AddSingleton<FlightValidator>();

builder.Services.AddSignalR();
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

const string FrontendDevCors = "FrontendDevCors";

builder.Services.AddCors(opt =>
{
    opt.AddPolicy(FrontendDevCors, policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(FrontendDevCors);
app.MapHub<FlightHub>("/hubs/flights").RequireCors(FrontendDevCors);
app.MapControllers().RequireCors(FrontendDevCors);

// Auto-migrate DB at startup (inside container too)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FlightDbContext>();
    await db.Database.MigrateAsync();
    await FlightBoard.Data.DataSeeder.SeedAsync(db);
}

app.Run();
