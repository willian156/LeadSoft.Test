using LeadSoft.Test.Controllers;
using LeadSoft.Test.DAO;
using LeadSoft.Test.Hubs;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
Env.Load();

var allowedOrigins = Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")?.Split(",");
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

builder.Services.AddDbContext<DataContext>(
    options => options.UseNpgsql(connectionString)
    );

builder.Services.AddCors(
    options =>
    {
        options.AddPolicy("AllowAll",
        policy => policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
    }
    );

builder.Services.AddSignalR().AddJsonProtocol();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();


app.UseCors("AllowAll");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<ProcessingDataHub>("/ProcessListener");

app.Run();
