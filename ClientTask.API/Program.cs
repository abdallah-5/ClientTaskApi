using ClientTask.API.Extensions;
using ClientTask.Core.Interfaces.IRepositories;
using ClientTask.Core.Interfaces.IServices;
using ClientTask.Core.Services;
using ClientTask.Infrastructure.Data;
using ClientTask.Infrastructure.Data.Repositories;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var ConnectionString = builder.Configuration.GetConnectionString("AppDbConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));

// Add services to the container.

builder.Services.AddApplicationServices();

builder.Services.AddCors(options => {
options.AddPolicy("AllowSpecificOrigin", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddHangfire(configuration => configuration
     .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
     .UseSimpleAssemblyNameTypeSerializer()
     .UseRecommendedSerializerSettings()
     .UseSqlServerStorage(builder.Configuration.GetConnectionString("AppDbConnection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure Hangfire dashboard (optional)
app.UseHangfireDashboard();

// Start Hangfire server
app.UseHangfireServer();

// Initialize Hangfire job scheduler
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var polygonService = services.GetRequiredService<IPolygonService>();
        polygonService.ScheduleJob();
    }
    catch (Exception ex)
    {
        // Handle any exceptions if needed
        Console.WriteLine($"Error initializing Hangfire job scheduler: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors(builder =>
{
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
