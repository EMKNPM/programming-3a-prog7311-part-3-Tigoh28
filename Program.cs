using GLMS.API_.Data;
using GLMS.API_.Repositories;
using GLMS.API_.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configure database setup using the verified configuration space
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<LogisticsService>();
builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
