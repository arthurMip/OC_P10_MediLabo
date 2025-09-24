using MongoDB.Driver;
using PatientNotesApi.Database;
using PatientNotesApi.Services;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

int countdown = 20;
for (int i = 0; i < countdown; i++)
{
    Console.WriteLine($"Start in: {countdown - i}");
    await Task.Delay(TimeSpan.FromSeconds(1));
}
Console.WriteLine($"Starting...");

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.Configure<MongoDbSettings>(config.GetSection("MongoDbSettings"));
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = config.GetSection("MongoDbSettings").Get<MongoDbSettings>();
    var client = new MongoClient(settings!.ConnectionString);

    return client;
});

builder.Services.AddSingleton<PatientNotesService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
