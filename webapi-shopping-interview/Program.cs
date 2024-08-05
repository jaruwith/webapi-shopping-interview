using Microsoft.EntityFrameworkCore;
using Serilog;
using webapi_shopping_interview.Data;


var builder = WebApplication.CreateBuilder(args);

// Configure logging with Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/myapp-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers();

// for test
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
var isProduction = builder.Configuration.GetValue<bool>("DatabaseSettings:IsProduction");

if (isProduction)
{
    builder.Services.AddDbContext<DbContext, AppDbContext>(options =>
        options.UseInMemoryDatabase("Interview"));
}
else
{
    builder.Services.AddDbContext<DbContext, AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();

// Log application start
Log.Information("Application Starting");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        dbContext.Database.EnsureCreated();
        logger.LogInformation("Database created successfully and seeding data.");
        // Add your seed data here
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while creating the database.");
    }
}

app.Run();

Log.Information("Application Ended");
Log.CloseAndFlush();