using PatientApi.Data;
using PatientApi.Data.Entities;
using PatientApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

int countdown = 20;
for (int i = 0; i < countdown; i++)
{
    Console.WriteLine($"Start in: {countdown - i}");
    await Task.Delay(TimeSpan.FromSeconds(1));
}
Console.WriteLine($"Starting...");


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


Console.WriteLine("DefaultConnection: " + builder.Configuration.GetConnectionString("DefaultConnection"));




builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseSeeding(SeedDatabase.SeedPatients);
});

var urlConfigs = builder.Configuration.GetSection("URL");

builder.Services.AddHttpClient("gateway", client =>
{
    client.BaseAddress = new Uri(urlConfigs["GATEWAY"]!);
});

builder.Services.AddHttpClient("notes_api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/api/notes");
});

builder.Services.AddHttpClient("report_api", client =>
{
    client.BaseAddress = new Uri("https://localhost:7000/api/diabetes-reports");
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!))
    };
});


builder.Services.AddScoped<PatientService>();

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();
    }
    catch (Exception)
    {
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
